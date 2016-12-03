﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;

public class Fleets : NetworkBehaviour {


	/*All movement and actions are performed by fleets rather than individual ships.  Though a fleet can consist of a single ship of course.
	Behaviors:
	Fleets move at the speed of their slowest member
	They reduce speed instantly on taking damage or remove lagging ships depending on settings */

	//tbh this will likely become a ship class rather than fleets.  



	//Statics_________________________________________________________
	List<Fleets> FleetsList;

	public static Sprite fleetSprite;
	ParticleSystem selectionEffect;

	BoxCollider coll;
	Vector3 collVecOrig;
	Vector3 collVecSprite;

	Vector3 minRange = new Vector3 (-29.4f,-31.8f,0f) - new Vector3 (-10.3f, -31f, 0f);

	float rotationSpeed = .2f;

	bool setGlow;

	public bool SetGlow { //enable or disable the glow effect 
		get {
			return setGlow;
		}
		set {
			setGlow = value;
			selectionEffect.gameObject.SetActive( value);
		}
	}




	public void enableSprite(){
		sr.enabled = true;
		coll.size = collVecSprite; //resize collider to sprite
		Debug.Log ("Sprites enabled");
	}

	public void disableSprite(){
		sr.enabled = false;
		coll.size = collVecOrig;
		Debug.Log ("Sprites Disabled");
	}


	void DrawPath(Vector3 start, Vector3 end, Color color, float duration)
	{
		GameObject myLine = new GameObject();
		myLine.transform.position = start;
		myLine.AddComponent<LineRenderer>();
		LineRenderer lr = myLine.GetComponent<LineRenderer>();
		lr.material = new Material(Shader.Find("Particles/Alpha Blended Premultiply"));
		lr.SetColors(color, color);
		lr.SetWidth(0.1f, 0.1f);
		lr.SetPosition(0, start);
		lr.SetPosition(1, end);
		GameObject.Destroy(myLine, duration);
	}



	public  void processMovement(){

		if (targetPosition != null && targetPosition != position && !((Vector3.Distance(targetPosition,position) < 10) && angularThrustEfficiency < .95)&& !((Vector3.Distance(targetPosition,position) < 20) && angularThrustEfficiency < .85)&& !((Vector3.Distance(targetPosition,position) < 30) && angularThrustEfficiency < .75)) {
			//Change effective movement speed based on angle to target.
			if (angularThrustEfficiency >= .92) {
				position = Vector3.MoveTowards (position, targetPosition + fleetGo.transform.up*(1-angularThrustEfficiency), movementSpeed * Time.deltaTime*angularThrustEfficiency);

			}
			if (angularThrustEfficiency >= .84 && angularThrustEfficiency < .92) {
				position = Vector3.MoveTowards (position, targetPosition + fleetGo.transform.up*(3-angularThrustEfficiency), movementSpeed * Time.deltaTime*angularThrustEfficiency);

			}
			if (angularThrustEfficiency >= .78 && angularThrustEfficiency < .84) {
				position = Vector3.MoveTowards (position, targetPosition + fleetGo.transform.up*(5-angularThrustEfficiency), movementSpeed * Time.deltaTime*angularThrustEfficiency);

			}
			if (angularThrustEfficiency >= .72 && angularThrustEfficiency < .78) {
				position = Vector3.MoveTowards (position, targetPosition + fleetGo.transform.up*(8-angularThrustEfficiency), movementSpeed * Time.deltaTime*angularThrustEfficiency);

			}
			if (angularThrustEfficiency >= .64 && angularThrustEfficiency < .72) {
				position = Vector3.MoveTowards (position, targetPosition + fleetGo.transform.up*(13-angularThrustEfficiency), movementSpeed * Time.deltaTime*angularThrustEfficiency);

			}
			if (angularThrustEfficiency >= .58 && angularThrustEfficiency < .64) {
				position = Vector3.MoveTowards (position, targetPosition + fleetGo.transform.up*(16-angularThrustEfficiency), movementSpeed * Time.deltaTime*angularThrustEfficiency);

			}
			if (angularThrustEfficiency >= .5 && angularThrustEfficiency < .6) {
				position = Vector3.MoveTowards (position, targetPosition + fleetGo.transform.up*(18-angularThrustEfficiency), movementSpeed * Time.deltaTime*angularThrustEfficiency);

			}
			if (angularThrustEfficiency >= .4 && angularThrustEfficiency < .5) {
				position = Vector3.MoveTowards (position, targetPosition + fleetGo.transform.up*(28-angularThrustEfficiency), movementSpeed * Time.deltaTime*angularThrustEfficiency);

			}
			}


	}





	//______________________________________________________________________
	GameObject SpritesGo;  //these two lines prepare sprites to be loadedf rom the container object
	Sprites sprites;

	public string fleetName;

	List<Ships>FleetShips = new List<Ships>(); //all ships in this fleet

	public bool removeSlowShips = false; //TODO

	public bool registered = false; //has been added to the list of all fleets

