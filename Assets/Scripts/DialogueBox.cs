using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueBox : MonoBehaviour {
    string currentLine;
    public Text nameText;
    public Text dialogueText;
    public Image playerThumb;
    public Image nonPlayerThumb;
    public GameObject dialogueContent;
    public GameObject choiceContent;
    public Text[] dialogueOptions;
    Choice selectedChoice;
    public Color selectedColor = new Color(255, 255, 255, 100);
    public Color deselectedColor = new Color(65, 75, 100, 100);
    CharacterID speaker;
    List<string> textPortions;
    string textToDisplay;
    bool doneDisplaying;
    float typeDelay = 0.05f;
    float delayMultiplier = 1;
    bool exitAfterSaying;
    bool choiceMode = false;

	void Start () {
        DialogueSystem.Instance().dialogueBox = this;
        DialogueSystem.Instance().CloseDialogueBox();
        Debug.Log("Registered dialogue box");
	}
	
	void Update () {
        if (choiceMode)
        {
            if (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Space))
            {
                choiceMode = false;
                DialogueSystem.Instance().ProcessChoice(selectedChoice);
            }
            else
            {
                switch (selectedChoice)
                {
                    case Choice.BLACK_WHERE:
                        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
                        {
                            dialogueOptions[(int)selectedChoice].transform.parent.GetComponent<Image>().color = deselectedColor;
                            selectedChoice = Choice.RED;
                            dialogueOptions[(int)selectedChoice].transform.parent.GetComponent<Image>().color = selectedColor;
                        }
                        else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
                        {
                            dialogueOptions[(int)selectedChoice].transform.parent.GetComponent<Image>().color = deselectedColor;
                            selectedChoice = Choice.BLUE_WHEN;
                            dialogueOptions[(int)selectedChoice].transform.parent.GetComponent<Image>().color = selectedColor;
                        }
                        break;
                    case Choice.BLUE_WHEN:
                        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
                        {
                            dialogueOptions[(int)selectedChoice].transform.parent.GetComponent<Image>().color = deselectedColor;
                            selectedChoice = Choice.BLACK_WHERE;
                            dialogueOptions[(int)selectedChoice].transform.parent.GetComponent<Image>().color = selectedColor;
                        }
                        else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
                        {
                            dialogueOptions[(int)selectedChoice].transform.parent.GetComponent<Image>().color = deselectedColor;
                            selectedChoice = Choice.YELLOW;
                            dialogueOptions[(int)selectedChoice].transform.parent.GetComponent<Image>().color = selectedColor;
                        }
                        else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
                        {
                            dialogueOptions[(int)selectedChoice].transform.parent.GetComponent<Image>().color = deselectedColor;
                            selectedChoice = Choice.GREEN;
                            dialogueOptions[(int)selectedChoice].transform.parent.GetComponent<Image>().color = selectedColor;
                        }
                        break;
                    case Choice.GREEN:
                        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
                        {
                            dialogueOptions[(int)selectedChoice].transform.parent.GetComponent<Image>().color = deselectedColor;
                            selectedChoice = Choice.BLUE_WHEN;
                            dialogueOptions[(int)selectedChoice].transform.parent.GetComponent<Image>().color = selectedColor;
                        }
                        else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
                        {
                            dialogueOptions[(int)selectedChoice].transform.parent.GetComponent<Image>().color = deselectedColor;
                            selectedChoice = Choice.CANCEL;
                            dialogueOptions[(int)selectedChoice].transform.parent.GetComponent<Image>().color = selectedColor;
                        }
                        break;
                    case Choice.RED:
                        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
                        {
                            dialogueOptions[(int)selectedChoice].transform.parent.GetComponent<Image>().color = deselectedColor;
                            selectedChoice = Choice.BLACK_WHERE;
                            dialogueOptions[(int)selectedChoice].transform.parent.GetComponent<Image>().color = selectedColor;
                        }
                        else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
                        {
                            dialogueOptions[(int)selectedChoice].transform.parent.GetComponent<Image>().color = deselectedColor;
                            selectedChoice = Choice.YELLOW;
                            dialogueOptions[(int)selectedChoice].transform.parent.GetComponent<Image>().color = selectedColor;
                        }
                        break;
                    case Choice.YELLOW:
                        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
                        {
                            dialogueOptions[(int)selectedChoice].transform.parent.GetComponent<Image>().color = deselectedColor;
                            selectedChoice = Choice.RED;
                            dialogueOptions[(int)selectedChoice].transform.parent.GetComponent<Image>().color = selectedColor;
                        }
                        else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
                        {
                            dialogueOptions[(int)selectedChoice].transform.parent.GetComponent<Image>().color = deselectedColor;
                            selectedChoice = Choice.BLUE_WHEN;
                            dialogueOptions[(int)selectedChoice].transform.parent.GetComponent<Image>().color = selectedColor;
                        }
                        else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
                        {
                            dialogueOptions[(int)selectedChoice].transform.parent.GetComponent<Image>().color = deselectedColor;
                            selectedChoice = Choice.CANCEL;
                            dialogueOptions[(int)selectedChoice].transform.parent.GetComponent<Image>().color = selectedColor;
                        }
                        break;
                    case Choice.CANCEL:
                        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
                        {
                            dialogueOptions[(int)selectedChoice].transform.parent.GetComponent<Image>().color = deselectedColor;
                            selectedChoice = Choice.YELLOW;
                            dialogueOptions[(int)selectedChoice].transform.parent.GetComponent<Image>().color = selectedColor;
                        }
                        else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
                        {
                            dialogueOptions[(int)selectedChoice].transform.parent.GetComponent<Image>().color = deselectedColor;
                            selectedChoice = Choice.GREEN;
                            dialogueOptions[(int)selectedChoice].transform.parent.GetComponent<Image>().color = selectedColor;
                        }
                        break;
                }
            } 
        }
        else
        {
            if (Input.GetKey(KeyCode.E) || Input.GetKey(KeyCode.Space))
            {
                delayMultiplier = 0.25f;
            }
            else
            {
                delayMultiplier = 1f;
            }
            if (doneDisplaying && (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Space)))
            {
                SayNextPortion();
            }
        }
	}

    void Clear()
    {
        nameText.text = "";
        dialogueText.text = "";
    }

    public void DisplayLine(CharacterID s, string newLine, bool exitLine)
    {
        choiceContent.SetActive(false);
        dialogueContent.SetActive(true);
        Clear();
        speaker = s;
        exitAfterSaying = exitLine;
        string line = newLine;
        textPortions = PartitionText(line);
        SayNextPortion();
    }

    public void DisplayChoices(string[] textOptions)
    {
        dialogueOptions[(int)selectedChoice].transform.parent.GetComponent<Image>().color = deselectedColor;
        selectedChoice = Choice.CANCEL;
        dialogueOptions[(int)selectedChoice].transform.parent.GetComponent<Image>().color = selectedColor;

        for (int i = 0; i<6; i++)
        {
            dialogueOptions[i].text = textOptions[i];
        }
        dialogueContent.SetActive(false);
        choiceContent.SetActive(true);
        choiceMode = true;
    }

    void SetName(CharacterID id)
    {
        Character speaker = DialogueSystem.Instance().characters.IDToCharacter(id);
        nameText.text = "<color=" + speaker.color + ">" + speaker.name + "</color>";
    }

    public void SetPortrait(CharacterID id)
    {
        if (id != CharacterID.BLACK && id != CharacterID.BLUE && id != CharacterID.BROWN && id != CharacterID.GREEN && id != CharacterID.PURPLE && id != CharacterID.RED && id != CharacterID.YELLOW)
        {
            nonPlayerThumb.gameObject.SetActive(false);
        }
        else
        {
            nonPlayerThumb.sprite = DialogueSystem.Instance().characters.IDToCharacter(id).thumb;
            nonPlayerThumb.gameObject.SetActive(true);
        }
    }

    void SayNextPortion()
    {
        if (textPortions.Count > 0)
        {
            Clear();
            SetName(speaker);
            textToDisplay = textPortions[0];
            textPortions.RemoveAt(0);
            doneDisplaying = false;
            StartCoroutine("SayLine");
        }
        else
        {
            if (exitAfterSaying)
            {
                DialogueSystem.Instance().CloseDialogueBox();
            }
            else
            {
                DialogueSystem.Instance().NextLine();
            }
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

    List<string> PartitionText(string allText)
    {
        string remaining = allText;
        List<string> portions = new List<string>();
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
            portions.Add(beginning);
            allText = end;
        }
        portions.Add(allText);
        return portions;
    }
}
