using UnityEngine;
using System.Collections;


public class GameManager : MonoBehaviour
{
    public const string GAME_NAME = "Marmalaka";
    public static GameManager singleton;

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

    public static void StartServer (int port)
    {

        Network.InitializeServer (5, port, false);

        Application.LoadLevel ("SceneLobby");

        Debug.Log ("Loaded Lobby");

    }


    public static void StartGame ()
    {

        singleton.networkView.RPC ("InvokeLevelLoad", RPCMode.AllBuffered, "controllerTestScene");

    }

    [RPC]
    void InvokeLevelLoad (string sceneName)
    {
        Application.LoadLevel (sceneName);
    }

    public static void ConnectToHost (string ip, int port)
    {

        Network.Connect (ip, port);

        Application.LoadLevel ("SceneConnecting");

        Debug.Log ("Loaded Connecting");

    }

    void OnConnectedToServer ()
    {
        Application.LoadLevel ("SceneLobby");
        Debug.Log ("Loaded Lobby");
    }

    void OnDisconnectedFromServer ()
    {
        if (Network.isClient) {
            Debug.LogError ("Disconnected from Server.");
        }
        Application.LoadLevel ("SceneMainMenu");
    }

    void OnFailedToConnect (NetworkConnectionError error)
    {
        Debug.LogError ("Could not connect to server: " + error);
        Application.LoadLevel ("SceneMainMenu");
    }
}
