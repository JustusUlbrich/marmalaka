using UnityEngine;
using System.Collections;

public class TransformShifter : MonoBehaviour
{

    void Update ()
    {
	
        float xShift = Input.GetAxis ("Horizontal");
        float yShift = Input.GetAxis ("Vertical");

        transform.position += new Vector3 (xShift, 0, yShift) * Time.deltaTime;

    }
}
