using UnityEngine;
using System.Collections;

public class GameObjectiveManager : MonoBehaviour
{

    public float gameDuration = 10;

    private float startTime;
    private float leftOverTime;

    public void Awake ()
    {
        startTime = Time.time;
        leftOverTime = gameDuration;

        if (!Network.isServer) {
            Destroy (this);
        }
    }

    public void OnGUI ()
    {
        GUILayout.BeginHorizontal ();

        GUILayout.FlexibleSpace ();



        GUILayout.Label ("Time remaining: " + leftOverTime);

        GUILayout.EndHorizontal ();
    }

    public void Update ()
    {

        float leftOverTime = gameDuration - (Time.time - startTime);

        if (leftOverTime < 0) {

            GameManager.EndGameConditionMet (0);

            enabled = false;
        }

        // CHECK FOR VICTORY
        //if()
        /*{
            GameManager.EndGameConditionMet (1);

            enabled = false;
        }*/
    }

}
