using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine.Networking;
using System;

public class PlayerControls : NetworkBehaviour {


	//use for locking regular controls while in menus
	bool controlsDisabled = false;

	public void disableControls(){
		controlsDisabled = true;
	}

	public void enableControls(){
		controlsDisabled = false;
	}



	Lists lists;
	TimeController timeController;

	public Camera cam;

    //The fleet list from the ListsController object
	List<Fleets> FleetsList;

    //All currently selected fleets
	public List<Fleets> selectedFleets;
    //used to manage fleet selection and debug in Unity 
	public Fleets[] selFleetArray; //debugging only

    //Our class used to store all the sprites we'll use
    Sprites sprites;

	NetworkPlayer netPlay;

	//these are used to control camera zoom levels
	const int perspZoomInLimit = 100; //minimum distance from plane
	const int perspZoomOutLimit = 5000; //max distance from plane
	const int perspZoomSpeed = 5000;

	const int orthoZoomInLimit = 5; //smallest size
	const int orthoZoomOutLimit = 5000; //max size of orthographic view, bigger is much more intensive
	const int orthoZoomSpeed = 1500; //multiplier for mouse input

	//How quickly the wasd buttons will move the camera
	const float wasdScrollSpeed = 7.5f; 
	const float backwards = -1;
	const float forwards = 1;

	Vector3 lastFramePosition;
	Transform LastCamTransform;

	struct eulerAngles{
		public float x;
		public float y;
		public float z;

		public eulerAngles(float x, float y, float z)
		{this.x = x;
			this.y = y;
			this.z = z;
		}
			}

	void recordEuler(eulerAngles euler){
		euler.x = cam.transform.rotation.eulerAngles.x;
		euler.y = cam.transform.rotation.eulerAngles.y;
		euler.z = cam.transform.rotation.eulerAngles.z;
	}


	// Use this for initialization
	void Start () {
		fetchComponents ();
		checkFleetSprites ();
		cam.orthographicSize = orthoZoomOutLimit;
		}

    //sets up all of our variables
	void fetchComponents(){
		timeController = GameObject.FindGameObjectWithTag ("Time").GetComponent<TimeController>();
		netPlay = GetComponentInParent<NetworkPlayer> ();
		cam = GetComponentInParent<NetworkPlayer> ().playerCam;
		sprites = GetComponentInParent<Sprites> ();
		FleetsList = GameObject.FindGameObjectWithTag ("Lists").GetComponent<Lists> ().FleetsList;

			}

    //center camera on the last selected fleet and zoom in
	public void zoomToFleet(){
		cam.transform.position = selectedFleets.LastOrDefault ().Position;
		cam.orthographicSize = orthoZoomInLimit;
		restoreCameraDistance ();
		checkFleetSprites ();
	}

    //Decide whether to enable/disable sprites on each fleet based on camera zoom
	void checkFleetSprites(){
		switch (cam.orthographic){
		case true:
			if (cam.orthographicSize > 499) {
				foreach (Fleets fl in FleetsList) {
					fl.enableSprite ();
				}
			} else {
				foreach (Fleets fl in FleetsList) {
				
					fl.disableSprite ();
				}
			}
			break;
	
		case false:
			if (cam.transform.position.z < -250) {
				foreach (Fleets fl in FleetsList) {
					fl.enableSprite ();
				}
			} else {
				foreach (Fleets fl in FleetsList) {

					fl.disableSprite ();
				}
			}
			break;
	}
	}

