﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Clue_Manager : MonoBehaviour {

	public static bool isOpen = false;
	public GameObject[] pages;
	public Transform offLocation;
	public Transform page1Location;
	public Transform page2Location;
	public int currentPageNumber;
	public int currentNumberOfPages;
	public GameObject currentPage;
	public GameObject nextPage;
	public GameObject backButton;
	public GameObject nextButton;
	public GameObject autoEntryPrefab;
	private GameObject localEntry;
	private EntryButton autoJournalEntry;
	private GameObject characterPage;
	public GameObject blueEntryPage;
	public GameObject blackEntryPage;
	public GameObject yellowEntryPage;
	public GameObject redEntryPage;
	public GameObject greenEntryPage;
    public GameObject otherEntryPage;
    public int currentBlueNotes;
	public int currentBlackNotes;
	public int currentYellowNotes;
	public int currentRedNotes;
	public int currentGreenNotes;
    public int currentOtherNotes;
    public Transform[] blueNotes;
	public Transform[] blackNotes;
	public Transform[] yellowNotes;
	public Transform[] redNotes;
	public Transform[] greenNotes;
    public Transform[] otherNotes;
    public Transform entryLocation;
	public List<string> journalEntry = new List<string>();

    public List<string> knownBlackClues = new List<string>();
    public List<string> knownBlueClues = new List<string>();
    public List<string> knownGreenClues = new List<string>();
    public List<string> knownRedClues = new List<string>();
    public List<string> knownYellowClues = new List<string>();
    public List<string> knownOtherClues = new List<string>();

    public GameObject blackStarterEntry;
    public GameObject blueStarterEntry;
    public GameObject greenStarterEntry;
    public GameObject redStarterEntry;
    public GameObject yellowStarterEntry;
    public GameObject otherStarterEntry;

	void Start()
	{

		currentPage.transform.position = page1Location.position;
		nextPage.transform.position = page2Location.position;

		currentNumberOfPages = 4;

        /*
		Button next_Btn = nextButton.GetComponent<Button> ();
		next_Btn.onClick.AddListener (GoNextPage);

		Button back_Btn = backButton.GetComponent<Button> ();
		back_Btn.onClick.AddListener (GoBackPage);*/

		backButton.SetActive (false);
		nextButton.SetActive (true);
	}

	void Update(){
		if ((Input.GetKeyDown (KeyCode.D) || Input.GetKeyDown (KeyCode.RightArrow)) && currentPageNumber < currentNumberOfPages - 1) 
		{
			GoNextPage ();
		}

		if ((Input.GetKeyDown (KeyCode.A) || Input.GetKeyDown (KeyCode.LeftArrow)) && currentPageNumber > 0) 
		{
			GoBackPage ();
		}

	}

	public void ChangePage()
	{
		currentPage = pages [(currentPageNumber)];
		nextPage = pages [(currentPageNumber+1)];
		currentPage.transform.position = page1Location.position;
		nextPage.transform.position = page2Location.position;

		UpdateJournalButtons ();
	}

	void PutAwayCurrentPages(){
		currentPage.transform.position = offLocation.position;
		nextPage.transform.position = offLocation.position;
	}

	public void GoNextPage(){
		if (currentPageNumber < currentNumberOfPages - 1) {
			PutAwayCurrentPages ();
			currentPageNumber += 2;
			ChangePage ();
		}
	}

	public void GoBackPage(){
		if (currentPageNumber > 0) {
			PutAwayCurrentPages ();
			currentPageNumber -= 2;
			ChangePage ();
		}
	}

	void UpdateJournalButtons(){
		if (currentPageNumber == currentNumberOfPages) {
			nextButton.SetActive (false);
		} else {
			nextButton.SetActive (true);
		}

		if (currentPageNumber == 0) {
			backButton.SetActive (false);
		} else {
			backButton.SetActive (true);
		}
	}

    public void ChooseClue(string clueID)
    {
        gameObject.SetActive(false);
        DialogueSystem.Instance().ProcessClue(clueID);
    }

    public void Open()
    {
        gameObject.SetActive(true);
    }

    public void Close()
    {
        gameObject.SetActive(false);
        Provider.GetInstance().dialogueBox.gameObject.SetActive(true);
        DialogueSystem.Instance().NextLine();
    }
		
	public void CreateAutoJournalEntry(string entryString, CharacterID character, string clueID){
        switch (character)
        {
            case CharacterID.BLACK:
                characterPage = blackEntryPage;
                if(currentBlackNotes == 0)
                {
                    Destroy(blackStarterEntry);
                }
                if(currentBlackNotes < 12)
                {
                    entryLocation = blackNotes[currentBlackNotes];
                }
                else
                {
                    return;
                }
                currentBlackNotes++;
                break;
            case CharacterID.BLUE:
                characterPage = blueEntryPage;
                if (currentBlueNotes == 0)
                {
                    Destroy(blueStarterEntry);
                }
                if (currentBlueNotes < 12)
                {
                    entryLocation = blueNotes[currentBlueNotes];
                }
                else
                {
                    return;
                }
                currentBlueNotes++;
                break;
            case CharacterID.GREEN:
                characterPage = greenEntryPage;
                if (currentGreenNotes == 0)
                {
                    Destroy(greenStarterEntry);
                }
                if (currentGreenNotes < 12)
                {
                    entryLocation = greenNotes[currentGreenNotes];
                }
                else
                {
                    return;
                }
                currentGreenNotes++;
                break;
            case CharacterID.RED:
                characterPage = redEntryPage;
                if (currentRedNotes == 0)
                {
                    Destroy(redStarterEntry);
                }
                if (currentRedNotes < 12)
                {
                    entryLocation = redNotes[currentRedNotes];
                }
                else
                {
                    return;
                }
                currentRedNotes++;
                break;
            case CharacterID.YELLOW:
                characterPage = yellowEntryPage;
                if (currentYellowNotes == 0)
                {
                    Destroy(yellowStarterEntry);
                }
                if (currentYellowNotes < 12)
                {
                    entryLocation = yellowNotes[currentYellowNotes];
                }
                else
                {
                    return;
                }
                currentYellowNotes++;
                break;
            default:
                characterPage = otherEntryPage;
                if (currentOtherNotes == 0)
                {
                    Destroy(otherStarterEntry);
                }
                if (currentOtherNotes < 12)
                {
                    entryLocation = otherNotes[currentOtherNotes];
                }
                else
                {
                    return;
                }
                currentOtherNotes++;
                break;
        }
        
		localEntry = Instantiate (autoEntryPrefab, entryLocation.position, entryLocation.rotation, characterPage.transform);
		autoJournalEntry = localEntry.GetComponent<EntryButton> ();
		autoJournalEntry.entryText.text = entryString;
        autoJournalEntry.clueID = clueID;
        autoJournalEntry.manager = this;
		journalEntry.Add (entryString);
	}

    public bool ClueKnown(CharacterID page, string clueID)
    {
        if(clueID == "NOCLUE")
        {
            return true;
        }
        switch (page)
        {
            case CharacterID.BLACK:
                return knownBlackClues.Contains(clueID);
            case CharacterID.BLUE:
                return knownBlueClues.Contains(clueID);
            case CharacterID.GREEN:
                return knownGreenClues.Contains(clueID);
            case CharacterID.RED:
                return knownRedClues.Contains(clueID);
            case CharacterID.YELLOW:
                return knownYellowClues.Contains(clueID);
            default:
                return knownOtherClues.Contains(clueID);
        }
    }

    public void UpdateClueKnown(CharacterID page, string clueID)
    {
        switch (page)
        {
            case CharacterID.BLACK:
                if (!knownBlackClues.Contains(clueID))
                {
                    knownBlackClues.Add(clueID);
                }
                break;
            case CharacterID.BLUE:
                if (!knownBlueClues.Contains(clueID))
                {
                    knownBlueClues.Add(clueID);
                }
                break;
            case CharacterID.GREEN:
                if (!knownGreenClues.Contains(clueID))
                {
                    knownGreenClues.Add(clueID);
                }
                break;
            case CharacterID.RED:
                if (!knownRedClues.Contains(clueID))
                {
                    knownRedClues.Add(clueID);
                }
                break;
            case CharacterID.YELLOW:
                if (!knownYellowClues.Contains(clueID))
                {
                    knownYellowClues.Add(clueID);
                }
                break;
            default:
                if (!knownOtherClues.Contains(clueID))
                {
                    knownOtherClues.Add(clueID);
                }
                break;
        }
    }
}
