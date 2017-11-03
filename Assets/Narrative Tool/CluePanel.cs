using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CluePanel : MonoBehaviour {

    public InputField clueName;
    public Toggle[] owners;
    public Toggle[] subjects;
    public InputField content;
    public InputField beggarSummary;
    public InputField journalSummary;
    public InputField tags;

    public ClueEditor editor;

    public void SetData(ClueFile file)
    {
        clueName.text = file.name;

        bool[] ownerToggles = IDsToToggles(file.owners);
        for (int i = 0; i < owners.Length; i++)
        {
            if (ownerToggles[i])
            {
                owners[i].isOn = true;
            }
        }

        bool[] subjectToggles = IDsToToggles(file.subjects);
        for(int i = 0; i < subjects.Length; i++)
        {
            if (subjectToggles[i])
            {
                subjects[i].isOn = true;
            }
        }

        content.text = file.content;
        beggarSummary.text = file.beggarSummary;
        journalSummary.text = file.journalSummary;
        tags.text = ListToString(file.tags);
    }

    public ClueFile GetData()
    {
        return new ClueFile(clueName.text, TogglesToIDs(owners), TogglesToIDs(subjects), content.text, beggarSummary.text, journalSummary.text, StringToList(tags.text));
    }

    public void DeleteClue()
    {
        editor.RemoveClue(this.gameObject);
        Destroy(this.gameObject);
    }
     
    private List<Suspect> TogglesToIDs(Toggle[] toggles)
    {
        List<Suspect> ids = new List<Suspect>();
        if (toggles[0].isOn)
        {
            ids.Add(Suspect.BLACK);
        }
        if (toggles[1].isOn)
        {
            ids.Add(Suspect.BLUE);
        }
        if (toggles[2].isOn)
        {
            ids.Add(Suspect.GREEN);
        }
        if (toggles[3].isOn)
        {
            ids.Add(Suspect.RED);
        }
        if (toggles[4].isOn)
        {
            ids.Add(Suspect.YELLOW);
        }
        return ids;
    }

    private bool[] IDsToToggles(List<Suspect> ids)
    {
        bool[] toggles = new bool[5];
        if (ids != null)
        {
            if (ids.Contains(Suspect.BLACK))
            {
                toggles[0] = true;
            }
            if (ids.Contains(Suspect.BLUE))
            {
                toggles[1] = true;
            }
            if (ids.Contains(Suspect.GREEN))
            {
                toggles[2] = true;
            }
            if (ids.Contains(Suspect.RED))
            {
                toggles[3] = true;
            }
            if (ids.Contains(Suspect.YELLOW))
            {
                toggles[4] = true;
            }
        }
        return toggles;
    }

    private List<string> StringToList(string tagString)
    {
        List<string> tagList = new List<string>();
        if(tagString.Length > 0)
        {
            string remaining = tagString;
            int commaIndex = remaining.IndexOf(',');
            while (commaIndex >= 0)
            {
                tagList.Add(remaining.Substring(0, commaIndex).ToUpper());
                remaining = remaining.Substring(commaIndex + 1, remaining.Length - (commaIndex + 1));
                while(remaining[0] == ' ')
                {
                    remaining = remaining.Substring(1, remaining.Length - 1);
                }
                commaIndex = remaining.IndexOf(',');
            }
            tagList.Add(remaining.ToUpper());
        }
        return tagList;
    }

    private string ListToString(List<string> tagList)
    {
        string final = "";
        if(tagList != null)
        {
            foreach (string tag in tagList)
            {
                final += tag.ToLower() + ", ";
            }
        }
        if(final.Length > 0)
        {
            final = final.Substring(0, final.Length - 2);
        }
        return final;
    }
}
