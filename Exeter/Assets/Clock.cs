using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Clock : MonoBehaviour {

	GameObject textGo;
	Text text;

	string time;

	int months; //12 min~ for 30 days
	int days; //24 seconds
	int hours;//1 second

	int[] daysInMonth = new int[12]{31,28,31,30,31,30,31,31,30,31,30,31};
	string[] monthNames = new string[12]{"January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"};
	int currMonth = 1;
	int currDay = 1;
	int currHour;
	const int hoursInDay = 24;



	// Use this for initialization
	void Start () {
		textGo = this.gameObject;
		text = textGo.GetComponent<Text> ();
	}



	// Update is called once per frame
	void Update () {
		
		hours = (int)Time.fixedTime;
		days = Mathf.FloorToInt(hours / 24);

		//convert to hours




		time = "The time will go here when i get smarter";


		text.text = time;
		}
}
