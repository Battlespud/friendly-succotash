using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Colonies : MonoBehaviour {



	public string Name;
	public Factions.FACTION Faction;

	public List<Facilities> FacilityList;
	public List<Mines> AllMinesList;
	public List<Mines> FuelMinesList;
	public List<Mines> SteelMinesList;
	public int[] Mines = new int[2];


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
