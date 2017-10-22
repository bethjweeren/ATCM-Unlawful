using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LetterUI : MonoBehaviour{

    void Update()
    {
        if ((Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return)))
        {
            Close();
        }
	
    }

    public void Close()
    {
        Provider.GetInstance().player.EndInteraction();
        this.gameObject.SetActive(false);
        Provider.GetInstance().player.time_manager.LeavePauseState();
    }
}
