using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tile_Swapper : MonoBehaviour {

	public bool isCheck = false;
	public bool isX = false;
	public bool isQuestion1 = false;
	public bool isQuestion2 = false;
	public bool isQuestion3 = false;
	private Button tile;
	public GameObject clickMe;
	public GameObject checkMark;
	public GameObject xMark;
	public GameObject question1;
	public GameObject question2;
	public GameObject question3;

	void Start () 
	{
		tile = GetComponent<Button> ();
		tile.onClick.AddListener (SwapTile);

		clickMe.SetActive (true);
		checkMark.SetActive (false);
		xMark.SetActive (false);
		question1.SetActive (false);
		question2.SetActive (false);
		question3.SetActive (false);
	}

	void SwapTile()
	{
		if (isCheck == false && isX == false && isQuestion1 == false && isQuestion2 == false && isQuestion3 == false) {
			clickMe.SetActive (false);
			isCheck = true;
			checkMark.SetActive (true);
		} else if (isCheck) {
			MakeBlank ();
			isX = true;
			xMark.SetActive (true);
		} else if (isX) {
			MakeBlank ();
			isQuestion1 = true;
			question1.SetActive (true);
		} else if (isQuestion1) {
			MakeBlank ();
			isQuestion2 = true;
			question2.SetActive (true);
		} else if (isQuestion2) {
			MakeBlank ();
			isQuestion3 = true;
			question3.SetActive (true);
		} else {
			MakeBlank ();
		}
	}

	public void MakeBlank(){
		isCheck = false;
		isX = false;
		isQuestion1 = false;
		isQuestion2 = false;
		isQuestion3 = false;
		checkMark.SetActive (false);
		xMark.SetActive (false);
		question1.SetActive (false);
		question2.SetActive (false);
		question3.SetActive (false);
	}
		
}
