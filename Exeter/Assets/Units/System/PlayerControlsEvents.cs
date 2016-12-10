using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public static class PlayerControlsEvents  {


	//these are used to control camera zoom levels
	const int perspZoomInLimit = 100; //minimum distance from plane
	const int perspZoomOutLimit = 20000; //max distance from plane
	const int perspZoomSpeed = 20000;

	const int orthoZoomInLimit = 5; //smallest size
	const int orthoZoomOutLimit = 5000; //max size of orthographic view, bigger is much more intensive
	const int orthoZoomSpeed = 1500; //multiplier for mouse input

	public static void MoveCamera(Camera cam, Vector3 diff){
		cam.transform.transform.Translate(diff);
	}

	public static void MoveCameraTo(Camera cam, Vector3 targetPosition){
		cam.transform.position = targetPosition;
		restoreCameraDistance (cam);
	}

	public static void RotateCamera(Camera cam, Vector3 diff){
	//enabling enabling float x will allow for horizontal rotation, but causes issues with z axis as well.
	float x = 0f; // diff.x;
	float y = diff.y * -1;
	float z = 0f;
	Quaternion testternion = cam.transform.rotation;
	Vector3 mappedDiff = new Vector3 (y, x, 0f);
	GameObject testObject = new GameObject();
	testObject.transform.position = cam.transform.position;
	testObject.transform.rotation = cam.transform.rotation;
	testObject.transform.Rotate (mappedDiff * (Time.deltaTime * 3.5f));
	testternion = testObject.transform.rotation;
//	Debug.Log(testObject.transform.rotation.x);
	if (testternion.eulerAngles.x > 59 && testternion.eulerAngles.x < 300) {
		Debug.Log ("Rotation will break! Aborting!");
			GameObject.Destroy (testObject);
			return;
	}
		GameObject.Destroy (testObject);
	cam.transform.Rotate (mappedDiff*(Time.deltaTime*3.5f));
		return;
	}

	public static void Zoom(Camera cam, Vector3 diff){
		switch (cam.orthographic) { //different camera modes handle zooming differently.  Perspective actually moves the camera, while ortho changes the canvas size to simulate it
		case true:
			{	
				float f = cam.orthographicSize - Input.GetAxis ("Mouse ScrollWheel")*orthoZoomSpeed;
				if (f > orthoZoomInLimit && f < orthoZoomOutLimit) {//how close you can zoom in or how far y ou can zoom out
					cam.orthographicSize = f;
				}
				//Check if we need to enable or disable the fleet sprites based on zoom range
				break;
			}
		case false:
			{
				diff.z = Input.GetAxis ("Mouse ScrollWheel") * perspZoomSpeed*Time.deltaTime;
				if ((diff.z < 0 && cam.transform.position.z > perspZoomOutLimit * -1) || (diff.z > 0 && cam.transform.position.z < perspZoomInLimit * -1)) {
					cam.transform.Translate (diff);
				}
				break;
			}
		}
	}

	public static void ZoomOutMax(Camera cam){
		cam.orthographicSize = orthoZoomOutLimit;
		cam.transform.position.Set(cam.transform.position.x,cam.transform.position.y,perspZoomOutLimit * -1);
	}

	public static void ZoomInMax(Camera cam){
		cam.orthographicSize = orthoZoomOutLimit;
		cam.transform.position.Set(cam.transform.position.x,cam.transform.position.y,perspZoomInLimit * -1);
	}

	static void restoreCameraDistance(Camera cam){
		cam.transform.position = new Vector3(cam.transform.position.x,cam.transform.position.y, -50);
		Debug.Log ("Restoring camera Z: " + cam.transform.position.z);
	}


	public static void zoomToFleet(Camera cam, List<Fleets> selectedFleets){
		cam.transform.position = selectedFleets.LastOrDefault ().Position;
		cam.orthographicSize = orthoZoomInLimit;
		restoreCameraDistance (cam);
	}

	public static void CheckFleetSprites(Camera cam, List<Fleets> FleetsList){
		switch (cam.orthographic){
		case true:
			if (cam.orthographicSize > 499) {
				foreach (Fleets fl in FleetsList) {
					fl.enableSprite ();
				}
			} else {
				foreach (Fleets fl in FleetsList) {

					fl.disableSprite ();
				}
			}
			break;

		case false:
			if (cam.transform.position.z < -250) {
				foreach (Fleets fl in FleetsList) {
					fl.enableSprite ();
				}
			} else {
				foreach (Fleets fl in FleetsList) {

					fl.disableSprite ();
				}
			}
			break;
		}
	}

	public static void FleetSpritesSwitch(bool b, List<Fleets> FleetsList){
		switch(b){
		case true:
			foreach (Fleets fl in FleetsList) {
				fl.enableSprite ();
			}
			break;
		case false:
			foreach (Fleets fl in FleetsList) {
				fl.disableSprite ();
			}
			break;
		}
	}



	public static void SelectPlanet(Camera cam, List<Planets> selectedPlanets){
		Ray ray;
		RaycastHit hit;
		ray = cam.ScreenPointToRay (Input.mousePosition);
		if (Physics.Raycast (ray, out hit)) {
			GameObject targ;
			if (hit.collider.name != "GravityWell")
			{
				targ = hit.collider.gameObject;
			}
			else
			{
				targ = hit.collider.gameObject.transform.parent.gameObject;
			}
			Debug.Log(targ.name);
			Planets hitPlanet = targ.GetComponent<Planets> ();
			Colonies.FoundColony (targ.GetComponent<AstroBody>());  //TODO Remove this later
			hitPlanet.checkOrbitSelection ();
			if(selectedPlanets.Contains(hitPlanet))
			{
				selectedPlanets.Remove(hitPlanet);
			}
			else
			{
				selectedPlanets.Add(hitPlanet);
			}
		}
	}

	public static void FoundColony(Camera cam){
		Ray ray;
		RaycastHit hit;
		ray = cam.ScreenPointToRay (Input.mousePosition);
		if (Physics.Raycast (ray, out hit)) {
			GameObject targ;
			if (hit.collider.name != "GravityWell")
			{
				targ = hit.collider.gameObject;
			}
			else
			{		//if we hit a gravity well, grab its parent planet
				targ = hit.collider.gameObject.transform.parent.gameObject;
			}
			if (targ.GetComponent<Colonies> () == null) {
			} else {
				Debug.Log ("Colony already present, aborting");
				return;
			}
		}
	}

	public static void AddColonist(Camera cam){
		Ray ray;
		RaycastHit hit;
		ray = cam.ScreenPointToRay (Input.mousePosition);
		if (Physics.Raycast (ray, out hit)) {
			GameObject targ;
			if (hit.collider.name != "GravityWell")
			{
				targ = hit.collider.gameObject;
			}
			else
			{
				targ = hit.collider.gameObject.transform.parent.gameObject;
			}
			if (targ.GetComponent<Colonies> () != null) {
				Colonies c = (targ.GetComponent<Colonies> ());  
				c.AddRandomColonist ();
			} else {
				Debug.Log ("No Colony present, aborting");
				return;
			}
		}
	}

	public static void SelectFleet(List<Fleets> FleetsList, Vector3 currFramePosition, List<Fleets> selectedFleets){
		foreach (Fleets fleet in FleetsList) {
			if (fleet.localPlayerAuthority) {
				Collider coll = fleet.fleetGo.GetComponent<Collider> ();
				if (coll.bounds.Contains (new Vector3 (currFramePosition.x, currFramePosition.y, coll.transform.position.z))) {
					changeSelection (fleet, selectedFleets);
				} else {
				}
			} else {
				//		Debug.Log ("currently parsed fleet isnt owned by player, skipping");
			}
		}
	}

	public static void changeSelection(Fleets fleet, List<Fleets> selectedFleets){
		if(selectedFleets.Contains(fleet)){
			selectedFleets.Remove(fleet);
			fleet.SetGlow = false;
			Debug.Log ("Unselected fleet: " + fleet.fleetName);
		}
		else{
			selectedFleets.Add(fleet);
			fleet.SetGlow = true;
			Debug.Log ("Selected fleet: " + fleet.fleetName);
		}
	}

	public static void MoveFleet(List<Fleets> selectedFleets, Vector3 currFramePosition){
		bool abort = false;
		foreach(Planets planet in Planets.PlanetList)
		{
			if (planet.gameObject.GetComponent<Collider>().bounds.Contains(currFramePosition) || planet.GravityWell.gameObject.GetComponent<Collider>().bounds.Contains(currFramePosition))
			{
				Missions.MoveToPlanetMission (selectedFleets, planet);
				//abort = true;
			}
		}
		if (abort)
		{
			return;
		}
		foreach (Fleets fleet in selectedFleets) {
			fleet.MoveTo(currFramePosition);
		}
	}


	static Vector3 GetWorldPositionOnPlane(Camera cam) {
		float z = 0;
		Ray ray = cam.ScreenPointToRay(Input.mousePosition);
		Plane xy = new Plane(Vector3.forward, new Vector3(0, 0, z));
		float distance;
		xy.Raycast(ray, out distance);
		return ray.GetPoint(distance);
	}


	public static Vector3 GetFramePosition(Camera cam){
		Vector3 position;
		position = GetWorldPositionOnPlane(cam);
		position.z = 0;
		return position;
	}

	public static void Dragging(Camera cam,Vector3 last, Vector3 curr){
		Vector3 diff = last - curr; 
		diff.z = 0; 
		cam.transform.Translate(diff);
	}

}
