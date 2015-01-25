using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerData
{
    public PlayerData (NetworkPlayer netPlayerParam, int localPlayerIdParam)
    {
        networkPlayer = netPlayerParam;
        localPlayerId = localPlayerIdParam;
    }

    public void SetCharacter (Character characterParam)
    {
        character = characterParam;
    }

    public NetworkPlayer networkPlayer;
    public int localPlayerId;
    public Character character = null;
}

public class GameManager : MonoBehaviour
{
    public const string GAME_NAME = "Synchronarchy - Conjunction Dysfunction";
    public static GameManager singleton;

    private int gameMode = -1;
    private int lastLevelPrefix = 0;

    public int localPlayerCount = 3;

    public IList<PlayerData> players;

    public Character characterPrefab;

    public int MOVE_ALLOWANCE = 4;

    public LevelGenerator levelGen;

    void Awake ()
    {
        if (singleton == null) {
            singleton = this;
        } else {
            Destroy (gameObject);
            return;
        }

        players = new List<PlayerData> ();

        DontDestroyOnLoad (gameObject);
    }

    public static void SetGameMode (int newMode)
    {
        singleton.gameMode = newMode;
    }

    public static void StartServer (int port, bool loadLobby)
    {

        Network.InitializeServer (5, port, false);

        for (int i = 0; i < singleton.localPlayerCount; i ++) {
            PlayerData newP = new PlayerData (Network.player, i);
            singleton.players.Add (newP);
        }

        //MasterServer.RegisterHost ("Synchronarchy - Conjunction Dysfunction", "", "");
        if (loadLobby)
            LoadLobby ();
    }

    public static void LoadLobby ()
    {
        singleton.networkView.RPC ("LoadLevel", RPCMode.AllBuffered, "SceneLobby", singleton.lastLevelPrefix + 1);

    }

    public void OnPlayerConnected (NetworkPlayer player)
    {
    }

    [RPC]
    public void NewClient (int newClientPlayerCount, NetworkMessageInfo msgInfo)
    {
        if (!Network.isServer)
            Debug.LogError ("Received Server Message as Client!!");

        for (int i = 0; i < newClientPlayerCount; i ++) {
            PlayerData newP = new PlayerData (msgInfo.sender, i);
            players.Add (newP);
        }



        SendFullPlayerList ();

    }

    public void OnConnectedToServer ()
    {

        // TODO read no. of local players!

        // Send local client info to server
        // // local player count
        networkView.RPC ("NewClient", RPCMode.Server, localPlayerCount);
        //networkView.RPC ("NewClient", RPCMode.AllBuffered, localPlayerCount);

    }

    [RPC]
    public void AddPlayer (NetworkPlayer newPlayer, int localId)
    {
        players.Add (new PlayerData (newPlayer, localId));
    }

    public void SendFullPlayerList ()
    {
        // TODO: DO NOT DELETE; CHECK FOR DUPLICATES!

        networkView.RPC ("DoDeletePlayerList", RPCMode.Others);

        foreach (PlayerData pData in players) {
            networkView.RPC ("AddPlayer", RPCMode.Others, pData.networkPlayer, pData.localPlayerId);
        }
    }

    [RPC]
    public void DoDeletePlayerList ()
    {
        players.Clear ();
    }

    public void SendPlayerList (NetworkPlayer receiver)
    {

        foreach (PlayerData pData in players) {
            networkView.RPC ("AddPlayer", receiver, pData.networkPlayer, pData.localPlayerId);
        }

    }

    public void SendPlayerList (IList<PlayerData> playerList)
    {
        
        foreach (PlayerData pData in playerList) {
            networkView.RPC ("AddPlayer", RPCMode.All, pData.networkPlayer, pData.localPlayerId);
        }
        
    }

    public void OnPlayerDisconnected (NetworkPlayer netPlayer)
    {
        Network.RemoveRPCs (netPlayer);
        networkView.RPC ("RemovePlayer", RPCMode.All, netPlayer);


    }

    [RPC]
    public void RemovePlayer (NetworkPlayer netPlayer)
    {

        IList<PlayerData> idsToRemove = new List<PlayerData> ();
        
        for (int i = 0; i < players.Count; i ++) {
            if (players [i].networkPlayer == netPlayer) {
                idsToRemove.Add (players [i]);
            }
        }
        
        for (int i = 0; i < idsToRemove.Count; i ++) {
            players.Remove (idsToRemove [i]);
        }

    }

