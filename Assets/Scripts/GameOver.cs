using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOver : MonoBehaviour {

	public Text reasonText;

	public void ReasonWhy(string reason){
		reasonText.text = reason;
	}

}
