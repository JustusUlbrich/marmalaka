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

    public NetworkPlayer networkPlayer;
    public int localPlayerId;
}

public class GameManager : MonoBehaviour
{
    public const string GAME_NAME = "Marmalaka";
    public static GameManager singleton;

    private int gameMode = -1;
    private int lastLevelPrefix = 0;

    public int localPlayerCount = 3;

    public IList<PlayerData> players;

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

    public static void StartServer (int port)
    {

        Network.InitializeServer (5, port, false);

        for (int i = 0; i < singleton.localPlayerCount; i ++) {
            PlayerData newP = new PlayerData (Network.player, i);
            //newPlayers.Add (newP);
            singleton.players.Add (newP);
        }

        singleton.networkView.RPC ("LoadLevel", RPCMode.AllBuffered, "SceneLobby", singleton.lastLevelPrefix + 1);

        Debug.Log ("Loaded Lobby");

    }

    public void OnPlayerConnected (NetworkPlayer player)
    {
    }

    [RPC]
    public void NewClient (int newClientPlayerCount, NetworkMessageInfo msgInfo)
    {
        if (!Network.isServer)
            Debug.LogError ("Received Server Message as Client!!");

        //SendPlayerList (msgInfo.sender);

        //IList<PlayerData> newPlayers = new List<PlayerData> ();

        for (int i = 0; i < newClientPlayerCount; i ++) {
            PlayerData newP = new PlayerData (msgInfo.sender, i);
            //newPlayers.Add (newP);
            players.Add (newP);
        }


        //SendPlayerList (newPlayers);

        SendFullPlayerList ();

    }

    public void OnConnectedToServer ()
    {

        // TODO read no. of local players!

        // Send local client info to server
        // // local player count
        networkView.RPC ("NewClient", RPCMode.Server, localPlayerCount);
    }

    [RPC]
    public void AddPlayer (NetworkPlayer newPlayer, int localId)
    {
        players.Add (new PlayerData (newPlayer, localId));
    }

    public void SendFullPlayerList ()
    {
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
                singleton.networkView.RPC ("LoadLevel", RPCMode.AllBuffered, "characterScene", singleton.lastLevelPrefix + 1);
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
}
