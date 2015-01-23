using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum PlayerActionType {Forward, TurnLeft, TurnRight, TurnBack, Jump, Attack};

public class PlayerAction {
    float timeInSlot;
    PlayerActionType actionType;
}

public class InputController : MonoBehaviour {
    ArrayList actions;

	// Use this for initialization
	void Start () {
        PlayerAction action = new PlayerAction();
        PlayerActionType type = PlayerActionType.Forward;

        actions.Add(action);
        actions.Add(2);
        actions.Add(type);
	}
	
	// Update is called once per frame
	void Update () {
	    
	}

    void checkKeyboardInput() {
        Input.GetButtonDown("P1Forward");
    }
}
