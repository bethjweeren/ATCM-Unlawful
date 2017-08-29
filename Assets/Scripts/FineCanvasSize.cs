using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FineCanvasSize : MonoBehaviour {

	public static RectTransform canvasSize;

	void Start()
	{
		canvasSize = GetComponent<RectTransform> ();
	}
		
}
