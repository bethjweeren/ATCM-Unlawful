using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SixChoice { BLACK, BLUE, GREEN, RED, YELLOW, CANCEL }
public enum FourChoice { MOTIVE, OPPORTUNITY, CLUE, CANCEL }
public enum ChoiceStyle { DEFAULT, FOUR, SIX }
enum Prompt { DEFAULT, ACCUSATION, MOTIVE, OPPORTUNITY }

public enum Suspect { BLACK, BLUE, GREEN, RED, YELLOW }

public class DialogueSystem
{
    public static readonly string folder = "Dialogue";

    static DialogueSystem instance;
    private static Object singletonLock = new Object();
    public Characters characters;
    NPCDialogue currentNPC;
    List<DialogueLine> quoteQueue;
    Prompt dialoguePrompt;
    CharacterID nonPlayerID;
    public int dialogueTextSize = 18;
    public bool blueIsKiller;

    List<ClueFile> clues;
    public List<ClueFile> items;
    public Scenario scenario;
    bool killedWithRightHand;

    public DialogueSystem()
    {
        //blueIsKiller = Random.value < 0.5f;
        blueIsKiller = false;

        characters = new Characters();
        quoteQueue = new List<DialogueLine>();

        scenario = new Scenario();
        Scenario.ExportJSON(scenario);

        if (scenario.killer == Suspect.BLUE || scenario.killer == Suspect.YELLOW) //Right-handed killer
        {
            killedWithRightHand = true;
        }
        else if(scenario.killer == Suspect.BLACK)
        {
            bool killedWithRightHand = Random.value < 0.5f;
        }
        else
        {
            killedWithRightHand = false;
        }

        clues = new List<ClueFile>();
        ClueList motives = ClueList.LoadJSON("Motive.json");
        foreach (ClueFile clue in motives.clues)
        {
            clues.Add(clue);
        }
        ClueList opportunities = ClueList.LoadJSON("Opportunity.json");
        foreach (ClueFile clue in opportunities.clues)
        {
            clues.Add(clue);
        }
        ClueList weapons = ClueList.LoadJSON("Weapon.json");
        foreach(ClueFile clue in weapons.clues)
        {
            clues.Add(clue);
        }

        ClueList itemFile = ClueList.LoadJSON("Items.json");
        items = itemFile.clues;


        //This part is temporary
        characters.NameDict.Add("LOCATION", CharacterID.VICTIM2);
        characters.CharacterDict.Add(CharacterID.VICTIM2, new Character("the town square", "LOCATION", "#FFFFFF"));

        characters.NameDict.Add("WEAPON", CharacterID.WEAPON);
        characters.CharacterDict.Add(CharacterID.WEAPON, new Character("a knife", "WEAPON", "#FFFFFF"));
    }

    public static DialogueSystem Instance()
    {
        lock (singletonLock)
        {
            if (instance == null)
            {
                instance = new DialogueSystem();
            }
        }
        return instance;
    }

    public void ProcessFourChoice(FourChoice selection)
    {
        switch (selection)
        {
            case FourChoice.MOTIVE:
                dialoguePrompt = Prompt.MOTIVE;
                Provider.GetInstance().dialogueBox.DisplayChoices(ChoiceStyle.SIX);
                break;
            case FourChoice.OPPORTUNITY:
                dialoguePrompt = Prompt.OPPORTUNITY;
                Provider.GetInstance().dialogueBox.DisplayChoices(ChoiceStyle.SIX);
                break;
            case FourChoice.CLUE:
                Provider.GetInstance().dialogueBox.gameObject.SetActive(false);
                Provider.GetInstance().clueSelector.Open();
                break;
            case FourChoice.CANCEL:
            default:
                Provider.GetInstance().dialogueBox.DisplayLine(nonPlayerID, currentNPC.GetCloser(), true);
                break;
        }
    }

    public void ProcessSixChoice(SixChoice selection)
    {
        switch (selection)
        {
            case SixChoice.BLACK:
                if (dialoguePrompt == Prompt.ACCUSATION)
                {
                    Accuse(CharacterID.BLACK);
                }
                else if (dialoguePrompt == Prompt.MOTIVE)
                {
                    ProcessClue("MOTIVE" + "BLACK");
                }
                else if (dialoguePrompt == Prompt.OPPORTUNITY)
                {
                    ProcessClue("OPP" + "BLACK");
                }
                break;
            case SixChoice.BLUE:
                if (dialoguePrompt == Prompt.ACCUSATION)
                {
                    Accuse(CharacterID.BLUE);
                }
                else if (dialoguePrompt == Prompt.MOTIVE)
                {
                    ProcessClue("MOTIVE" + "BLUE");
                }
                else if (dialoguePrompt == Prompt.OPPORTUNITY)
                {
                    ProcessClue("OPP" + "BLUE");
                }
                break;
            case SixChoice.GREEN:
                if (dialoguePrompt == Prompt.ACCUSATION)
                {
                    Accuse(CharacterID.GREEN);
                }
                else if (dialoguePrompt == Prompt.MOTIVE)
                {
                    ProcessClue("MOTIVE" + "GREEN");
                }
                else if (dialoguePrompt == Prompt.OPPORTUNITY)
                {
                    ProcessClue("OPP" + "GREEN");
                }
                break;
            case SixChoice.RED:
                if (dialoguePrompt == Prompt.ACCUSATION)
                {
                    Accuse(CharacterID.RED);
                }
                else if (dialoguePrompt == Prompt.MOTIVE)
                {
                    ProcessClue("MOTIVE" + "RED");
                }
                else if (dialoguePrompt == Prompt.OPPORTUNITY)
                {
                    ProcessClue("OPP" + "RED");
                }
                break;
            case SixChoice.YELLOW:
                if (dialoguePrompt == Prompt.ACCUSATION)
                {
                    Accuse(CharacterID.YELLOW);
                }
                else if (dialoguePrompt == Prompt.MOTIVE)
                {
                    ProcessClue("MOTIVE" + "YELLOW");
                }
                else if (dialoguePrompt == Prompt.OPPORTUNITY)
                {
                    ProcessClue("OPP" + "YELLOW");
                }
                break;
            case SixChoice.CANCEL:
            default:
                if (dialoguePrompt == Prompt.ACCUSATION)
                {
                    Provider.GetInstance().dialogueBox.DisplayLine(nonPlayerID, currentNPC.GetCloser(), true);
                }
                else
                {
                    NextChoice();
                }
                break;
        }
    }

