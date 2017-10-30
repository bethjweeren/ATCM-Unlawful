using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClueEditor : MonoBehaviour {

    public InputField fileName;
    public GameObject contentPanel;

    List<GameObject> clues;
    public GameObject clueFab;
    float panelHeight = 320;

	// Use this for initialization
	void Start () {
        clues = new List<GameObject>();
	}

    public void RemoveClue(GameObject clue)
    {
        if (clues.Contains(clue))
        {
            int index = clues.IndexOf(clue);
            clues.Remove(clue);
            while(index < clues.Count)
            {
                RectTransform clueTF = clues[index].GetComponent<RectTransform>();
                clueTF.anchoredPosition = new Vector2(clueTF.anchoredPosition.x, clueTF.anchoredPosition.y + panelHeight);
                index++;
            }
            UpdateContentHeight();
        }
    }

    public void AddEntry()
    {
        AddClue(new ClueFile());
    }

    public void AddClue(ClueFile data)
    {
        GameObject currentClue = Instantiate(clueFab, contentPanel.transform);
        currentClue.GetComponent<RectTransform>().anchoredPosition = new Vector2(4, -panelHeight * clues.Count - 4);
        clues.Add(currentClue);
        CluePanel clueScript = currentClue.GetComponent<CluePanel>();
        clueScript.editor = this;
        clueScript.SetData(data);
        UpdateContentHeight();
        
    }

    private void UpdateContentHeight()
    {
        contentPanel.GetComponent<RectTransform>().sizeDelta = new Vector2(0, panelHeight * clues.Count);
    }

    public void Save()
    {
        if(fileName.text != "")
        {
            List<ClueFile> allClues = new List<ClueFile>();
            foreach(GameObject clue in clues)
            {
                allClues.Add(clue.GetComponent<CluePanel>().GetData());
            }

            ClueList.ExportJSON(new ClueList(allClues), fileName.text + ".json");
        }
    }

    public void Load()
    {
        Clear();
        if (fileName.text != "")
        {
            List<ClueFile> allClues = ClueList.LoadJSON(fileName.text + ".json").clues;
            foreach(ClueFile clue in allClues)
            {
                AddClue(clue);
            }
        }
    }

    void Clear()
    {
        int numberOfClues = clues.Count;
        for (int i = 0; i < numberOfClues; i++)
        {
            Debug.Log("Cleared");
            RemoveClue(clues[0]);
        }
        UpdateContentHeight();
    }

}
