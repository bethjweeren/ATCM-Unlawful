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
    protected bool firstMeeting = true;
    protected bool oneLiners;
    protected Quotes quotes;

	// Use this for initialization
	virtual protected void Start () {
        Debug.Log("GenericInit");
        LoadQuotes();
    }

    void LoadQuotes()
    {
        quotes = Quotes.LoadJSON(dialogueFile);
        if (quotes != null)
        {
            if (quotes.introductions.Count == 0)
            {
                firstMeeting = false;
            }
        }
    }

    virtual public void Interact()
    {
        if(quotes != null)
        {
            DialogueSystem.Instance().OpenDialogueBox(id, this, firstMeeting, false);
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

    /*public string GetOpinion(CharacterID subject)
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
    }*/

    public string ReplaceSuspect(string line, CharacterID suspect)
    {
        Character suspectCharacter = DialogueSystem.Instance().characters.IDToCharacter(suspect);
        string hint = line;
        return Regex.Replace(hint, "Suspect", suspectCharacter.identifier);
    }


    
}
