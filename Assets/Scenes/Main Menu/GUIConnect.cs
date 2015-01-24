using UnityEngine;
using System.Collections;

public class GUIConnect : MonoBehaviour
{

		public string ip = "127.0.0.1";
		public string portString = "4444";

		void OnGUI ()
		{

				GUILayout.BeginVertical ();

				GUILayout.Space (100);

				if (!Network.isServer && !Network.isClient) {
						
						GUILayout.Label (" ===   Server   === ");
						if (GUILayout.Button ("Start Server")) {
			
								GameManager.singleton.StartServer (4444);
						}

						

						GUILayout.Space (20);

						GUILayout.Label (" ===   Client   === ");

						GUILayout.BeginHorizontal ();

						GUILayout.Label ("IP:");
						ip = GUILayout.TextField (ip);

						GUILayout.EndHorizontal ();

						GUILayout.BeginHorizontal ();

						portString = GUILayout.TextField (portString);

						GUILayout.EndHorizontal ();

						if (GUILayout.Button ("Connect")) {
								int port = int.Parse (portString);

								GameManager.singleton.ConnectToHost (ip, port);
						}

				}

				GUILayout.EndVertical ();
		}

		
}
