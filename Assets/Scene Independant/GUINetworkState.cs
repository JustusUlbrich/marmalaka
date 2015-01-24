using UnityEngine;
using System.Collections;
using System;
using System.Text;

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


        GUILayout.BeginArea (new Rect (20, 20, 120, 600));
        GUILayout.BeginVertical ();
        GUILayout.BeginHorizontal ();
        GUILayout.FlexibleSpace ();
        GUILayout.Label ("Player List");
        GUILayout.FlexibleSpace ();
        GUILayout.EndHorizontal ();
        GUILayout.TextArea (PopulatePlayerListString ());
        GUILayout.EndVertical ();
        GUILayout.EndArea ();

    }

    string PopulatePlayerListString ()
    {
//        string pList = "";
        StringBuilder pList = new StringBuilder ();

        for (int i = 0; i < GameManager.singleton.players.Count; i++) {
            PlayerData curPlayer = GameManager.singleton.players [i];

            pList.Append (curPlayer.networkPlayer);
            pList.Append (" | ");
            pList.Append (curPlayer.localPlayerId);
            pList.AppendLine ();
        }

        return pList.ToString ();
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
