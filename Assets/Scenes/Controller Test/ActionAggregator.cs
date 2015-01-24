using UnityEngine;
using System.Collections;

public class ActionAggregator : MonoBehaviour
{

    [RPC]
    public void AddAction (int localPlayerId, int actionType, int timeSlotNo, float deltaInSlot)
    {

        if (!Network.isServer) {
            Debug.LogError ("Server function called on Client!!");
        }

        PlayerAction newPlayerAction = new PlayerAction (localPlayerId, actionType, timeSlotNo, deltaInSlot);

        Debug.Log ("RECEIVED BY SERVER: Player Action: LPid: " + newPlayerAction.localPlayerId + " | Type " + newPlayerAction.actionType.ToString () + " |  SlotNo" + newPlayerAction.timerData.slotSequenceNo + "  | Time:" + newPlayerAction.timerData.timeInSlot);

        
    }

    public void AddActionServerLocal (PlayerAction newPlayerAction)
    {
        
        if (!Network.isServer) {
            Debug.LogError ("Server function called on Client!!");
        }
        
        Debug.Log ("RECEIVED BY SERVER: Player Action: LPid: " + newPlayerAction.localPlayerId + " | Type " + newPlayerAction.actionType.ToString () + " |  SlotNo" + newPlayerAction.timerData.slotSequenceNo + "  | Time:" + newPlayerAction.timerData.timeInSlot);
        
        
    }
    
}
