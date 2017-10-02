using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;

public struct ClueEntry
{
    public string text, summary, nextClueID;

    public ClueEntry(string text, string summary, string next)
    {
        this.text = text;
        this.summary = summary;
        nextClueID = next;
    }
}

public class NPCDialogue : MonoBehaviour, IInteractable {

    public CharacterID id;
    Dictionary<string, ClueEntry> clueResponses;
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
            
            clueResponses = new Dictionary<string, ClueEntry>();
            switch (id)
            {
                case CharacterID.BLACK:
                    clueResponses.Add("blackIsMean", new ClueEntry("Yes, I called [Red] a loser. So what? What are you going to do, have me arrested?", "Stop [Black], he violated the law", "blueSnitch"));
                    break;
                case CharacterID.BLUE:
                    clueResponses.Add("blueSnitch", new ClueEntry("Huh? [Black] is being a colossal jerk again? Figures. I'd go teach him a lesson, but I hate working on an empty belly.", "[Blue] is too lazy to get his own **** baguette", "yellowBread"));
                    clueResponses.Add("blueGotBread", new ClueEntry("Thanks, I'll get to arresting that loudmouth. | Oh, by the way I was the one who killed [Victim]. | I mean come on, he was asking for it... even his NAME meant 'DEAD'.", "[Blue] totally killed [Victim]", "fin"));
                    break;
                case CharacterID.GREEN:
                    clueResponses.Add("greenStuff", new ClueEntry("Oh, [Yellow] wants her stuff back? Fine.", "[Green] gave me [Yellow]'s things back", "yellowGotStuff"));
                    break;
                case CharacterID.RED:
                    clueResponses.Add("talkToRed", new ClueEntry("Hey, [Black] said some mean things to me.", "[Black] is a huge jerk", "blackIsMean"));
                    break;
                case CharacterID.YELLOW:
                    clueResponses.Add("yellowBread", new ClueEntry("Hey there honey, I can't bake any bread right now because I lent my pans to [Green]. Like, ALL of my pans.", "How do you even lend your entire kitchen to someone?", "greenStuff"));
                    clueResponses.Add("yellowGotStuff", new ClueEntry("Thanks, here you go - one fresh loaf", "Got the stupid bread", "blueGotBread"));
                    break;
            }
            

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
            Provider.GetInstance().player.EndInteraction();
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

    public string GetFirstHint()
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

    public string CheckClue(string clueID)
    {
        ClueEntry response;
        try
        {
            if (!clueResponses.TryGetValue(clueID, out response))
            {
                return quotes.genericDontKnow[Random.Range(0, quotes.genericDontKnow.Count)];
            }

        }
        catch
        {
            return quotes.startHintYes[Random.Range(0, quotes.startHintYes.Count)];
        }
        DialogueSystem.Instance().CreateJournalEntry(response.summary, id, response.nextClueID);
        return response.text;
    }

    public string ReplaceSuspect(string line, CharacterID suspect)
    {
        Character suspectCharacter = DialogueSystem.Instance().characters.IDToCharacter(suspect);
        string hint = line;
        return Regex.Replace(hint, "Suspect", suspectCharacter.identifier);
    }
}
