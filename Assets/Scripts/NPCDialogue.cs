using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;

public class NPCDialogue : MonoBehaviour, IInteractable {

    public CharacterID id;
    public string dialogueFile;
    bool firstMeeting = true;
    bool randomNPC;
    public bool likesBlack;
    string blackOpinion;
    public bool likesBlue;
    string blueOpinion;
    public bool likesGreen;
    string greenOpinion;
    public bool likesRed;
    string redOpinion;
    public bool likesYellow;
    string yellowOpinion;

    public bool hasHint;
    public CharacterID hintBlames;

    Quotes quotes;

	// Use this for initialization
	void Start () {
        quotes = Quotes.LoadJSON(dialogueFile);
        if (quotes != null)
        {
            if (quotes.introductions.Count == 0)
            {
                firstMeeting = false;
            }
        }
        if (id == CharacterID.RANDO)
        {
            randomNPC = true;
        }
        if (Characters.IsSuspect(id))
        {
            switch (id)
            {
                case CharacterID.BLACK:
                    likesBlack = true;
                    break;
                case CharacterID.BLUE:
                    likesBlue = true;
                    break;
                case CharacterID.GREEN:
                    likesGreen = true;
                    break;
                case CharacterID.RED:
                    likesRed = true;
                    break;
                case CharacterID.YELLOW:
                    likesYellow = true;
                    break;
            }
            if (likesBlack)
            {
                blackOpinion = quotes.motiveBlackInnocent[Random.Range(0, quotes.motiveBlackInnocent.Count)];
            }
            else
            {
                blackOpinion = quotes.motiveBlackGuilty[Random.Range(0, quotes.motiveBlackGuilty.Count)];
            }
            if (likesBlue)
            {
                blueOpinion = quotes.motiveBlueInnocent[Random.Range(0, quotes.motiveBlueInnocent.Count)];
            }
            else
            {
                blueOpinion = quotes.motiveBlueGuilty[Random.Range(0, quotes.motiveBlueGuilty.Count)];
            }
            if (likesGreen)
            {
                greenOpinion = quotes.motiveGreenInnocent[Random.Range(0, quotes.motiveGreenInnocent.Count)];
            }
            else
            {
                greenOpinion = quotes.motiveGreenGuilty[Random.Range(0, quotes.motiveGreenGuilty.Count)];
            }
            if (likesRed)
            {
                redOpinion = quotes.motiveRedInnocent[Random.Range(0, quotes.motiveRedInnocent.Count)];
            }
            else
            {
                redOpinion = quotes.motiveRedGuilty[Random.Range(0, quotes.motiveRedGuilty.Count)];
            }
            if (likesYellow)
            {
                yellowOpinion = quotes.motiveYellowInnocent[Random.Range(0, quotes.motiveYellowInnocent.Count)];
            }
            else
            {
                yellowOpinion = quotes.motiveYellowGuilty[Random.Range(0, quotes.motiveYellowGuilty.Count)];
            }
        }
        
        
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Interact()
    {
        if(quotes != null)
        {
            DialogueSystem.Instance().OpenDialogueBox(id, this, firstMeeting, randomNPC);
            firstMeeting = false;
        }
        else
        {
            DialogueSystem.Instance().player.EndInteraction();
        }
    }

    public string GetIntroduction()
    {
        return quotes.introductions[Random.Range(0, quotes.introductions.Count)];
    }

    public string GetOpener()
    {
        return quotes.openers[Random.Range(0, quotes.openers.Count)];
    }

    public string GetCloser()
    {
        return quotes.closers[Random.Range(0, quotes.closers.Count)];
    }

    public string GetOpinion(CharacterID subject)
    {
        switch (subject)
        {
            case CharacterID.BLACK:
                return blackOpinion;
            case CharacterID.BLUE:
                return blueOpinion;
            case CharacterID.GREEN:
                return greenOpinion;
            case CharacterID.RED:
                return redOpinion;
            case CharacterID.YELLOW:
                return yellowOpinion;
            default:
                return "I don't know who you're talking about.";
        }
    }

    public string GetHint()
    {
        if (hasHint)
        {
            Character suspect = DialogueSystem.Instance().characters.IDToCharacter(hintBlames);
            string hint = quotes.startHintYes[Random.Range(0, quotes.startHintYes.Count)];
            //Regex suspectPattern = new Regex("/suspect/i");
            hint = Regex.Replace(hint, "Suspect", suspect.identifier);
            return hint;
        }
        else
        {
            return quotes.startHintNo[Random.Range(0, quotes.startHintNo.Count)];
        }
    }
}
