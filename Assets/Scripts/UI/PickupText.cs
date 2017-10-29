using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PickupText : MonoBehaviour {

	public Text pickUpText;
	//public float textTimerMax = 10f;
	//public float textTimer = 0f;
	private WaitForSeconds switchDuration = new WaitForSeconds(3f);

	void Start(){
		pickUpText.text = "";
	}

	public void ChangeText(string name){
		StartCoroutine (SwitchOut (name));
	}

	public void ChangeTextAlert()
	{
		StartCoroutine(CannotPickup());
	}

	public IEnumerator SwitchOut(string name){
		pickUpText.text = "Picked up the " + name;
		yield return switchDuration;
		pickUpText.text = "";
	}

	public IEnumerator CannotPickup()
	{
		pickUpText.text = "No space left in inventory";
		yield return switchDuration;
		pickUpText.text = "";
	}
}
