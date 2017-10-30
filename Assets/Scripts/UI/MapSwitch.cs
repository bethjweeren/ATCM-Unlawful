using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapSwitch : MonoBehaviour {

	public MapTracker mapTracker;

	void OnTriggerEnter2D(Collider2D other) {
		if(other.tag == "Player"){
			mapTracker.SwitchMap ();
		}
	}
}
