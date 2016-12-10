using UnityEngine;
using System.Collections;


public class NaturalResources : NonShipEntity {



    //Arrays start at 0, so try to remember to keep the indices organized as such


	//how fast things mine, later we'll move this over to individual mines
	public const int MiningRate = 10;

    //TODO
    //set mass and volume per unit and rebalance once cargo components are designed

    //set this equal to the number of enum coices
    public const int numberOfResources = 2;
    //enum holds the values (ie, the array locations) for each resource.
	public enum NaturalResourcesType{ FUEL = 0, STEEL = 1}; //TODO add more later

    //whats still in the ground
	public float[] ResourcesRemaining = new float[numberOfResources]{100,100};
    //%access effieciency
	public float[] Availability = new float[numberOfResources]{1,1};
    //usable resources
	public float[] ResourcesMined = new float[numberOfResources]{0,0};


	public NaturalResources(float[] remaining, float[] availability, float[] mined)
    {
        ResourcesRemaining = remaining;
        Availability = availability;
		ResourcesMined = mined;
    }







    //deprecated
    /*
	public NaturalResourcesType NaturalResourceType;

	//how effective mining this resource will be, between .01 - 1.0 
	//If a mine is rated for 1000 units per turn, and accessibility is .5, it will only mine 500 units of this resource.
	float accessibility; 

	public float amountUnavailable; //how much of this resource is in the ground
	public float amountAvailable; //how much has been mined and is ready for immediate use

	public NaturalResources( NaturalResourcesType t, float amountUn, float amountA = 0,  float access = 1){
		NaturalResourceType = t;
		type = NonShipEntityType.RESOURCE;

		accessibility = access;
		amountAvailable = amountA;
		amountUn = amountUnavailable;

		volumePerUnit = 1;
		massPerUnit = 1;
	}

    */



}
