using UnityEngine;
using System.Collections;

public class Mines : Facilities {

    //What this mine will... mine
	public NaturalResources.NaturalResourcesType targetResource;

	public bool mineActive = true;
    //how much it mines per cycle, we'll figure this out later TODO:
    public float eff = 10;

	public Mines(NaturalResources.NaturalResourcesType targResource){
        targetResource = targResource;

        type = NonShipEntityType.FACILITY;
		facilityType = FacilityType.MINE;
		massPerUnit = FacilityCosts.mineMass;
		volumePerUnit = FacilityCosts.mineVolume;
        ResourceCost = FacilityCosts.MineCost;
        movable = FacilityCosts.mineMovable;
	}

	public void updateEff(){
		//decide how effiecent this will be 
	}
    
}
