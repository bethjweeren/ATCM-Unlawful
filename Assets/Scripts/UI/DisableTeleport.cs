using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisableTeleport : MonoBehaviour {

    public GameObject teleportButton;

    void OnTriggerEnter2D(Collider2D other) {
		if(other.tag == "Player"){
            teleportButton.GetComponent<Button>().interactable = false;
        }
	}
	
	void OnTriggerExit2D(Collider2D other) {
        Time_Manager time = Provider.GetInstance().timeManager;
        if (other.tag == "Player")
        {
            teleportButton.GetComponent<Button>().interactable = true;
        }
	}
}
