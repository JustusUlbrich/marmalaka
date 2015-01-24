using UnityEngine;
using System;
using System.Collections;

[Serializable]
public class SlotTimerData
{
    public SlotTimerData ()
    { 
    
    }

    public float timeInSlot = 0.0f;
    public int slotSequenceNo = 0;
}

public class SlotTimerScript : MonoBehaviour
{
    private SlotTimerData timerData;

    //TODO move me into settings and keep it private
    public float maxSlotTime;

    public static SlotTimerScript singleton;

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
        timerData = new SlotTimerData ();
    }
	
    // Update is called once per frame
    void Update ()
    {
        timerData.timeInSlot += Time.deltaTime;

        if (timerData.timeInSlot >= maxSlotTime) {
            timerData.timeInSlot -= maxSlotTime;

            timerData.slotSequenceNo++;
        }
            
    }

    public static SlotTimerData getTimerData ()
    {
        return singleton.timerData;
    }


}
