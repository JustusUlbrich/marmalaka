﻿using UnityEngine;
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

    public PlayerAction (NetworkPlayer netPlayerParam, int localPlayerIdParam, int actionTypeParam, int turnNoParam, int moveNoParam, float deltaInSlotParam)
    {
        netPlayer = netPlayerParam;
        localPlayerId = (int)localPlayerIdParam;
        actionType = (PlayerActionType)actionTypeParam;

        timerData = new TurnTimerData ();
        timerData.turnNumber = (int)turnNoParam;
        timerData.moveNumber = (int)moveNoParam;
        timerData.timeInTurn = (float)deltaInSlotParam;
    }

    public NetworkPlayer netPlayer;
    public int localPlayerId;
    public PlayerActionType actionType;
    public TurnTimerData timerData;
}

public class InputController : MonoBehaviour
{
    public int[] remainingMoveAllowance;

    // Use this for initialization
    void Start ()
    {
        int localPlayerCount = GameManager.singleton.localPlayerCount;

        initMoveAllowance(localPlayerCount);
    }
	
    // Update is called once per frame
    void Update ()
    {
        int localPlayerCount = GameManager.singleton.localPlayerCount;

        for (int playerID = 0; playerID < localPlayerCount; ++playerID)
            checkKeyboardInput (playerID);

    }

    void initMoveAllowance(int playerCount)
    {
        remainingMoveAllowance = new int[playerCount];
        for (int i = 0; i < playerCount; i++)
        {
            remainingMoveAllowance[i] = GameManager.singleton.MOVE_ALLOWANCE;
        }
    }

    void InputTurnStart (TurnTimerData timerData)
    {
        int localPlayerCount = GameManager.singleton.localPlayerCount;

        initMoveAllowance(localPlayerCount);
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

        if (action.actionType != PlayerActionType.Undefined && remainingMoveAllowance[playerID] > 0) {

            remainingMoveAllowance[playerID] --;
            action.timerData = TurnTimer.getInputTimerData ();

            networkView.RPC ("AddAction", RPCMode.All, action.netPlayer, action.localPlayerId, (int)action.actionType, action.timerData.turnNumber, action.timerData.moveNumber, action.timerData.timeInTurn);
        }
    }
}
