using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This is a temporary script just to convey information. This may be deleted later.
public class DisableSplash : MonoBehaviour
{
	public GameObject splash;
	public GameObject player;
	private Animator playerAnimator;
	private PlayerController playerController;
	// Use this for initialization
	void Start()
	{
		playerController = player.GetComponent<PlayerController>();
		playerController.enabled = false;
		playerAnimator = player.GetComponent<Animator>();
		playerAnimator.enabled = false;
	}

	// Update is called once per frame
	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			splash.SetActive(false);
			playerController.enabled = true;
			playerAnimator.enabled = true;
		}
	}
}
