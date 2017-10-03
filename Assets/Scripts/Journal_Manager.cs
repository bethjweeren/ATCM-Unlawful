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
	public Transform blueNotes1, blueNotes2, blueNotes3, blueNotes4, blueNotes5, blueNotes6, blueNotes7, blueNotes8;
	public Transform blackNotes1, blackNotes2, blackNotes3, blackNotes4, blackNotes5, blackNotes6, blackNotes7, blackNotes8;
	public Transform yellowNotes1, yellowNotes2, yellowNotes3, yellowNotes4, yellowNotes5, yellowNotes6, yellowNotes7, yellowNotes8;
	public Transform redNotes1, redNotes2, redNotes3, redNotes4, redNotes5, redNotes6, redNotes7, redNotes8;
	public Transform greenNotes1, greenNotes2, greenNotes3, greenNotes4, greenNotes5, greenNotes6, greenNotes7, greenNotes8;
	public Transform brownNotes1, brownNotes2, brownNotes3, brownNotes4, brownNotes5, brownNotes6, brownNotes7, brownNotes8;
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
                switch (currentBlackNotes)
                {
                    case 8:
                        return;
                    case 7:
                        entryLocation = blackNotes8;
                        break;
                    case 6:
                        entryLocation = blackNotes7;
                        break;
                    case 5:
                        entryLocation = blackNotes6;
                        break;
                    case 4:
                        entryLocation = blackNotes5;
                        break;
                    case 3:
                        entryLocation = blackNotes4;
                        break;
                    case 2:
                        entryLocation = blackNotes3;
                        break;
                    case 1:
                        entryLocation = blackNotes2;
                        break;
                    case 0:
                    default:
                        entryLocation = blackNotes1;
                        break;
                }
                currentBlackNotes++;
                break;
            case CharacterID.BLUE:
                characterPage = blueEntryPage;
                switch (currentBlueNotes)
                {
                    case 8:
                        return;
                    case 7:
                        entryLocation = blueNotes8;
                        break;
                    case 6:
                        entryLocation = blueNotes7;
                        break;
                    case 5:
                        entryLocation = blueNotes6;
                        break;
                    case 4:
                        entryLocation = blueNotes5;
                        break;
                    case 3:
                        entryLocation = blueNotes4;
                        break;
                    case 2:
                        entryLocation = blueNotes3;
                        break;
                    case 1:
                        entryLocation = blueNotes2;
                        break;
                    case 0:
                    default:
                        entryLocation = blueNotes1;
                        break;
                }
                currentBlueNotes++;
                break;
            case CharacterID.GREEN:
                characterPage = greenEntryPage;
                switch (currentGreenNotes)
                {
                    case 8:
                        return;
                    case 7:
                        entryLocation = greenNotes8;
                        break;
                    case 6:
                        entryLocation = greenNotes7;
                        break;
                    case 5:
                        entryLocation = greenNotes6;
                        break;
                    case 4:
                        entryLocation = greenNotes5;
                        break;
                    case 3:
                        entryLocation = greenNotes4;
                        break;
                    case 2:
                        entryLocation = greenNotes3;
                        break;
                    case 1:
                        entryLocation = greenNotes2;
                        break;
                    case 0:
                    default:
                        entryLocation = greenNotes1;
                        break;
                }
                currentGreenNotes++;
                break;
            case CharacterID.RED:
                characterPage = redEntryPage;
                switch (currentRedNotes)
                {
                    case 8:
                        return;
                    case 7:
                        entryLocation = redNotes8;
                        break;
                    case 6:
                        entryLocation = redNotes7;
                        break;
                    case 5:
                        entryLocation = redNotes6;
                        break;
                    case 4:
                        entryLocation = redNotes5;
                        break;
                    case 3:
                        entryLocation = redNotes4;
                        break;
                    case 2:
                        entryLocation = redNotes3;
                        break;
                    case 1:
                        entryLocation = redNotes2;
                        break;
                    case 0:
                    default:
                        entryLocation = redNotes1;
                        break;
                }
                currentRedNotes++;
                break;
			case CharacterID.BROWN:
				characterPage = brownEntryPage;
				switch (currentBrownNotes)
				{
				case 8:
					return;
				case 7:
					entryLocation = brownNotes8;
					break;
				case 6:
					entryLocation = brownNotes7;
					break;
				case 5:
					entryLocation = brownNotes6;
					break;
				case 4:
					entryLocation = brownNotes5;
					break;
				case 3:
					entryLocation = brownNotes4;
					break;
				case 2:
					entryLocation = brownNotes3;
					break;
				case 1:
					entryLocation = brownNotes2;
					break;
				case 0:
				default:
					entryLocation = brownNotes1;
					break;
				}
				currentRedNotes++;
				break;
            case CharacterID.YELLOW:
                characterPage = yellowEntryPage;
                switch (currentYellowNotes)
                {
                    case 8:
                        return;
                    case 7:
                        entryLocation = yellowNotes8;
                        break;
                    case 6:
                        entryLocation = yellowNotes7;
                        break;
                    case 5:
                        entryLocation = yellowNotes6;
                        break;
                    case 4:
                        entryLocation = yellowNotes5;
                        break;
                    case 3:
                        entryLocation = yellowNotes4;
                        break;
                    case 2:
                        entryLocation = yellowNotes3;
                        break;
                    case 1:
                        entryLocation = yellowNotes2;
                        break;
                    case 0:
                    default:
                        entryLocation = yellowNotes1;
                        break;
                }
                currentYellowNotes++;
                break;
            default:
                return;
        }
        
		localEntry = Instantiate (autoEntryPrefab, entryLocation.position, entryLocation.rotation, characterPage.transform);
		autoJournalEntry = localEntry.GetComponent<AutoJournalEntry> ();
		autoJournalEntry.entryText.text = entryString;
		journalEntry.Add (entryString);
	}
}
