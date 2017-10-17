using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ButtonResume : MonoBehaviour
{
	public Button buttonResume;
	public GameObject menu;
	public GameObject player;
	public Time_Manager time_manager;
	private PlayerController playerController;

	void Start()
	{
		Button btn = buttonResume.GetComponent<Button>();
		btn.onClick.AddListener(TaskOnClick);
		playerController = player.GetComponent<PlayerController>();
	}

	public void TaskOnClick()
	{
		playerController.SwitchToMainState();
		time_manager.LeavePauseState();
		menu.SetActive(false);
	}
}