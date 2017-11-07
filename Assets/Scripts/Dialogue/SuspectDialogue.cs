using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;

public class SuspectDialogue : CharacterDialogue {

    Dictionary<string, ClueEntry> clueResponses;
    List<ClueFile> allClues;

    private readonly bool[] rightHanded = new bool[5] { true, true, false, false, true };

    override protected void Start () {
        base.Start();
        oneLiners = false;
        InitializeClues();
    }

    void InitializeClues()
    {
        //clueResponses.Add("", new ClueEntry("", "", ""));
        clueResponses = new Dictionary<string, ClueEntry>();
        allClues = DialogueSystem.Instance().GetCluesByOwner(IDToSuspect(id));

        List<ClueFile>[] motivesPositive = new List<ClueFile>[5] { new List<ClueFile>(), new List<ClueFile>(), new List<ClueFile>(), new List<ClueFile>(), new List<ClueFile>() };
        List<ClueFile>[] motivesNegative = new List<ClueFile>[5] { new List<ClueFile>(), new List<ClueFile>(), new List<ClueFile>(), new List<ClueFile>(), new List<ClueFile>() };
        List<ClueFile>[] opportunityGuilty = new List<ClueFile>[5] { new List<ClueFile>(), new List<ClueFile>(), new List<ClueFile>(), new List<ClueFile>(), new List<ClueFile>() };
        List<ClueFile>[] opportunityUnknown = new List<ClueFile>[5] { new List<ClueFile>(), new List<ClueFile>(), new List<ClueFile>(), new List<ClueFile>(), new List<ClueFile>() };
        List<ClueFile>[] opportunityInnocent = new List<ClueFile>[5] { new List<ClueFile>(), new List<ClueFile>(), new List<ClueFile>(), new List<ClueFile>(), new List<ClueFile>() };
        List<ClueFile> meansYes = new List<ClueFile>();
        List<ClueFile> meansNo = new List<ClueFile>();
        List<ClueFile> method = new List<ClueFile>();
        List<ClueFile> glovePositive = new List<ClueFile>();
        List<ClueFile> gloveNegative = new List<ClueFile>();

        foreach (ClueFile clue in allClues)
        {
            if (clue.tags.Contains("MOTIVE") && !clue.tags.Contains("GLOVE"))
            {
                if (clue.tags.Contains("POSITIVE"))
                {
                    CategorizeClue(clue, motivesPositive);
                }
                if (clue.tags.Contains("NEGATIVE"))
                {
                    CategorizeClue(clue, motivesNegative);
                }
            }
            if (clue.tags.Contains("OPPORTUNITY"))
            {
                if (clue.tags.Contains("GUILTY"))
                {
                    CategorizeClue(clue, opportunityGuilty);
                }
                if (clue.tags.Contains("UNKNOWN"))
                {
                    CategorizeClue(clue, opportunityUnknown);
                }
                if (clue.tags.Contains("INNOCENT"))
                {
                    CategorizeClue(clue, opportunityInnocent);
                }
            }
            if (clue.tags.Contains("MEANS"))
            {
                if (clue.tags.Contains("YES"))
                {
                    meansYes.Add(clue);
                }
                if (clue.tags.Contains("NO"))
                {
                    meansNo.Add(clue);
                }
            }
            if (clue.tags.Contains("METHOD"))
            {
                method.Add(clue);
            }
            if (clue.tags.Contains("GLOVE"))
            {
                if (clue.tags.Contains("POSITIVE"))
                {
                    glovePositive.Add(clue);
                }
                if (clue.tags.Contains("NEGATIVE"))
                {
                    gloveNegative.Add(clue);
                }
            }
        }

        Scenario scenario = DialogueSystem.Instance().scenario;
        Motive[] scenarioMotives = new Motive[5];
        Opportunity[] scenarioOpportunities = new Opportunity[5];

        switch (IDToSuspect(id))
        {
            case Suspect.BLACK:
                scenarioMotives = scenario.motives.black;
                scenarioOpportunities = scenario.opportunities.black;
                break;
            case Suspect.BLUE:
                scenarioMotives = scenario.motives.blue;
                scenarioOpportunities = scenario.opportunities.blue;
                break;
            case Suspect.GREEN:
                scenarioMotives = scenario.motives.green;
                scenarioOpportunities = scenario.opportunities.green;
                break;
            case Suspect.RED:
                scenarioMotives = scenario.motives.red;
                scenarioOpportunities = scenario.opportunities.red;
                break;
            case Suspect.YELLOW:
                scenarioMotives = scenario.motives.yellow;
                scenarioOpportunities = scenario.opportunities.yellow;
                break;
        }

        //Populate response dictionary
        for(int i = 0; i < 5; i++)
        {
            switch (scenarioMotives[i])
            {
                case Motive.POSITIVE:
                    CreateResponse(motivesPositive[i], "MOTIVE" + ((Suspect)i).ToString());
                    break;
                case Motive.NEGATIVE:
                    CreateResponse(motivesNegative[i], "MOTIVE" + ((Suspect)i).ToString());
                    break;
            }

            switch (scenarioOpportunities[i])
            {
                case Opportunity.GUILTY:
                    CreateResponse(opportunityGuilty[i], "OPP" + ((Suspect)i).ToString());
                    break;
                case Opportunity.UNKNOWN:
                    CreateResponse(opportunityUnknown[i], "OPP" + ((Suspect)i).ToString());
                    break;
                case Opportunity.INNOCENT:
                    CreateResponse(opportunityInnocent[i], "OPP" + ((Suspect)i).ToString());
                    break;
            }
        }

        int suspectID = (int)IDToSuspect(id);
        switch (scenarioMotives[suspectID])
        {
            case Motive.POSITIVE:
                CreateResponse(motivesPositive[suspectID], "MOTIVE", "MOTIVE" + ((Suspect)suspectID).ToString());
                break;
            case Motive.NEGATIVE:
                CreateResponse(motivesNegative[suspectID], "MOTIVE", "MOTIVE" + ((Suspect)suspectID).ToString());
                break;
        }

        switch (scenarioOpportunities[suspectID])
        {
            case Opportunity.GUILTY:
                CreateResponse(opportunityGuilty[suspectID], "OPP", "OPP" + ((Suspect)suspectID).ToString());
                break;
            case Opportunity.UNKNOWN:
                CreateResponse(opportunityUnknown[suspectID], "OPP", "OPP" + ((Suspect)suspectID).ToString());
                break;
            case Opportunity.INNOCENT:
                CreateResponse(opportunityInnocent[suspectID], "OPP", "OPP" + ((Suspect)suspectID).ToString());
                break;
        }

        switch (scenario.weapon[suspectID])
        {
            case Means.YES:
                CreateResponse(meansYes, "WEAPON" + ((Suspect)suspectID).ToString());
                CreateResponse(meansYes, "WEAPON", "WEAPON" + ((Suspect)suspectID).ToString());
                break;
            case Means.NO:
                CreateResponse(meansNo, "WEAPON" + ((Suspect)suspectID).ToString());
                CreateResponse(meansNo, "WEAPON", "WEAPON" + ((Suspect)suspectID).ToString());
                break;
        }

        CreateResponse(method, "METHOD", "NOCLUE");

        if(rightHanded[suspectID] ^ DialogueSystem.Instance().WasKilledWithRightHand()) //XOR. If the hands of the two don't match, then have a positive remark
        {
            CreateResponse(glovePositive, "GLOVE", "GLOVE" + ((Suspect)suspectID).ToString());
        }
        else
        {
            CreateResponse(gloveNegative, "GLOVE", "GLOVE" + ((Suspect)suspectID).ToString());
        }
    } 

    void CategorizeClue(ClueFile clue, List<ClueFile>[] category)
    {
        foreach(Suspect subject in clue.subjects)
        {
            int index = (int)subject;
            //if(category[index] == null)
            //{
            //    category[index] = new List<ClueFile>();
            //}
            category[index].Add(clue);
        }
    }

    void CreateResponse(List<ClueFile> source, string prompt, string trigger)
    {
        ClueFile clue = source[Random.Range(0, source.Count)];
        clueResponses.Add(prompt, new ClueEntry(clue.content, clue.journalSummary, trigger));
    }

    void CreateResponse(List<ClueFile> source, string prompt)
    {
        CreateResponse(source, prompt, prompt);
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
}
