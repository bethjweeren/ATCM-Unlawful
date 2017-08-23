using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Quotes {
    public List<DialogueLine> allQuotes;

    public Quotes()
    {
        allQuotes = new List<DialogueLine>();
    }
}
