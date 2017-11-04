using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ItemUI_Image : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {

	public Text itemText;
	public string desc;

	public void OnPointerEnter (PointerEventData eventData){
		itemText.text = desc;
	}

	public void OnPointerExit (PointerEventData eventData){
		itemText.text = "";
	}

}
