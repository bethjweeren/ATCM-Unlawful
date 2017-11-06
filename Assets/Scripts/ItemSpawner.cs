using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
	public List<GameObject> itemsToSpawn; //These objects NEED to have the Item script or else this script will break
	private Time_Manager timeManager;
	private float timeBetweenChecks = .1f;
	// Use this for initialization
	void Start()
	{
		timeManager = GameObject.FindObjectOfType<Time_Manager>();
		foreach (GameObject obj in itemsToSpawn) //Start off with everything deactivated
		{
			obj.SetActive(false);
		}
		StartCoroutine(CheckToSpawn());
	}

	//If an item should spawn, spawn it. (Er, I mean enable it)
	IEnumerator CheckToSpawn()
	{
		foreach (GameObject obj in itemsToSpawn)
		{
			Item i = obj.GetComponent<Item>();
			//If the object actually exists
			if (i != null)
			{
				//Check if it's the right time to spawn it
				if ((i.dayToSpawn <= timeManager.day) && (i.hourToSpawn <= timeManager.hour))
				{
					obj.SetActive(true); //Spawn the object (Well the object's already there; you're just enabling it.)
				}
			}
			else
				print("The item " + obj + " does not have the Item script, so it can't be spawned!");
		}

		//Wait the time between checks (for efficiency reasons)
		yield return new WaitForSeconds(timeBetweenChecks);
		StartCoroutine(CheckToSpawn());
	}
}