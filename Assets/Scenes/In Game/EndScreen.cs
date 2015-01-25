using UnityEngine;
using System.Collections;

public class EndScreen : MonoBehaviour
{


    public float showDuration = 5.0f;
    private float timeLeft;

    // 0 = back to main menu
    // 1 = new game 
    // 2 = same players / lobby again
    public int endGameSolution = 0;

    private bool show = false;
    private float endTime;
    private int endCondition;
    private int turnCount;

    public void SetActive (int endConditionParam)
    {
        endTime = Time.time;
        show = true;
        endCondition = endConditionParam;
        timeLeft = showDuration;

        TurnTimer.singleton.enabled = false;
        turnCount = TurnTimer.getPlayTimerData ().turnNumber;

        Network.Disconnect ();
    }

    public void Update ()
    {
        if (show) {
            timeLeft -= Time.deltaTime;

            if (timeLeft < 0) {
                Destroy (gameObject);
                Application.LoadLevel ("SceneMainMenu");
            }
        }
    }

    public void OnGUI ()
    {
        if (show)
            GUI.ModalWindow (0, new Rect (Screen.width / 2 - 100, Screen.height / 2 - 40, 200, 80), DoEndGameWindow, "Game Over!");
    }

    public void DoEndGameWindow (int windowId)
    {

        GUILayout.BeginHorizontal ();
        GUILayout.FlexibleSpace ();
        GUILayout.BeginVertical ();
        GUILayout.FlexibleSpace ();

        switch (endCondition) {
        case 0:
            {
                // Time UP

                GUILayout.Label ("Time Over! You Lost! " + turnCount + " turns played!");
                GUILayout.Label ("Going Back to Main Menu in " + timeLeft + " seconds!");

                break;
            }
        case 1:
            {
                // Target reached
                GUILayout.Label ("You reached the target in time!");
                GUILayout.Label ("Going Back to Main Menu in " + timeLeft + " seconds!");

                break;
            }
        }

        GUILayout.FlexibleSpace ();
        GUILayout.EndVertical ();
        GUILayout.FlexibleSpace ();
        GUILayout.EndHorizontal ();
    }


}
