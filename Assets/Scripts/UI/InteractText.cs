using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractText : MonoBehaviour {

	public GameObject interactText;
	public GameObject interactTextBG;

	void OnTriggerEnter2D(Collider2D other) {
		if(other.tag == "Player"){
			interactTextBG.SetActive(true);
			interactText.SetActive(true);
		}
	}
	
	void OnTriggerExit2D(Collider2D other) {
		if(other.tag == "Player"){
			interactTextBG.SetActive(false);
			interactText.SetActive(false);
		}
	}
}
