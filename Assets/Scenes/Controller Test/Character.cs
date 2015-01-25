using UnityEngine;
using System.Collections;

public class Character : MonoBehaviour {

	public GameObject characterGO;
	public Vector2 viewingDirection;

	// Use this for initialization
	void Start () {
		viewingDirection = (1, 0);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void doAction(PlayerAction action) {
		switch (action.actionType) {
			case PlayerActionType.Forward : {
				moveForward();
				break;
			}
			case PlayerActionType.TurnLeft : {
				turnLeft ();
				break;
			}
			case PlayerActionType.TurnRight : {
				turnRight();
				break;
			}
			case PlayerActionType.TurnBack : {
				turnBack();
				break;
			}
			case PlayerActionType.Jump :  {
				jump ();
				break;
			}
			case PlayerActionType.Attack : {
				attack();
				break;
			}
		}
	}

	private void moveForward ()	{
		Vector3 position = getPosition ();

	}

	private void turnLeft() {
		Vector3 position = getPosition ();
	}

	private void turnRight() {
	}

	private void turnBack() {
	}

	//Jump up and down and move it all around *lalala
	private void jump() {
	}

	private void attack () {
		throw new System.NotImplementedException ();
	}

	private Vector3 getPosition ()
	{
		return characterGO.transform.position;
	}
}
