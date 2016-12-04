using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Facilities : NonShipEntity {

	public enum FacilityType{MINE,CONSTRUCTION,ACADEMY}

	public FacilityType facilityType;

    public bool activated = true; //should be passed over for all checks if disabled.
    public bool inCargo = false;  //when stored in ship for transport or something. To prevent nullref with location

    //Can this be moved by freighter?
    public bool movable;

    //   public Planets location;

        //how much it costs to build
    public float[] ResourceCost;



}
