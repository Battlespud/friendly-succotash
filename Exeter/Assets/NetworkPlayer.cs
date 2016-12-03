using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class NetworkPlayer : NetworkBehaviour {





	//__Camera management


	[SyncVar] public string playerID;
	public Camera playerCam;


	void Awake(){
		playerCam = GetComponentInChildren<Camera> ();
		playerCam.gameObject.SetActive (false); //deactivate the camera

	}


	[Command]
	void CmdSetPlayerID(string newID){
		playerID = newID;
	}


	public override void OnStartLocalPlayer(){
		string myPlayerID = string.Format ("Player {0}", netId.Value);
		CmdSetPlayerID (myPlayerID);
		playerCam.gameObject.SetActive (true); //set active only for local player

	}




	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {


	
	}
}
