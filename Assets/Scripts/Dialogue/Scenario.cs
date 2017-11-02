using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

[Serializable]
public class Scenario{

    public static Scenario LoadJSON(string filename)
    {
        try
        {
            string json;
            StreamReader fileReader = new StreamReader(Path.Combine(DialogueSystem.folder, filename), Encoding.Default);
            json = fileReader.ReadLine();
            Scenario scenario = JsonUtility.FromJson<Scenario>(json);
            fileReader.Close();
            Debug.Log("Read success: " + Path.Combine("Scenarios", filename));
            return scenario;
        }
        catch
        {
            Debug.Log("Failed to load from JSON: " + Path.Combine(DialogueSystem.folder, filename));
            return new Scenario();
        }
    }

    public static void ExportJSON(Scenario clues)
    {
        try
        {
            string json = JsonUtility.ToJson(clues);
            Debug.Log(json);
            StreamWriter fileWriter = new StreamWriter(Path.Combine("Scenarios", GetTimeStampFileName() + ".json"), false, Encoding.Default);
            fileWriter.WriteLine(json);
            fileWriter.Close();
        }
        catch
        {
            Debug.Log("Failed to save scenario to JSON");
        }

    }

    public static string GetTimeStampFileName()
    {
        DateTime now = DateTime.Now;
        return now.Year + "-" + now.Month.ToString("00") + "-" + now.Day.ToString("00") + " " + now.Hour.ToString("00") + "h" + now.Minute.ToString("00") + "m" + now.Second.ToString("00") + "s";
    }

}
