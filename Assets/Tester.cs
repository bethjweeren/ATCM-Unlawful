using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tester : MonoBehaviour
{
    public DialogueBox dialogue;
    DialogueSystem dialogueSystem;

	void Start ()
    {
        dialogueSystem = DialogueSystem.Instance();
        StartCoroutine("DoDisplay");
    }

    IEnumerator DoDisplay()
    {
        yield return new WaitForSeconds(2);
        foreach (Quotes.DialogueLine d in dialogueSystem.quotes.allQuotes)
        {
            dialogue.DisplayLine(d);
            yield return new WaitForSeconds(5);
        }
        StartCoroutine("DoDisplay");
    }
}
