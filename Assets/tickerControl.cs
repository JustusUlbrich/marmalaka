using UnityEngine;
using System.Collections;

public class tickerControl : MonoBehaviour {
    float maxSlotTime;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {

        maxSlotTime = TurnTimer.singleton.moveTime * TurnTimer.singleton.numberOfMoves;

        float timeInSlot = TurnTimer.getTimerData().timeInTurn;
        float angle = -360 * (timeInSlot / maxSlotTime);

        this.transform.localRotation = Quaternion.AngleAxis(angle, new Vector3(0, 0, 1));
	}
}
