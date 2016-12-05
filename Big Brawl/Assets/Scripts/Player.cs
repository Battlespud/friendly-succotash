using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}

	void movePlayer() {
		rotatePlayer ();
		transform.Translate (PlayerControls.movePlayer ());
	}

	void rotatePlayer(){
		if (Input.GetKey (KeyCode.UpArrow)) {
			if (Input.GetKey (KeyCode.UpArrow) && Input.GetKey (KeyCode.RightArrow)) {
				transform.Rotate (0, 45, 0);
			} else if (Input.GetKey (KeyCode.UpArrow) && Input.GetKey (KeyCode.LeftArrow)) {
				transform.Rotate (0, 315, 0);
			} else {
				transform.Rotate (0, 0, 0);
			}
		}

		if (Input.GetKey (KeyCode.DownArrow)) {
			if (Input.GetKey (KeyCode.DownArrow) && Input.GetKey (KeyCode.RightArrow)) {
				transform.Rotate (0, 135, 0);
			} else if (Input.GetKey (KeyCode.DownArrow) && Input.GetKey (KeyCode.LeftArrow)) {
				transform.Rotate (0, 225, 0);
			} else {
				transform.Rotate (0, 180, 0);
			}
		}

		if (Input.GetKey (KeyCode.RightArrow)) {
			transform.Rotate (0, 90, 0);
		}

		if (Input.GetKey (KeyCode.LeftArrow)) {
			transform.Rotate (0, 270, 0);
		}
	}
	
	// Update is called once per frame
	void Update () {
		movePlayer ();
	}
}
