using UnityEngine;
using System.Collections;

public class BackdropGUI : MonoBehaviour
{


    public Texture backdrop;
    public Texture arrow;

    Rect arrowRect = new Rect (0, 0, 100, 100);
    public float pingPongLength = 0.6f;

    public void Start ()
    {
    }

    public void Update ()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.y = Screen.height - mousePos.y;

        if (arrowRect.Contains (mousePos)) {

            if (Input.GetMouseButtonDown (0)) {

                GameManager.SetGameMode (1);
                GameManager.StartServer (4444, false);
                GameManager.StartGame ();

            }
        }
    }
    public void OnGUI ()
    {
        GUI.DrawTexture (new Rect (0, 0, Screen.width, Screen.height), backdrop, ScaleMode.ScaleToFit);

        float arrowFac = Mathf.Min ((arrow.width / (float)1920) * Screen.width, (arrow.height / (float)1080) * Screen.height);

        float pingPongVal = Mathf.PingPong (Time.time, pingPongLength);

        float arrowWidth = arrowFac + pingPongVal * 20;
        float arrowHeight = arrowFac + pingPongVal * 20;

        arrowRect = new Rect (Screen.width - Screen.width / 5.0f - arrowWidth / 2.0f, 
                              Screen.height / 2 - arrowHeight / 2 - Screen.height / 12, 
                              arrowWidth, arrowHeight);


        GUI.DrawTexture (arrowRect, arrow, ScaleMode.ScaleToFit);
    }

}
