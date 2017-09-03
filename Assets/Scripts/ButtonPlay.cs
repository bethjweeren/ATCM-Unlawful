using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ButtonPlay : MonoBehaviour
{
	public Button buttonPlay;
	public GameObject menu;
	public GameObject player;
	private Animator playerAnimator;
	private PlayerController playerController;

	void Start()
	{
		Button btn = buttonPlay.GetComponent<Button>();
		btn.onClick.AddListener(TaskOnClick);
		playerController = player.GetComponent<PlayerController>();
		playerController.enabled = false;
		playerAnimator = player.GetComponent<Animator>();
		playerAnimator.enabled = false;
	}

	void TaskOnClick()
	{
		menu.SetActive(false);
		playerController.enabled = true;
		playerAnimator.enabled = true;
	}
}