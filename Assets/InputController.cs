using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum PlayerActionType {Forward, TurnLeft, TurnRight, TurnBack, Jump, Attack, None};

public class PlayerAction {
    PlayerAction()
    {
        timeInSlot = 0.f;
        actionType = PlayerActionType.None;
    }

    float timeInSlot;
    PlayerActionType actionType;
}

public class InputController : MonoBehaviour {
    List<PlayerAction> actions;

	// Use this for initialization
	void Start () {
        PlayerAction action = new PlayerAction();

        actions.Add(action);
	}
	
	// Update is called once per frame
	void Update () {
	    
	}

    void checkKeyboardInput() {
        Input.GetButtonDown("P1Forward");
    }
}
