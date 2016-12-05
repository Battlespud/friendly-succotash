using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlayerControls {
	const float moveSpeed = 4f;
	//x(+R/-L),y(+U/-D),z(+F/-B)
	//Input.GetKey (KeyCode.UpArrow)
	//Input.GetKey (KeyCode.DownArrow)
	//Input.GetKey(KeyCode.RightArrow)
	//Input.GetKey(KeyCode.LeftArrow)
	public static Vector3 movePlayer(){
		
		Vector3 position = new Vector3 (0, 0, 0);

		if (Input.GetKey (KeyCode.UpArrow)) {
			if (Input.GetKey (KeyCode.UpArrow) && Input.GetKey (KeyCode.RightArrow)) {
				position += new Vector3 (1, 0, 1)*2;
				return (position * moveSpeed) * Time.deltaTime;
			} else if (Input.GetKey (KeyCode.UpArrow) && Input.GetKey (KeyCode.LeftArrow)) {
				position += new Vector3 (-1, 0, 1)*2;
				return (position * moveSpeed) * Time.deltaTime;
			} else {
				position += new Vector3 (0, 0, 1);
				return ((position * moveSpeed) * Time.deltaTime)*2;
			}
		}

		if (Input.GetKey (KeyCode.DownArrow)) {
			if (Input.GetKey (KeyCode.DownArrow) && Input.GetKey (KeyCode.RightArrow)) {
				position += new Vector3 (1, 0, -1)*2;
				return (position * moveSpeed) * Time.deltaTime;
			} else if (Input.GetKey (KeyCode.DownArrow) && Input.GetKey (KeyCode.LeftArrow)) {
				position += new Vector3 (-1, 0, -1)*2;
				return (position * moveSpeed) * Time.deltaTime;
			} else {
				position += new Vector3 (0, 0, -1);
				return ((position * moveSpeed) * Time.deltaTime)*2;
			}
		}

		if (Input.GetKey (KeyCode.RightArrow)) {
			position += new Vector3 (1, 0, 0);
			return ((position * moveSpeed) * Time.deltaTime)*2;
		}

		if (Input.GetKey (KeyCode.LeftArrow)) {
			position += new Vector3 (-1, 0, 0);
			return ((position * moveSpeed) * Time.deltaTime)*2;
		}

		return position * moveSpeed;
	}
}