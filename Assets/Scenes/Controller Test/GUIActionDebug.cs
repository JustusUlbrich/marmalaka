using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;

public class GUIActionDebug : MonoBehaviour
{

    private Rect plannedArea = new Rect (160, 20, 200, 400);
    private Rect playingArea = new Rect (380, 20, 200, 400);
    private Rect historyArea = new Rect (600, 20, 200, 400);


    Vector2 plannedScrollPos = Vector2.zero;
    Vector2 playingScrollPos = Vector2.zero;
    Vector2 historyScrollPos = Vector2.zero;

    
    public void OnGUI ()
    {

        DrawActionList (plannedArea, "Planned", ref plannedScrollPos, ActionAggregator.GetActionList ());
        DrawActionList (playingArea, "Queued", ref playingScrollPos, ActionExecuter.GetPlayingActionList ());
        DrawActionList (historyArea, "Played", ref historyScrollPos, ActionHistory.GetHistory ());


        /*
        GUILayout.BeginArea (plannedArea);
        GUILayout.BeginVertical ();
        GUILayout.BeginHorizontal ();
        GUILayout.FlexibleSpace ();
        GUILayout.Label ("Planned Actions");
        GUILayout.FlexibleSpace ();
        GUILayout.EndHorizontal ();
        plannedScrollPos = GUILayout.BeginScrollView (plannedScrollPos);
        GUILayout.TextArea (BuildPlannedActionListString (actionList));
        GUILayout.EndScrollView ();
        GUILayout.EndVertical ();
        GUILayout.EndArea ();

        GUILayout.BeginArea (playingArea);
        GUILayout.BeginVertical ();
        GUILayout.BeginHorizontal ();
        GUILayout.FlexibleSpace ();
        GUILayout.Label ("Playing Actions");
        GUILayout.FlexibleSpace ();
        GUILayout.EndHorizontal ();
        playingScrollPos = GUILayout.BeginScrollView (playingScrollPos);
        IList<PlayerAction> playingActionList = ActionExecuter.GetPlayingActionList ();

        GUILayout.TextArea (BuildPlannedActionListString (playingActionList));
        GUILayout.EndScrollView ();

        
        GUILayout.EndVertical ();
        GUILayout.EndArea ();*/

    }

    public void DrawActionList (Rect area, string title, ref Vector2 scrollBarState, IList<PlayerAction> actionList)
    {

        GUILayout.BeginArea (area);
        GUILayout.BeginVertical ();
        GUILayout.BeginHorizontal ();
        GUILayout.FlexibleSpace ();

        GUILayout.Label (title);

        GUILayout.FlexibleSpace ();
        GUILayout.EndHorizontal ();
        scrollBarState = GUILayout.BeginScrollView (scrollBarState);
        GUILayout.TextArea (BuildPlannedActionListString (actionList));
        GUILayout.EndScrollView ();
        GUILayout.EndVertical ();
        GUILayout.EndArea ();

    }

    public string BuildPlannedActionListString (IList<PlayerAction> actionList)
    {

        StringBuilder actionString = new StringBuilder ();

        foreach (PlayerAction pAction in actionList) {
            DebugUtility.AppendActionString (actionString, pAction, true);
        }

        return actionString.ToString ();

    }
}
