using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ItemUI_Image : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {

	public Image itemTextBG;
	public Text itemText;
	public string desc;
    public GameObject picture;
    public bool isDesc;

    void Start()
    {
        if (isDesc)
        {
            picture.SetActive(false);
        }
    }

	public void OnPointerEnter (PointerEventData eventData){
		itemTextBG.enabled = true;
		itemText.text = desc;
        if (isDesc)
        {
            picture.SetActive(true);
        }
	}

	public void OnPointerExit (PointerEventData eventData){
		itemTextBG.enabled = false;
		itemText.text = "";
        if (isDesc)
        {
            picture.SetActive(false);
        }
    }

}
