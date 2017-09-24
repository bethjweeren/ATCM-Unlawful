using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ButtonStart : MonoBehaviour
{
	public Button buttonStart;
	public GameObject menu;
	public Time_Manager time_manager;
	private Animator playerAnimator;
	private PlayerController playerController;

	void Start()
	{
		Button btn = buttonStart.GetComponent<Button>();
		btn.onClick.AddListener(TaskOnClick);
		GameObject player = GameObject.FindGameObjectWithTag("Player");
		playerController = player.GetComponent<PlayerController>();
		playerController.enabled = false;
		playerAnimator = player.GetComponent<Animator>();
		playerAnimator.enabled = false;
		time_manager.SwitchToPausedState();
		time_manager.enabled = false;
	}

	void TaskOnClick()
	{
		playerController.enabled = true;
		playerAnimator.enabled = true;
		time_manager.LeavePauseState();
		time_manager.enabled = true;
		menu.SetActive(false);
	}
}