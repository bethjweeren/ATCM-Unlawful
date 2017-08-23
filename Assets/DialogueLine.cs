using System;

[Serializable]
public class DialogueLine
{
    public Names spokenBy;
    public string line;

    public DialogueLine(Names speaker, string content)
    {
        spokenBy = speaker;
        line = content;
    }
}
