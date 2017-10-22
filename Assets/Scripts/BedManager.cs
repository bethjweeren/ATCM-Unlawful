using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BedManager : MonoBehaviour {

	public Time_Manager timeManager;

	void OnTriggerEnter2D(Collider2D other) {
		if(other.tag == "Player" && timeManager.currentTimeState == Time_Manager.State.Rest){
			timeManager.TurnOnSkipBtn ();
		}
	}

	void OnTriggerExit2D(Collider2D other) {
		if(other.tag == "Player"){
			timeManager.TurnOffSkipBtn ();
		}
	}
}
