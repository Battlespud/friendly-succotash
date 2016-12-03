using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class OrbitAlphaController : MonoBehaviour {


    

    //All this does is make the orbital path and gravity well transparent
    //Planet should have an orbital path as a sibling and a gravity well as a child for this to work, any other configuration is not supported at the moment.

    //Place this on a planet gameobject
    

//deprecated
    public enum ControllerType { GRAVITYWELL, ORBITALPATH };
    public ControllerType controllerType;


	//Gameobject of the planet this path is for.
	GameObject planetGo;
    //Specific objects, only saved here for future use and testing.  Use the arrays to process changes
      //gravity well
          GameObject gravityGo;
      //orbit
           GameObject orbitGo;

    //All gameobjects and sprites set to be managed by this script.
    List<GameObject> managedObjects;
    List<SpriteRenderer> managedRenderers;

    //Planet's Sphere collider
	Collider coll;

    //Transparency when unselected, Default .05
	public const float regularVisibility = .05f;
    //Transparency when selected, Default .80
	public const float selectedVisibility = .80f;

	//set equal to either of the above to change
	float visiblePercent = regularVisibility;
        //getter-setter
	public float VisiblePercent {
		get {
			return visiblePercent;
		}
		set {
			visiblePercent = value;
            //visually updated the graphics
			updateAlpha ();
		}
	}

    public void bypassSetVisiblePercent(float f)
    {

        visiblePercent = f;
    }

	void updateAlpha(){
        foreach (SpriteRenderer sr in managedRenderers)
        {
            sr.color = new Color(1f, 1f, 1f, visiblePercent);
        }
	}

	// Use this for initialization
	void Start () {
		getPlanetGo ();
        setupArrays();
        updateAlpha();
	}

	//Function will only work if no other gameobjects are put as siblings!!!
	void getPlanetGo(){
        planetGo = this.gameObject;
        foreach (Transform child in transform.parent)
        {
            if (child.name.Contains("Path"))
            {
                orbitGo = child.gameObject;
         //       Debug.Log("Assigning " + child.name + " to " + planetGo.name);
            }
        }
        gravityGo = planetGo.transform.FindChild("GravityWell").gameObject;
	}

    void setupArrays()
    {
        managedRenderers = new List<SpriteRenderer>();
        managedObjects = new List<GameObject>();
        managedObjects.Add(orbitGo);
        managedObjects.Add(gravityGo);
        foreach(var a in managedObjects)
        {
            managedRenderers.Add(a.GetComponent<SpriteRenderer>());
        }
    }


    //deprecated
    public void reassignTo(OrbitAlphaController original)
    {
        this.visiblePercent = original.visiblePercent;

    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
