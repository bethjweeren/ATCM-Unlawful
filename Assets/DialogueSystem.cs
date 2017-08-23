using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;


public class DialogueSystem
{
    Characters characters;
    public Quotes quotes;

    public DialogueSystem()
    {
        characters = new Characters();
        quotes = new Quotes();
    }

    public string formatColors(string line)
    {
        string final = "";
        int startIndex = line.IndexOf('[');
        while (startIndex > -1)
        {
            final += line.Substring(0, startIndex);
            int endIndex = line.IndexOf(']');
            string name = line.Substring(startIndex + 1, endIndex - (startIndex + 1));
            Character person = characters.NameToCharacter(characters.StringToName(name.ToUpper()));
            name = "<color=" + person.color + ">" + person.name + "</color>";
            final += name;
            line = line.Substring(endIndex + 1, line.Length - (endIndex + 1));
            startIndex = line.IndexOf('[');
        }
        final += line;
        return final;
    }

    public void LoadJSON(string filename)
    {
        try
        {
            string json;
            StreamReader fileReader = new StreamReader(filename, Encoding.Default);
            json = fileReader.ReadLine();
            quotes = JsonUtility.FromJson<Quotes>(json);
            fileReader.Close();
            Debug.Log("read success");
            foreach(DialogueLine quote in quotes.allQuotes)
            {
                quote.line = formatColors(quote.line);
            }
        }
        catch
        {
            Debug.Log("Failed to load from JSON");
        }
        
    }

    public void ExportJSON(string filename)
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
            Debug.Log("Failed to save to JSON");
        }
        
    }
}
