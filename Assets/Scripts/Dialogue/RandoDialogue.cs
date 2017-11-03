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
            StartCoroutine("ExamineBody");
        }
        DialogueSystem.Instance().OpenDialogueBox(id, this, firstMeeting, oneLiners);
        firstMeeting = false;
    }

    IEnumerator ExamineBody()
    {
        DialogueSystem.Instance().CreateJournalEntry("The victim was [Victim].", CharacterID.VICTIM, "MOTIVE");
        yield return new WaitForSeconds(0.05f);
        DialogueSystem.Instance().CreateJournalEntry("[Victim] was strangled.", CharacterID.VICTIM, "WEAPON");
        yield return new WaitForSeconds(0.05f);
        DialogueSystem.Instance().CreateJournalEntry("[Victim] was killed in the Town Square.", CharacterID.VICTIM, "OPP");
    }
}
