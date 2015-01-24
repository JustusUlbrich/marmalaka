using UnityEngine;
using System.Collections;
using System.Text;

public class DebugUtility
{

    public static string BuildPlayerActionString (PlayerAction pAction, bool shortVersion = false)
    {
        StringBuilder strBuilder = new StringBuilder ();

        if (shortVersion) {
            strBuilder.Append (":: ");
            strBuilder.Append (pAction.netPlayer);
            strBuilder.Append (" | ");
            strBuilder.Append (pAction.localPlayerId);
            strBuilder.Append (" | ");
            strBuilder.Append (pAction.actionType);
            strBuilder.Append (" | ");
            strBuilder.Append (pAction.timerData.slotSequenceNo);
            strBuilder.Append (" | ");
            strBuilder.Append (pAction.timerData.timeInSlot);
            strBuilder.AppendLine ();
        } else {
            strBuilder.Append ("NetId: ");
            strBuilder.Append (pAction.netPlayer);
            strBuilder.Append (" | LocId: ");
            strBuilder.Append (pAction.localPlayerId);
            strBuilder.Append (" | ActType: ");
            strBuilder.Append (pAction.actionType);
            strBuilder.Append (" | Turn: ");
            strBuilder.Append (pAction.timerData.slotSequenceNo);
            strBuilder.Append (" | TurnDelta: ");
            strBuilder.Append (pAction.timerData.timeInSlot);
            strBuilder.AppendLine ();
        }
        return strBuilder.ToString ();
    }

    public static StringBuilder AppendPlayerActionString (StringBuilder strBuilder, PlayerAction pAction, bool shortVersion = false)
    {
        if (shortVersion) {
            strBuilder.Append (":: ");
            strBuilder.Append (pAction.netPlayer);
            strBuilder.Append (" | ");
            strBuilder.Append (pAction.localPlayerId);
            strBuilder.Append (" | ");
            strBuilder.Append (pAction.actionType);
            strBuilder.Append (" | ");
            strBuilder.Append (pAction.timerData.slotSequenceNo);
            strBuilder.Append (" | ");
            strBuilder.Append (pAction.timerData.timeInSlot);
            strBuilder.AppendLine ();
        } else {
            strBuilder.Append ("NetId: ");
            strBuilder.Append (pAction.netPlayer);
            strBuilder.Append (" | LocId: ");
            strBuilder.Append (pAction.localPlayerId);
            strBuilder.Append (" | ActType: ");
            strBuilder.Append (pAction.actionType);
            strBuilder.Append (" | Turn: ");
            strBuilder.Append (pAction.timerData.slotSequenceNo);
            strBuilder.Append (" | TurnDelta: ");
            strBuilder.Append (pAction.timerData.timeInSlot);
            strBuilder.AppendLine ();
        }
        return strBuilder;
    }
    
}
