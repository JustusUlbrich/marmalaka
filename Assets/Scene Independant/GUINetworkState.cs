using UnityEngine;
using System.Collections;

public class GUINetworkState : MonoBehaviour
{

    private bool showStatus = true;

    void Awake ()
    {
        DontDestroyOnLoad (gameObject);
    }

    void OnGUI ()
    {

        showStatus = GUI.Toggle (new Rect (0, Screen.height - 25, 10, 35), showStatus, "!");

        if (showStatus) {

            GUI.Label (new Rect (15, Screen.height - 25, Screen.width, 20), GameStatusText () + " | " + Application.loadedLevelName);
        }

    }

    string GameStatusText ()
    {
        string status = "";
        
        if (Network.isClient)
            status += "Client | ";
        else if (Network.isServer)
            status += "Server | ";
        else
            status += "Disconnected | ";
        
        status += Network.connections.Length + " Connections";
        
        return status;
    }
}
