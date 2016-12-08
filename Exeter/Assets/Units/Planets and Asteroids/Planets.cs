using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Planets : AstroBody {

	//Planets have a gravity well and orbit either the sun or other planets.  Moons are considered planets


	//A list of every planet in the system, used for easy pathfinding later
	public static List<Planets> PlanetList = new List<Planets>();

	   //List used for all ships in orbit, these will be locked to the planets rotation and revolution without using fuel 
    public List<Fleets> InOrbitList;

    //Gravity well, used for determining what the planet carries with it, maybe for orbital mechanics later.
    public GameObject GravityWell;
    public float gravityStrength; //TODO

	public GameObject planetOrbitGo;
    public OrbitAlphaController planetOrbitAlphaController;
	bool orbitSelected = false;

	 void selectOrbit(){
		orbitSelected = true;
        planetOrbitAlphaController.VisiblePercent = OrbitAlphaController.selectedVisibility;
	}

	 void unselectOrbit(){
		orbitSelected = false;
        planetOrbitAlphaController.VisiblePercent = OrbitAlphaController.regularVisibility;
    }

	public void checkOrbitSelection(){
		Debug.Log ( BodyGo.name + " is selected!" + !orbitSelected);
		switch (orbitSelected) {
		case true:
			unselectOrbit ();
			break;
		case false:
			selectOrbit ();
			break;
		}
	}

	void getAlphaController(){
        planetOrbitAlphaController = BodyGo.GetComponent<OrbitAlphaController>();
	}

    void getGravityWell()
    {
        GravityWell = BodyGo.transform.FindChild("GravityWell").gameObject;
    }

	// Use this for initialization
	new void Start () {
		BodyType = AstroBodyType.PLANET;
		BodyGo = this.gameObject;
		position = BodyGo.transform.position;
		getAlphaController ();
        getGravityWell();
	}

    private void Awake()
    {
        PlanetList.Add(this);
    }

    // Update is called once per frame
	new void Update () {
		base.Update ();
		position = BodyGo.transform.position;
	}

    public Vector3 GetPlanetPosition()
    {
        return position;
    }

}
