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

    public TurnTimerData (int turnNoParam, int moveNumberParam, float turnTimeParam)
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
    private TurnTimerData inputTimerData;
    private TurnTimerData playTimerData;

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

        inputTimerData = new TurnTimerData ();
        playTimerData = new TurnTimerData (-1, 0, 0.0f);
    }

    void Start ()
    {
        turnTime = numberOfMoves * moveTime;
        timeInMove = 0.0f;

        SendMessage ("InputMoveStart", new TurnTimerData (singleton.inputTimerData), SendMessageOptions.DontRequireReceiver);
        SendMessage ("InputTurnStart", new TurnTimerData (singleton.inputTimerData), SendMessageOptions.DontRequireReceiver);

        SendMessage ("PlayMoveStart", new TurnTimerData (singleton.playTimerData), SendMessageOptions.DontRequireReceiver);
        SendMessage ("PlayTurnStart", new TurnTimerData (singleton.playTimerData), SendMessageOptions.DontRequireReceiver);

    }
	
    // Update is called once per frame
    void LateUpdate ()
    {
        inputTimerData.timeInTurn += Time.deltaTime;
        playTimerData.timeInTurn = inputTimerData.timeInTurn;

        timeInMove += Time.deltaTime;

        bool moveOver = false;
        bool turnOver = false;

        if (timeInMove >= moveTime) {

            timeInMove -= moveTime;
            moveOver = true;

        }

        if (inputTimerData.timeInTurn >= turnTime) {

            inputTimerData.timeInTurn -= turnTime;
            playTimerData.timeInTurn = inputTimerData.timeInTurn;
            turnOver = true;

        }

        if (turnOver) {

            SendMessage ("InputMoveOver", new TurnTimerData (singleton.inputTimerData), SendMessageOptions.DontRequireReceiver);
            SendMessage ("InputTurnOver", new TurnTimerData (singleton.inputTimerData), SendMessageOptions.DontRequireReceiver);

            SendMessage ("PlayMoveOver", new TurnTimerData (singleton.playTimerData), SendMessageOptions.DontRequireReceiver);
            SendMessage ("PlayTurnOver", new TurnTimerData (singleton.playTimerData), SendMessageOptions.DontRequireReceiver);

            inputTimerData.turnNumber++;
            inputTimerData.moveNumber = 0;

            playTimerData.turnNumber++;
            playTimerData.moveNumber = 0;

            SendMessage ("InputMoveStart", new TurnTimerData (singleton.inputTimerData), SendMessageOptions.DontRequireReceiver);
            SendMessage ("PlayMoveStart", new TurnTimerData (singleton.playTimerData), SendMessageOptions.DontRequireReceiver);



        } else if (moveOver) {

            SendMessage ("InputMoveOver", new TurnTimerData (singleton.inputTimerData), SendMessageOptions.DontRequireReceiver);
            SendMessage ("PlayMoveOver", new TurnTimerData (singleton.playTimerData), SendMessageOptions.DontRequireReceiver);

            inputTimerData.moveNumber++;
            playTimerData.moveNumber++;

            SendMessage ("InputMoveStart", new TurnTimerData (singleton.inputTimerData), SendMessageOptions.DontRequireReceiver);
            SendMessage ("PlayMoveStart", new TurnTimerData (singleton.playTimerData), SendMessageOptions.DontRequireReceiver);

        }
            
    }

    //public static TurnTimerData getTimerData ()
    //{
    //    return new TurnTimerData (singleton.timerData);
    //}
    public static TurnTimerData getInputTimerData ()
    {
        return new TurnTimerData (singleton.inputTimerData);
    }

    public static TurnTimerData getPlayTimerData ()
    {
        return new TurnTimerData (singleton.playTimerData);
    }
}
