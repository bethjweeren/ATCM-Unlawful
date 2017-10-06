using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour, IInteractable {
    public GameObject letterUI;

    void Update()
    {
        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return)) && letterUI.gameObject.activeInHierarchy)
        {
            Close();
        }
    }

    public void Interact()
    {
        PlayerController player = Provider.GetInstance().player;
        player.currentState = PlayerController.State.INTERACTING;
        player.StopMoving();
        letterUI.SetActive(true);
    }

    public void Close()
    {
        Provider.GetInstance().player.EndInteraction();
        letterUI.gameObject.SetActive(false);
        Destroy(this.gameObject);
    }
}
