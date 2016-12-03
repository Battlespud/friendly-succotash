using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;



public class Characters {
	static int i;
	public static List<Characters> CharacterList;

	public static string[] RankNames;

	static string rank1 = "Lieutenant Commander";
	static string rank2 = "Commander";
	static string rank3 = "Captain";
	static string rank4 = "Lord Captain";
	static string rank5 = "Commodore";
	static string rank6 = "Admiral";
	static string rank7 = "Fleet Admiral";


	public static void resetRankNames(){
		RankNames = new string[]{ rank1, rank2, rank3, rank4, rank5, rank6, rank7 };
	}


	 const int maxRank = 7;
	 const int minRank = 1;

	int rank;

	int idNumber;

	string firstName;
	string lastName;

	int age = 28;

	string background = "Conscript";
	string homeworld = "Earth"; //where the military academy they trained at is located.  affects loyalty

	public string report;

	float combatXP; //gain combat traits
	float staffXP; //gain traits benefitting staff officers

	float merit; //whether should be promoted


	//record keeping
	int kills; 
	int shipsLost;
	string[] shipLostNames;


}
