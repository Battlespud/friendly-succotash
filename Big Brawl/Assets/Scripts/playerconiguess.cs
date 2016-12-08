using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerconiguess : MonoBehaviour {


	GameObject player;


	// Use this for initialization
	void Start () {
		player = this.gameObject;
	}
	//how much we turn
	int interval = 30;
	//hw fast we move
	int speed = 2;

	const KeyCode left = KeyCode.LeftArrow;
	const KeyCode right = KeyCode.RightArrow;
	const KeyCode forward = KeyCode.UpArrow;
	const KeyCode back = KeyCode.DownArrow;

	// Update is called once per frame
	void Update () {


		if (Input.GetKeyDown (left)) {
			turn (-1);
		}
		if (Input.GetKeyDown (right)) {
			turn (1);

		}		
		if (Input.GetKeyDown (forward)) {
			move (1);

		}		
		if (Input.GetKeyDown (back)) {
			move (-1);
		}






	}

	void turn(int i){
		player.transform.Rotate (0, i * interval, 0);
	}

	void move(int i){
		player.transform.Translate (new Vector3(0,0,i*speed));
	}
}
