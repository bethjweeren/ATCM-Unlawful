using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public enum Motive { POSITIVE, NEGATIVE }
public enum Opportunity { GUILTY, UNKNOWN, INNOCENT }
public enum Means { YES, NO }

[System.Serializable]
public class Scenario{

    public Suspect killer;
    public MotiveList motives;
    public OpportunityList opportunities;
    public Means[] weapon;

    public Scenario()
    {
        killer = (Suspect)Random.Range(0, 5);
        AssignMotives();
        AssignOpportunities();
        weapon = new Means[5];
        AssignMeans();
    }

    void AssignMotives()
    {
        Motive[][] recipientMotiveTemplates = new Motive[][] {
                                      new Motive[5] { Motive.POSITIVE, Motive.NEGATIVE, Motive.NEGATIVE, Motive.NEGATIVE, Motive.NEGATIVE },
                                      new Motive[5] { Motive.POSITIVE, Motive.NEGATIVE, Motive.NEGATIVE, Motive.NEGATIVE, Motive.NEGATIVE },
                                      new Motive[5] { Motive.POSITIVE, Motive.POSITIVE, Motive.NEGATIVE, Motive.NEGATIVE, Motive.NEGATIVE },
                                      new Motive[5] { Motive.POSITIVE, Motive.POSITIVE, Motive.POSITIVE, Motive.NEGATIVE, Motive.NEGATIVE },
                                      new Motive[5] { Motive.POSITIVE, Motive.POSITIVE, Motive.POSITIVE, Motive.POSITIVE, Motive.NEGATIVE }
                                    };
        List<List<Motive>> remainingMotives = new List<List<Motive>>();
        for (int i = 0; i < recipientMotiveTemplates.Length; i++)
        {
            remainingMotives.Add(new List<Motive>(recipientMotiveTemplates[i]));
        }

        List<Motive>[] recipientMotives = new List<Motive>[5];

        //Set up killer motives
        int index = Random.Range(0, 3);
        recipientMotives[(int)killer] = remainingMotives[index];
        remainingMotives.RemoveAt(index);

        //Set up everyone else's motives
        for (int i = 0; i < recipientMotives.Length; i++)
        {
            if ((int)killer == i)
            {
                continue;
            }
            index = Random.Range(0, remainingMotives.Count);
            recipientMotives[i] = remainingMotives[index];
            remainingMotives.RemoveAt(index);
        }

        //Assign entries
        List<Motive[]> assignedMotives = new List<Motive[]>();
        for (int i = 0; i < 5; i++)
        {
            Motive[] currentSuspectResponses = new Motive[5];
            for (int j = 0; j < 5; j++)
            {
                List<Motive> characterMotive = recipientMotives[j];
                index = Random.Range(0, characterMotive.Count);
                currentSuspectResponses[j] = characterMotive[index];
                characterMotive.RemoveAt(index);
            }
            assignedMotives.Add(currentSuspectResponses);
        }
        motives = new MotiveList(assignedMotives);
    }

