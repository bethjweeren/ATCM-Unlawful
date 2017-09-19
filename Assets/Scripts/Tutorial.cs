using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour {
    public GameObject letterUI;

    private void OnTriggerEnter2D(Collider2D other)
    {
        other.GetComponent<PlayerController>().currentState = PlayerController.State.INTERACTING;
        other.GetComponent<PlayerController>().StopMoving();
        letterUI.SetActive(true);
        Destroy(this.gameObject);
    }

    public void Close()
    {
        DialogueSystem.Instance().player.currentState = PlayerController.State.MAIN;
        this.gameObject.SetActive(false);
    }
}
