using UnityEngine;
using System.Collections;

public class tickerControl : MonoBehaviour {
    float maxSlotTime;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {

        maxSlotTime = SlotTimerScript.singleton.maxSlotTime;

        float timeInSlot  = SlotTimerScript.getTimerData().timeInSlot;
        float angle = -360 * (timeInSlot / maxSlotTime);

        this.transform.localRotation = Quaternion.AngleAxis(angle, new Vector3(0, 0, 1));
        //this.transform.rotation = Quaternion.AngleAxis(angle, new Vector3(0, 0, 1));
	}
}
