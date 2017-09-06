using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DialogueSystem
{
    static DialogueSystem instance;
    private static Object singletonLock = new Object();
    public Characters characters;
    public PlayerController player;
    public DialogueBox dialogueBox;

    public DialogueSystem()
    {
        characters = new Characters();
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

    public void OpenDialogueBox()
    {
        dialogueBox.transform.parent.gameObject.SetActive(true);
    }

    public void CloseDialogueBox()
    {
        dialogueBox.transform.parent.gameObject.SetActive(false);
        player.EndInteraction();
    }
}
