using UnityEngine;
using System.Collections;

public enum EndCondition
{
    TimeUp = 0,
    TargetReached = 1,
    Died = 2
}
;

public class GameObjectiveManager : MonoBehaviour
{

    public float gameDuration = 10;

    private float startTime;
    private float leftOverTime;
    private bool started = false;

    public void Awake ()
    {

    }

    public void RealGameLoaded ()
    {
        if (!Network.isServer) {
            Destroy (this);
        }
    }

    public void StartGameObjectiveWatch ()
    {
        startTime = Time.time;
        leftOverTime = gameDuration;
        started = true;
    }

    public void OnGUI ()
    {
        if (started) {
            GUILayout.BeginHorizontal ();

            GUILayout.FlexibleSpace ();



            GUILayout.Label ("Time remaining: " + leftOverTime);

            GUILayout.EndHorizontal ();
        }
    }

    public void Update ()
    {
        if (started) {
            leftOverTime = gameDuration - (Time.time - startTime);

            if (leftOverTime < 0) {

                GameManager.EndGameConditionMet (0);

                enabled = false;
            }
        }

        // CHECK FOR VICTORY
        //if()
        /*{
            GameManager.EndGameConditionMet (1);

            enabled = false;
        }*/
    }

}
