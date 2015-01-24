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

    public static void QueueActions (IList<PlayerAction> newTurnMoves)
    {

        if (newTurnMoves.Count != 0) {
            //Debug.LogError ("Still have Actions to Execute");
        }

        singleton.turnMoves = newTurnMoves;

    }

    public static IList<PlayerAction> GetPlayingActionList ()
    {
        return singleton.turnMoves;
    }


}
