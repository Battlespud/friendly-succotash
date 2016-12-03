using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Ships : MonoBehaviour {

	//make this more efficient later
	public static Sprite shipSprite;


	public static List<Ships> ShipsList;

	public Ships (Vector2 pos){
		position = pos;
		ShipsList.Add (this);
	}


	public Fleets assignedFleet = null;

	//Conventional engines should = a speed of 2 at game start
	public float movementSpeed = 20;

	GameObject shipGo;
	SpriteRenderer shipSr;

	Vector2 position;

	public Vector2 Position {
		get {
			return position;
		}
		set {
			position = value;
		}
	}

	// Use this for initialization
	void Start () {
		shipSprite = Resources.Load<Sprite>("ship");
		shipSr.sprite = shipSprite;
	}
	
	// Update is called once per frame
	void Update () {
		checkPosition ();
		checkSprite (); //optimize
	}

	private void checkPosition()
	{
		if (assignedFleet != null) {

		}
	}

	private void checkSprite(){    //if in a fleet, hide sprite
		if (assignedFleet = null) {
			shipSr.sprite = shipSprite;
		} else {
			shipSr.sprite = null;
		}
	}


}
