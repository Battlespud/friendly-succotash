using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Planets : MonoBehaviour {

	//This component will be added to every celestial body that we want to have resources
	//or have any real relevance to the game.  For now we'll just have predetermined
	//resources for the major planets, and we'll add a random system for everything else later.

	//A list of every planet in the system, used for easy pathfinding later
	public static List<Planets> PlanetList = new List<Planets>();



    //List used for all ships in orbit, these will be locked to the planets rotation and revolution without using fuel 
    public List<Fleets> InOrbitList;

    //Gravity well, used for determining what the planet carries with it, maybe for orbital mechanics later.
    public GameObject GravityWell;
    public float gravityStrength;

    //How Resources Work
    //Need 1 list for each resource type, and 1 overall
	public List<Facilities> FacilityList;
    public List<Mines> AllMinesList;
    public List<Mines> FuelMinesList;
    public List<Mines> SteelMinesList;
    public NaturalResources Resources;
    public int[] Mines = new int[2];

    void MineResources()
    {
        //TODO, just a placeholder for now

    }

    //Refined fuel ready for use
    public float FuelReserves; 

    //import template from specific planet script
    public void setupNaturalResources(NaturalResources template)
    {
        Resources = template;
        Debug.Log("Resources on " + planetGo.name + " have been initialized");
    }

	//fetch the gameobject that represents the planet and keep our position here up to date with its coordinates
	Vector3 position;
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
	public string Name;


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



	// Use this for initialization
	void Start () {
		planetGo = this.gameObject;
		position = planetGo.transform.position;
		getAlphaController ();
	}

    private void Awake()
    {
        PlanetList.Add(this);
    }

    // Update is called once per frame
    void Update () {
		position = planetGo.transform.position;
        MineResources();
	}
}