	// Update is called once per frame
	void Update () {

		//Check if this is the correct player
		if (!isLocalPlayer || controlsDisabled) {
			return;
		}

		//set current frame positions
		//poll where the mouse is at the start of each frame and store it
		Vector3 currFramePosition = GetWorldPositionOnPlane();  
		//always keep the mouse 1 layer closer to the camera than our highest layer in order to keep it visible.
		//be very careful changing this as it can break selection
		currFramePosition.z = 0; 
		Vector3 diff = new Vector3(0,0,0);  //storage container for the difference between where we were and where we moved the mouse to.

		// Handle screen dragging
		if( Input.GetMouseButton(1) ) {	// Right
			//determine difference in each vector component
				diff = lastFramePosition - currFramePosition; 
			//prevent dragging along the z layer by accident.  Prevents major glitches, do not alter
				diff.z = 0; 
			//move the camera the same amount our mouse moved
				cam.transform.Translate (diff); 
		}

		//WASD Scrolling
		//TODO add zoom ratio speed effect
		if (Input.GetKey(KeyCode.W)) {
			diff.y = wasdScrollSpeed*forwards;
			cam.transform.transform.Translate(diff);
		}		
		if (Input.GetKey(KeyCode.A)&& !Input.GetMouseButton(0)) { 
			diff.x = wasdScrollSpeed*backwards;
			cam.transform.transform.Translate(diff);
		}		
		if (Input.GetKey(KeyCode.S)) { 
			diff.y = wasdScrollSpeed*backwards;
			cam.transform.transform.Translate(diff);
		}		
		if (Input.GetKey(KeyCode.D)) { 
			diff.x = wasdScrollSpeed*forwards;
			cam.transform.transform.Translate(diff);
		}


		if( Input.GetMouseButton(2) ) {	// Middle Click
			Debug.Log("Attempting rotation");
			switch (cam.orthographic) {

			case true:
				//orthographic cam cant rotate
				break;

			case false:
				{
					//determine difference in each vector component
					diff = lastFramePosition - currFramePosition; 
					//enabling enabling float x will allow for horizontal rotation, but causes issues with z axis as well.
					//TODO fix z axis problems to enable x rotation
					float x = 0f; // diff.x;
					float y = diff.y * -1;
					float z = 0f;

					Quaternion testternion = cam.transform.rotation;
					Vector3 mappedDiff = new Vector3 (y, x, 0f);
					GameObject testObject = new GameObject();
					testObject.transform.position = cam.transform.position;
					testObject.transform.rotation = cam.transform.rotation;
					testObject.transform.Rotate (mappedDiff * (Time.deltaTime * 3.5f));
					testternion = testObject.transform.rotation;
					//Destroy (testObject);


					Debug.Log(testObject.transform.rotation.x);
					if (testternion.eulerAngles.x > 59 && testternion.eulerAngles.x < 300) {
						Debug.Log ("Rotation will break! Aborting!");
						Destroy (testObject);
						return;
					}

					Destroy (testObject);

					cam.transform.Rotate (mappedDiff*(Time.deltaTime*3.5f));
					//cam.transform.rotation = Quaternion.AngleAxis(cam.transform.rotation.x + (Time.deltaTime*mappedDiff.x), Vector3.right);
					break;
				}
			}
		}


		//Handle Camera Zooming via scroll wheel
		if( Input.GetAxis ("Mouse ScrollWheel") != 0f ) {	// zoom function.  Works, but requires a switch to perspective camera and changes to other parts of the script to compoensate.
			switch (cam.orthographic) { //different camera modes handle zooming differently.  Perspective actually moves the camera, while ortho changes the canvas size to simulate it
			case true:
				{	
					float f = cam.orthographicSize - Input.GetAxis ("Mouse ScrollWheel")*orthoZoomSpeed;
					if (f > orthoZoomInLimit && f < orthoZoomOutLimit) {//how close you can zoom in or how far y ou can zoom out
						cam.orthographicSize = f;
					}
					//Check if we need to enable or disable the fleet sprites based on zoom range
					checkFleetSprites ();
					break;
				}
			case false:
				{
					diff.z = Input.GetAxis ("Mouse ScrollWheel") * perspZoomSpeed*Time.deltaTime;
					if ((diff.z < 0 && cam.transform.position.z > perspZoomOutLimit * -1) || (diff.z > 0 && cam.transform.position.z < perspZoomInLimit * -1)) {
						cam.transform.Translate (diff);
					}
					checkFleetSprites ();
					break;
				}

			}
		} //end of zooming


		//Spawn fleet for testing
		if (Input.GetKeyDown(KeyCode.F)) { 
			Debug.Log (currFramePosition);
			Instantiate (sprites.FleetPrefab, currFramePosition, Quaternion.identity);
			checkFleetSprites ();
		}

        //Assign test mission moving from Earth to Mars to selected fleet.  Will throw exception if no fleet selected, so dont be a fucking retard
        if (Input.GetKeyDown(KeyCode.L))
        {
            Missions.TransportMission(null, selectedFleets.LastOrDefault(), GameObject.FindGameObjectWithTag("Earth").GetComponent<Planets>(), GameObject.FindGameObjectWithTag("Mars").GetComponent<Planets>(), 1);
        }

        //Switch to perspective Camera
        if (Input.GetKeyDown(KeyCode.P)) { 
			LastCamTransform = cam.transform;
			cam.orthographic = false;
			float z = cam.orthographicSize * -2;
			cam.transform.position.Set (LastCamTransform.position.x, LastCamTransform.position.y, z);
		}

		//Switch to Orthographic Camera
		if (Input.GetKeyDown(KeyCode.O)) { 
			LastCamTransform = cam.transform;
			cam.orthographic = true;
			cam.transform.position = LastCamTransform.position;
			cam.orthographicSize = LastCamTransform.position.z / -2;
		}

		//Trigger planet selection to test orbit alpha
		if (Input.GetKeyDown(KeyCode.G)) { 
			Ray ray;
			RaycastHit hit;
			 ray = cam.ScreenPointToRay (Input.mousePosition);
			if (Physics.Raycast (ray, out hit)) {
                GameObject targ;
                if (hit.collider.name != "GravityWell")
                {
                    targ = hit.collider.gameObject;
                }
                else
                {
                    targ = hit.collider.gameObject.transform.parent.gameObject;
                }
                Debug.Log(targ.name);
                targ.GetComponent<Planets>().checkOrbitSelection ();
			}
		}

        //on left mouse button click, selection
        if (Input.GetMouseButtonDown(0) && !Input.GetKey(KeyCode.A)) { 
			//Debug.Log("Attempting selection!");
			//dumb way without raycasts
				foreach (Fleets fleet in FleetsList) {
				//	Debug.Log ("Parsing Fleets");
					if (fleet.localPlayerAuthority) {
						Collider coll = fleet.fleetGo.GetComponent<Collider> ();
						if (coll.bounds.Contains (new Vector3 (currFramePosition.x, currFramePosition.y, coll.transform.position.z))) {
							changeSelection (fleet);
						} else {
							Debug.Log (new Vector3 (currFramePosition.x, currFramePosition.y, coll.transform.position.z) + " does not match an owned fleet");
						}
					} else {
				//		Debug.Log ("currently parsed fleet isnt owned by player, skipping");
					}
				}


		}

        //on left mouse button click + A key, movement
        if (Input.GetMouseButtonDown (0) && Input.GetKey(KeyCode.A)) {
            bool abort = false;
            foreach(Planets planet in Planets.PlanetList)
            {
                if (planet.gameObject.GetComponent<Collider>().bounds.Contains(currFramePosition))
                {
                    abort = true;
                }
            }
            if (abort)
            {
                return;
            }
			foreach (Fleets fleet in selectedFleets) {
				fleet.MoveTo(currFramePosition);
			}
		}



		//Timescale stuff_______________________________________________________
		if (Input.GetKeyDown(KeyCode.Equals)) { //on left mouse button click
			timeController.CmdSpeedUp(netPlay.playerID);
		}

		if (Input.GetKeyDown(KeyCode.Minus)) { //on left mouse button click
			timeController.CmdSlowDown(netPlay.playerID);
		}

		if (Input.GetKeyDown(KeyCode.Space)) { //on left mouse button click
			timeController.CmdPause(netPlay.playerID);
		}



		//______________________________________________________________________

		//TODO add wasd scrolling

		//grab and save the current mouse position to use as a reference for next frame.
		lastFramePosition = GetWorldPositionOnPlane();
		lastFramePosition.z = 0;


		selFleetArray = selectedFleets.ToArray (); //only for debugging purposes~



	} //end of update

	void changeSelection(Fleets fleet){
		if(selectedFleets.Contains(fleet)){
			selectedFleets.Remove(fleet);
			fleet.SetGlow = false;
			Debug.Log ("Unselected fleet: " + fleet.fleetName);
			}
			else{
				selectedFleets.Add(fleet);
			fleet.SetGlow = true;
			Debug.Log ("Selected fleet: " + fleet.fleetName);
			}
	}

	void restoreCameraDistance(){
		cam.transform.position = new Vector3(cam.transform.position.x,cam.transform.position.y, -50);
		Debug.Log ("Restoring camera Z: " + cam.transform.position.z);
	}


	public Vector3 GetWorldPositionOnPlane() {
		float z = 0;
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		Plane xy = new Plane(Vector3.forward, new Vector3(0, 0, z));
		float distance;
		xy.Raycast(ray, out distance);
		return ray.GetPoint(distance);
	}



}
