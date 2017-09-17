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

    static string FormatColors(string quote)
    {
        string line = quote;
        string final = "";
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
        return line;
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
