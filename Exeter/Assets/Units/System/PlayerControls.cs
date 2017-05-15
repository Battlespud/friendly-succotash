using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine.Networking;
using System;


public class PlayerControls : NetworkBehaviour {


	//KEYBINDS


//Mouse buttons.  Take note of which ones are get mouse button, and which are getmousebutton down
	bool LeftClick(){
		return Input.GetMouseButtonDown (0);
	}

	bool RightClick(){
		return Input.GetMouseButton (1);
	}

	bool MiddleClick(){
		return Input.GetMouseButton (2);
	}

//WASD Scrolling
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


//Keybinds
	const KeyCode SpawnFleetKey = KeyCode.F;
	const KeyCode SelectPlanetKey = KeyCode.G;
	const KeyCode AddColonistKey = KeyCode.Z;
	const KeyCode FoundColonyKey = KeyCode.C;
	const KeyCode StopKey = KeyCode.Backspace;
	const KeyCode MoveKey = KeyCode.V;


//Time Controls
	const KeyCode SpeedUp = KeyCode.Equals;
	const KeyCode SlowDown = KeyCode.Minus;
	const KeyCode Pause = KeyCode.Space;

//Keybinds for various actions, plus methods
	bool SpawnFleet()
	{
		return Input.GetKeyDown(SpawnFleetKey);
	}


	bool SelectPlanet(){
		return		Input.GetKeyDown (SelectPlanetKey);
	}


	bool FoundColony(){
		return  	Input.GetKeyDown(FoundColonyKey);
	}

	bool AddColonist(){
		return  	Input.GetKeyDown(AddColonistKey);
	}

	bool SelectFleet(){
		return (LeftClick () && !Move ());
	}

	bool MoveFleet(){
		return(LeftClick() && Move ());
	}

	bool Stop(){
		return	Input.GetKeyDown (StopKey);
	}

	bool Move(){
		return	Input.GetKey (MoveKey);
	}

	bool Scroll(){
		return (Input.GetAxis ("Mouse ScrollWheel") != 0f); 
	}




//use for locking regular controls while in menus
	bool controlsDisabled = false;

	public void disableControls(){
		controlsDisabled = true;
	}

	public void enableControls(){
		controlsDisabled = false;
	}


	//The timecontroller shared by all players
	TimeController timeController;

	//This player's camera
	public Camera cam;

	//A list of various static lists.  Probably deprecated idk
	Lists lists;
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
	//Multiplayer stuff
	NetworkPlayer netPlay;



	//How quickly the wasd buttons will move the camera
	const float wasdScrollSpeed = 7.5f; 
	const float backwards = -1;
	const float forwards = 1;

	Transform LastCamTransform;

	//custom struct for holding rotations without dealing with quaternions
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
		//Check our sprites three times a second and update as neccessary.  Will run constantly after start
		InvokeRepeating ("InvokeSpriteCheck", 1, .33f);

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


	int shift(){
		if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey (KeyCode.RightShift))
		{
			return 4;
		}
		return 1;
	}



	//________________________________________________________________________________________________________________


	void Update () {

		//Check if this is the correct player
		if (!isLocalPlayer) {
			return;
		}
		//record our current frame position
		Vector3 currFramePosition = PlayerControlsEvents.GetFramePosition (cam);

		Vector3 diff = new Vector3(0,0,0);  //storage container for the difference between where we were and where we moved the mouse to.

		// Handle screen dragging
		if(RightClick()) {	// Right
			PlayerControlsEvents.Dragging(cam, lastFramePosition,currFramePosition);
		}


		//WASD Scrolling, hold shift to go fast
		if (GoUp()) {
			diff.y += wasdScrollSpeed*forwards*shift();
			PlayerControlsEvents.MoveCamera (cam, diff);

		}		
		if (GoLeft()) { 
			diff.x += wasdScrollSpeed*backwards*shift();
			PlayerControlsEvents.MoveCamera (cam, diff);

		}		
		if (GoDown()) { 
			diff.y += wasdScrollSpeed*backwards*shift();
			PlayerControlsEvents.MoveCamera (cam, diff);

		}		
		if (GoRight()) { 
			diff.x += wasdScrollSpeed*forwards*shift();
			PlayerControlsEvents.MoveCamera (cam, diff);
		}


		if(MiddleClick()) {	// Middle Click
			if (!cam.orthographic) {
					diff = lastFramePosition - currFramePosition; 
					PlayerControlsEvents.RotateCamera (cam, diff);
				}
			}



		//Handle Camera Zooming via scroll wheel
		if(Scroll()) {
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

        //Switch to perspective Camera. Default
        if (Input.GetKeyDown(KeyCode.P)) { 
			LastCamTransform = cam.transform;
			cam.orthographic = false;
			float z = cam.orthographicSize * -2;
			cam.transform.position.Set (LastCamTransform.position.x, LastCamTransform.position.y, z);
		}

		//Switch to Orthographic Camera. Not supported
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

		if (FoundColony()) { 
			PlayerControlsEvents.FoundColony (cam);
		}

		if (AddColonist()) { 
			PlayerControlsEvents.AddColonist (cam);
		}

        //on left mouse button click, selection
		if (SelectFleet()) { 
			PlayerControlsEvents.SelectFleet(FleetsList, currFramePosition, selectedFleets);
		}

        //on left mouse button click + V key, moveto
		if (MoveFleet()) {
			PlayerControlsEvents.MoveFleet (selectedFleets, currFramePosition);
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

		lastFramePosition = PlayerControlsEvents.GetFramePosition(cam);
	} //end of update

	//checks our sprites
	void InvokeSpriteCheck(){
		PlayerControlsEvents.CheckFleetSprites (cam, FleetsList);
	}










}
