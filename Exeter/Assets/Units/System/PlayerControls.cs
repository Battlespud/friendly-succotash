using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine.Networking;
using System;

public class PlayerControls : NetworkBehaviour {


	//KEYBINDS

	bool LeftClick(){
		return Input.GetMouseButton (0);
	}

	bool RightClick(){
		return Input.GetMouseButton (1);
	}

	bool MiddleClick(){
		return Input.GetMouseButton (2);
	}

		//wasd
	const KeyCode Up =  KeyCode.W;
	const KeyCode Down = KeyCode.S;
	const KeyCode Left = KeyCode.A;
	const KeyCode Right = KeyCode.D;

	bool GoUp(){
		return Input.GetKey (Up);
	}
	bool GoDown(){
		return Input.GetKey (Down);
	}
	bool GoLeft(){
		return Input.GetKey (Left);
	}
	bool GoRight(){
		return Input.GetKey (Right);
	}


	const KeyCode SpeedUp = KeyCode.Equals;
	const KeyCode SlowDown = KeyCode.Minus;
	const KeyCode Pause = KeyCode.Pause;


	bool SpawnFleet()
	{
		return Input.GetKeyDown(SpawnFleetKey);
	}
	const KeyCode SpawnFleetKey = KeyCode.F;


	bool SelectPlanet(){
		return		Input.GetKeyDown (SelectPlanetKey);
	}
	const KeyCode SelectPlanetKey = KeyCode.G;

	bool Stop(){
		return	Input.GetKeyDown (StopKey);
	}
	const KeyCode StopKey = KeyCode.Backspace;

	bool Move(){
		return	Input.GetKey (MoveKey);
	}
	const KeyCode MoveKey = KeyCode.V;

	//use for locking regular controls while in menus
	bool controlsDisabled = false;

	public void disableControls(){
		controlsDisabled = true;
	}

	public void enableControls(){
		controlsDisabled = false;
	}


	//A list of various static lists.  Probably deprecated idk
	Lists lists;
	//The timecontroller shared by all players
	TimeController timeController;

	//This player's camera
	public Camera cam;

    //The fleet list from the ListsController object
	List<Fleets> FleetsList;

    //All currently selected fleets
	public List<Fleets> selectedFleets;
	//All currently highlighted planets
	public List<Planets> selectedPlanets;
    //used to manage fleet selection and debug in Unity 
	public Fleets[] selFleetArray; //debugging only

    //Our class used to store all the sprites we'll use
    Sprites sprites;

	NetworkPlayer netPlay;



	//How quickly the wasd buttons will move the camera
	const float wasdScrollSpeed = 7.5f; 
	const float backwards = -1;
	const float forwards = 1;

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
	//Records current camera rotation to a euler angles 
	void recordEuler(eulerAngles euler){
		euler.x = cam.transform.rotation.eulerAngles.x;
		euler.y = cam.transform.rotation.eulerAngles.y;
		euler.z = cam.transform.rotation.eulerAngles.z;
	}


	// Use this for initialization
	void Start () {
		fetchComponents ();
		PlayerControlsEvents.CheckFleetSprites (cam, FleetsList);
		PlayerControlsEvents.ZoomOutMax (cam);
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
		PlayerControlsEvents.MoveCameraTo(cam, selectedFleets.LastOrDefault ().Position);
		PlayerControlsEvents.ZoomInMax (cam);
	}

    //Decide whether to enable/disable sprites on each fleet based on camera zoom

	//Where the mouse was last frame, used for telling difference between this and last frame
	public Vector3 lastFramePosition;




