using UnityEngine;
using System.Collections;

public class GUIConnect : MonoBehaviour
{

    private Rect modeWindowRect = new Rect (Screen.width / 2 - 75, Screen.height / 2 - 100, 150, 100);
    private Rect netWindowRect = new Rect (Screen.width / 2 - 75, Screen.height / 2, 150, 100);
    private Rect startWindowRec = new Rect (Screen.width / 2 - 75, Screen.height / 2 + 100, 150, 50);

    public string ip = "127.0.0.1";
    public string portString = "4444";

    public int gameMode = 0;
    private string[] gameModeStrings = {"Battle Arena", "Duck, Duck, Noob!", "Training Ground"};

    private bool serverMode = true;

    void OnGUI ()
    {

        modeWindowRect = GUI.Window (0, modeWindowRect, DoGameModeWindow, "Game Mode");
        netWindowRect = GUI.Window (1, netWindowRect, DoNetWindow, "Networking Mode");
        startWindowRec = GUI.Window (2, startWindowRec, DoStartWindow, "Ready?");
    }

    public void DoGameModeWindow (int winId)
    {
        GUILayout.BeginVertical ();

        for (int i = 0; i < 3; i ++) {

            if (gameMode == i) {

                GUILayout.BeginHorizontal ();
                GUILayout.FlexibleSpace ();
                GUILayout.Label (gameModeStrings [i]);
                GUILayout.FlexibleSpace ();
                GUILayout.EndHorizontal ();

            } else if (GUILayout.Button (gameModeStrings [i])) {
                gameMode = i;
            }
        }

        GUILayout.EndVertical ();
    }

    public void DoNetWindow (int winId)
    {
        GUILayout.BeginHorizontal ();
        GUILayout.Label ("IP:");
        ip = GUILayout.TextField (ip);
        GUILayout.EndHorizontal ();

        GUILayout.BeginHorizontal ();
        GUILayout.Label ("PortNo:");
        portString = GUILayout.TextField (portString);
        GUILayout.EndHorizontal ();

        GUILayout.BeginHorizontal ();

        serverMode = GUILayout.Toggle (serverMode, "Server");
        serverMode = !GUILayout.Toggle (!serverMode, "Client");

        GUILayout.EndHorizontal ();
    }

    public void DoStartWindow (int winId)
    {
        if (GUILayout.Button ("Start")) {

            GameManager.SetGameMode (gameMode);

            int portNo = int.Parse (portString);

            if (serverMode) {
                GameManager.StartServer (portNo);
            } else {
                GameManager.ConnectToHost (ip, portNo);
            }

        }
    }
        
}
