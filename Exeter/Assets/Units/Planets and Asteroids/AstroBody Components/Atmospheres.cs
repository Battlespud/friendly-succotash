using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Atmospheres : MonoBehaviour {

	public bool breathable = false;


	// How much of each gas
	public float oxygen;
	public float nitrogen;
	public float carbon;
	public float hydrogen;
	//total atmospheric pressure
	public float pressure;
	//Determined by distance from sun
	public float baseTemp;
	//Determined by gas ratios
	public float minTemp;
	public float maxTemp;
	public float avgTemp;
	//Global warming or cooling
	public bool isWarming = false;
	public bool isCooling = false;
	//Allows various events
	public bool acidRain = false;
	public bool volcanic = false;
	public bool supercell = false;


	//Whether this even has an atmosphere or is just a barren rock.  
	//All objects will still need this component to allow for terraforming though
	public enum AtmosphereType{NONE =0, NORMAL =1}; 
	public AtmosphereType atmosphereType = AtmosphereType.NONE;

	//TODO
	public void setupPlanetAtmosphere(){
		atmosphereType = AtmosphereType.NONE;
		allGases ();
	}

	public void setupAsteroidAtmosphere(){
		atmosphereType = AtmosphereType.NONE;
		allGases ();
	}

	void allGases(float t = 0){
		oxygen = t;
		nitrogen = t;
		carbon = t;
		hydrogen = t;
		pressure = t * 4;

		//TODO
		if (t == 0) {
			avgTemp = 0;
			minTemp = 0;
			maxTemp = 0;
		}
		breathable = false;

	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
