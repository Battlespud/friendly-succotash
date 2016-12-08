using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AstroBody : MonoBehaviour {

	//parent class for all planets, asteroids etc.  This is what allows for colonies




	//this gameobject
	public GameObject BodyGo;

	//All the colonies here
	public List<Colonies> ColoniesPresentList;

	//default atmosphere, update in the specific classes
	Atmospheres Atmosphere;

	//location in space, will be updated with the rotation automatically
	public Vector3 position;

	//Is this a planet (planets are expected to have gravity wells) or an asteroid (can't orbit an asteroid and by default wont have an atmosphere)
	public enum AstroBodyType{ASTEROID, PLANET};
	public AstroBodyType BodyType;

	//name grabbed from the gameobject
	public string Name;

	//Natural resources present, if any
	public NaturalResources Resources;
	//For now we'll just have predetermined
	//resources for the major planets, and we'll add a random system for everything else later.
	public void setupNaturalResources(NaturalResources template)
	{
		Resources = template;
		Debug.Log("Resources on " + BodyGo.name + " have been initialized");
	}

	void MineResources()
	{
		//TODO, just a placeholder for now
	//	Debug.Log("I should spam the console if astrobody is virtualized properly");
	}

	// Use this for initialization
	virtual public void Start () {
		
	}
	
	// Update is called once per frame
	virtual public void Update () {
		MineResources();
	}
}
