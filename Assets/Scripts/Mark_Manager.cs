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
	public Button createBlueButton;
	public Button createYellowButton;
	public Button createRedButton;
	public Button createBlackButton;
	public Button createGreenButton;
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

	void Start()
	{
		Button createBlue_Btn = createBlueButton.GetComponent<Button> ();
		createBlue_Btn.onClick.AddListener (StartPlacingBlue);

		Button createYellow_Btn = createYellowButton.GetComponent<Button> ();
		createYellow_Btn.onClick.AddListener (StartPlacingYellow);

		Button createRed_Btn = createRedButton.GetComponent<Button> ();
		createRed_Btn.onClick.AddListener (StartPlacingRed);

		Button createBlack_Btn = createBlackButton.GetComponent<Button> ();
		createBlack_Btn.onClick.AddListener (StartPlacingBlack);

		Button createGreen_Btn = createGreenButton.GetComponent<Button> ();
		createGreen_Btn.onClick.AddListener (StartPlacingGreen);

		StopPlacing ();
	}

	void Update()
	{
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
		StopPlacing ();
		isPlacing = true;
		isPlacingBlue = true;
		blueCursor.SetActive (true);
		Debug.Log ("placingBlue");
	}

	public void StartPlacingYellow()
	{
		StopPlacing ();
		isPlacing = true;
		isPlacingYellow = true;
		yellowCursor.SetActive (true);
		Debug.Log ("placingYellow");
	}

	public void StartPlacingRed()
	{
		StopPlacing ();
		isPlacing = true;
		isPlacingRed = true;
		redCursor.SetActive (true);
		Debug.Log ("placingRed");
	}

	public void StartPlacingBlack()
	{
		StopPlacing ();
		isPlacing = true;
		isPlacingBlack = true;
		blackCursor.SetActive (true);
		Debug.Log ("placingBack");
	}

	public void StartPlacingGreen()
	{
		StopPlacing ();
		isPlacing = true;
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
}
