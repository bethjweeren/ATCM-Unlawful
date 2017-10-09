using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Journal_Manager : MonoBehaviour {

	public static bool isOpen = false;
	public GameObject[] pages;
	public Transform offLocation;
	public Transform page1Location;
	public Transform page2Location;
	public int currentPageNumber;
	public int currentNumberOfPages;
	public GameObject currentPage;
	public GameObject nextPage;
	public GameObject mapPage1;
	public GameObject mapPage2;
	public GameObject backButton;
	public GameObject nextButton;
	public Transform tabLeft1;
	public Transform tabLeft2;
	public Transform tabLeft3;
	public Transform tabRight1;
	public Transform tabRight2;
	public Transform tabRight3;
	public Button mapTab;
	public Button charactersTab;
	public Button notesTab;
	public Button addPages;
	public GameObject autoEntryPrefab;
	private GameObject localEntry;
	private AutoJournalEntry autoJournalEntry;
	private GameObject characterPage;
	public GameObject blueEntryPage;
	public GameObject blackEntryPage;
	public GameObject yellowEntryPage;
	public GameObject redEntryPage;
	public GameObject greenEntryPage;
	public GameObject brownEntryPage;
	public int currentBlueNotes;
	public int currentBlackNotes;
	public int currentYellowNotes;
	public int currentRedNotes;
	public int currentGreenNotes;
	public int currentBrownNotes;
	public Transform[] blueNotes;
	public Transform[] blackNotes;
	public Transform[] yellowNotes;
	public Transform[] redNotes;
	public Transform[] greenNotes;
	public Transform[] brownNotes;
	public Transform entryLocation;
	public List<string> journalEntry = new List<string>();

	void Start()
	{

		currentPage.transform.position = page1Location.position;
		nextPage.transform.position = page2Location.position;

		currentNumberOfPages = 14;

		Button next_Btn = nextButton.GetComponent<Button> ();
		next_Btn.onClick.AddListener (GoNextPage);

		Button back_Btn = backButton.GetComponent<Button> ();
		back_Btn.onClick.AddListener (GoBackPage);

		mapTab.onClick.AddListener (BringUpMap);
		charactersTab.onClick.AddListener (BringUpCharacters);
		notesTab.onClick.AddListener (BringUpNotes);
		addPages.onClick.AddListener (AddNotesPage);

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

		if (Input.GetKeyDown (KeyCode.M)) {
			BringUpMap ();
		}


	}

	public void ChangePage()
	{
		currentPage = pages [(currentPageNumber)];
		nextPage = pages [(currentPageNumber+1)];
		currentPage.transform.position = page1Location.position;
		nextPage.transform.position = page2Location.position;

		UpdateJournalButtons ();

		if (currentPageNumber >= 0 && currentPageNumber <= 8) {
			charactersTab.transform.position = tabLeft1.position;
			mapTab.transform.position = tabRight2.position;
			notesTab.transform.position = tabRight1.position;
		} else if (currentPageNumber >= 10 && currentPageNumber <= 12) {
			charactersTab.transform.position = tabLeft1.position;
			mapTab.transform.position = tabLeft2.position;
			notesTab.transform.position = tabRight1.position;
		} else {
			charactersTab.transform.position = tabLeft1.position;
			mapTab.transform.position = tabLeft2.position;
			notesTab.transform.position = tabLeft3.position;
		}
	}

	public void BringUpMap(){
		PutAwayCurrentPages ();
		currentPageNumber = 10;
		ChangePage ();
	}

	public void BringUpCharacters(){
		PutAwayCurrentPages ();
		currentPageNumber = 0;
		ChangePage ();
	}

	public void BringUpNotes(){
		PutAwayCurrentPages ();
		currentPageNumber = 14;
		ChangePage ();
	}

	void PutAwayCurrentPages(){
		currentPage.transform.position = offLocation.position;
		nextPage.transform.position = offLocation.position;
		mapPage1.transform.position = offLocation.position;
		mapPage2.transform.position = offLocation.position;
	}

	void GoNextPage(){
		if (currentPageNumber < currentNumberOfPages - 1) {
			PutAwayCurrentPages ();
			currentPageNumber += 2;
			ChangePage ();
		}
	}

	void GoBackPage(){
		if (currentPageNumber > 0) {
			PutAwayCurrentPages ();
			currentPageNumber -= 2;
			ChangePage ();
		}
	}

	void AddNotesPage()
	{
		if (currentNumberOfPages <= pages.Length - 4) {
			currentNumberOfPages += 2;
		}

		UpdateJournalButtons ();
			
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
		
	public void CreateAutoJournalEntry(string entryString, CharacterID character){
        switch (character)
        {
            case CharacterID.BLACK:
                characterPage = blackEntryPage;
                if (currentBlackNotes < 12)
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
                characterPage = brownEntryPage;
                if (currentBrownNotes < 12)
                {
                    entryLocation = brownNotes[currentBrownNotes];
                }
                else
                {
                    return;
                }
                currentBrownNotes++;
                break;
        }

        localEntry = Instantiate (autoEntryPrefab, entryLocation.position, entryLocation.rotation, characterPage.transform);
		autoJournalEntry = localEntry.GetComponent<AutoJournalEntry> ();
		autoJournalEntry.entryText.text = entryString;
		journalEntry.Add (entryString);
	}
}
