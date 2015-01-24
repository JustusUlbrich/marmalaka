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

		public void StartServer (int port)
		{

				Network.InitializeServer (5, port, false);

				Application.LoadLevel ("SceneLobby");

				Debug.Log ("Loaded Lobby");

		}

		public void ConnectToHost (string ip, int port)
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
				Debug.LogError ("Disconnected from Server.");
				Application.LoadLevel ("SceneMainMenu");
		}

		void OnFailedToConnect (NetworkConnectionError error)
		{
				Debug.LogError ("Could not connect to server: " + error);
				Application.LoadLevel ("SceneMainMenu");
		}
}
