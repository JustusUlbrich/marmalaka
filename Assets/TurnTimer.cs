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
    }

    public TurnTimerData (int turnNoParam, float turnTimeParam)
    { 
        turnNumber = turnNoParam;
        timeInTurn = turnTimeParam;
    }

    public float timeInTurn = 0.0f;
    public int turnNumber = 0;
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
    }

    // Use this for initialization
    void Start ()
    {
        timerData = new TurnTimerData ();
		turnTime = numberOfMoves * moveTime;
		timeInMove = 0.0f;
    }
	
    // Update is called once per frame
    void Update ()
    {
        timerData.timeInTurn += Time.deltaTime;
		timeInMove += Time.deltaTime;
		if(timeInMove >= moveTime) {
			timeInMove -= moveTime;
			SendMessage ("MoveOver", timerData.turnNumber, SendMessageOptions.RequireReceiver);
		}

        if (timerData.timeInTurn >= turnTime) {
            timerData.timeInTurn -= turnTime;

            SendMessage ("TurnOver", timerData.turnNumber, SendMessageOptions.RequireReceiver);
            timerData.turnNumber++;

        }
            
    }

    public static TurnTimerData getTimerData ()
    {
        return new TurnTimerData (singleton.timerData);
    }


}
