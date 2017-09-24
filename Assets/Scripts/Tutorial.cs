using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour {
    public GameObject letterUI;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return))
        {
            Close();
        }
    }

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
