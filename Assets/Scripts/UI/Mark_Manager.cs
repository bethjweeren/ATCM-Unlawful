using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Mark_Manager : MonoBehaviour {

	public bool isPlacing = false;
	public bool isPlacingBlue = false;
	public bool isPlacingYellow = false;
	public bool isPlacingRed = false;
	public bool isPlacingBlack = false;
	public bool isPlacingGreen = false;
	public Button createBlue_Btn;
	public Button createYellow_Btn;
	public Button createRed_Btn;
	public Button createBlack_Btn;
	public Button createGreen_Btn;
	public Button clearAll_Btn;
	public GameObject blueCursor;
	public GameObject yellowCursor;
	public GameObject redCursor;
	public GameObject blackCursor;
	public GameObject greenCursor;
	public GameObject blueMark;
	public GameObject yellowMark;
	public GameObject redMark;
	public GameObject blackMark;
	public GameObject greenMark;
	public GameObject mapArea;
	public Camera realCamera;
	GameObject[] marks;

	void Start()
	{
		createBlue_Btn.onClick.AddListener (StartPlacingBlue);

		createYellow_Btn.onClick.AddListener (StartPlacingYellow);

		createRed_Btn.onClick.AddListener (StartPlacingRed);

		createBlack_Btn.onClick.AddListener (StartPlacingBlack);

		createGreen_Btn.onClick.AddListener (StartPlacingGreen);

		clearAll_Btn.onClick.AddListener (ClearAll);

		StopPlacing ();
	}

	void Update()
	{
		ClickedOffMap ();

		if (isPlacingBlue) 
		{
			PlaceBlue ();
			if (Input.GetButtonDown ("Fire2")) 
			{
				isPlacing = false;
				StopPlacing ();
			}
		} else if (isPlacingYellow) 
		{
			PlaceYellow ();
			if (Input.GetButtonDown ("Fire2")) 
			{
				isPlacing = false;
				StopPlacing ();
			}
		} else if (isPlacingRed) 
		{
			PlaceRed ();
			if (Input.GetButtonDown ("Fire2")) 
			{
				isPlacing = false;
				StopPlacing ();
			}
		} else if (isPlacingBlack) 
		{
			PlaceBlack ();
			if (Input.GetButtonDown ("Fire2")) 
			{
				isPlacing = false;
				StopPlacing ();
			}
		} else if (isPlacingGreen) 
		{
			PlaceGreen ();
			if (Input.GetButtonDown ("Fire2")) 
			{
				isPlacing = false;
				StopPlacing ();
			}
		}

		if (Input.GetKeyDown (KeyCode.Q) && isPlacing) 
		{
			isPlacing = false;
			StopPlacing ();
		}

	}

	public void StartPlacingBlue()
	{
		StartedPlacing ();
		isPlacingBlue = true;
		blueCursor.SetActive (true);
		Debug.Log ("placingBlue");
	}

	public void StartPlacingYellow()
	{
		StartedPlacing ();
		isPlacingYellow = true;
		yellowCursor.SetActive (true);
		Debug.Log ("placingYellow");
	}

	public void StartPlacingRed()
	{
		StartedPlacing ();
		isPlacingRed = true;
		redCursor.SetActive (true);
		Debug.Log ("placingRed");
	}

	public void StartPlacingBlack()
	{
		StartedPlacing ();
		isPlacingBlack = true;
		blackCursor.SetActive (true);
		Debug.Log ("placingBack");
	}

	public void StartPlacingGreen()
	{
		StartedPlacing ();
		isPlacingGreen = true;
		greenCursor.SetActive (true);
		Debug.Log ("placingGreen");
	}

	void PlaceBlue()
	{
		blueCursor.transform.position = Input.mousePosition;
		if (Input.GetButtonDown ("Fire1")) 
		{
			Instantiate (blueMark, Input.mousePosition, Quaternion.identity, gameObject.transform);
		}
	}

	void PlaceYellow()
	{
		yellowCursor.transform.position = Input.mousePosition;
		if (Input.GetButtonDown ("Fire1")) 
		{
			Instantiate (yellowMark, Input.mousePosition, Quaternion.identity, gameObject.transform);
		}
	}

	void PlaceRed()
	{
		redCursor.transform.position = Input.mousePosition;
		if (Input.GetButtonDown ("Fire1")) 
		{
			Instantiate (redMark, Input.mousePosition, Quaternion.identity, gameObject.transform);
		}
	}

	void PlaceBlack()
	{
		blackCursor.transform.position = Input.mousePosition;
		if (Input.GetButtonDown ("Fire1")) 
		{
			Instantiate (blackMark, Input.mousePosition, Quaternion.identity, gameObject.transform);
		}
	}

	void PlaceGreen()
	{
		greenCursor.transform.position = Input.mousePosition;
		if (Input.GetButtonDown ("Fire1")) 
		{
			Instantiate (greenMark, Input.mousePosition, Quaternion.identity, gameObject.transform);
		}
	}

	void ClearAll(){
		marks = GameObject.FindGameObjectsWithTag ("MapMark");
		foreach (GameObject mark in marks) {
			Destroy (mark);
		}
	}

	void StartedPlacing ()
	{
		StopPlacing ();
		isPlacing = true;
	}

	void StopPlacing ()
	{
		isPlacingBlue = false;
		isPlacingYellow = false;
		isPlacingRed = false;
		isPlacingBlack = false;
		isPlacingGreen = false;
		blueCursor.SetActive (false);
		yellowCursor.SetActive (false);
		redCursor.SetActive (false);
		blackCursor.SetActive (false);
		greenCursor.SetActive (false);
	}

	void ClickedOffMap()
	{
		if (Input.GetMouseButton(0))
		{
			RectTransform rectTransform = mapArea.GetComponent<RectTransform>();
			if (!RectTransformUtility.RectangleContainsScreenPoint (rectTransform, Input.mousePosition, realCamera)) {
				isPlacing = false;
				StopPlacing ();
			}
		}
	}
}
