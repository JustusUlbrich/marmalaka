using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

[Serializable]
public enum PlayerActionType
{
    Forward,
    TurnLeft,
    TurnRight,
    TurnBack,
    Jump,
    Attack,
    Nop,
    Undefined }
;

[Serializable]
public class PlayerAction
{
    public PlayerAction ()
    {
        localPlayerId = 0;
        actionType = PlayerActionType.Undefined;
        timerData = new TurnTimerData ();
    }

    public PlayerAction (NetworkPlayer netPlayerParam, int localPlayerIdParam, int actionTypeParam, int timeSlotNoParam, float deltaInSlotParam)
    {
        netPlayer = netPlayerParam;
        localPlayerId = (int)localPlayerIdParam;
        actionType = (PlayerActionType)actionTypeParam;

        timerData = new TurnTimerData ();
        timerData.turnNumber = (int)timeSlotNoParam;
        timerData.timeInTurn = (float)deltaInSlotParam;
    }

    public StringBuilder AppendActionString (StringBuilder strBuilder, bool shortVersion = false)
    {
        if (shortVersion) {
            strBuilder.Append (":: ");
            strBuilder.Append (netPlayer);
            strBuilder.Append (" | ");
            strBuilder.Append (localPlayerId);
            strBuilder.Append (" | ");
            strBuilder.Append (actionType);
            strBuilder.Append (" | ");
            strBuilder.Append (timerData.turnNumber);
            strBuilder.Append (" | ");
            strBuilder.Append (timerData.timeInTurn);
            strBuilder.AppendLine ();
        } else {
            strBuilder.Append ("NetId: ");
            strBuilder.Append (netPlayer);
            strBuilder.Append (" | LocId: ");
            strBuilder.Append (localPlayerId);
            strBuilder.Append (" | ActType: ");
            strBuilder.Append (actionType);
            strBuilder.Append (" | Turn: ");
            strBuilder.Append (timerData.turnNumber);
            strBuilder.Append (" | TurnDelta: ");
            strBuilder.Append (timerData.timeInTurn);
            strBuilder.AppendLine ();
        }
        return strBuilder;
    }

    public NetworkPlayer netPlayer;
    public int localPlayerId;
    public PlayerActionType actionType;
    public TurnTimerData timerData;
}

public class InputController : MonoBehaviour
{
    // Use this for initialization
    void Start ()
    {

    }
	
    // Update is called once per frame
    void Update ()
    {
        int localPlayerCount = GameManager.singleton.localPlayerCount;

        for (int playerID = 0; playerID < localPlayerCount; ++playerID)
            checkKeyboardInput (playerID);

    }

    void checkKeyboardInput (int playerID)
    {
        PlayerAction action = new PlayerAction ();

        action.netPlayer = Network.player;
        action.localPlayerId = playerID;
        string playerString = "P" + (playerID);

        if (Input.GetButtonDown (playerString + "Forward"))
            action.actionType = PlayerActionType.Forward;
        else if (Input.GetButtonDown (playerString + "TurnLeft"))
            action.actionType = PlayerActionType.TurnLeft;
        else if (Input.GetButtonDown (playerString + "TurnRight"))
            action.actionType = PlayerActionType.TurnRight;
        else if (Input.GetButtonDown (playerString + "TurnBack"))
            action.actionType = PlayerActionType.TurnBack;
        else if (Input.GetButtonDown (playerString + "Jump"))
            action.actionType = PlayerActionType.Jump;
        else if (Input.GetButtonDown (playerString + "Attack"))
            action.actionType = PlayerActionType.Attack;

        if (action.actionType != PlayerActionType.Undefined) {

            action.timerData = TurnTimer.getTimerData ();

            Debug.Log ("SENT TO SERVER: " + action.AppendActionString (new StringBuilder ()).ToString ());

            if (Network.isServer) {
                SendMessage ("AddActionServerLocal", action, SendMessageOptions.RequireReceiver);
            } else {
                networkView.RPC ("AddAction", RPCMode.Server, action.netPlayer, action.localPlayerId, (int)action.actionType, action.timerData.turnNumber, action.timerData.timeInTurn);
            }
        }
    }
}
