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
        if (id == CharacterID.RANDO || id == CharacterID.VICTIM)
        {
            randomNPC = true;
        }

        

        if (Characters.IsSuspect(id))
        {
            //clueResponses.Add("", new ClueEntry("", "", ""));
            clueResponses = new Dictionary<string, ClueEntry>();
            switch (id)
            {
                case CharacterID.BLACK:
                    //Motive
                    clueResponses.Add("BLACKMOTIVEGREEN", new ClueEntry("Would you truly expect a critic of alcohol and a vintner to get along?", "[Black] said: [Green] and [Victim] had clashing opinion on alcohol.", "blackClashGreen"));
                    clueResponses.Add("BLACKMOTIVEYELLOW", new ClueEntry("Even those who do not follow the Lord must find solace in something. | Her comfort was found in [Victim] tending to her husband until the end.", "[Black] said: [Yellow] was happy that [Victim] made Mr. Yellow's last moments comfortable.", "blackHusbandYellow"));
                    clueResponses.Add("BLACKMOTIVERED", new ClueEntry("Their relationship appears to have been that of supply partners.  [Red]'s high quality art supplies often came from [Victim] importing them to [Red] when he visits. | In fact, much of the art around the church was made by [Red] using [Victim]'s supplies, so I doubt that their relationship was too sour.", "[Black] said: [Red] and [Victim] were supply partners.", "blackSupplierRed"));
                    clueResponses.Add("BLACKMOTIVEBLACK", new ClueEntry("[Victim] was... less than religious.  I believe he was a fool to reject the Lord. | Look where that landed him.", "[Black] said: [Black] believes that [Victim] was a fool to reject religion.", "blackReligionBlack"));
                    clueResponses.Add("BLACKMOTIVEBLACK", new ClueEntry("I cannot imagine that [Blue] cares for the man who promotes abstinence. | His confessionals, though, reveal a deeper hate of [Victim].  Why though, he would not say.", "[Black] said: [Blue] didn't like being judged by [Victim] for drinking.", "blackDrinkingBlue"));
                    //Motive Duplicates

                    //Opportunity
                    clueResponses.Add("", new ClueEntry("", "", ""));
                    clueResponses.Add("", new ClueEntry("", "", ""));
                    clueResponses.Add("", new ClueEntry("", "", ""));
                    clueResponses.Add("", new ClueEntry("", "", ""));
                    clueResponses.Add("", new ClueEntry("", "", ""));

                    break;
                case CharacterID.BLUE:
                    clueResponses.Add("BLUEMOTIVEGREEN", new ClueEntry("[Green] liked wine, %who can blame him,% and [Victim] didn't.  What is there to figure out?", "[Blue] said: [Victim] was against what [Green] stood for.", "blueAgainstGreen"));
                    clueResponses.Add("BLUEMOTIVEYELLOW", new ClueEntry("I dunno.  Why are you askin' me about [Yellow]? %not like she talks to me anyway% | Well, she's been a bit colder to me since her husband died.  Maybe she blames [Victim] for his death?", "[Blue] said: [Yellow] blamed [Victim] for her husband's death.", "blueHusbandYellow"));
                    clueResponses.Add("BLUEMOTIVERED", new ClueEntry("%(sigh)% Geez, why should I know? | I guess I saw [Victim] purchasing supplies from [Red] each time he was in town.  %not like her keeper or anything%", "[Blue] said: [Victim] often purchased supplies from [Red].", "blueSupplierRed"));
                    clueResponses.Add("BLUEMOTIVEBLACK", new ClueEntry("I think [Victim] gave the old man some news he didn't quite like. | Ever since a particular meeting, [Black] seems too angry for words when [Victim] is brought up.", "[Blue] said: [Victim] really ticked [Black] off somehow.", "blueAngryBlack"));
                    clueResponses.Add("BLUEMOTIVEBLUE", new ClueEntry("[Victim] was a playboy for the ages.  He couldn't keep his hands off of [Yellow]. | I'm not happy that he's dead though. %but at least she won't be harassed anymore.%", "[Blue] said: [Blue] didn't like that [Victim] harassed [Yellow].", "blueHarassYellow"));
                    //Motive Duplicates

                    //Opportunity
                    clueResponses.Add("", new ClueEntry("", "", ""));
                    clueResponses.Add("", new ClueEntry("", "", ""));
                    clueResponses.Add("", new ClueEntry("", "", ""));
                    clueResponses.Add("", new ClueEntry("", "", ""));
                    clueResponses.Add("", new ClueEntry("", "", ""));

                    break;
                case CharacterID.GREEN:
                    clueResponses.Add("GREENMOTIVEGREEN", new ClueEntry("[Victim] may have been my personal doctor, but I assure you that it was purely ... out of necessity.", "[Green] said: [Victim] was [Green]'s personal doctor.", "greenDoctorGreen"));
                    clueResponses.Add("GREENMOTIVEYELLOW", new ClueEntry("It's a bit ... tragic really.  [Yellow]'s husband died under [Victim]'s care. | I doubt she ever really got over that...", "[Green] said: [Yellow]'s husband died under [Victim]'s care.", "greenHusbandYellow"));
                    clueResponses.Add("GREENMOTIVERED", new ClueEntry("They were business partners. | [Victim] brought art supplies and medical skills, [Red] supplied finished art, chocolates, and crafts. | The partnership appears mutual.  I doubt that [Red] would kill her ... supplier.", "[Green] said: [Victim] was [Red]'s craft supplier.", "greenSupplierRed"));
                    clueResponses.Add("GREENMOTIVEBLACK", new ClueEntry("When I brought [Victim] in, [Black] seemed to be more ... relaxed. | I suppose it saved him the headache of having to deal with the deceased.", "[Green] says: [Black] was calmed by [Victim]'s visits.", "greenHeadacheBlack"));
                    clueResponses.Add("GREENMOTVIEBLUE", new ClueEntry("A visit from [Victim] seems to calm everyone down.  Naturally, [Blue] seems laziest during [Victim]'s visits. | I doubt that he loathes the ... days off.", "[Blue] enjoys slacking when [Victim] visits.", "greenSlackBlue"));
                    //Motive Duplicates

                    //Opportunity
                    clueResponses.Add("", new ClueEntry("", "", ""));
                    clueResponses.Add("", new ClueEntry("", "", ""));
                    clueResponses.Add("", new ClueEntry("", "", ""));
                    clueResponses.Add("", new ClueEntry("", "", ""));
                    clueResponses.Add("", new ClueEntry("", "", ""));

                    break;
                case CharacterID.RED:
                    clueResponses.Add("REDMOTIVEGREEN", new ClueEntry("[Green] has the money to expedite [Victim]'s services when needed. | %must be nice having that kind of money.%", "[Red] said: [Green] expedited [Victim]'s visits when he needed him.", "redExpeditedGreen"));
                    clueResponses.Add("REDMOTIVEYELLOW", new ClueEntry("Can't blame her for hating [Victim]. | With the amount of money that [Victim] took as payment despite failing to keep [Yellow]'s husband alive, I'd hate the bastard too.", "[Red] said: [Yellow] was charged after [Victim] failed to save her husband.", "redHusbandYellow"));
                    clueResponses.Add("REDMOTIVERED", new ClueEntry("I think [Victim] bought things from me just to turn around and charge me for his medical services. | [Victim] wasn't exactly the most curtious of shoppers though.  Always picking apart my worship of God.", "[Red] said: [Victim] was a bad customer to [Red].", "redMotiveRed"));
                    clueResponses.Add("REDMOTIVEBLACK", new ClueEntry("The friar may not like most people, but he can't really complain about someone that's done so much good for the town.", "[Red] said: [Black] liked the good that [Victim] did for the town.", "redLikedBlack"));
                    clueResponses.Add("REDMOTIVEBLUE", new ClueEntry("Rumor has it that [Yellow] had a thing for [Victim], as messed up as that sounds. | Even worse, I think [Victim] returned the feelings. | So yeah, you guessed it, [Blue] hates him. %(sigh)%", "[Red] said: [Blue] is jealous of [Victim]'s love life.", "redJealousBlue"));
                    //Motive Duplicates

                    //Opportunity
                    clueResponses.Add("", new ClueEntry("", "", ""));
                    clueResponses.Add("", new ClueEntry("", "", ""));
                    clueResponses.Add("", new ClueEntry("", "", ""));
                    clueResponses.Add("", new ClueEntry("", "", ""));
                    clueResponses.Add("", new ClueEntry("", "", ""));

                    break;
                case CharacterID.YELLOW:
                    clueResponses.Add("YELLOWMOTIVEGREEN", new ClueEntry("I don't know about their personal relationship, but I know that [Victim] only comes into town when [Green] expedites him. | [Green] must really prefer [Victim] to be his doctor.  Don't you think, honey?", "[Yellow] said: [Green] expedited [Victim]'s visits when he needed him.", "yellowExpeditedGreen"));
                    clueResponses.Add("YELLOWMOTIVEYELLOW", new ClueEntry("[Victim] seemed like a kind man.  However, %and pardon my bluntness, honey,% [Victim] was not a good doctor. | He is what we had though.  I doubt anyone is looking forward to the upcoming months without a doctor.", "[Yellow] said: [Yellow] thinks that [Victim] was a bad doctor.", "yellowHusbandYellow"));
                    clueResponses.Add("YELLOWMOTIVERED", new ClueEntry("I really shouldn't say anything, but... | Well, [Victim] was a very outspoken athiest, and you know [Red], hot tempered and religious. | I can't imagine their conversations lasted long or ended well.", "[Yellow] said: [Red] and [Victim]'s religious views clashed.", "yellowReligionRed"));
                    clueResponses.Add("YELLOWMOTIVEBLACK", new ClueEntry("Ah, yes, the friar didn't seem to have any major problems with [Victim]. | In fact, he seemed less sour than usual when [Victim] was in town.  Probably took some of the stress of death off his mind.", "[Yellow] said: [Black] seemed calmed by [Victim]'s visits.", "yellowHeadacheBlack"));
                    clueResponses.Add("YELLOWMOTIVEBLUE", new ClueEntry("Oh, that [Blue].  His face turns red as an apple pie when he sees [Victim] at my bakery. | [Victim] seems very respectful and is always such a sweetie to me.  I can't imagine why [Blue] wouldn't like him.", "[Yellow] said: [Blue] didn't like [Victim], for some reason.", "yellowFaceBlue"));
                    //Motive Duplicates
                    clueResponses.Add("greenHusbandYellow", new ClueEntry("[Victim] seemed like a kind man.  However, %and pardon my bluntness, honey,% [Victim] was not a good doctor. | He is what we had though.  I doubt anyone is looking forward to the upcoming months without a doctor.", "[Yellow] said: [Yellow] thinks that [Victim] was a bad doctor.", "yellowHusbandYellow"));
                    clueResponses.Add("redHusbandYellow", new ClueEntry("[Victim] seemed like a kind man.  However, %and pardon my bluntness, honey,% [Victim] was not a good doctor. | He is what we had though.  I doubt anyone is looking forward to the upcoming months without a doctor.", "[Yellow] said: [Yellow] thinks that [Victim] was a bad doctor.", "yellowHusbandYellow"));
                    clueResponses.Add("blackHusbandYellow", new ClueEntry("[Victim] seemed like a kind man.  However, %and pardon my bluntness, honey,% [Victim] was not a good doctor. | He is what we had though.  I doubt anyone is looking forward to the upcoming months without a doctor.", "[Yellow] said: [Yellow] thinks that [Victim] was a bad doctor.", "yellowHusbandYellow"));
                    //Opportunity
                    clueResponses.Add("", new ClueEntry("", "", ""));
                    clueResponses.Add("", new ClueEntry("", "", ""));
                    clueResponses.Add("", new ClueEntry("", "", ""));
                    clueResponses.Add("", new ClueEntry("", "", ""));
                    clueResponses.Add("", new ClueEntry("", "", ""));

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
            if(id == CharacterID.VICTIM)
            {
                StartCoroutine("ExamineBody");

            }
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

    IEnumerator ExamineBody()
    {
        DialogueSystem.Instance().CreateJournalEntry("The victim was [Victim].", CharacterID.VICTIM, "motive");
        yield return new WaitForSeconds(0.05f);
        DialogueSystem.Instance().CreateJournalEntry("[Victim] was strangled.", CharacterID.VICTIM, "method");
        yield return new WaitForSeconds(0.05f);
        DialogueSystem.Instance().CreateJournalEntry("[Victim] was killed in the Town Square.", CharacterID.VICTIM, "opportunity");
    }
}
