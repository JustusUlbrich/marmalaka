using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;


public class ActionAggregator : MonoBehaviour
{
    public static ActionAggregator singleton;

    IList<PlayerAction> actionList;

    // Used as a parameter for the invoked method QueueActions
    private TurnTimerData turnToQueue;
    // how long to wait for packets of old turn
    public float addDelayTime = 2.0f;

    public void Awake ()
    {
        if (singleton == null) {
            singleton = this;
        } else {
            Destroy (gameObject);
        }

        actionList = new List<PlayerAction> ();
    }

    public static IList<PlayerAction> GetActionList ()
    {
        return singleton.actionList;
    }

    public static void InsertAction (PlayerAction action)
    {
        int currentIndex = singleton.actionList.Count;

        bool added = false;

        if (currentIndex == 0) {
            singleton.actionList.Add (action);
            added = true;
        }

        while (currentIndex > 0 && !added) {
            PlayerAction compAction = singleton.actionList [currentIndex - 1];

            if (compAction.timerData.turnNumber > action.timerData.turnNumber ||
                compAction.timerData.turnNumber == action.timerData.turnNumber &&
                compAction.timerData.timeInTurn > action.timerData.timeInTurn) {

                currentIndex --;

            } else {

                singleton.actionList.Insert (currentIndex, action);
                added = true;

            }
        }

        if (!added)
            singleton.actionList.Insert (0, action);

    }
       
    public void QueueActions ()
    {
        IList<PlayerAction> turnActionList = new List<PlayerAction> ();
        
        foreach (PlayerAction pAction in actionList) {
            if (pAction.timerData.turnNumber == turnToQueue.turnNumber) {
                turnActionList.Add (pAction);
            }
        }
        
        foreach (PlayerAction pAction in turnActionList) {
            actionList.Remove (pAction);
        }

        ActionExecuter.QueueActions (turnActionList);
    }

    public IEnumerator DelayedQueueActions ()
    {
        yield return new WaitForSeconds (addDelayTime);


        QueueActions ();

    }

    public void InputTurnOver (TurnTimerData timerData)
    {
        turnToQueue = new TurnTimerData (timerData);

        //QueueActions ();
        StartCoroutine (DelayedQueueActions ());
    }

    [RPC]
    public void AddAction (NetworkPlayer netPlayer, int localPlayerId, int actionType, int turnNo, int moveNo, float deltaInSlot)
    {
        PlayerAction action = new PlayerAction (netPlayer, localPlayerId, actionType, turnNo, moveNo, deltaInSlot);

        //Debug.Log ("RECEIVED: " + DebugUtility.AppendActionString (new StringBuilder (), action).ToString ());

        InsertAction (action);
    }
    
}
