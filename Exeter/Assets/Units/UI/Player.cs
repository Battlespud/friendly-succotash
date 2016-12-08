using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Player : NetworkBehaviour {

	List<Fleets> PlayerFleets = new List<Fleets> ();
	List<Colonies> PlayerColonies = new List<Colonies>();

	public Factions.FACTION Faction;








	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
