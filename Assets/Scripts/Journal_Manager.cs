using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Journal_Manager : MonoBehaviour {

	public GameObject[] pages;
	public Transform offLocation;
	public Transform page1Location;
	public Transform page2Location;
	public int currentPageNumber;
	public GameObject currentPage;
	public GameObject nextPage;
	public GameObject mapPage1;
	public GameObject mapPage2;

	void Start()
	{
		if (currentPage == null) 
		{
			currentPageNumber = 0;
			ChangePage ();
		}

		currentPage.transform.position = page1Location.position;
		nextPage.transform.position = page2Location.position;
	}

	void Update(){
		if ((Input.GetKeyDown (KeyCode.D) || Input.GetKeyDown (KeyCode.RightArrow)) && currentPageNumber < pages.Length - 2) 
		{
			PutAwayCurrentPages ();
			currentPageNumber += 2;
			ChangePage ();
		}

		if ((Input.GetKeyDown (KeyCode.A) || Input.GetKeyDown (KeyCode.LeftArrow)) && currentPageNumber > 0) 
		{
			PutAwayCurrentPages ();
			currentPageNumber -= 2;
			ChangePage ();
		}

		if (Input.GetKeyDown (KeyCode.M)) {
			BringUpMap ();
		}
	}

	void ChangePage()
	{
		currentPage = pages [(currentPageNumber)];
		nextPage = pages [(currentPageNumber+1)];
		currentPage.transform.position = page1Location.position;
		nextPage.transform.position = page2Location.position;
	}

	void BringUpMap(){
		currentPageNumber = 6;
		ChangePage ();
	}

	void PutAwayCurrentPages(){
		currentPage.transform.position = offLocation.position;
		nextPage.transform.position = offLocation.position;
		mapPage1.transform.position = offLocation.position;
		mapPage2.transform.position = offLocation.position;
	}
}
