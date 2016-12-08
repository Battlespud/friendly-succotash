using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroids : AstroBody {

	// Use this for initialization
	new void Start () {
		BodyType = AstroBodyType.ASTEROID;
		BodyGo = this.gameObject;
		Name = BodyGo.name;
	}

	// Update is called once per frame
	new void Update () {
		base.Update ();
		position = BodyGo.transform.position;
	}
}
