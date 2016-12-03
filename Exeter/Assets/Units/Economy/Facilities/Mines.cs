using UnityEngine;
using System.Collections;

public class Mines : Facilities {

    //What this mine will... mine
	public NaturalResources.NaturalResourcesType targetResource;

    //how much it mines per cycle, we'll figure this out later TODO:
    public  const float eff = 10;

	public Mines(NaturalResources.NaturalResourcesType targResource){
		type = NonShipEntityType.FACILITY;
		facilityType = FacilityType.MINE;
		targetResource = targResource;
		massPerUnit = 1000;
		volumePerUnit = 1000;
	}




}
