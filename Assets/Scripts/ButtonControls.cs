using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ButtonControls : MonoBehaviour
{
	public Button buttonControls;
	public GameObject menu;
	public GameObject controls;

	void Start()
	{
		Button btn = buttonControls.GetComponent<Button>();
		btn.onClick.AddListener(TaskOnClick);
	}

	void TaskOnClick()
	{
		menu.SetActive(false);
		controls.SetActive(true);
	}
}