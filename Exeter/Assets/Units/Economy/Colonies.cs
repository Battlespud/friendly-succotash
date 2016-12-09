using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Colonies : MonoBehaviour {

	public static int ReproductionTimer = 300; //how often in seconds to pop growth
	public static float ReproductionRate = .02f;

	public Colonies(string name = "wut a nice place lol"){
		Name = name;
	}


	public string Name;
	public Factions.FACTION Faction;

	public List<POP> Population;



	public int PopulationCount{
		get{ return Population.Count; }
	}

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

	public void initializeList ()
	{
		Population = new List<POP>();
	}

	public static void FoundColony(AstroBody body){
		Colonies c;
		c = body.BodyGo.AddComponent<Colonies>();
		c.initializeList ();
		c.Name = "America";
		Debug.Log ("Made a place named " + c.Name + " on " + body.Name);
		c.AddRandomColonist ();
	}

	public void AddRandomColonist(){
		POP colonist = new POP ();
		int i = Random.Range (0, 2);
		if (i == 0) {
			colonist.firstName = "Tim";
			colonist.lastName = "Kaine";
			colonist.gender = POP.Gender.Female;
			colonist.race = POP.Race.Cuck;
		} else {
			colonist.firstName = "Mike";
			colonist.lastName = "Pence";
			colonist.gender = POP.Gender.Male;
			colonist.race = POP.Race.White;
		}
		registerPop (colonist);
		Debug.Log (colonist.Name + " is at " + Name);
	}

	public void registerPop(POP pop)
	{
		Population.Add (pop);
		Debug.Log (PopulationCount);
		int patriots = 0;
		int cucks = 0;
		foreach (POP colonist in Population) {
			if (colonist.race == POP.Race.Cuck) {
				cucks++;
			}
			if (colonist.race == POP.Race.White) {
				patriots++;
			}
		}
		Debug.Log (this.Name + " has " + cucks + " useless cucks, and " + patriots + " nimble navigators. Total Population: " + PopulationCount );
	}

}
