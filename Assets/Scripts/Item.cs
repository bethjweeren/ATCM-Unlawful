using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Item : MonoBehaviour, IInteractable {

	public GameObject linkedItem;
	public ItemsManager itemsManager;
	public PlayerController player;
	public PickupText pickUpText;

	public void Interact(){
		pickUpText.ChangeText (gameObject.name);
		Debug.Log ("hititem");
		itemsManager.AddItem (linkedItem);
		Destroy (gameObject);
	}
}
