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
		foreach (GameObject obj in itemsToSpawn)
		{
			obj.SetActive(false);
		}
		StartCoroutine(CheckToSpawn());
	}

	IEnumerator CheckToSpawn()
	{
		foreach (GameObject obj in itemsToSpawn)
		{
			Item i = obj.GetComponent<Item>();
			if (i != null)
			{
				if ((i.dayToSpawn == timeManager.day) && (i.hourToSpawn == timeManager.hour))
				{
					obj.SetActive(true);
				}
			}
			else
				print("The item " + obj + " does not have the Item script, so it can't be spawned!");
		}

		//Wait a tenth of a second
		yield return new WaitForSeconds(timeBetweenChecks);
		StartCoroutine(CheckToSpawn());
	}
}