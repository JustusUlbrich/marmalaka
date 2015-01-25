using UnityEngine;
using System.Collections;
using System.Text;

public class DebugUtility
{

    public static string BuildActionString (PlayerAction pAction, bool shortVersion = false)
    {
        StringBuilder strBuilder = new StringBuilder ();

        AppendActionString (strBuilder, pAction, shortVersion);

        return strBuilder.ToString ();
    }

    public static StringBuilder AppendActionString (StringBuilder strBuilder, PlayerAction pAction, bool shortVersion = false)
    {
        if (shortVersion) {
            strBuilder.Append (":: ");
            strBuilder.Append (pAction.netPlayer);
            strBuilder.Append (" | ");
            strBuilder.Append (pAction.localPlayerId);
            strBuilder.Append (" | ");
            strBuilder.Append (pAction.actionType);
            strBuilder.Append (" | ");
            strBuilder.Append (pAction.timerData.turnNumber);
            strBuilder.Append (" | ");
            strBuilder.Append (pAction.timerData.moveNumber);
            strBuilder.Append (" | ");
            strBuilder.Append (pAction.timerData.timeInTurn);
            strBuilder.AppendLine ();
        } else {
            strBuilder.Append ("NetId: ");
            strBuilder.Append (pAction.netPlayer);
            strBuilder.Append (" | LocId: ");
            strBuilder.Append (pAction.localPlayerId);
            strBuilder.Append (" | ActType: ");
            strBuilder.Append (pAction.actionType);
            strBuilder.Append (" | Turn: ");
            strBuilder.Append (pAction.timerData.turnNumber);
            strBuilder.Append (" | Move: ");
            strBuilder.Append (pAction.timerData.moveNumber);
            strBuilder.Append (" | TurnDelta: ");
            strBuilder.Append (pAction.timerData.timeInTurn);
            strBuilder.AppendLine ();
        }
        return strBuilder;
    }
    
}
