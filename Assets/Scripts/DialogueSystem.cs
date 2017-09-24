﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Choice { BLACK_ALIBI, BLUE_LASTSEEN, GREEN_SUSPECTS, RED_OPINION, YELLOW_CLUE, CANCEL }
enum Prompt { DEFAULT, ACCUSATION, OPINION }

public class DialogueSystem
{
    static DialogueSystem instance;
    private static Object singletonLock = new Object();
    public Characters characters;
    public PlayerController player;
    public DialogueBox dialogueBox;
    public GameOver endGame;
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
                    quoteQueue.Add(new DialogueLine(currentNPC.GetHint(), nonPlayerID));
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
                    dialogueBox.DisplayLine(nonPlayerID, "This hasn't been implemented yet.", false);
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
        player.EndInteraction();
    }

    private void Accuse(CharacterID choice)
    {
        dialogueBox.transform.parent.gameObject.SetActive(false);
        player.EndInteraction();
        player.gameObject.SetActive(false);
        if (choice == CharacterID.BLUE)
        {
            endGame.WinGame("You guessed correctly");
        }
        else
        {
            endGame.LoseGame("The killer was <color=#193BFF>Bleu</color>");
        }
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
