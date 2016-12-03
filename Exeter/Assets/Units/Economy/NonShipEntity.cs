using UnityEngine;
using System.Collections;

public class NonShipEntity {

	//all non ship things will inhereit from here
	//Since these things wont appear on screen directly, update or interact with other objects, we dont need them to use
	//the unity functions.  Effectively, this is the spreadsheet part of the game
	// ex. Resources, facilities

	public float volumePerUnit;
	public float massPerUnit;

	public enum NonShipEntityType{ FACILITY, RESOURCE, COMPONENT, POPULATION };

	public NonShipEntityType type;  





}
