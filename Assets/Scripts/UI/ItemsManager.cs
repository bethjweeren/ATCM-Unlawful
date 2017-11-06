using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemsManager : MonoBehaviour {

	public Transform itemPosition1, itemPosition2, itemPosition3, itemPosition4, itemPosition5, itemPosition6;
	public List<GameObject> items = new List<GameObject>();
	public AudioClip backpackSound;
	public AudioSource audioSource;

	void Start()
	{
		audioSource.clip = backpackSound;
	}

	public void AddItem(GameObject itemToAdd){
		audioSource.clip = backpackSound;
		audioSource.Play();
		items.Add (itemToAdd);
		UpdateInventory ();
	}

	//Just for PlayerController when the player opens the inventory
	public void PlayBackpackSound()
	{
		audioSource.clip = backpackSound;
		audioSource.Play();
	}

	//void Update(){
	//	UpdateInventory ();
	//}


	public void UpdateInventory(){
		if (items.Count == 1) {
			items [0].transform.position = itemPosition1.position;
		} else if (items.Count == 2) {
			items [0].transform.position = itemPosition1.position;
			items [1].transform.position = itemPosition2.position;
		} else if (items.Count == 3) {
			items [0].transform.position = itemPosition1.position;
			items [1].transform.position = itemPosition2.position;
			items [2].transform.position = itemPosition3.position;
		} else if (items.Count == 4) {
			items [0].transform.position = itemPosition1.position;
			items [1].transform.position = itemPosition2.position;
			items [2].transform.position = itemPosition3.position;
			items [3].transform.position = itemPosition4.position;
		} else if (items.Count == 5) {
			items [0].transform.position = itemPosition1.position;
			items [1].transform.position = itemPosition2.position;
			items [2].transform.position = itemPosition3.position;
			items [3].transform.position = itemPosition4.position;
			items [4].transform.position = itemPosition5.position;
		} else if (items.Count == 6) {
			items [0].transform.position = itemPosition1.position;
			items [1].transform.position = itemPosition2.position;
			items [2].transform.position = itemPosition3.position;
			items [3].transform.position = itemPosition4.position;
			items [4].transform.position = itemPosition5.position;
			items [5].transform.position = itemPosition6.position;
		}
	}
}
