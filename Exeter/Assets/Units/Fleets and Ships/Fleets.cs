using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;

public class Fleets : NetworkBehaviour {



	public const float mFuel = 20;
	public float fuel = mFuel;
	public float Fuel {
		get {
			return fuel; 
		}
		set {
			fuel = value;
			if (fuel <= 0) {
				fuel = 0;
				hasFuel = false;
				endMission ();
			}
		}
	}

	bool hasFuel = true;

    /*All movement and actions are performed by fleets rather than individual ships.  Though a fleet can consist of a single ship of course.
	Behaviors:

        //LOLJK FUCK THAT THIS IS NOW THE CLASS FOR INDIVIDUAL SHIPS BECAUSE I SAY SO SHITLORDSSSSS
        //it handles all the mechanics for the ships, but doesnt handle components or individual stats, these are imported from a ShipClass

	Fleets move at the speed of their slowest member
	They reduce speed instantly on taking damage or remove lagging ships depending on settings */

    //tbh this will likely become a ship class rather than fleets.  

	public GameObject CenterOfGravity;

    //MISSION, EXPERIMENTAL [it work nigga]
    void intializeMissionVariables()
    {
        PlanetsMissionList.Add(planetOneContainer);
        PlanetsMissionList.Add(planetTwoContainer);
        PlanetsMissionList.Add(planetThreeContainer);
        PlanetsMissionList.Add(planetFourContainer);

        FleetsMissionList.Add(fleetOneContainer);
        FleetsMissionList.Add(fleetTwoContainer);

        noeMissionList.Add(noeOneContainer);
        noeMissionList.Add(noeTwoContainer);
    }

    int missionStage = 0; //0 is no mission
    
   public int MissionStage
    {
        get
        {
            return missionStage;
        }
        set
        {
            missionStage = value;
            Missions.CheckMission(AssignedMission, missionStage, this);
        }
    }

    public Missions.MissionType AssignedMission = Missions.MissionType.NONE;

    public void AssignMission(Missions.MissionType mis)
    {
        Debug.Log("Assigned mission!");
        AssignedMission = mis;
        missionStage = 1;
        OnMission = true;
    }
    //Am i assigned a mission
    bool OnMission = false;
    //This stage consists of moving from one position to another, will advance stage when arrived
    public bool missionInTransit = false;

     bool targetIsPlanet = false;
     Planets targetPlanet;

    public void setTargetPlanet(Planets planet)
    {
        targetPlanet = planet;
        targetIsPlanet = true;
    }

    public void unsetTargetPlanet()
    {
        targetPlanet = null;
        targetIsPlanet = false;
     }

    public void MissionLoadItem()
    {
        // load an item
        //TODO
        Debug.Log("Loaded item!");
        MissionStage = 3;
    }

    public void MissionUnloadItem()
    {
        // load an item
        //TODO
        Debug.Log("Unloaded item!");
        MissionStage = 5;
    }

    //These are all used to store our variables over the mission stages

    public float floatContainerOne;
    public float floatContainerTwo;

    public Planets planetOneContainer;
    public Planets planetTwoContainer;
    public Planets planetThreeContainer;
    public Planets planetFourContainer;

    public List<Planets> PlanetsMissionList = new List<Planets>();

    public Fleets fleetOneContainer;
    public Fleets fleetTwoContainer;

    public List<Fleets> FleetsMissionList = new List<Fleets>();

    public NonShipEntity noeOneContainer;
    public NonShipEntity noeTwoContainer;

    public List<NonShipEntity> noeMissionList = new List<NonShipEntity>();


    public void endMission()
    {
		TargetPosition = position;
		if (targetPlanet != null) {
			Orbit (this, targetPlanet.GravityWell.transform);
		} else
			Orbit (CenterOfGravity.transform);
        //null our all of the containers and stuff TODO
        missionStage = 0;
        AssignedMission = Missions.MissionType.NONE;
        OnMission = false;
		targetIsPlanet = false;
		targetPlanet = null;
		missionInTransit = false;
    }

	public static void Orbit(Fleets fleet, Transform transform){
		fleet.orbiting = true;
		fleet.fleetGo.transform.SetParent (transform);
	}

	public static void DeOrbit(Fleets fleet, Transform transform){
		fleet.orbiting = false;
		fleet.fleetGo.transform.SetParent (null);
	}

	public void Orbit(Transform transform){
		orbiting = true;
		fleetGo.transform.SetParent (transform);
	}

	public void DeOrbit(){
		orbiting = false;
		fleetGo.transform.SetParent (null);
	}

    //This where we put the actual ships stats and components 
    //name of the ship, fluff
    string name;
    //this individual shipclass
    ShipClass shipClass;
    //pointer to the generic ShipClass object
    ShipClass parentShipClass;




















	//Statics_________________________________________________________
    public Factions.FACTION Faction;
    //reference ot the list in our lists container.  Only used for assigning, we should move this to the initializer later
    List<Fleets> FleetsList;

	public static Sprite fleetSprite;
	ParticleSystem selectionEffect;

	BoxCollider coll;
	Vector3 collVecOrig;
	Vector3 collVecSprite;

	Vector3 minRange = new Vector3 (-29.4f,-31.8f,0f) - new Vector3 (-10.3f, -31f, 0f);

