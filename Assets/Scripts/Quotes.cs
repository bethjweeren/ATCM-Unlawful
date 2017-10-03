using System;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;

[Serializable]
public class Quotes {
    public List<string> introductions;
    public List<string> openers;
    public List<string> closers;
    public List<string> itemYes;
    public List<string> itemNo;
    public List<string> itemBut;
    public List<string> startHintYes;
    public List<string> startHintNo;
    public List<string> motiveGreenInnocent;
    public List<string> motiveGreenGuilty;
    public List<string> motiveYellowInnocent;
    public List<string> motiveYellowGuilty;
    public List<string> motiveRedInnocent;
    public List<string> motiveRedGuilty;
    public List<string> motiveBlackInnocent;
    public List<string> motiveBlackGuilty;
    public List<string> motiveBlueInnocent;
    public List<string> motiveBlueGuilty;
    public List<string> genericDontKnow;

    public Quotes()
    {
        introductions = new List<string>();
        openers = new List<string>();
        closers = new List<string>();
    }

    public static Quotes LoadJSON(string filename)
    {
        try
        {
            string json;
            StreamReader fileReader = new StreamReader(filename, Encoding.Default);
            json = fileReader.ReadLine();
            Quotes quotes = JsonUtility.FromJson<Quotes>(json);
            fileReader.Close();
            Debug.Log("Read success: " + filename);

            /*
            List<string> temp = new List<string>();
            foreach (string line in quotes.introductions)
            {
                temp.Add(FormatColors(line));
            }
            quotes.introductions = temp;

            temp = new List<string>();
            foreach (string line in quotes.openers)
            {
                temp.Add(FormatColors(line));
            }
            quotes.openers = temp;

            temp = new List<string>();
            foreach (string line in quotes.closers)
            {
                temp.Add(FormatColors(line));
            }
            quotes.closers = temp;
            */
            return quotes;
        }
        catch
        {
            Debug.Log("Failed to load from JSON: " + filename);
            return new Quotes();
        }
    }

    public static void ExportJSON(Quotes quotes, string filename)
    {
        try
        {
            string json = JsonUtility.ToJson(quotes);
            Debug.Log(json);
            StreamWriter fileWriter = new StreamWriter(filename, false, Encoding.Default);
            fileWriter.WriteLine(json);
            fileWriter.Close();
        }
        catch
        {
            Debug.Log("Failed to save to JSON: " + filename);
        }

    }

    /// <summary>
    /// Replaces format symbols with rich text formatting.
    /// </summary>
    /// <param name="quote">The unformatted string.</param>
    /// <returns>The formatted string.</returns>
    public static string FormatColors(string quote)
    {
        string line = quote;
        string final = "";
        //Process color codes.
        int startIndex = line.IndexOf('[');
        while (startIndex > -1)
        {
            final += line.Substring(0, startIndex);
            int endIndex = line.IndexOf(']');
            string name = line.Substring(startIndex + 1, endIndex - (startIndex + 1));
            DialogueSystem dialogueSys = DialogueSystem.Instance();
            Character person = dialogueSys.characters.IDToCharacter(dialogueSys.characters.NameToID(name.ToUpper()));
            name = "<color=" + person.color + ">" + person.name + "</color>";
            final += name;
            line = line.Substring(endIndex + 1, line.Length - (endIndex + 1));
            startIndex = line.IndexOf('[');
        }
        final += line;

        line = final;
        final = "";
        bool small = false;
        //Process text size.
        int formatIndex = line.IndexOf('%');
        while(formatIndex > -1)
        {
            final += line.Substring(0, formatIndex);
            string format;
            if (!small)
            {
                int smallSize = 3*(DialogueSystem.Instance().dialogueTextSize / 4);
                format = "<size=" + smallSize.ToString() + ">";
                small = true;
            }
            else
            {
                format = "</size>";
                small = false;
            }
            final += format;
            line = line.Substring(formatIndex + 1, line.Length - (formatIndex + 1));
            formatIndex = line.IndexOf('%');
        }
        final += line;
        if (small)
        {
            final += "</size>";
        }
        return final;
    }
}

/*
    [Serializable]
    public class DialogueLine
    {
        //public CharacterID spokenBy;
        public string line;

        public DialogueLine(CharacterID speaker, string content)
        {
            //spokenBy = speaker;
            line = content;
        }

        
    }

*/
