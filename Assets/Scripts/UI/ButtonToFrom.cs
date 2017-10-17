using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ButtonToFrom : MonoBehaviour
{
	public Button button;
	public GameObject screenTo;
	public GameObject screenFrom;

	void Start()
	{
		Button btn = button.GetComponent<Button>();
		btn.onClick.AddListener(TaskOnClick);
	}

	void TaskOnClick()
	{
		screenFrom.SetActive(false);
		screenTo.SetActive(true);
	}
}
