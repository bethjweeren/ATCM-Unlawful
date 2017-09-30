using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bin : MonoBehaviour, IInteractable {

	public Possessions_Manager possessionsManager;
	public int moneyAmount = 0;

	void Start(){
		AssignRandomAmount ();
	}

	public void AssignRandomAmount(){
		moneyAmount = Random.Range (0, 5);
	}

	public void Interact(){
		possessionsManager.ChangeMoney (moneyAmount);
		moneyAmount = 0;
		DialogueSystem.Instance().player.EndInteraction();
	}
}
