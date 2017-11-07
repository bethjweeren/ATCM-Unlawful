using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractText : MonoBehaviour {

	public GameObject interactText;

	void OnTriggerEnter2D(Collider2D other) {
		if(other.tag == "Player"){
			interactText.SetActive(true);
		}
	}
	
	void OnTriggerExit2D(Collider2D other) {
		if(other.tag == "Player"){
			interactText.SetActive(false);
		}
	}
}
