using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

[Serializable]
public class ClueList {

    public List<ClueFile> clues;

    public ClueList()
    {
        clues = new List<ClueFile>();
    }

    public ClueList(List<ClueFile> clues)
    {
        this.clues = clues;
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
    public string name;
    public List<CharacterID> owners;
    public List<CharacterID> subjects;
    public string content;
    public string beggarSummary;
    public string journalSummary;
    public List<string> tags;

    public ClueFile(string clueName, List<CharacterID> clueOwners, List<CharacterID> clueSubjects, string clueContent, string clueBeggarSummary, string clueJournalSummary, List<string> clueTags)
    {
        name = clueName;
        owners = clueOwners;
        subjects = clueSubjects;
        content = clueContent;
        beggarSummary = clueBeggarSummary;
        journalSummary = clueJournalSummary;
        tags = clueTags;
    }
}

public struct Clue
{
    public CharacterID owner;
    public CharacterID subject;
    public string content;
    public string journalSummary;
    public string beggarSummary;
    public string id;
    public bool truth;
}
