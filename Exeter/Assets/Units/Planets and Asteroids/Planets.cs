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
    public float gravityStrength;

    //How Resources Work
    //Need 1 list for each resource type, and 1 overall




    //Refined fuel ready for use
    public float FuelReserves; 

    //import template from specific planet script


	//fetch the gameobject that represents the planet and keep our position here up to date with its coordinates

	GameObject planetGo;
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
		Debug.Log ( planetGo.name + " is selected!" + !orbitSelected);
		switch (orbitSelected) {
		case true:
			unselectOrbit ();
			break;
		case false:
			selectOrbit ();
			break;
		}
	}

	//we'll set this from the script relating to the specific planet, not here
    //deprecated for now
    /*
	//Returns a string listing amount available of each resource
	public string getResourceString(){ 
		string resourceString = System.String.Empty; //initialize to empty
		foreach (NaturalResources r in PlanetNaturalResourcesList) {
			resourceString += r.type.ToString() +": " + r.amountAvailable + "; ";
		}
		return(resourceString);
	}

    */

	void getAlphaController(){
        planetOrbitAlphaController = planetGo.GetComponent<OrbitAlphaController>();
	}

    void getGravityWell()
    {
        GravityWell = planetGo.transform.FindChild("GravityWell").gameObject;
    }

	// Use this for initialization
	new void Start () {
		planetGo = this.gameObject;
		BodyGo = this.gameObject;
		position = planetGo.transform.position;
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
		position = planetGo.transform.position;
	}

    public Vector3 GetPlanetPosition()
    {
        return position;
    }

}
