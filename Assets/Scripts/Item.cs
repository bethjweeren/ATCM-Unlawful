using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour, IInteractable {

	public GameObject linkedItem;
	public ItemsManager itemsManager;
	public PlayerController player;

	public void Interact(){
		player.SayHello ();
		Debug.Log ("hititem");
		itemsManager.AddItem (linkedItem);
		Destroy (gameObject);
	}
}
