using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;

public class SuspectDialogue : CharacterDialogue {

    Dictionary<string, ClueEntry> clueResponses;
    List<ClueFile> allClues;
    /*
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
    */


    // Use this for initialization
    override protected void Start () {
        base.Start();
        oneLiners = false;
        StartCoroutine("FetchClues");
        
        //DialogueSystem.Instance().AddSuspectListener(this);
        //clueResponses.Add("", new ClueEntry("", "", ""));
        clueResponses = new Dictionary<string, ClueEntry>();

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

    public static Suspect IDToSuspect(CharacterID charID)
    {
        switch (charID)
        {
            case CharacterID.BLACK:
                return Suspect.BLACK;
            case CharacterID.BLUE:
                return Suspect.BLUE;
            case CharacterID.GREEN:
                return Suspect.GREEN;
            case CharacterID.RED:
                return Suspect.RED;
            case CharacterID.YELLOW:
                return Suspect.YELLOW;
        }
        throw new System.Exception("Non-suspect ID was given to a suspect: " + charID.ToString());
    }

    public static CharacterID SuspectToID(Suspect suspect)
    {
        switch (suspect)
        {
            case Suspect.BLACK:
                return CharacterID.BLACK;
            case Suspect.BLUE:
                return CharacterID.BLUE;
            case Suspect.GREEN:
                return CharacterID.GREEN;
            case Suspect.RED:
                return CharacterID.RED;
            case Suspect.YELLOW:
                return CharacterID.YELLOW;
        }
        throw new System.Exception("Invalid suspect value given to character " + suspect.ToString());
    }

    IEnumerator FetchClues()
    {
        yield return new WaitForSeconds(3);
        allClues = DialogueSystem.Instance().GetCluesByOwner(IDToSuspect(id));
        Debug.Log(id.ToString() + " " + allClues.Count);

    }
}
