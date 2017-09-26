using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Provider : MonoBehaviour {
    static Provider instance;
    public PlayerController player;
    public DialogueBox dialogueBox;
    public GameOver gameOver;

    // Use this for initialization
    void Start () {
        instance = this;
        DialogueSystem.Instance(); //Initialize the dialogue system
        DialogueSystem.Instance().CloseDialogueBox();
        gameObject.gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        if(instance == this)
        {
            instance = null;
        }
    }

    public static Provider GetInstance()
    {
        return instance;
    }
}
