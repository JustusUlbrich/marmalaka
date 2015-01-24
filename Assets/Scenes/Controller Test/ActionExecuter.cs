using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ActionExecuter : MonoBehaviour
{
    public static ActionExecuter singleton;

    public int turnPlaying = -1;
    public IList<PlayerAction> turnMoves;

    public void Awake ()
    {
        if (singleton == null) {
            singleton = this;
        } else {
            Destroy (gameObject);
        }

        turnMoves = new List<PlayerAction> ();
        turnPlaying = -1;

    }

    public void TurnOver (int turnNo)
    {
        turnPlaying = turnNo;
    }
    
    public static void QueueActions (IList<PlayerAction> newTurnActions)
    {
        if (singleton.turnMoves.Count != 0) {
            Debug.LogError ("ACTION QUEUE NOT EMPTY ON NEW MOVE!");
        }

        singleton.turnMoves = newTurnActions;

    }

    public static IList<PlayerAction> GetPlayingActionList ()
    {
        return singleton.turnMoves;
    }


}
