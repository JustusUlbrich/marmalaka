using UnityEngine;
using System.Collections;

public class GUILobby : MonoBehaviour
{

    void OnGUI ()
    {
        if (Network.isServer) {
            if (GUI.Button (new Rect (Screen.width - 100, Screen.height - 25, 100, 20), "Start Game")) {

                GameManager.StartGame ();

            }
        }
    }
}
