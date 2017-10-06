using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Provider : MonoBehaviour {
    static Provider instance;
    public PlayerController player;
    public DialogueBox dialogueBox;
    public GameOver gameOver;
    public Journal_Manager journal;
    public Clue_Manager clueSelector;
    public AlertSystem alertSystem;

    // Use this for initialization
    void Start () {
        instance = this;
        DialogueSystem.Instance(); //Initialize the dialogue system
        gameOver.gameObject.SetActive(false);
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
