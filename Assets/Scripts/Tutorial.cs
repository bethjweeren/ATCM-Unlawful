using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("hit");
        other.GetComponent<PlayerController>().currentState = PlayerController.State.INTERACTING;
        other.GetComponent<PlayerController>().StopMoving();
        Quotes letter = Quotes.LoadJSON("letter.json");
        DialogueSystem.Instance().OpenDialogueBox(CharacterID.PLAYER, letter, false, true);
        Destroy(this.gameObject);
    }
}
