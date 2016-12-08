using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Colonies : MonoBehaviour {

	public Colonies(string name = "wut a nice place lol"){
		Name = name;
	}


	public string Name;
	public Factions.FACTION Faction;

	public List<Facilities> FacilityList;
	public List<Mines> AllMinesList;
	public List<Mines> FuelMinesList;
	public List<Mines> SteelMinesList;
	public int[] Mines = new int[2];
	public float FuelReserves; 


	// Use this for initialization
	void Start () {
		
	}

	// Update is called once per frame
	void Update () {
		
	}

	public static void FoundColony(AstroBody body){
		Colonies c = new Colonies ();
		c = body.BodyGo.AddComponent<Colonies>();
		c.name = "wut a nice place lol";
	}


}
