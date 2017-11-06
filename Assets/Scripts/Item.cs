using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Item : MonoBehaviour, IInteractable
{

	public GameObject linkedItem;
	public ItemsManager itemsManager;
	public PlayerController player;
	public PickupText pickUpText;
	public int dayToSpawn;
	public int hourToSpawn;
	public GameObject itemSpawner;

	public void Interact()
	{
		if (itemsManager.items.Count == 0)
		{
			pickUpText.ChangeTextInfo(gameObject.name);
			Debug.Log("hititem");
			itemsManager.AddItem(linkedItem);
			Provider.GetInstance().player.EndInteraction();
			itemSpawner.GetComponent<ItemSpawner>().itemsToSpawn.Remove(gameObject);
			Destroy(gameObject);
		}
		else if (itemsManager.items.Count < 6)
		{
			pickUpText.ChangeText(gameObject.name);
			Debug.Log("hititem");
			itemsManager.AddItem(linkedItem);
			Provider.GetInstance().player.EndInteraction();
			itemSpawner.GetComponent<ItemSpawner>().itemsToSpawn.Remove(gameObject);
			Destroy(gameObject);
		}
		else
		{
			pickUpText.ChangeTextAlert();
			Provider.GetInstance().player.EndInteraction();
		}
	}
}
