using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class QuickBar : MonoBehaviour {


	Dropdown drop;

	// Use this for initialization
	void Start () {
		drop = this.gameObject.gameObject.GetComponent<Dropdown> ();
		drop.onValueChanged.AddListener (delegate {
			valueChanged (drop);
		});
	}

			void valueChanged(Dropdown Targ){
		switch (Targ.value) { //first option = 0
			
		case 0:
			{
				break;
			}
		case 1:
			{
				break;
			}

		}
			}


	// Update is called once per frame
	void Update () {

	}
}
