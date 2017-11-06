using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PickupText : MonoBehaviour {

	public Text pickUpText;
	public GameObject pickUpTextBG;
	//public float textTimerMax = 10f;
	//public float textTimer = 0f;
	private WaitForSeconds switchDuration = new WaitForSeconds(3f);

	void Start(){
		pickUpText.text = "";
	}

	//Tells player that the item was picked up 
	public void ChangeText(string name){
		StartCoroutine (SwitchOut (name));
	}

	//Tells player that the item was picked up and to open the inventory
	public void ChangeTextInfo(string name)
	{
		StartCoroutine(SwitchOutInfo(name));
	}

	//Tells player that the inventory is full
	public void ChangeTextAlert()
	{
		StartCoroutine(CannotPickup());
	}

	public IEnumerator SwitchOut(string name){
		pickUpTextBG.SetActive(true);
		pickUpText.text = "Picked up " + name;
		yield return switchDuration;
		pickUpText.text = "";
		pickUpTextBG.SetActive(false);
	}

	public IEnumerator SwitchOutInfo(string name)
	{
		pickUpTextBG.SetActive(true);
		pickUpText.text = "Picked up " + name + "\nOpen inventory (I) to view.";
		yield return (new WaitForSeconds(6f));
		pickUpText.text = "";
		pickUpTextBG.SetActive(false);
	}


	public IEnumerator CannotPickup()
	{
		pickUpTextBG.SetActive(true);
		pickUpText.text = "No space left in inventory";
		yield return switchDuration;
		pickUpText.text = "";
		pickUpTextBG.SetActive(false);
	}
}
