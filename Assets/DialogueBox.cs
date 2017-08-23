using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueBox : MonoBehaviour {
    Quotes.DialogueLine currentLine;
    public Text nameText;
    public Text dialogueText;
    DialogueSystem dialogueSystem;

	void Start () {
        dialogueSystem = DialogueSystem.Instance();
        Debug.Log(dialogueSystem);
	}
	
	void Update () {
		
	}

    void Clear()
    {
        dialogueText.text = "";
    }

    public void DisplayLine(Quotes.DialogueLine newLine)
    {
        currentLine = newLine;
        Clear();
        Debug.Log(currentLine.spokenBy);
        SetName(currentLine.spokenBy);
        SetText(currentLine.line);
    }

    void SetName(CharacterID id)
    {
        Character speaker = dialogueSystem.characters.IDToCharacter(id);
        nameText.text = "<color=" + speaker.color + ">" + speaker.name + "</color>";
    }

    void SetText(string line)
    {
        dialogueText.text = line;
    }
}
