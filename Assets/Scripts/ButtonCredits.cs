using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ButtonCredits : MonoBehaviour
{
	public Button buttonCredits;
	public GameObject menu;
	public GameObject credits;

	void Start()
	{
		Button btn = buttonCredits.GetComponent<Button>();
		btn.onClick.AddListener(TaskOnClick);
	}

	void TaskOnClick()
	{
		menu.SetActive(false);
		credits.SetActive(true);
	}
}