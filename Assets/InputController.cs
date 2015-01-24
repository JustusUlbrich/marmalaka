using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum PlayerActionType { Forward, TurnLeft, TurnRight, TurnBack, Jump, Attack, Nop, Undefined };

public class PlayerAction {
    public PlayerAction()
    {
        actionType = PlayerActionType.Undefined;
        timerData = new SlotTimerData();
    }

    public PlayerActionType actionType;
    public SlotTimerData timerData;
}

public class InputController : MonoBehaviour {
  	// Use this for initialization
	void Start () {

    }
	
	// Update is called once per frame
	void Update () {
        checkKeyboardInput();

	}

    void checkKeyboardInput() {
        PlayerAction action = new PlayerAction();

        string playerString = "P1";

        if (Input.GetButtonDown(playerString + "Forward"))
            action.actionType = PlayerActionType.Forward;

        else if (Input.GetButtonDown(playerString + "TurnLeft"))
            action.actionType = PlayerActionType.TurnLeft;

        else if (Input.GetButtonDown(playerString + "TurnRight"))
            action.actionType = PlayerActionType.TurnRight;

        else if (Input.GetButtonDown(playerString + "TurnBack"))
            action.actionType = PlayerActionType.TurnBack;

        else if (Input.GetButtonDown(playerString + "Jump"))
            action.actionType = PlayerActionType.Jump;

        else if (Input.GetButtonDown(playerString + "Attack"))
            action.actionType = PlayerActionType.Attack;


        if (action.actionType != PlayerActionType.Undefined)
        {
            action.timerData = SlotTimerScript.getTimerData();
            //TODO Send action to network sever
            Debug.Log("Player Action: Type " + action.actionType.ToString() + "  SlotNo" + action.timerData.slotSequenceNo + "  Time:" + action.timerData.timeInSlot);
        }
    }
}
