using UnityEngine;
using System.Collections;

public class GUILobby : MonoBehaviour
{

		void OnGUI ()
		{
				if (Network.isServer) {
						if (GUI.Button (new Rect (50, 50, 100, 20), "Start Game")) {

								GameManager.singleton.StartGame ();

						}
				}
		}
}
