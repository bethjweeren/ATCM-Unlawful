using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Grid_Manager : MonoBehaviour {

	GameObject[] blocks;
	public Button clearAllButton;

	void Start()
	{
		clearAllButton.onClick.AddListener (ClearAll);
	}

	void ClearAll()
	{
		blocks = GameObject.FindGameObjectsWithTag ("MarkerBlock");

		foreach (GameObject block in blocks) {
			block.GetComponent<Tile_Swapper> ().MakeBlank ();
		}
	}
}
