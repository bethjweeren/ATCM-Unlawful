using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandoDialogue : NPCDialogue {

    // Use this for initialization
    public bool hasIntro;

    new void Start()
    {
        base.Start();
        oneLiners = !hasIntro;
        firstMeeting = hasIntro;
	}
	
	override public void Interact()
    {
        DialogueSystem.Instance().OpenDialogueBox(CharacterID.VICTIM, this, firstMeeting, oneLiners);
        firstMeeting = false;
    }
}
