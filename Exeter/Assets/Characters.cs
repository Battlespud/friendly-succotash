using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;



public class Characters {

    //Where the character is assigned to
    GameObject assignment;
    //List of all previous assignments, no real purpose except fluff
    List<GameObject> pastAssignments = new List<GameObject>();

    //Methods for changing assignments
    public static string WhereIs(Characters c)
    {
        return c.assignment.name;
    }

    public string WhereAmI()
    {
        return assignment.name;
    }

    public void assignTo(GameObject newAssignment)
    {
        pastAssignments.Add(assignment);
        assignment = newAssignment;
    }

    public static void assignCharacterTo(Characters c, GameObject newAssignment)
    {
        c.pastAssignments.Add(c.assignment);
        c.assignment = newAssignment;
    }

    //counter
    static int i;

    //List of all characters
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
		RankNames = new string[7]{ rank1, rank2, rank3, rank4, rank5, rank6, rank7 };
	}

    //remember that array index starts at 0, not 1.  ie. Rank1 = position 0
	 const int maxRank = 6;
	 const int minRank = 0;

	int rank;

    public int Rank
    {
        get
        {
            return rank;
        }

        set
        {
            rank = value;
            rankName = RankNames[rank];
        }
    }
    //determined by rank int and rankNames array, set automatically
    //Can be safely overwritten with custom ranks, but will be lost on rankup.
    public string rankName;

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
    List<string> killsNames = new List<string>(); 

	int shipsLost;
	List<string> shipLostNames = new List<string>();




}
