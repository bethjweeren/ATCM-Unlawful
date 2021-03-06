﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ButtonStart : MonoBehaviour
{
	public Button buttonStart;
	public GameObject menu;
	public Time_Manager time_manager;
	private Animator playerAnimator;
	private PlayerController playerController;
	private GameObject[] npcs;

	void Start()
	{
		Button btn = buttonStart.GetComponent<Button>();
		btn.onClick.AddListener(TaskOnClick);
		GameObject player = GameObject.FindGameObjectWithTag("Player");
		playerController = player.GetComponent<PlayerController>();
		playerController.enabled = false;
		playerAnimator = player.GetComponent<Animator>();
		playerAnimator.enabled = false;
		npcs = GameObject.FindGameObjectsWithTag("NPC");
		foreach(GameObject npc in npcs)
		{
			npc.GetComponent<NPC>().enabled = false;
		}
		time_manager.SwitchToPausedState(); //Needs to happen AFTER NPCs are enabled, because LeavePauseState calls FreezeNPCs()W;
		time_manager.enabled = false;
	}

	void TaskOnClick()
	{
		playerController.enabled = true;
		playerAnimator.enabled = true;
		foreach (GameObject npc in npcs)
		{
			npc.GetComponent<NPC>().enabled = true; ;
		}
		time_manager.LeavePauseState(); //Needs to happen AFTER NPCs are enabled, because LeavePauseState calls UnfreezeNPCs();
		time_manager.enabled = true;
		menu.SetActive(false);
	}
}