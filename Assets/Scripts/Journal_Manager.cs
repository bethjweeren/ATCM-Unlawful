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
	public int currentBlueNotes;
	public int currentBlackNotes;
	public int currentYellowNotes;
	public int currentRedNotes;
	public int currentGreenNotes;
	public Transform blueNotes1, blueNotes2, blueNotes3, blueNotes4, blueNotes5, blueNotes6, blueNotes7, blueNotes8;
	public Transform blackNotes1, blackNotes2, blackNotes3, blackNotes4, blackNotes5, blackNotes6, blackNotes7, blackNotes8;
	public Transform yellowNotes1, yellowNotes2, yellowNotes3, yellowNotes4, yellowNotes5, yellowNotes6, yellowNotes7, yellowNotes8;
	public Transform redNotes1, redNotes2, redNotes3, redNotes4, redNotes5, redNotes6, redNotes7, redNotes8;
	public Transform greenNotes1, greenNotes2, greenNotes3, greenNotes4, greenNotes5, greenNotes6, greenNotes7, greenNotes8;
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
		} else if (currentPageNumber == 10) {
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
		currentPageNumber = 12;
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
		
	public void CreateAutoJournalEntry(string entryString, string character){
		if (character == "Blue") {
			characterPage = blueEntryPage;
			if (currentBlueNotes == 0) {
				entryLocation = blueNotes1;
				currentBlueNotes++;
			} else if (currentBlueNotes == 1) {
				entryLocation = blueNotes2;
				currentBlueNotes++;
			} else if (currentBlueNotes == 2) {
				entryLocation = blueNotes3;
				currentBlueNotes++;
			} else if (currentBlueNotes == 3) {
				entryLocation = blueNotes4;
				currentBlueNotes++;
			} else if (currentBlueNotes == 4) {
				entryLocation = blueNotes5;
				currentBlueNotes++;
			} else if (currentBlueNotes == 5) {
				entryLocation = blueNotes6;
				currentBlueNotes++;
			} else if (currentBlueNotes == 6) {
				entryLocation = blueNotes7;
				currentBlueNotes++;
			} else if (currentBlueNotes == 7) {
				entryLocation = blueNotes8;
				currentBlueNotes++;
			} else {
				entryLocation = blueNotes1;
				currentBlueNotes++;
			}
		} else if (character == "Black") {
			characterPage = blackEntryPage;
			if (currentBlackNotes == 0) {
				entryLocation = blackNotes1;
				currentBlackNotes++;
			} else if (currentBlackNotes == 1) {
				entryLocation = blackNotes2;
				currentBlackNotes++;
			} else if (currentBlackNotes == 2) {
				entryLocation = blackNotes3;
				currentBlackNotes++;
			} else if (currentBlackNotes == 3) {
				entryLocation = blackNotes4;
				currentBlackNotes++;
			} else if (currentBlackNotes == 4) {
				entryLocation = blackNotes5;
				currentBlackNotes++;
			} else if (currentBlackNotes == 5) {
				entryLocation = blackNotes6;
				currentBlackNotes++;
			} else if (currentBlackNotes == 6) {
				entryLocation = blackNotes7;
				currentBlackNotes++;
			} else if (currentBlackNotes == 7) {
				entryLocation = blackNotes8;
				currentBlackNotes++;
			} else {
				entryLocation = blackNotes1;
				currentBlackNotes++;
			}
		} else if (character == "Yellow") {
			characterPage = yellowEntryPage;
			if (currentYellowNotes == 0) {
				entryLocation = yellowNotes1;
				currentYellowNotes++;
			} else if (currentYellowNotes == 1) {
				entryLocation = yellowNotes2;
				currentYellowNotes++;
			} else if (currentYellowNotes == 2) {
				entryLocation = yellowNotes3;
				currentYellowNotes++;
			} else if (currentYellowNotes == 3) {
				entryLocation = yellowNotes4;
				currentYellowNotes++;
			} else if (currentYellowNotes == 4) {
				entryLocation = yellowNotes5;
				currentYellowNotes++;
			} else if (currentYellowNotes == 5) {
				entryLocation = yellowNotes6;
				currentYellowNotes++;
			} else if (currentYellowNotes == 6) {
				entryLocation = yellowNotes7;
				currentYellowNotes++;
			} else if (currentYellowNotes == 7) {
				entryLocation = yellowNotes8;
				currentYellowNotes++;
			} else {
				entryLocation = yellowNotes1;
				currentYellowNotes++;
			}
		} else if (character == "Green") {
			characterPage = greenEntryPage;
			if (currentGreenNotes == 0) {
				entryLocation = greenNotes1;
				currentGreenNotes++;
			} else if (currentGreenNotes == 1) {
				entryLocation = greenNotes2;
				currentGreenNotes++;
			} else if (currentGreenNotes == 2) {
				entryLocation = greenNotes3;
				currentGreenNotes++;
			} else if (currentGreenNotes == 3) {
				entryLocation = greenNotes4;
				currentGreenNotes++;
			} else if (currentGreenNotes == 4) {
				entryLocation = greenNotes5;
				currentGreenNotes++;
			} else if (currentGreenNotes == 5) {
				entryLocation = greenNotes6;
				currentGreenNotes++;
			} else if (currentGreenNotes == 6) {
				entryLocation = greenNotes7;
				currentGreenNotes++;
			} else if (currentGreenNotes == 7) {
				entryLocation = greenNotes8;
				currentGreenNotes++;
			} else {
				entryLocation = greenNotes1;
				currentGreenNotes++;
			}
		} else if (character == "Red") {
			characterPage = redEntryPage;
			if (currentRedNotes == 0) {
				entryLocation = redNotes1;
				currentRedNotes++;
			} else if (currentRedNotes == 1) {
				entryLocation = redNotes2;
				currentRedNotes++;
			} else if (currentRedNotes == 2) {
				entryLocation = redNotes3;
				currentRedNotes++;
			} else if (currentRedNotes == 3) {
				entryLocation = redNotes4;
				currentRedNotes++;
			} else if (currentRedNotes == 4) {
				entryLocation = redNotes5;
				currentRedNotes++;
			} else if (currentRedNotes == 5) {
				entryLocation = redNotes6;
				currentRedNotes++;
			} else if (currentRedNotes == 6) {
				entryLocation = redNotes7;
				currentRedNotes++;
			} else if (currentRedNotes == 7) {
				entryLocation = redNotes8;
				currentRedNotes++;
			} else {
				entryLocation = redNotes1;
				currentRedNotes++;
			}
		} else {
			characterPage = blueEntryPage;
			if (currentBlueNotes == 0) {
				entryLocation = blueNotes1;
				currentBlueNotes++;
			} else if (currentBlueNotes == 1) {
				entryLocation = blueNotes2;
				currentBlueNotes++;
			} else if (currentBlueNotes == 2) {
				entryLocation = blueNotes3;
				currentBlueNotes++;
			} else if (currentBlueNotes == 3) {
				entryLocation = blueNotes4;
				currentBlueNotes++;
			} else if (currentBlueNotes == 4) {
				entryLocation = blueNotes5;
				currentBlueNotes++;
			} else if (currentBlueNotes == 5) {
				entryLocation = blueNotes6;
				currentBlueNotes++;
			} else if (currentBlueNotes == 6) {
				entryLocation = blueNotes7;
				currentBlueNotes++;
			} else if (currentBlueNotes == 7) {
				entryLocation = blueNotes8;
				currentBlueNotes++;
			} else {
				entryLocation = blueNotes1;
				currentBlueNotes++;
			}
		}
		localEntry = Instantiate (autoEntryPrefab, entryLocation.position, entryLocation.rotation, characterPage.transform);
		autoJournalEntry = localEntry.GetComponent<AutoJournalEntry> ();
		autoJournalEntry.entryText.text = entryString;
		journalEntry.Add (entryString);
	}
}
