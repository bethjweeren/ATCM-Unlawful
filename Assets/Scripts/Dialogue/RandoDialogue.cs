using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandoDialogue : NPCDialogue {

    // Use this for initialization
    public bool hasIntro;

    new void Start()
    {
        base.Start();
        oneLiners = true;
        firstMeeting = hasIntro;
	}
	
	override public void Interact()
    {
        if (id == CharacterID.VICTIM)
        {
            DialogueSystem.Instance().CreateJournalEntry("The victim was [Victim].", CharacterID.VICTIM, "MOTIVE");
            DialogueSystem.Instance().CreateJournalEntry("[Victim] was stabbed.", CharacterID.VICTIM, "METHOD");
            DialogueSystem.Instance().CreateJournalEntry("[Victim] was killed in the Town Square.", CharacterID.VICTIM, "OPP");
        }
        DialogueSystem.Instance().OpenDialogueBox(id, this, firstMeeting, oneLiners);
        firstMeeting = false;
    }

}
