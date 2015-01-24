using UnityEngine;
using System.Collections;


public class GameManager : MonoBehaviour
{
    public const string GAME_NAME = "Marmalaka";
    public static GameManager singleton;

    private int gameMode = -1;
    private int lastLevelPrefix = 0;

    void Awake ()
    {
        if (singleton == null) {
            singleton = this;
        } else {
            Destroy (gameObject);
            return;
        }

        DontDestroyOnLoad (gameObject);
    }

    public static void SetGameMode (int newMode)
    {
        singleton.gameMode = newMode;
    }

    public static void StartServer (int port)
    {

        Network.InitializeServer (5, port, false);

        singleton.networkView.RPC ("LoadLevel", RPCMode.AllBuffered, "SceneLobby", singleton.lastLevelPrefix + 1);

        Debug.Log ("Loaded Lobby");

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
                singleton.networkView.RPC ("LoadLevel", RPCMode.AllBuffered, "controllerTestScene", singleton.lastLevelPrefix + 1);
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
    }

    void OnFailedToConnect (NetworkConnectionError error)
    {
        Debug.LogError ("Could not connect to server: " + error);
        //networkView.RPC ("LoadLevel", RPCMode.AllBuffered, level, lastLevelPrefix + 1);
    }
}