	float rotationSpeed = .2f;

	bool setGlow;
	bool orbiting = false;
	public bool Orbiting { 
		get { return orbiting; } 
		set {
			bool atPlanet = false;
			foreach (Planets planet in Planets.PlanetList) {
				if (planet.GravityWell.GetComponent<Collider> ().bounds.Contains (position)) {
					Orbit (planet.GravityWell.transform);
					atPlanet = true;
				}
				if (!atPlanet) {
					Orbit (CenterOfGravity.transform);
				}
			} 
		}
	}

	public bool SetGlow { //enable or disable the glow effect 
		get {
			return setGlow;
		}
		set {
			setGlow = value;
			selectionEffect.gameObject.SetActive( value);
		}
	}

    //order the ship where to go.  Assigning to TargetPosition does the same, but this is a bit neater
    public void MoveTo(Vector3 targ)
    {
        TargetPosition = targ;
    }


	public void enableSprite(){
		sr.enabled = true;
		coll.size = collVecSprite; //resize collider to sprite
	//	Debug.Log ("Sprites enabled");
	}

	public void disableSprite(){
		sr.enabled = false;
		coll.size = collVecOrig;
	//	Debug.Log ("Sprites Disabled");
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
		float angularThrustFactor = 10000;
		if (targetPosition != null && targetPosition != position && hasFuel) {
			//Change effective movement speed based on angle to target.
		
			/*if (angularThrustEfficiency >= .92) {
				angularThrustFactor = 1;
			}
			if (angularThrustEfficiency >= .84 && angularThrustEfficiency < .92) {
				angularThrustFactor = 15;

			}
			if (angularThrustEfficiency >= .78 && angularThrustEfficiency < .84) {
				angularThrustFactor = 20;

			}
			if (angularThrustEfficiency >= .78) {
				angularThrustFactor = 100 * (1 - angularThrustEfficiency);
			}
			if (angularThrustEfficiency >= .72 && angularThrustEfficiency < .78) {
				angularThrustFactor = 100 * (1 - angularThrustEfficiency) + 20;

			} */
			if (angularThrustEfficiency >= .72) {
				angularThrustFactor = 100 * (1 - angularThrustEfficiency);

			}
			if (angularThrustEfficiency >= .64 && angularThrustEfficiency < .72) {
				angularThrustFactor = 250;

			}
			if (angularThrustEfficiency >= .58 && angularThrustEfficiency < .64) {
				angularThrustFactor = 1000;

			}
			if (angularThrustEfficiency >= .5 && angularThrustEfficiency < .6) {
				angularThrustFactor = 10000;

			}
			if (angularThrustEfficiency >= .4 && angularThrustEfficiency < .5) {
				angularThrustFactor = 10000;
			}
		//	position = Vector3.MoveTowards (position, targetPosition + fleetGo.transform.up*(angularThrustFactor-angularThrustEfficiency), movementSpeed * Time.deltaTime*angularThrustEfficiency);
			position = Vector3.MoveTowards (position, targetPosition, movementSpeed *(1/angularThrustFactor) * Time.deltaTime*angularThrustEfficiency);

			Fuel -= 1*Time.deltaTime;

			if (Vector3.Distance (TargetPosition, position) < this.movementSpeed*Time.deltaTime*angularThrustEfficiency) {
				this.position = TargetPosition;
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
			if (!hasFuel) {
				Debug.Log ("Out of fuel...");
				return;
			}
			orbiting = false;
			DeOrbit ();
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
		float max = 1000;

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

		CenterOfGravity = GameObject.FindWithTag ("SystemGravity");
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

        //when we reach our targetposition, update the mission
        //must be within the lower third of the gravity well.  We use this because the planet is constantly moving and alot ofthe time the ship cant make it to the exact point in a reasonable time
		if (OnMission) {
			
			if (targetIsPlanet) {
				TargetPosition = targetPlanet.GetPlanetPosition();
				float orbitalDistance = targetPlanet.gameObject.transform.localScale.x;
				if(targetPlanet.gameObject.transform.parent.parent.name == "Earth"){
					orbitalDistance *= 6f;
				}
				if(targetPlanet.transform.parent.parent.name == "Mars"){
					orbitalDistance *=1.8f;
				}
				if (Vector3.Distance (this.position, targetPlanet.GetPlanetPosition ()) < (targetPlanet.GravityWell.transform.localScale.x * orbitalDistance *.9) && missionInTransit) {
					Debug.Log ("Reached mission waypoint");
					MissionStage += 1;
				}
			}

		}

 

        //rotates over time towards target
        fleetGo.transform.rotation = Quaternion.Slerp (fleetGo.transform.rotation, targetRot, rotationSpeed*Time.deltaTime);
		angularThrustEfficiency = 1f - (Quaternion.Angle (targetRot, fleetGo.transform.rotation)) / 180f;


		processMovement ();
		if (orbiting) {
			position = fleetGo.transform.position;
			targetPosition = position;
		}


			fleetGo.transform.position = position;
		
		if (position == TargetPosition) {
			Orbiting = true;
		}


		//Draw a waypoint line to current target location
		if (position != targetPosition) {
			DrawPath (position, targetPosition, Color.green, Time.deltaTime);
		}


	}

   





}
