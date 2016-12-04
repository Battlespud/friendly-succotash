using UnityEngine;
using System.Collections;

public static class Factions {


	//A Faction represents either a government, rebel group or commercial organization.  They can be created dynamically.


    public enum FACTION { PIRATE = 0, EARTH = 1, MARS = 2 };
    public static string[] FactionNames = new string[3]{ "Pirates", "United Nations", "Mars Congressional Republic" };


}
