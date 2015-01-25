using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;

public class ActionExecuter : MonoBehaviour
{
    public static ActionExecuter singleton;

    public IList<PlayerAction> turnMoves;

    public void Awake ()
    {
        if (singleton == null) {
            singleton = this;
        } else {
            Destroy (gameObject);
        }

        turnMoves = new List<PlayerAction> ();

    }

    public void PlayTurnStart (TurnTimerData timerData)
    {
        
    }

    public void PlayTurnOver (TurnTimerData timerData)
    {
        for (int i = turnMoves.Count-1; i >= 0; i --) {
            if (turnMoves [i].timerData.turnNumber <= timerData.turnNumber)
                turnMoves.RemoveAt (i);
        }
    }



    public void PlayMoveStart (TurnTimerData timerData)
    {

        if (turnMoves.Count > 0) {
            PlayerAction currentAction = turnMoves [0];

            if (currentAction.timerData.turnNumber < timerData.turnNumber) {
                turnMoves.RemoveAt (0);
                PlayMoveStart (timerData);
            } else if (currentAction.timerData.turnNumber > timerData.turnNumber) {
                // Only from next turn, do no ting
            } else {
                // Execute!
                turnMoves.RemoveAt (0);

                PlayerData pData = GameManager.GetPlayerData (currentAction.netPlayer, currentAction.localPlayerId);
                
                if (pData.character == null) {
                    Debug.LogError ("CHARACTER REFERENCE NULL");
                } else {
                    pData.character.SendMessage ("doAction", currentAction, SendMessageOptions.RequireReceiver);

                    ActionHistory.AppendToHistory (currentAction);
                }
            }

        }

        /*IList<PlayerAction> executedActions = new List<PlayerAction> ();

        foreach (PlayerAction pAction in turnMoves) {

            if (pAction.timerData.turnNumber - timerData.turnNumber > 1) {
                Debug.LogError ("ERROR TURN NO IN LIST AND METHOD CALL MORE THAN 1 APART!");
            }

            if (pAction.timerData.turnNumber == timerData.turnNumber) {


                PlayerData pData = GameManager.GetPlayerData (pAction.netPlayer, pAction.localPlayerId);

                if (pData.character == null)
                    Debug.LogError ("CHARACTER REFERENCE NULL");
                else {
                    pData.character.SendMessage ("doAction", pAction, SendMessageOptions.RequireReceiver);

                    executedActions.Add (pAction);
                    ActionHistory.AppendToHistory (pAction);
                }

            }
        }
        
        foreach (PlayerAction pAction in executedActions) {
            turnMoves.Remove (pAction);
        }*/


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
