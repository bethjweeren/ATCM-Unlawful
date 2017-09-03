using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ButtonBack : MonoBehaviour
{
	public Button buttonBack;
	public GameObject menu;
	public GameObject screenToDisable;

	void Start()
	{
		Button btn = buttonBack.GetComponent<Button>();
		btn.onClick.AddListener(TaskOnClick);
	}

	void TaskOnClick()
	{
		screenToDisable.SetActive(false);
		menu.SetActive(true);
	}
}