    void AssignOpportunities()
    {
        Opportunity[][] recipientOpportunityTemplates = new Opportunity[][] {
                                      new Opportunity[4] { Opportunity.GUILTY, Opportunity.GUILTY, Opportunity.UNKNOWN, Opportunity.UNKNOWN },
                                      new Opportunity[4] { Opportunity.GUILTY, Opportunity.GUILTY, Opportunity.UNKNOWN, Opportunity.UNKNOWN },
                                      new Opportunity[4] { Opportunity.GUILTY, Opportunity.GUILTY, Opportunity.UNKNOWN, Opportunity.UNKNOWN },
                                      new Opportunity[4] { Opportunity.UNKNOWN, Opportunity.UNKNOWN, Opportunity.INNOCENT, Opportunity.INNOCENT },
                                      new Opportunity[4] { Opportunity.UNKNOWN, Opportunity.UNKNOWN, Opportunity.UNKNOWN, Opportunity.INNOCENT }
                                    };
        List<List<Opportunity>> remainingOpportunitys = new List<List<Opportunity>>();
        for (int i = 0; i < recipientOpportunityTemplates.Length; i++)
        {
            remainingOpportunitys.Add(new List<Opportunity>(recipientOpportunityTemplates[i]));
        }

        List<Opportunity>[] recipientOpportunities = new List<Opportunity>[5];

        //Set up killer motives
        int index = Random.Range(0, 3);
        recipientOpportunities[(int)killer] = remainingOpportunitys[index];
        remainingOpportunitys.RemoveAt(index);

        //Set up everyone else's motives
        for (int i = 0; i < recipientOpportunities.Length; i++)
        {
            if ((int)killer == i)
            {
                continue;
            }
            index = Random.Range(0, remainingOpportunitys.Count);
            recipientOpportunities[i] = remainingOpportunitys[index];
            remainingOpportunitys.RemoveAt(index);
        }

        //Assign entries
        List<Opportunity[]> assignedOpportunities = new List<Opportunity[]>(); 
        for (int i = 0; i < 5; i++)
        {
            Opportunity[] currentSuspectResponses = new Opportunity[5];
            for (int j = 0; j < 5; j++)
            {
                if (i == j)
                {
                    currentSuspectResponses[j] = Opportunity.INNOCENT;
                }
                else
                {
                    List<Opportunity> characterOpportunity = recipientOpportunities[j];
                    index = Random.Range(0, characterOpportunity.Count);
                    currentSuspectResponses[j] = characterOpportunity[index];
                    characterOpportunity.RemoveAt(index);
                }
            }
            assignedOpportunities.Add(currentSuspectResponses);
        }
        opportunities = new OpportunityList(assignedOpportunities);
    }

    public void AssignMeans()
    {
        Means[] recipientMeansTemplate = { Means.YES, Means.YES, Means.YES, Means.YES, Means.NO };
        List<Means> remainingMeans = new List<Means>(recipientMeansTemplate);

        //Killer means
        int index = Random.Range(0, 4);
        weapon[(int)killer] = remainingMeans[index];
        remainingMeans.RemoveAt(index);

        //Set up everyone else's means
        for (int i = 0; i < weapon.Length; i++)
        {
            if ((int)killer == i)
            {
                continue;
            }
            index = Random.Range(0, remainingMeans.Count);
            weapon[i] = remainingMeans[index];
            remainingMeans.RemoveAt(index);
        }
    }

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
            Debug.Log("Failed to load from JSON: " + Path.Combine("Scenarios", filename));
            return new Scenario();
        }
    }

    public static void ExportJSON(Scenario scenario)
    {
        try
        {
            string json = JsonUtility.ToJson(scenario);
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
        System.DateTime now = System.DateTime.Now;
        return (string)(now.Year + "-" + now.Month.ToString("00") + "-" + now.Day.ToString("00") + " " + now.Hour.ToString("00") + "h" + now.Minute.ToString("00") + "m" + now.Second.ToString("00") + "s");
    }


}

[System.Serializable]
public struct MotiveList
{
    public Motive[] black;
    public Motive[] blue;
    public Motive[] green;
    public Motive[] red;
    public Motive[] yellow;

    public MotiveList(List<Motive[]> motives)
    {
        black = motives[0];
        blue = motives[1];
        green = motives[2];
        red = motives[3];
        yellow = motives[4];
    }
}

[System.Serializable]
public struct OpportunityList
{
    public Opportunity[] black;
    public Opportunity[] blue;
    public Opportunity[] green;
    public Opportunity[] red;
    public Opportunity[] yellow;

    public OpportunityList(List<Opportunity[]> opportunities)
    {
        black = opportunities[0];
        blue = opportunities[1];
        green = opportunities[2];
        red = opportunities[3];
        yellow = opportunities[4];
    }
}