using UnityEngine;
using System;
using System.Collections;

[Serializable]
public class TurnTimerData
{
    public TurnTimerData ()
    {
    }

    public TurnTimerData (TurnTimerData other)
    { 
        turnNumber = other.turnNumber;
        timeInTurn = other.timeInTurn;
        moveNumber = other.moveNumber;
    }

    public TurnTimerData (int turnNoParam, float turnTimeParam, int moveNumberParam)
    { 
        turnNumber = turnNoParam;
        timeInTurn = turnTimeParam;
        moveNumber = moveNumberParam;
    }

    public float timeInTurn = 0.0f;
    public int turnNumber = 0;
    public int moveNumber = 0;
}

public class TurnTimer : MonoBehaviour
{
    private TurnTimerData timerData;

    //TODO move me into settings and keep it private
    private float turnTime;
    public int numberOfMoves;
    public float moveTime;
    private float timeInMove;

    public static TurnTimer singleton;

    void Awake ()
    {
        if (singleton == null) {
            singleton = this;
        } else {
            Destroy (gameObject);
        }

        timerData = new TurnTimerData ();
    }

    // Use this for initialization
    void Start ()
    {
        timerData = new TurnTimerData ();
        turnTime = numberOfMoves * moveTime;
        timeInMove = 0.0f;

        SendMessage ("MoveStart", new TurnTimerData (singleton.timerData), SendMessageOptions.DontRequireReceiver);

    }
	
    // Update is called once per frame
    void Update ()
    {
        timerData.timeInTurn += Time.deltaTime;
        timeInMove += Time.deltaTime;

        bool moveOver = false;
        bool turnOver = false;

        if (timeInMove >= moveTime) {
            timeInMove -= moveTime;
            moveOver = true;
        }

        if (timerData.timeInTurn >= turnTime) {
            timerData.timeInTurn -= turnTime;
            turnOver = true;
        }

        if (turnOver) {
            SendMessage ("MoveOver", new TurnTimerData (singleton.timerData), SendMessageOptions.DontRequireReceiver);
            SendMessage ("TurnOver", new TurnTimerData (singleton.timerData), SendMessageOptions.DontRequireReceiver);

            timerData.turnNumber++;
            timerData.moveNumber = 0;

            SendMessage ("MoveStart", new TurnTimerData (singleton.timerData), SendMessageOptions.DontRequireReceiver);
        } else if (moveOver) {
            SendMessage ("MoveOver", new TurnTimerData (singleton.timerData), SendMessageOptions.DontRequireReceiver);

            timerData.moveNumber ++;

            SendMessage ("MoveStart", new TurnTimerData (singleton.timerData), SendMessageOptions.DontRequireReceiver);

        }
            
    }

    public static TurnTimerData getTimerData ()
    {
        return new TurnTimerData (singleton.timerData);
    }


}
