using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;

public class TimeController : NetworkBehaviour {

	Text timeDilationText;
	[SyncVar] public string timeDilationString;

	[SyncVar] bool autoPauseModeEnabled = false;

	//Timescales
	[SyncVar]public bool paused = false;

    //The various timescales we can reach ingame
    public static float[] ValidTimescales = new float[10] { .25f, .5f, .75f, 1f, 1.5f, 2f, 3f, 5f, 10f, 25f };
    //which timescale we should use, 3 is 1f speed
    [SyncVar]public int TimescaleSelector = 3;

	[Command]
	public void CmdPause (string playerID){
		if (paused) {
			paused = false;
			Debug.Log ("Unpaused by " + playerID);
		} else {
			paused = true;
			Debug.Log ("Paused by " + playerID);
		}
	}

	[Command]
	public void CmdSlowDown(string id){
		Debug.Log ("Slowed Down");
        TimescaleSelector--;
	}

	[Command]
	public void CmdSpeedUp(string id){
		Debug.Log ("Sped Up");
        TimescaleSelector++;
	}



	// Use this for initialization
	void Start () {
		timeDilationText = gameObject.GetComponentInChildren<Text> ();

	}
	
	// Update is called once per frame
	void Update () {
		if(TimescaleSelector >= 9) { TimescaleSelector = 9;}
		if (TimescaleSelector <= 0) {TimescaleSelector = 0;	}

			Time.timeScale = ValidTimescales[TimescaleSelector];
		if (paused) {
			Time.timeScale = 0f;
		}
		timeDilationString = Time.timeScale.ToString();
		timeDilationText.text = timeDilationString;
	}
}
