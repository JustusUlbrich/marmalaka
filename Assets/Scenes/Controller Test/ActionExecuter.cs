﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;

public class ActionExecuter : MonoBehaviour
{
    public static ActionExecuter singleton;

    public IList<PlayerAction> turnMoves;

    // TODO No Need for this shyte
    public Transform ActionVisualizerEffectShitRemoveMeWhenItsDone;

    public void Awake ()
    {
        if (singleton == null) {
            singleton = this;
        } else {
            Destroy (gameObject);
        }

        turnMoves = new List<PlayerAction> ();

    }

    public void TurnOver (TurnTimerData timerData)
    {
        // First move has to be called here, to trigger move to start at beginning of move slot
        // TODO RENAME MoveOver TO MoveStart
        //MoveOver (timerData);
    }

    public void PlayMoveStart (TurnTimerData timerData)
    {
        IList<PlayerAction> executedActions = new List<PlayerAction> ();

        foreach (PlayerAction pAction in turnMoves) {

            if (pAction.timerData.turnNumber - timerData.turnNumber > 1) {
                Debug.LogError ("ERROR TURN NO IN LIST AND METHOD CALL MORE THAN 1 APART!");
            }

            if (pAction.timerData.moveNumber == timerData.moveNumber) {

                // TODO Get Character Object from GameManager and SendMessage on it
                Instantiate (ActionVisualizerEffectShitRemoveMeWhenItsDone, new Vector3 (timerData.turnNumber * 5, 0, timerData.moveNumber * 5), Quaternion.identity);

                executedActions.Add (pAction);
                ActionHistory.AppendToHistory (pAction);

            }
        }
        
        foreach (PlayerAction pAction in executedActions) {
            turnMoves.Remove (pAction);
        }


    }
    
    public static void QueueActions (IList<PlayerAction> newTurnActions)
    {
        /*if (singleton.turnMoves.Count != 0) {
            Debug.LogError ("ACTION QUEUE NOT EMPTY ON NEW MOVE!");

            foreach (PlayerAction pAction in singleton.turnMoves)
                Debug.LogError ("NEVER EXECUTED: " + DebugUtility.AppendActionString (new StringBuilder (), pAction).ToString ());
        }*/

        foreach (PlayerAction pAction in newTurnActions) {
            singleton.turnMoves.Add (pAction);

        }


    }

    public static IList<PlayerAction> GetPlayingActionList ()
    {
        return singleton.turnMoves;
    }


}