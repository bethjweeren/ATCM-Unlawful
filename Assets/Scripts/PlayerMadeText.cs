using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMadeText : MonoBehaviour {

	public Transform textLocation;
	private RectTransform rt;

	void Start()
	{
		rt = GetComponent<RectTransform> ();
	}

	public string stringToEdit = "Personal Journal";
	void OnGUI() {
		stringToEdit = GUI.TextArea(new Rect(textLocation.position.x , textLocation.position.y , FineCanvasSize.canvasSize.rect.width/4, (3*FineCanvasSize.canvasSize.rect.height/8)), stringToEdit, 200);
	}

}
