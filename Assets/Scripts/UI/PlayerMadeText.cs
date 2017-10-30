using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMadeText : MonoBehaviour {

	public Transform textLocation;
	public string stringToEdit = "Personal Journal";
	public GameObject textArea;
	public Camera realCamera;
		
	void OnGUI() {
		GUIStyle myStyle = new GUIStyle (GUI.skin.GetStyle("label"));
		GUI.contentColor = Color.gray;
		if (Input.GetMouseButton(0))
		{
			RectTransform rectTransform = textArea.GetComponent<RectTransform>();
			if (RectTransformUtility.RectangleContainsScreenPoint (rectTransform, Input.mousePosition, realCamera)) {
				GUI.contentColor = Color.black;
				GUI.skin.settings.cursorColor = Color.black;
				if (this.stringToEdit == "Personal Journal") {
					this.stringToEdit = "";
				}
			}
		}
		stringToEdit = GUI.TextArea(new Rect(textLocation.position.x , textLocation.position.y , FineCanvasSize.canvasSize.rect.width/4, (3*FineCanvasSize.canvasSize.rect.height/4)), stringToEdit, 1000, myStyle);
	}
}