    public static PlayerData GetPlayerData (NetworkPlayer netPlayer, int localPlayer)
    {
        foreach (PlayerData pData in singleton.players) {
            if (pData.networkPlayer == netPlayer && pData.localPlayerId == localPlayer) {
                return pData;
            }
        }

        Debug.LogError ("GOT ACTION WITH NO MATCHING PLAYER DATA");
        return null;
    }

    public static void StartGame ()
    {

        switch (singleton.gameMode) {
        case 0:
            {
                singleton.networkView.RPC ("LoadLevel", RPCMode.AllBuffered, "controllerTestScene", singleton.lastLevelPrefix + 1);

                break;
            }
        case 1:
            {
                singleton.networkView.RPC ("LoadLevel", RPCMode.AllBuffered, "GameTest", singleton.lastLevelPrefix + 1);
                break;
            }
        case 2:
            {
                singleton.networkView.RPC ("LoadLevel", RPCMode.AllBuffered, "SceneControllerTest", singleton.lastLevelPrefix + 1);
                break;
            }
        }


    }

    [RPC]
    public IEnumerator LoadLevel (string level, int levelPrefix)
    {
        lastLevelPrefix = levelPrefix;
        
        // There is no reason to send any more data over the network on the default channel,
        // because we are about to load the level, thus all those objects will get deleted anyway
        Network.SetSendingEnabled (0, false);    
        
        // We need to stop receiving because first the level must be loaded first.
        // Once the level is loaded, rpc's and other state update attached to objects in the level are allowed to fire
        Network.isMessageQueueRunning = false;
        
        // All network views loaded from a level will get a prefix into their NetworkViewID.
        // This will prevent old updates from clients leaking into a newly created scene.
        Network.SetLevelPrefix (levelPrefix);
        Application.LoadLevel (level);
        yield return new WaitForEndOfFrame ();
        yield return new WaitForEndOfFrame ();

        // Allow receiving data again
        Network.isMessageQueueRunning = true;
        // Now the level has been loaded and we can start sending out data to clients
        Network.SetSendingEnabled (0, true);

    }



    public static void ConnectToHost (string ip, int port)
    {

        Network.Connect (ip, port);

    }

    void OnDisconnectedFromServer ()
    {
        if (Network.isClient) {
            Debug.LogError ("Disconnected from Server.");
        }
        //networkView.RPC ("LoadLevel", RPCMode.AllBuffered, level, lastLevelPrefix + 1);
        Destroy (gameObject);
        Application.LoadLevel ("SceneMainMenu");
    }

    void OnFailedToConnect (NetworkConnectionError error)
    {
        Debug.LogError ("Could not connect to server: " + error);
        //networkView.RPC ("LoadLevel", RPCMode.AllBuffered, level, lastLevelPrefix + 1);
        Destroy (gameObject);
        Application.LoadLevel ("SceneMainMenu");
    }

    public void OnLevelWasLoaded (int levelIndex)
    {
        if (Application.loadedLevelName.CompareTo ("characterScene") == 0 ||
            Application.loadedLevelName.CompareTo ("controllerTestScene") == 0 ||
            Application.loadedLevelName.CompareTo ("GameTest") == 0) {
            Character newCharacter = Network.Instantiate (singleton.characterPrefab, Vector3.zero + Vector3.up * 1.0f, Quaternion.identity, 0) as Character;
            
            foreach (PlayerData pData in singleton.players) {
                pData.character = newCharacter;
            }

            SendMessage ("RealGameLoaded");
            SendMessage ("StartGameObjectiveWatch");

        }

    }

    public static void EndGameConditionMet (int endCondition)
    {
        if (!Network.isServer)
            Debug.LogError ("NOT SERVER AND CHECKING WIN CONDITIONS!");

        singleton.networkView.RPC ("ApplyEndOfGame", RPCMode.All, endCondition);
    }

    [RPC]
    public void ApplyEndOfGame (int endCondition)
    {
        EndScreen endScreen = GetComponent<EndScreen> ();

        endScreen.SetActive (endCondition);
    }
}
