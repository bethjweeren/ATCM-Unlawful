using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterDialogue : NPCDialogue {

    // Use this for initialization
    new void Start()
    {
        base.Start();
        oneLiners = false;
	}
	
	override public void Interact()
    {
        DialogueSystem.Instance().OpenDialogueBox(id, this, firstMeeting, oneLiners);
        firstMeeting = false;
    }

    
}
