using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemoNPC : MonoBehaviour, IInteractable {

    public CharacterID id;
    public string dialogueFile;
    bool firstMeeting = true;
    bool randomNPC;
    Quotes quotes;

	// Use this for initialization
	void Start () {
        if(id == CharacterID.RANDO)
        {
            randomNPC = true;
        }
        quotes = Quotes.LoadJSON(dialogueFile);
        if (quotes != null)
        {
            if (quotes.introductions.Count == 0)
            {
                firstMeeting = false;
            }
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Interact()
    {
        if(quotes != null)
        {
            DialogueSystem.Instance().OpenDialogueBox(id, quotes, firstMeeting, randomNPC);
            firstMeeting = false;
        }
        else
        {
            DialogueSystem.Instance().player.EndInteraction();
        }
    }
}