	// Update is called once per frame
	void Update () {

		//Check if this is the correct player
		if (!isLocalPlayer || controlsDisabled) {
			return;
		}
		//record our current frame position
		Vector3 currFramePosition = PlayerControlsEvents.GetFramePosition (cam);
	


		//set current frame positions
		//poll where the mouse is at the start of each frame and store it
		//be very careful changing this as it can break selection
		Vector3 diff = new Vector3(0,0,0);  //storage container for the difference between where we were and where we moved the mouse to.

		// Handle screen dragging
		if(RightClick()) {	// Right
			diff = PlayerControlsEvents.Dragging(cam, lastFramePosition,currFramePosition);
		}

		//WASD Scrolling
		//TODO add zoom ratio speed effect
		if (GoUp()) {
			diff.y += wasdScrollSpeed*forwards;
		}		
		if (GoLeft()) { 
			diff.x += wasdScrollSpeed*backwards;
		}		
		if (GoDown()) { 
			diff.y += wasdScrollSpeed*backwards;
		}		
		if (GoRight()) { 
			diff.x += wasdScrollSpeed*forwards;
		}


		if(MiddleClick()) {	// Middle Click
			switch (cam.orthographic) {
			case true:
				//orthographic cam cant rotate
				break;
			case false:
				{
					//determine difference in each vector component
					diff = lastFramePosition - currFramePosition; 
					PlayerControlsEvents.RotateCamera (cam, diff);
					break;
				}
			}
		}


		//Handle Camera Zooming via scroll wheel
		if( Input.GetAxis ("Mouse ScrollWheel") != 0f ) {
			PlayerControlsEvents.Zoom(cam, diff);

		} 

		//Spawn fleet for testing
		if (SpawnFleet()) { 
			Instantiate (sprites.FleetPrefab, currFramePosition, Quaternion.identity);
		}

        //Assign test mission moving from Earth to Mars to selected fleet.  Will throw exception if no fleet selected, so dont be a fucking retard
        if (Input.GetKeyDown(KeyCode.L))
        {
            Missions.TransportMission(null, selectedFleets.LastOrDefault(), GameObject.FindGameObjectWithTag("Earth").GetComponent<Planets>(), GameObject.FindGameObjectWithTag("Mars").GetComponent<Planets>(), 1);
        }

		if(Input.GetKeyDown(KeyCode.Semicolon)){
			Missions.MoveToPlanetMission(selectedFleets.LastOrDefault(), selectedPlanets.LastOrDefault());
		}

		if (Stop ()) {
			foreach (Fleets fl in selectedFleets) {
				fl.endMission ();
			}
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
		if (SelectPlanet()) { 
			PlayerControlsEvents.SelectPlanet (cam, selectedPlanets);
		}

        //on left mouse button click, selection
		if (Input.GetMouseButtonDown(0) && !Move()) { 
			//Debug.Log("Attempting selection!");
			//dumb way without raycasts
				foreach (Fleets fleet in FleetsList) {
				//	Debug.Log ("Parsing Fleets");
					if (fleet.localPlayerAuthority) {
						Collider coll = fleet.fleetGo.GetComponent<Collider> ();
						if (coll.bounds.Contains (new Vector3 (currFramePosition.x, currFramePosition.y, coll.transform.position.z))) {
							changeSelection (fleet);
						} else {
							//Debug.Log (new Vector3 (currFramePosition.x, currFramePosition.y, coll.transform.position.z) + " does not match an owned fleet");
						}
					} else {
				//		Debug.Log ("currently parsed fleet isnt owned by player, skipping");
					}
				}


		}

        //on left mouse button click + V key, movement
		if (Input.GetMouseButtonDown (0) && Move()) {
            bool abort = false;
            foreach(Planets planet in Planets.PlanetList)
            {
				if (planet.gameObject.GetComponent<Collider>().bounds.Contains(currFramePosition) || planet.GravityWell.gameObject.GetComponent<Collider>().bounds.Contains(currFramePosition))
                {
					Missions.MoveToPlanetMission (selectedFleets, planet);
                    //abort = true;
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
		if (Input.GetKeyDown(SpeedUp)) { //on left mouse button click
			timeController.CmdSpeedUp(netPlay.playerID);
		}

		if (Input.GetKeyDown(SlowDown)) { //on left mouse button click
			timeController.CmdSlowDown(netPlay.playerID);
		}

		if (Input.GetKeyDown(Pause)) { //on left mouse button click
			timeController.CmdPause(netPlay.playerID);
		}
		//______________________________________________________________________

		PlayerControlsEvents.MoveCamera (cam, diff);
		lastFramePosition = currFramePosition;
		//updates our sprites once per second
		InvokeRepeating ("InvokeSpriteCheck", 1, 1);
	} //end of update


	void InvokeSpriteCheck(){
		PlayerControlsEvents.CheckFleetSprites (cam, FleetsList);
	}

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








}
