using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Mark_Mover : MonoBehaviour, IPointerDownHandler {

	public void OnPointerDown (PointerEventData eventData)
	{
		if (eventData.button == PointerEventData.InputButton.Right) 
		{
			Destroy (gameObject);
		}
	}
}
