using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tester : MonoBehaviour {
    public DialogueSystem dialogue;

	void Start () {
        dialogue = new DialogueSystem();

        dialogue.LoadJSON("test.json");
        dialogue.ExportJSON("output2.json");
    }
}
