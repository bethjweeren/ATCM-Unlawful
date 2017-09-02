using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueBox : MonoBehaviour {
    Quotes.DialogueLine currentLine;
    public Text nameText;
    public Text dialogueText;
    string textToDisplay;
    bool doneDisplaying;
    float typeDelayMultiplier = 1;

	void Start () {
        DialogueSystem.Instance().dialogueBox = this;
        DialogueSystem.Instance().CloseDialogueBox();
        Debug.Log("Registered dialogue box");
	}
	
	void Update () {
        if (Input.GetKey(KeyCode.E) || Input.GetKey(KeyCode.Space))
        {
            typeDelayMultiplier = 0.5f;
        }
        else
        {
            typeDelayMultiplier = 1f;
        }
        if(doneDisplaying && (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Space))){
            DialogueSystem.Instance().CloseDialogueBox();
        }
	}

    void Clear()
    {
        nameText.text = "";
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
        Character speaker = DialogueSystem.Instance().characters.IDToCharacter(id);
        nameText.text = "<color=" + speaker.color + ">" + speaker.name + "</color>";
    }

    void SetText(string line)
    {
        textToDisplay = line;
        doneDisplaying = false;
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
                    yield return new WaitForSeconds(0.05f * typeDelayMultiplier);
                }

            }
            else
            {
                dialogueText.text = dialogueText.text + textToDisplay[0];
                textToDisplay = textToDisplay.Substring(1, textToDisplay.Length - 1);
            }
            yield return new WaitForSeconds(0.05f * typeDelayMultiplier);
        }
        doneDisplaying = true;
    }
}
