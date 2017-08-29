using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This is a temporary script just to convey information. This may be deleted later.
public class DisableSplash : MonoBehaviour
{
	public GameObject splash;
	// Use this for initialization
	void Start()
	{
		
	}

	// Update is called once per frame
	void Update()
	{
		if (Input.anyKeyDown)
			splash.SetActive(false);
	}
}
