using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueBox : MonoBehaviour {
    Quotes.DialogueLine currentLine;
    public Text nameText;
    public Text dialogueText;
    string textToDisplay;
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
        textToDisplay = line;
        StartCoroutine("SayLine");
    }

    IEnumerator SayLine()
    {
        while (textToDisplay.Length > 0)
        {
            if(textToDisplay[0] == '<')
            {
                string baseText = dialogueText.text;

                string startFormat = textToDisplay.Substring(0, textToDisplay.IndexOf('>') +1);
                textToDisplay = textToDisplay.Substring(startFormat.Length, textToDisplay.Length - startFormat.Length);

                string nameToDisplay = textToDisplay.Substring(0, textToDisplay.IndexOf('<'));
                textToDisplay = textToDisplay.Substring(nameToDisplay.Length, textToDisplay.Length - nameToDisplay.Length);

                string endFormat = textToDisplay.Substring(0, textToDisplay.IndexOf('>') + 1);
                textToDisplay = textToDisplay.Substring(endFormat.Length, textToDisplay.Length - endFormat.Length);

                while(nameToDisplay.Length > 0)
                {
                    startFormat = startFormat + nameToDisplay[0];
                    nameToDisplay = nameToDisplay.Substring(1, nameToDisplay.Length - 1);
                    dialogueText.text = baseText + startFormat + endFormat;
                    yield return new WaitForSeconds(0.05f);
                }

            }
            else
            {
                dialogueText.text = dialogueText.text + textToDisplay[0];
                textToDisplay = textToDisplay.Substring(1, textToDisplay.Length - 1);
            }
            yield return new WaitForSeconds(0.05f);
        }
    }
}
