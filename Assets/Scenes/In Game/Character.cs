using UnityEngine;
using System.Collections;

public class Character : MonoBehaviour
{

    public int viewingDirectionIndex;
    public Vector3 viewingDirection;
    private Vector3[] VIEWING_DIRECTIONS;

    private float startTime;
    private Vector3 fromPos;
    private Vector3 toPos;

    private Quaternion fromRot;
    private Quaternion toRot;

    // Use this for initialization
    void Start ()
    {
        VIEWING_DIRECTIONS = new Vector3[] {new Vector3 (1, 0, 0), 
			new Vector3 (0, 0, 1), 
			new Vector3 (-1, 0, 0), 
			new Vector3 (0, 0, -1)};
        viewingDirectionIndex = 1;
        viewingDirection = VIEWING_DIRECTIONS [0];

        fromRot = Quaternion.identity;
        toRot = Quaternion.identity;
    }
	
    // Update is called once per frame
    void Update ()
    {
        float timeFactor = (Time.time - startTime) / TurnTimer.singleton.moveTime;

        transform.position = Vector3.Lerp (fromPos, toPos, timeFactor);
    }

    public void doAction (PlayerAction action)
    {
        switch (action.actionType) {
        case PlayerActionType.Forward:
            {
                moveForward ();
                break;
            }
        case PlayerActionType.TurnLeft:
            {
                turnLeft ();
                break;
            }
        case PlayerActionType.TurnRight:
            {
                turnRight ();
                break;
            }
        case PlayerActionType.TurnBack:
            {
                turnBack ();
                break;
            }
        case PlayerActionType.Jump:
            {
                jump ();
                break;
            }
        case PlayerActionType.Attack:
            {
                attack ();
                break;
            }
        }
    }

    private void moveForward ()
    {
        Vector3 position = getPosition ();
        fromPos = position;


        //position += VIEWING_DIRECTIONS [viewingDirectionIndex];
        Vector3 targetPos = toPos + VIEWING_DIRECTIONS [viewingDirectionIndex];

        GameObject targetCellContent = GameManager.singleton.levelGen.top3DGrid [((int)targetPos.x), 0, ((int)targetPos.z)];

        if (targetCellContent != null) {
            Debug.Log ("Found Tag: " + targetCellContent.tag);

        } else {
            //transform.position = position;
            fromPos = position;
            toPos = targetPos;
            startTime = Time.time;
        }

    }

    private void turnLeft ()
    {
        Vector3 position = getPosition ();
        int prevDirectionIndex = viewingDirectionIndex;
        viewingDirectionIndex ++;
        if (viewingDirectionIndex >= VIEWING_DIRECTIONS.Length) {
            viewingDirectionIndex = 0;
        }
        position += VIEWING_DIRECTIONS [viewingDirectionIndex];
        transform.LookAt (position);
    }

    private void turnRight ()
    {
        Vector3 position = getPosition ();
        viewingDirectionIndex --;
        if (viewingDirectionIndex < 0) {
            viewingDirectionIndex = VIEWING_DIRECTIONS.Length - 1;
        }
        position += VIEWING_DIRECTIONS [viewingDirectionIndex];
        transform.LookAt (position);
    }

    private void turnBack ()
    {
        Vector3 position = getPosition ();
        viewingDirection.x *= -1;
        viewingDirection.y *= -1;
        position += VIEWING_DIRECTIONS [viewingDirectionIndex];
        transform.LookAt (position);
    }

    //Jump up and down and move it all around *lalala
    private void jump ()
    {
        //TODO
    }

    private void attack ()
    {
        //TODO
    }

    private Vector3 getPosition ()
    {
        return transform.position;
    }
}
