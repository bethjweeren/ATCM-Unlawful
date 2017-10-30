using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyToMorgue : MonoBehaviour
{
	public Transform newLocation; //Where the morgue is
	private Time_Manager timeManager;
	private int dayToMoveBody = 2;
	private int hourToMoveBody = 6;
	private float timeBetweenChecks = 5f; //Check time every 5 seconds.

	// Use this for initialization
	void Start ()
	{
		timeManager = GameObject.FindObjectOfType<Time_Manager>();
		StartCoroutine(CheckTime());
	}

	IEnumerator CheckTime()
	{
		if ((timeManager.day == dayToMoveBody) && (timeManager.hour >= hourToMoveBody))
		{
			transform.position = newLocation.position;
		}
		else
		{
			yield return new WaitForSeconds(timeBetweenChecks);
			StartCoroutine(CheckTime());
		}
	}
}
