using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Colonies : MonoBehaviour {

	public static int ReproductionTimer = 300; //how often in seconds to pop growth
	public static float ReproductionRate = .02f;

	public AstroBody  astroBody;
	public NaturalResources Minerals;

	public Colonies(string name = "wut a nice place lol"){
		Name = name;
	}


	public string Name;
	public Factions.FACTION Faction;

	public List<POP> Population;



	public int PopulationCount{
		get{ return Population.Count; }
	}

	public List<Mines> MinesList = new List<Mines> ();
	public List<Facilities> FacilityList;
	public List<Mines> FuelMinesList;
	public List<Mines> SteelMinesList;
	public int[] ColonyMines;
	public float FuelReserves; 


	// Use this for initialization
	void Start () {
		Minerals = this.gameObject.GetComponent<AstroBody> ().Resources;
		ColonyMines = new int[2]{1,1};
		Debug.Log (Minerals.ResourcesRemaining [0] + " " + Minerals.Availability [0] + " " + Minerals.ResourcesMined [0] + " " + ColonyMines[0]);
		InvokeRepeating ("ShowMinerals", 1, 1);
		InvokeRepeating ("UpdateMineEff", 5, 5);
		MinesList = new List<Mines>();
		MinesList.Add (new Mines (NaturalResources.NaturalResourcesType.FUEL));

		InvokeRepeating ("Mining", 1, 1);
	}


	// Update is called once per frame
	void Update () {
		}


	//this is if we want to make mines generic and only process according to how many of each we have
	void Mining(){
		int i = 0;
		while (i < NaturalResources.numberOfResources) {
			if (Minerals.ResourcesRemaining [i] > 0) {
				float amountMined = miningFormula (i);
			}
			i++;
		}
	}

	float miningFormula(int i ){
		float f = Minerals.Availability [i] * ColonyMines [i] * NaturalResources.MiningRate;
		Minerals.ResourcesRemaining [i] -= f;
		if (Minerals.ResourcesRemaining [i] < 0) {
			f += Minerals.ResourcesRemaining [i];
			Minerals.ResourcesRemaining [i] = 0;
		}
		Minerals.ResourcesMined [i] += f;
		return f;
	}

	//If we want mines to be more complex and require pops etc then we should use this
	void DoMining()
	{
		foreach (Mines mine in this.MinesList) {
			float amountMined = mine.eff * NaturalResources.MiningRate * Time.deltaTime * Minerals.Availability[(int)mine.targetResource];
			Minerals.ResourcesRemaining [(int)mine.targetResource] -= amountMined;
			if (Minerals.ResourcesRemaining [(int)mine.targetResource] < 0) {
				amountMined += Minerals.ResourcesRemaining [(int)mine.targetResource];
				Minerals.ResourcesRemaining [(int)mine.targetResource] = 0;
			}
			Minerals.ResourcesMined [(int)mine.targetResource] += amountMined;
		}

	}

	void UpdateMineEff()
	{
		foreach (Mines mine in MinesList) {
			mine.updateEff ();
		}

	}

	void ShowMinerals(){
		Debug.Log ("Fuel: " + Minerals.ResourcesMined [0] + " Steel:" + Minerals.ResourcesMined [1]);
	}


	public void initializeList ()
	{
		Population = new List<POP>();
	}

	public static void FoundColony(AstroBody body){
		Colonies c= body.BodyGo.AddComponent<Colonies>();
		c.astroBody = body;
		c.Minerals = body.Resources;
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