	//Gameobject that represents this fleet
	public GameObject fleetGo;

	SpriteRenderer sr;

	public float movementSpeed; //current move speed
	public float mMovementSpeed; //max move speed

	//Modifier to movement speed based on angle to target and turning speeed
	float angularThrustEfficiency = 1;


	Vector3 position; //coordinates on map
		public Vector3 Position {
		get {
			return position;
		}
		set {
			//this shouldnt be needed, but just in case something goes wrong with our z axis
			value.z = 0;
			position = value;
			updateMovement (); //whenever fleet position is changed this function will be called
		}
	}

	//Where we want to go
	Vector3 targetPosition;

	public Vector3 TargetPosition {
		get {
			return targetPosition;
		}
		set {
			targetPosition = value;
			if (targetPosition != null) {
				//Point at the target location to maximize angular thrust
				LookTowards ();
			}

		}
	}

	Quaternion targetRot;

	void LookTowards(){
		Vector3 dir = targetPosition - position;
		var angle = Mathf.Atan2 (dir.y, dir.x) * Mathf.Rad2Deg +180; //subtract 90 because our sprites front is up instead of right
		//fleetGo.transform.rotation;
		targetRot = Quaternion.AngleAxis (angle, Vector3.forward);
		targetRot.eulerAngles.Set(-90, targetRot.eulerAngles.y, targetRot.eulerAngles.z);
	}

	void updateMovement(){
		//Make sure each ships position matches the overall fleet position in case one gets seperated later
		foreach (Ships ship in FleetShips) {
			if (ship.movementSpeed >= movementSpeed) { 
				ship.Position = position;
			} else if(removeSlowShips){
				removeFromFleet (ship); //Remove ships that cant keep up if neccessary
			}
			fleetGo.transform.position = position;
		}
	}

	void removeFromFleet(Ships ship){
		FleetShips.Remove (ship);
		ship.assignedFleet = null;
	}



	void setMovementSpeed(){
		float max = 10;

		foreach (Ships ship in FleetShips) {
			if (ship.movementSpeed < max) { //lower max movement speed to whatever the slowest ships is
				max = ship.movementSpeed;
			}
		}
		mMovementSpeed = max;
		if (movementSpeed > mMovementSpeed) {
			movementSpeed = mMovementSpeed;
		}
		movementSpeed = mMovementSpeed; //TODO
	}






	// Use this for initialization
	void Start () {
		SetupGo ();
		SetupFleets ();
		SetupCollider ();
		SetupSprite ();
		AddList ();
		Debug.Log ("Fleet created: " + fleetName);

	}

	//Setup_________________________________________________________________________________________________
	void SetupGo(){
		fleetGo = this.gameObject;
		if (fleetGo == null) {
			Debug.Log ("Fatal GameObject load error");
		}

		SpritesGo = GameObject.FindGameObjectWithTag ("GameController");
		sprites = SpritesGo.GetComponent<Sprites>();
		fleetSprite = sprites.FleetSprite;
		if (fleetSprite == null) {
			Debug.Log ("Fleet sprite load error");
		}

		fleetGo.transform.position.Set(position.x, position.y, position.z);
		fleetGo.AddComponent<SpriteRenderer>();

		selectionEffect = fleetGo.GetComponentInChildren<ParticleSystem> ();
		SetGlow = false;
	}

	public void SetupFleets(){
		position = new Vector3(fleetGo.transform.position.x, fleetGo.transform.position.y,0);
		targetPosition = position;
		FleetShips = new List<Ships> ();
		FleetsList = GameObject.FindGameObjectWithTag ("Lists").GetComponent<Lists>().FleetsList;
		fleetName = "debugFleet";
	}

	void SetupCollider(){
		coll = fleetGo.GetComponent<BoxCollider> ();
		collVecOrig = coll.size;
	}

	void SetupSprite(){
		sr = fleetGo.GetComponent<SpriteRenderer> ();
		sr.sprite = fleetSprite;
		sr.enabled = true;
		//We need to adjust the z, otherwise it wont detect clicks since sprites have no thickness
		collVecSprite = new Vector3(sr.sprite.bounds.size.x,sr.sprite.bounds.size.y, 10);
	}

	void AddList(){
		FleetsList.Add (this);
		registered = true;
		Debug.Log (FleetsList.Count);
	}

	//__________________________________________________________________________________________________

	// Update is called once per frame
	void Update() {

		if (!localPlayerAuthority) {
			return;
		}

		setMovementSpeed ();
	

		//rotates over time towards target
		fleetGo.transform.rotation = Quaternion.Slerp (fleetGo.transform.rotation, targetRot, rotationSpeed*Time.deltaTime);
		angularThrustEfficiency = 1f - (Quaternion.Angle (targetRot, fleetGo.transform.rotation)) / 180f;


		processMovement ();
		fleetGo.transform.position = position;

		//Draw a waypoint line to current target location
		if (position != targetPosition) {
			DrawPath (position, targetPosition, Color.green, Time.deltaTime);
		}

	}





}
