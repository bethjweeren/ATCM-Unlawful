using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueButton : MonoBehaviour {

    public DialogueBox manager;
    public int number;

    public void DoClick()
    {
        manager.ClickButton(number);
    }
}
