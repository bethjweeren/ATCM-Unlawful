using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyDialogue : NPCDialogue {

    // Use this for initialization
    new void Start()
    {
        base.Start();
        oneLiners = true;
	}
	
	override public void Interact()
    {
        StartCoroutine("ExamineBody");
        DialogueSystem.Instance().OpenDialogueBox(CharacterID.VICTIM, this, firstMeeting, oneLiners);
        firstMeeting = false;
    }

    IEnumerator ExamineBody()
    {
        DialogueSystem.Instance().CreateJournalEntry("The victim was [Victim].", CharacterID.VICTIM, "motive");
        yield return new WaitForSeconds(0.05f);
        DialogueSystem.Instance().CreateJournalEntry("[Victim] was strangled.", CharacterID.VICTIM, "means");
        yield return new WaitForSeconds(0.05f);
        DialogueSystem.Instance().CreateJournalEntry("[Victim] was killed in the Town Square.", CharacterID.VICTIM, "opportunity");
    }
}
