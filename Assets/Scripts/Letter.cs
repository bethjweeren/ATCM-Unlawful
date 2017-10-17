using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Letter : MonoBehaviour, IInteractable {
    public GameObject letterUI;

    public void Interact()
    {
        PlayerController player = Provider.GetInstance().player;
        player.currentState = PlayerController.State.INTERACTING;
        player.StopMoving();
        letterUI.SetActive(true);
        Destroy(this.gameObject);
    }
}
