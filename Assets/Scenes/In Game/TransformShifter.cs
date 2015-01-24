using UnityEngine;
using System.Collections;

public class TransformShifter : MonoBehaviour
{

    public Camera cam;

    void Update ()
    {
	
        float forwardShift = Input.GetAxis ("Vertical");
        float horizontalShift = Input.GetAxis ("Horizontal");

        Vector3 projectedCamForward = cam.transform.forward;
        projectedCamForward.y = 0;

        projectedCamForward.x = projectedCamForward.x * forwardShift;
        projectedCamForward.z = projectedCamForward.z * forwardShift;

        Vector3 projectedCamRight = cam.transform.right;
        projectedCamRight.y = 0;
        
        projectedCamRight.x = projectedCamRight.x * horizontalShift;
        projectedCamRight.z = projectedCamRight.z * horizontalShift;

        transform.position += projectedCamForward * Time.deltaTime * 5;
        transform.position += projectedCamRight * Time.deltaTime * 5;

    }
}
