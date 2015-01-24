using UnityEngine;
using System.Collections;

public class GUINetworkState : MonoBehaviour
{

		void Awake ()
		{
				DontDestroyOnLoad (gameObject);
		}

		void OnGUI ()
		{
			
		
				GUI.Label (new Rect (0, Screen.height - 30, Screen.width, 20), GameStatusText () + " | " + Application.loadedLevelName);
		}

		string GameStatusText ()
		{
				string status = "";
		
				if (Network.isClient)
						status += "Client | ";
				else if (Network.isServer)
						status += "Server | ";
				else
						status += "Disconnected | ";
		
				status += Network.connections.Length + " Connections";
		
				return status;
		}
}
