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

    [Serializable]
    public class DialogueLine
    {
        public CharacterID spokenBy;
        public string line;

        public DialogueLine(CharacterID speaker, string content)
        {
            spokenBy = speaker;
            line = content;
        }
    }
}
