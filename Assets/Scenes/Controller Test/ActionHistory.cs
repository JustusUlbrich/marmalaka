using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ActionHistory : MonoBehaviour
{

    public static ActionHistory singleton;

    private IList<PlayerAction> actionHistoryList;

    public void Awake ()
    {
        if (singleton == null) {
            singleton = this;
        } else {
            Destroy (gameObject);
        }

        actionHistoryList = new List<PlayerAction> ();
    }

    public static void AppendToHistory (PlayerAction handledAction)
    {
        singleton.actionHistoryList.Add (handledAction);
    }

    public static IList<PlayerAction> GetHistory ()
    {
        return singleton.actionHistoryList;
    }
}
