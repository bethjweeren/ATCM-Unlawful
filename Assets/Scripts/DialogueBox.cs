using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueBox : MonoBehaviour {
    Quotes.DialogueLine currentLine;
    public Text nameText;
    public Text dialogueText;
    public Image PlayerThumb;
    public Image NonPlayerThumb;
    List<string> textPortions;
    string textToDisplay;
    bool doneDisplaying;
    float typeDelay = 0.05f;
    float delayMultiplier = 1;

	void Start () {
        DialogueSystem.Instance().dialogueBox = this;
        DialogueSystem.Instance().CloseDialogueBox();
        Debug.Log("Registered dialogue box");
	}
	
	void Update () {
        if (Input.GetKey(KeyCode.E) || Input.GetKey(KeyCode.Space))
        {
            delayMultiplier = 0.25f;
        }
        else
        {
            delayMultiplier = 1f;
        }
        if(doneDisplaying && (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Space))){
            SayNextPortion();
        }
	}

    void Clear()
    {
        nameText.text = "";
        dialogueText.text = "";
    }

    public void DisplayLine(Quotes.DialogueLine newLine)
    {
        Clear();
        currentLine = newLine;
        PartitionText(currentLine.line);
        SayNextPortion();
    }

    void SetName(CharacterID id)
    {
        Character speaker = DialogueSystem.Instance().characters.IDToCharacter(id);
        nameText.text = "<color=" + speaker.color + ">" + speaker.name + "</color>";
        if(id != CharacterID.BLACK && id != CharacterID.BLUE && id != CharacterID.BROWN && id != CharacterID.GREEN && id != CharacterID.PURPLE && id != CharacterID.RED && id != CharacterID.YELLOW)
        {
            NonPlayerThumb.gameObject.SetActive(false);
        }
        else
        {
            NonPlayerThumb.sprite = speaker.thumb;
            NonPlayerThumb.gameObject.SetActive(true);
        }
    }

    void SayNextPortion()
    {
        if (textPortions.Count > 0)
        {
            Clear();
            SetName(currentLine.spokenBy);
            textToDisplay = textPortions[0];
            textPortions.RemoveAt(0);
            doneDisplaying = false;
            StartCoroutine("SayLine");
        }
        else
        {
            DialogueSystem.Instance().CloseDialogueBox();
        }
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
                    yield return new WaitForSeconds(typeDelay * delayMultiplier);
                }

            }
            else
            {
                dialogueText.text = dialogueText.text + textToDisplay[0];
                textToDisplay = textToDisplay.Substring(1, textToDisplay.Length - 1);
            }
            yield return new WaitForSeconds(typeDelay * delayMultiplier);
        }
        doneDisplaying = true;
    }

    void PartitionText(string allText)
    {
        string remaining = allText;
        textPortions = new List<string>();
        while(allText.Length > 255)
        {
            string beginning = allText.Substring(0, 255);
            string end = allText.Substring(255, allText.Length - 255);
            int lastSpace = beginning.LastIndexOf(' ');
            if (lastSpace != 254)
            {
                end = beginning.Substring(lastSpace + 1, beginning.Length - (lastSpace + 1)) + end;
                beginning = beginning.Substring(0, lastSpace+1);
            }
            textPortions.Add(beginning);
            allText = end;
        }
        textPortions.Add(allText);
    }
}
