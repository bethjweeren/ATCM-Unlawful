using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Choice { BLACK_ALIBI, BLUE_LASTSEEN, GREEN_SUSPECTS, RED_OPINION, YELLOW_CLUE, CANCEL }
enum Prompt { DEFAULT, ACCUSATION, OPINION }

public class DialogueSystem
{
    static DialogueSystem instance;
    private static Object singletonLock = new Object();
    public Characters characters;
    DialogueBox dialogueBox;
    NPCDialogue currentNPC;
    List<DialogueLine> quoteQueue;
    Prompt dialoguePrompt;
    CharacterID nonPlayerID;
    public int dialogueTextSize = 18;

    public DialogueSystem()
    {
        characters = new Characters();
        quoteQueue = new List<DialogueLine>();
        //This part is temporary
        characters.NameDict.Add("LOCATION", CharacterID.VICTIM2);
        characters.CharacterDict.Add(CharacterID.VICTIM2, new Character("the town square", "LOCATION", "#FFFFFF"));

        characters.NameDict.Add("WEAPON", CharacterID.WEAPON);
        characters.CharacterDict.Add(CharacterID.WEAPON, new Character("twine", "WEAPON", "#FFFFFF"));

        while(Provider.GetInstance() == null)
        {

        }
        dialogueBox = Provider.GetInstance().dialogueBox;
        CloseDialogueBox();
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

    public void ProcessChoice(Choice selection)
    {
        switch (selection)
        {
            case Choice.BLACK_ALIBI:
                if(dialoguePrompt == Prompt.ACCUSATION)
                {
                    Accuse(CharacterID.BLACK);
                }
                else if(dialoguePrompt == Prompt.OPINION)
                {
                    quoteQueue.Add(new DialogueLine(currentNPC.GetOpinion(CharacterID.BLACK), nonPlayerID));
                    NextLine();
                }
                else
                {
                    dialogueBox.DisplayLine(nonPlayerID, "This hasn't been implemented yet.", false);
                }
                break;
            case Choice.BLUE_LASTSEEN:
                if (dialoguePrompt == Prompt.ACCUSATION)
                {
                    Accuse(CharacterID.BLUE);
                }
                else if (dialoguePrompt == Prompt.OPINION)
                {
                    quoteQueue.Add(new DialogueLine(currentNPC.GetOpinion(CharacterID.BLUE), nonPlayerID));
                    NextLine();
                }
                else
                {
                    dialogueBox.DisplayLine(nonPlayerID, "This hasn't been implemented yet.", false);
                }
                break;
            case Choice.GREEN_SUSPECTS:
                if (dialoguePrompt == Prompt.ACCUSATION)
                {
                    Accuse(CharacterID.GREEN);
                }
                else if (dialoguePrompt == Prompt.OPINION)
                {
                    quoteQueue.Add(new DialogueLine(currentNPC.GetOpinion(CharacterID.GREEN), nonPlayerID));
                    NextLine();
                }
                else
                {
                    quoteQueue.Add(new DialogueLine(currentNPC.GetFirstHint(), nonPlayerID));
                    NextLine();
                }
                break;
            case Choice.RED_OPINION:
                if (dialoguePrompt == Prompt.ACCUSATION)
                {
                    Accuse(CharacterID.RED);
                }
                else if (dialoguePrompt == Prompt.OPINION)
                {
                    quoteQueue.Add(new DialogueLine(currentNPC.GetOpinion(CharacterID.RED), nonPlayerID));
                    NextLine();
                }
                else
                {
                    dialoguePrompt = Prompt.OPINION;
                    dialogueBox.DisplayChoices(new string[6] { "<color=#191919>Noir</color>.", "<color=#193BFF>Bleu</color>.", "<color=#11B211>Vert</color>.", "<color=#FF1919>Rouge</color>.", "<color=#FFFF32>Jaune</color>.", "Nevermind." });
                }
                break;
            case Choice.YELLOW_CLUE:
                if (dialoguePrompt == Prompt.ACCUSATION)
                {
                    Accuse(CharacterID.YELLOW);
                }
                else if (dialoguePrompt == Prompt.OPINION)
                {
                    quoteQueue.Add(new DialogueLine(currentNPC.GetOpinion(CharacterID.YELLOW), nonPlayerID));
                    NextLine();
                }
                else
                {
                    Provider.GetInstance().clueSelector.Open();
                }
                break;
            case Choice.CANCEL:
            default:
                if(dialoguePrompt == Prompt.OPINION)
                {
                    NextChoice();
                }
                else
                {
                    dialogueBox.DisplayLine(nonPlayerID, currentNPC.GetCloser(), true);
                }
                break;
        }
    }

    public void ProcessClue(string clueID)
    {
        quoteQueue.Add(new DialogueLine(currentNPC.CheckClue(clueID), nonPlayerID));
        NextLine();
    }

    public void NextLine()
    {
        if(quoteQueue.Count > 0)
        {
            DialogueLine nextLine = quoteQueue[0];
            quoteQueue.RemoveAt(0);
            dialogueBox.DisplayLine(nextLine.speaker, nextLine.line, false);
        }
        else
        {
            NextChoice();
        }
    }

    void NextChoice()
    {
        string[] choiceText;
        if (nonPlayerID == CharacterID.PURPLE)
        {
            dialoguePrompt = Prompt.ACCUSATION;
            choiceText = new string[6] { "<color=#191919>Noir</color>.", "<color=#193BFF>Bleu</color>.", "<color=#11B211>Vert</color>.", "<color=#FF1919>Rouge</color>.", "<color=#FFFF32>Jaune</color>.", "Nevermind." };
        }
        else
        {
            dialoguePrompt = Prompt.DEFAULT;
            choiceText = new string[6] { "Where were you..?", "When did you last see the victim?", "Who could have done this?", "Tell me a little about...", "Can I ask you about..?", "Goodbye." };
        }
        dialogueBox.DisplayChoices(choiceText);
    }

    public void OpenDialogueBox(CharacterID id, NPCDialogue npc, bool firstMeeting, bool oneliner)
    {
        dialogueBox.transform.parent.gameObject.SetActive(true);
        dialogueBox.SetPortrait(id);
        currentNPC = npc;
        nonPlayerID = id;
        if (firstMeeting)
        {
            dialogueBox.DisplayLine(id, npc.GetIntroduction(), oneliner);
        }
        else
        {
            dialogueBox.DisplayLine(id, npc.GetOpener(), oneliner);
        }
    }

    public void CloseDialogueBox()
    {
        dialogueBox.transform.parent.gameObject.SetActive(false);
        Provider.GetInstance().player.EndInteraction();
    }

    private void Accuse(CharacterID choice)
    {
        dialogueBox.transform.parent.gameObject.SetActive(false);
        PlayerController player = Provider.GetInstance().player;
        player.EndInteraction();
        player.gameObject.SetActive(false);
        GameOver gameOver = Provider.GetInstance().gameOver;
        if (choice == CharacterID.BLUE)
        {
            gameOver.WinGame("You guessed correctly");
        }
        else
        {
            gameOver.LoseGame("The killer was <color=#193BFF>Bleu</color>");
        }
    }

    public void CreateJournalEntry(string summary, CharacterID page, string clueID)
    {
        Provider provider = Provider.GetInstance();
        provider.journal.CreateAutoJournalEntry(Quotes.FormatColors(summary), page);
        provider.clueSelector.CreateAutoJournalEntry(Quotes.FormatColors(summary), page, clueID);
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
