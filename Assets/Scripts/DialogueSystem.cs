using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Choice { BLACK_WHERE, BLUE_WHEN, GREEN, RED, YELLOW, CANCEL }

public class DialogueSystem
{
    static DialogueSystem instance;
    private static Object singletonLock = new Object();
    public Characters characters;
    public PlayerController player;
    public DialogueBox dialogueBox;
    Quotes quotes;
    List<DialogueLine> quoteQueue;
    CharacterID nonPlayerID;

    public DialogueSystem()
    {
        characters = new Characters();
        quoteQueue = new List<DialogueLine>();
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
            string[] choiceText;
            if(nonPlayerID == CharacterID.PURPLE)
            {
                choiceText = new string[6] { "<color=#191919>Noir</color>.", "<color=#193BFF>Bleu</color>.", "<color=#11B211>Vert</color>.", "<color=#FF1919>Rouge</color>.", "<color=#FFFF32>Jaune</color>.", "Nevermind."};
            }
            else
            {
                choiceText = new string[6] { "Where were you..?", "When did you last see the victim?", "Who could have done this?", "What's your opinion of...", "Can I ask you about..?", "Goodbye." };
            }
            dialogueBox.DisplayChoices(choiceText);
        }
    }

    public void OpenDialogueBox(CharacterID id, Quotes npcQuotes, bool firstMeeting)
    {
        dialogueBox.transform.parent.gameObject.SetActive(true);
        dialogueBox.SetPortrait(id);
        quotes = npcQuotes;
        nonPlayerID = id;
        string line;
        if (firstMeeting)
        {
            line = quotes.introductions[Random.Range(0, quotes.introductions.Count)];
            dialogueBox.DisplayLine(id, line, false);
        }
        else
        {
            line = quotes.openers[Random.Range(0, quotes.openers.Count)];
            dialogueBox.DisplayLine(id, line, false);
        }
    }

    public void CloseDialogueBox()
    {
        dialogueBox.transform.parent.gameObject.SetActive(false);
        player.EndInteraction();
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
