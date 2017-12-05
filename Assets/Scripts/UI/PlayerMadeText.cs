using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMadeText : MonoBehaviour {

	public Transform textLocation;
	public string stringToEdit = "Personal Journal";
	private string oldString;
	public GameObject textArea;
	public Camera realCamera;
	public AudioClip scribbleSound;
	private AudioSource audioSource;

	void Start()
	{
		audioSource = GetComponentInParent<AudioSource>();
		oldString = stringToEdit;
	}

	void OnGUI()
	{
		GUIStyle myStyle = new GUIStyle (GUI.skin.GetStyle("label"));
		GUI.contentColor = Color.gray;
		GUI.skin.label.fontSize = 20;
		if (Input.GetMouseButton(0))
		{
			RectTransform rectTransform = textArea.GetComponent<RectTransform>();
			if (RectTransformUtility.RectangleContainsScreenPoint (rectTransform, Input.mousePosition, realCamera)) {
				GUI.contentColor = Color.black;
				GUI.skin.settings.cursorColor = Color.black;
				if (this.stringToEdit == "Personal Journal") {
					this.stringToEdit = "";
					oldString = "";
				}
			}
		}
		stringToEdit = GUI.TextArea(new Rect(textLocation.position.x , textLocation.position.y , FineCanvasSize.canvasSize.rect.width/2, (3*FineCanvasSize.canvasSize.rect.height/4)), stringToEdit, 1000, myStyle);
		if (oldString.CompareTo(stringToEdit) != 0)
		{
			audioSource.clip = scribbleSound;
			audioSource.Play();
		}
		oldString = stringToEdit;
	}
}
