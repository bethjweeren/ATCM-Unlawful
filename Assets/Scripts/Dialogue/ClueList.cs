using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

[Serializable]
public class ClueList {

    public List<ClueFile> allClues;

    public ClueList()
    {
        allClues = new List<ClueFile>();
        allClues.Add(new ClueFile());
        ExportJSON(this, "cluetest.json");
    }

    public static ClueList LoadJSON(string filename)
    {
        try
        {
            string json;
            StreamReader fileReader = new StreamReader(filename, Encoding.Default);
            json = fileReader.ReadLine();
            ClueList clues = JsonUtility.FromJson<ClueList>(json);
            fileReader.Close();
            Debug.Log("Read success: " + filename);
            return clues;
        }
        catch
        {
            Debug.Log("Failed to load from JSON: " + filename);
            return new ClueList();
        }
    }

    public static void ExportJSON(ClueList clues, string filename)
    {
        try
        {
            string json = JsonUtility.ToJson(clues);
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
}

[Serializable]
public struct ClueFile
{
    public List<CharacterID> owners;
    public List<CharacterID> subjects;
    public string content;
    public string journalSummary;
    public string beggarSummary;
    public List<string> tags;
}

public struct Clue
{
    public CharacterID owner;
    public CharacterID subject;
    public string content;
    public string journalSummary;
    public string beggarSummary;
    public bool truth;
}
