using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bed : MonoBehaviour, IInteractable {

	public GameObject interactText;
    public GameObject playerHouseArea;
    private Time_Manager time;
    bool inBed;

	void Start()
	{
		time = FindObjectOfType<Time_Manager>();
	}

	void OnTriggerEnter2D(Collider2D other) {
        Debug.Log(time == null);
        if(time == null)
        {
            time = Provider.GetInstance().timeManager;
        }
		if (other.tag == "Player" && time.currentTimeState == Time_Manager.State.Rest)
		{
			interactText.SetActive(true);
			inBed = true;
		}
	}
	
	void OnTriggerExit2D(Collider2D other) {
		if(other.tag == "Player"){
			interactText.SetActive(false);
            inBed = false;
		}
	}

    public void Interact()
    {
        if (inBed)
        {
			Provider.GetInstance().timeManager.SkipRestPeriod();
			interactText.SetActive(false);
			inBed = false;
		}
		else
			Provider.GetInstance().player.EndInteraction();
	}

    public void TeleportHome()
    {
        StartCoroutine("DoTeleport");
    }

    IEnumerator DoTeleport()
    {
        Vector3 originalScale = playerHouseArea.transform.localScale;
        playerHouseArea.transform.localScale = new Vector3(1000, 1000, 10);
        for (float f = 0f; f <= 1f; f += 0.07f)
        {
            yield return null;
        }
        Provider.GetInstance().player.transform.position = playerHouseArea.transform.position;
        playerHouseArea.transform.localScale = originalScale;
    }
}
