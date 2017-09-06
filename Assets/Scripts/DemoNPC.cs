using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemoNPC : MonoBehaviour, IInteractable {

    public CharacterID id;
    public string dialogueFile;
    Quotes quotes;

	// Use this for initialization
	void Start () {
        quotes = Quotes.LoadJSON(dialogueFile);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Interact()
    {
        if(quotes != null)
        {
            if(quotes.allQuotes.Count != 0)
            {
                Quotes.DialogueLine line = quotes.allQuotes[Random.Range(0, quotes.allQuotes.Count)];
                DialogueSystem.Instance().OpenDialogueBox();
                DialogueSystem.Instance().dialogueBox.DisplayLine(line);
            }
            else
            {
                DialogueSystem.Instance().player.EndInteraction();
            } 
        }
        else
        {
            DialogueSystem.Instance().player.EndInteraction();
        }
    }
}
