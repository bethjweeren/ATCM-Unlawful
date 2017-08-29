using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tile_Swapper : MonoBehaviour {

	public bool isCheck = false;
	public bool isX = false;
	private Button tile;
	public GameObject checkMark;
	public GameObject xMark;

	void Start () 
	{
		tile = GetComponent<Button> ();
		tile.onClick.AddListener (SwapTile);

		checkMark.SetActive (false);
		xMark.SetActive (false);
	}

	void SwapTile()
	{
		if (isCheck == false && isX == false) {
			isCheck = true;
			checkMark.SetActive (true);
		} else if (isCheck) {
			isCheck = false;
			isX = true;
			checkMark.SetActive (false);
			xMark.SetActive (true);
		} else {
			isX = false;
			checkMark.SetActive (false);
			xMark.SetActive (false);
		}
	}
		
}