    public void ProcessClue(string clueID)
    {
        Provider.GetInstance().dialogueBox.gameObject.SetActive(true);
        quoteQueue.Add(new DialogueLine(((SuspectDialogue)currentNPC).CheckClue(clueID), nonPlayerID));
        NextLine();
    }

    public void NextLine()
    {
        if(quoteQueue.Count > 0)
        {
            DialogueLine nextLine = quoteQueue[0];
            quoteQueue.RemoveAt(0);
            Provider.GetInstance().dialogueBox.DisplayLine(nextLine.speaker, nextLine.line, false);
        }
        else
        {
            NextChoice();
        }
    }

    void NextChoice()
    {
        ChoiceStyle style;
        if (nonPlayerID == CharacterID.PURPLE)
        {
            dialoguePrompt = Prompt.ACCUSATION;
            style = ChoiceStyle.SIX;
        }
        else
        {
            dialoguePrompt = Prompt.DEFAULT;
            style = ChoiceStyle.FOUR;
        }
        Provider.GetInstance().dialogueBox.DisplayChoices(style);
    }

    public void OpenDialogueBox(CharacterID id, NPCDialogue npc, bool firstMeeting, bool oneliner)
    {
        Provider.GetInstance().dialogueBox.transform.parent.gameObject.SetActive(true);
        Provider.GetInstance().dialogueBox.SetPortrait(id);
        currentNPC = npc;
        nonPlayerID = id;
        if (firstMeeting)
        {
            Provider.GetInstance().dialogueBox.DisplayLine(id, npc.GetIntroduction(), oneliner);
        }
        else
        {
            Provider.GetInstance().dialogueBox.DisplayLine(id, npc.GetOpener(), oneliner);
        }
    }

    public void PlayItemPickup(string text)
    {
        Provider.GetInstance().dialogueBox.transform.parent.gameObject.SetActive(true);
        Provider.GetInstance().dialogueBox.SetPortrait(CharacterID.PLAYER);
        Provider.GetInstance().dialogueBox.DisplayLine(CharacterID.PLAYER, text, true);
    }

    public void CloseDialogueBox()
    {
        Provider.GetInstance().dialogueBox.transform.parent.gameObject.SetActive(false);
        Provider.GetInstance().player.EndInteraction();
    }

    private void Accuse(CharacterID choice)
    {
        Provider.GetInstance().dialogueBox.transform.parent.gameObject.SetActive(false);
        PlayerController player = Provider.GetInstance().player;
        player.EndInteraction();
        player.gameObject.SetActive(false);
        GameOver gameOver = Provider.GetInstance().gameOver;
        if (scenario.killer == SuspectDialogue.IDToSuspect(choice))
        {
            gameOver.WinGame("You guessed correctly");
        }
        else
        {
            Character killer;
            characters.CharacterDict.TryGetValue(SuspectDialogue.SuspectToID(scenario.killer), out killer);
            //gameOver.LoseGame("The killer was <color=" + killer.color + ">" + killer.name + "</color>");
            gameOver.LoseGame("You guessed incorrectly");
        }
        
    }

    public void CreateJournalEntry(string summary, CharacterID page, string clueID)
    {
        Provider provider = Provider.GetInstance();
        if (!provider.clueSelector.ClueKnown(page, clueID))
        {
            provider.journal.CreateAutoJournalEntry(Quotes.FormatColors(summary), page);
            provider.clueSelector.CreateAutoJournalEntry(Quotes.FormatColors(summary), page, clueID);
            provider.alertSystem.CreateAlert("<color=" + characters.IDToCharacter(page).color + ">New entry added to Journal</color>");
            provider.clueSelector.UpdateClueKnown(page, clueID);
        }
    }

    public List<ClueFile> GetCluesByOwner(Suspect owner)
    {
        List<ClueFile> ownClues = new List<ClueFile>();
        foreach(ClueFile clue in clues)
        {
            if (clue.owners.Contains(owner))
            {
                ownClues.Add(clue);
            }
        }
        return ownClues;
    }

    public bool WasKilledWithRightHand()
    {
        return killedWithRightHand;
    }

    public void Reset()
    {
        instance = null;
    }
}



struct DialogueLine
{
    public string line;
    public CharacterID speaker;

    public DialogueLine(string l, CharacterID s)
    {
        line = l;
        speaker = s;
    }
}
