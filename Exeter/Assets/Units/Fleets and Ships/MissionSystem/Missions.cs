using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Missions  {

    //a mission is multiple chained events in a certain sequence. They are used by AI ships primarily, but we should be able to automate player shipsthe same way tbh.


        public enum MissionType { NONE, TRANSPORT, MOVETOPLANET};


	public static void CheckMission(MissionType mission, int stage, Fleets fleet)
	{
		Debug.Log("CheckMission() triggered");
		switch (mission)
		{
		case MissionType.TRANSPORT:
			HandleTransportMission(stage, fleet);
			break;
		
		case MissionType.MOVETOPLANET:
			HandleMoveToPlanetMission (stage, fleet);
			break;

		default:
			break;
		}

		return;
	}



	public static void MoveToPlanetMission (Fleets fleet, Planets destination){
		fleet.MoveTo (destination.gameObject.transform.position);
		fleet.AssignMission (MissionType.MOVETOPLANET);
		fleet.planetOneContainer = destination;
		fleet.missionInTransit = true;
		fleet.setTargetPlanet (destination);

	}

	public static void MoveToPlanetMission (Fleets[] fleets, Planets destination){
		foreach (Fleets fleet in fleets) {
			fleet.MoveTo (destination.gameObject.transform.position);
			fleet.AssignMission (MissionType.MOVETOPLANET);
			fleet.planetOneContainer = destination;
			fleet.missionInTransit = true;
			fleet.setTargetPlanet (destination);
		}
	}

	public static void MoveToPlanetMission (List<Fleets> fleets, Planets destination){
		foreach (Fleets fleet in fleets) {
			fleet.MoveTo (destination.gameObject.transform.position);
			fleet.AssignMission (MissionType.MOVETOPLANET);
			fleet.planetOneContainer = destination;
			fleet.missionInTransit = true;
			fleet.setTargetPlanet (destination);
		}
	}

	public static void HandleMoveToPlanetMission(int stage, Fleets fleet){
		if (stage == 2) {
			Debug.Log ("Successfully moved to " + fleet.planetOneContainer.name);
			fleet.endMission ();
		} else {
			Debug.Log ("Something has gone terribly wrong with MoveToPlanet Mission.");
		}
	}






    //TransportMission
    public static void TransportMission(NonShipEntity cargo, Fleets fleet, Planets origin, Planets destination, float amount)
    {
        //Go to where we need to pickup cargo from
        fleet.MoveTo(origin.gameObject.transform.position);
        fleet.AssignMission(MissionType.TRANSPORT);
        fleet.planetOneContainer = origin;
        fleet.planetTwoContainer = destination;
        fleet.floatContainerOne = amount;
      //  fleet.noeOneContainer = cargo;
        fleet.missionInTransit = true;
        fleet.setTargetPlanet(origin);
    }

    static void HandleTransportMission(int stage, Fleets fleet)
    {
        switch (stage)
        {
            case 2: //We have arrived at the origin planet
                //TODO
                //pick up the stuff
                fleet.missionInTransit = false;
                fleet.unsetTargetPlanet();
                fleet.MissionLoadItem();
                break;

            case 3:
                fleet.MoveTo(fleet.planetTwoContainer.gameObject.transform.position);
                fleet.setTargetPlanet(fleet.planetTwoContainer);
                fleet.missionInTransit = true;
                break;

            case 4: //We have arrived at destination planet
                //TODO
                //unload the stuff
                fleet.missionInTransit = false;
                fleet.MissionUnloadItem();
                break;
            case 5: //All done!
                fleet.endMission();
                break;
            default:
                Debug.Log("Something has gone terribly wrong with the mission of " + fleet);
                break;

        }


    }









}
