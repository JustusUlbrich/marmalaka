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
        transform.rotation = Quaternion.Slerp(fromRot, toRot, timeFactor);
    }

    public void doAction (PlayerAction action)
    {
        clearPostionTransition();
        clearRotationTransition();

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
        Vector3 position = getPosition();

        ////Check if we are on a street and next is also street
        //GameObject groundCellContent = GameManager.singleton.levelGen.floorGrid[((int) toPos.x)][((int)toPos.z)];
        //if (groundCellContent != null)
        //{
        //    if (groundCellContent.CompareTag("street"))
        //    {

        //    }
        //}

        Vector3 targetPos = position + VIEWING_DIRECTIONS [viewingDirectionIndex];

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

    private void clearPostionTransition()
    {
        fromPos = toPos;
        transform.position = toPos;
    }

    private void clearRotationTransition()
    {
        fromRot = toRot;
        transform.rotation = toRot;
    }

    private void turnLeft ()
    {
        viewingDirectionIndex ++;
        if (viewingDirectionIndex >= VIEWING_DIRECTIONS.Length) {
            viewingDirectionIndex = 0;
        }

        startTime = Time.time;
        fromRot = transform.rotation;
        toRot = Quaternion.LookRotation(VIEWING_DIRECTIONS[viewingDirectionIndex]);
    }

    private void turnRight ()
    {
        viewingDirectionIndex --;
        if (viewingDirectionIndex < 0) {
            viewingDirectionIndex = VIEWING_DIRECTIONS.Length - 1;
        }

        startTime = Time.time;
        fromRot = transform.rotation;
        toRot = Quaternion.LookRotation(VIEWING_DIRECTIONS[viewingDirectionIndex]);
    }

    private void turnBack ()
    {
        viewingDirectionIndex = (viewingDirectionIndex + 2) % 4;

        startTime = Time.time;
        fromRot = transform.rotation;
        toRot = Quaternion.LookRotation(VIEWING_DIRECTIONS[viewingDirectionIndex]);
    }

    //Jump up and down and move it all around *lalala
    private void jump ()
    {
        //TODO
    }

    private void attack ()
    {
        Vector3 targetPos = toPos + VIEWING_DIRECTIONS[viewingDirectionIndex];

        GameObject targetCellContent = GameManager.singleton.levelGen.top3DGrid[((int)targetPos.x), ((int)targetPos.y), ((int)targetPos.z)];

        int currentHeight = (int)targetPos.y;
        while (targetCellContent != null && targetCellContent.CompareTag("house"))
        {
            GameObject.Destroy(targetCellContent);

            currentHeight++;
            if (currentHeight < GameManager.singleton.levelGen.maxHillHeight)
                targetCellContent = GameManager.singleton.levelGen.top3DGrid[((int)targetPos.x), currentHeight, ((int)targetPos.z)];
            else
                targetCellContent = null;
        }

        
    }

    private Vector3 getPosition ()
    {
        return transform.position;
    }
}
