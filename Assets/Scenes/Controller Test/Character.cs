using UnityEngine;
using System.Collections;

public class Character : MonoBehaviour {

	public GameObject characterGO;
	public int viewingDirectionIndex;
	public Vector3 viewingDirection;
	private Vector3[] VIEWING_DIRECTIONS;

	// Use this for initialization
	void Start () {
		VIEWING_DIRECTIONS = new Vector3[] {new Vector3(1, 0, 0), 
			new Vector3(0, 0, 1), 
			new Vector3(-1, 0, 0), 
			new Vector3(0, 0, -1)};
		viewingDirectionIndex = 0;
		viewingDirection = VIEWING_DIRECTIONS[0];
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
		Vector3 position = getPosition();
		position += VIEWING_DIRECTIONS[viewingDirectionIndex];
		characterGO.transform.position = position;
	}

	private void turnLeft() {
		Vector3 position = getPosition();
		viewingDirectionIndex ++;
		if(viewingDirectionIndex >= VIEWING_DIRECTIONS.Length) {
			viewingDirectionIndex = 0;
		}
		position += VIEWING_DIRECTIONS[viewingDirectionIndex];
		characterGO.transform.LookAt(position);
	}

	private void turnRight() {
		Vector3 position = getPosition();
		viewingDirectionIndex --;
		if(viewingDirectionIndex < 0) {
			viewingDirectionIndex = VIEWING_DIRECTIONS.Length - 1;
		}
		position += VIEWING_DIRECTIONS[viewingDirectionIndex];
		characterGO.transform.LookAt(position);
	}

	private void turnBack() {
		Vector3 position = getPosition();
		viewingDirection.x *= -1;
		viewingDirection.y *= -1;
		position += VIEWING_DIRECTIONS[viewingDirectionIndex];
		characterGO.transform.LookAt(position);
	}

	//Jump up and down and move it all around *lalala
	private void jump() {
		//TODO
	}

	private void attack () {
		//TODO
	}

	private Vector3 getPosition ()
	{
		return characterGO.transform.position;
	}
}
