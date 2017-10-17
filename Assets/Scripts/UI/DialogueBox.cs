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
    public GameObject fourChoiceContent;
    public GameObject sixChoiceContent;
    public GameObject[] fourChoiceOptions;
    public GameObject[] sixChoiceOptions;
    FourChoice selectedFourChoice;
    SixChoice selectedSixChoice;
    ChoiceStyle choiceMode;
    public Color selectedColor = new Color(255, 255, 255, 100);
    public Color deselectedColor = new Color(65, 75, 100, 100);
    CharacterID speaker;
    List<string> textPortions;
    string textToDisplay;
    bool doneDisplaying;
    bool skipText;
    float typeDelay = 0.015f;
    bool exitAfterSaying;

	void Start () {
        DialogueSystem.Instance().dialogueTextSize = GetTextSize();
	}
	
	void Update () {
        if (choiceMode != ChoiceStyle.DEFAULT)
        {
            if (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Space))
            {
                if(choiceMode == ChoiceStyle.FOUR)
                {
                    DialogueSystem.Instance().ProcessFourChoice(selectedFourChoice);
                }
                else
                {
                    DialogueSystem.Instance().ProcessSixChoice(selectedSixChoice);
                }
            }
            else
            {
                if(choiceMode == ChoiceStyle.FOUR)
                {
                    switch (selectedFourChoice)
                    {
                        case FourChoice.MOTIVE:
                            if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
                            {
                                FourSelect(FourChoice.CLUE);
                            }
                            else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
                            {
                                FourSelect(FourChoice.OPPORTUNITY);
                            }
                            break;
                        case FourChoice.OPPORTUNITY:
                            if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
                            {
                                FourSelect(FourChoice.MOTIVE);
                            }
                            else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
                            {
                                FourSelect(FourChoice.CANCEL);
                            }
                            break;
                        case FourChoice.CLUE:
                            if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
                            {
                                FourSelect(FourChoice.MOTIVE);
                            }
                            else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
                            {
                                FourSelect(FourChoice.CANCEL);
                            }
                            break;
                        case FourChoice.CANCEL:
                            if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
                            {
                                FourSelect(FourChoice.CLUE);
                            }
                            else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
                            {
                                FourSelect(FourChoice.OPPORTUNITY);
                            }
                            break;
                    }
                }
                else if(choiceMode == ChoiceStyle.SIX)
                {
                    switch (selectedSixChoice)
                    {
                        case SixChoice.BLACK:
                            if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
                            {
                                SixSelect(SixChoice.RED);
                            }
                            else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
                            {
                                SixSelect(SixChoice.BLUE);
                            }
                            break;
                        case SixChoice.BLUE:
                            if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
                            {
                                SixSelect(SixChoice.BLACK);
                            }
                            else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
                            {
                                SixSelect(SixChoice.YELLOW);
                            }
                            else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
                            {
                                SixSelect(SixChoice.GREEN);
                            }
                            break;
                        case SixChoice.GREEN:
                            if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
                            {
                                SixSelect(SixChoice.BLUE);
                            }
                            else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
                            {
                                SixSelect(SixChoice.CANCEL);
                            }
                            break;
                        case SixChoice.RED:
                            if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
                            {
                                SixSelect(SixChoice.BLACK);
                            }
                            else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
                            {
                                SixSelect(SixChoice.YELLOW);
                            }
                            break;
                        case SixChoice.YELLOW:
                            if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
                            {
                                SixSelect(SixChoice.RED);
                            }
                            else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
                            {
                                SixSelect(SixChoice.BLUE);
                            }
                            else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
                            {
                                SixSelect(SixChoice.CANCEL);
                            }
                            break;
                        case SixChoice.CANCEL:
                            if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
                            {
                                SixSelect(SixChoice.YELLOW);
                            }
                            else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
                            {
                                SixSelect(SixChoice.GREEN);
                            }
                            break;
                    }
                }
                
            } 
        }
        else
        {
            if (Input.GetKey(KeyCode.E) || Input.GetKey(KeyCode.Space))
            {
                skipText = true;
            }
            else
            {
                skipText = false;
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

    void FourSelect(FourChoice option)
    {
        fourChoiceOptions[(int)selectedFourChoice].GetComponent<Image>().color = deselectedColor;
        selectedFourChoice = option;
        fourChoiceOptions[(int)selectedFourChoice].GetComponent<Image>().color = selectedColor;
    }

    void SixSelect(SixChoice option)
    {
        sixChoiceOptions[(int)selectedSixChoice].GetComponent<Image>().color = deselectedColor;
        selectedSixChoice = option;
        sixChoiceOptions[(int)selectedSixChoice].GetComponent<Image>().color = selectedColor;
    }

    public void DisplayLine(CharacterID s, string newLine, bool exitLine)
    {
        choiceMode = ChoiceStyle.DEFAULT;
        fourChoiceContent.SetActive(false);
        sixChoiceContent.SetActive(false);
        dialogueContent.SetActive(true);
        Clear();
        speaker = s;
        exitAfterSaying = exitLine;
        string line = newLine;
        textPortions = PartitionText(line);
        SayNextPortion();
    }

    public void DisplayChoices(ChoiceStyle style)
    {
        dialogueContent.SetActive(false);
        if (style == ChoiceStyle.FOUR)
        {
            choiceMode = ChoiceStyle.FOUR;
            sixChoiceContent.SetActive(false);

            fourChoiceOptions[(int)selectedFourChoice].GetComponent<Image>().color = deselectedColor;
            selectedFourChoice = FourChoice.CANCEL;
            fourChoiceOptions[(int)selectedFourChoice].GetComponent<Image>().color = selectedColor;

            fourChoiceContent.SetActive(true);
        }
        else
        {
            choiceMode = ChoiceStyle.SIX;
            fourChoiceContent.SetActive(false);

            sixChoiceOptions[(int)selectedSixChoice].GetComponent<Image>().color = deselectedColor;
            selectedSixChoice = SixChoice.CANCEL;
            sixChoiceOptions[(int)selectedSixChoice].GetComponent<Image>().color = selectedColor;

            sixChoiceContent.SetActive(true);
        }
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
                    if (skipText)
                    {
                        yield return new WaitForEndOfFrame();
                    }
                    else
                    {
                        yield return new WaitForSeconds(typeDelay);
                    }
                }

            }
            else
            {
                dialogueText.text = dialogueText.text + textToDisplay[0];
                textToDisplay = textToDisplay.Substring(1, textToDisplay.Length - 1);
            }
            if (skipText)
            {
                yield return new WaitForEndOfFrame();
            }
            else
            {
                yield return new WaitForSeconds(typeDelay);
            }
        }
        doneDisplaying = true;
    }

    /// <summary>
    /// Cuts long lines of text into smaller portions.
    /// </summary>
    /// <param name="allText">The full line of text</param>
    /// <returns>A list of strings containing the smaller portions.</returns>
    List<string> PartitionText(string allText)
    {
        string remaining = Quotes.FormatColors(allText);
        List<string> portions = new List<string>();
        while(remaining.Length > 255 || (remaining.IndexOf('|') >= 0 && remaining.IndexOf('|') <= 254))
        {
            if (remaining.IndexOf('|') >= 0 && remaining.IndexOf('|') <= 254)
            {
                int breakIndex = remaining.IndexOf('|');
                string beginning = remaining.Substring(0, breakIndex - 1);
                string end = remaining.Substring(breakIndex + 2, remaining.Length - (breakIndex + 2));
                portions.Add(beginning);
                remaining = end;
            }
            else
            {
                string beginning = remaining.Substring(0, 255);
                string end = remaining.Substring(255, remaining.Length - 255);
                int lastSpace = beginning.LastIndexOf(' ');
                if (lastSpace != 254)
                {
                    end = beginning.Substring(lastSpace + 1, beginning.Length - (lastSpace + 1)) + end;
                    beginning = beginning.Substring(0, lastSpace + 1);
                }
                portions.Add(beginning);
                remaining = end;
            }
        }
        portions.Add(remaining);
        return portions;
    }

    public int GetTextSize()
    {
        return dialogueText.fontSize;
    }
}
