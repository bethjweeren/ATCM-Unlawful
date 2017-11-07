using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOver : MonoBehaviour {

    public Text headerText;
    public Text reasonText;
    public Color winColor;
    public Color loseColor;
	public AudioSource winSound;
	public AudioSource loseSound;
    public GameObject clock;
    public GameObject journal;
    public GameObject inventory;
    public GameObject sleepText;

	public void LoseGame(string reason)
    {
        clock.SetActive(false);
        journal.SetActive(false);
        inventory.SetActive(false);
        if (loseSound != null)
			loseSound.Play();
		headerText.text = "Game Over";
        headerText.color = loseColor;
        reasonText.text = reason;
        gameObject.SetActive(true);
    }

    public void WinGame(string reason)
    {
        clock.SetActive(false);
        journal.SetActive(false);
        inventory.SetActive(false);
        if (winSound != null)
			winSound.Play();
		headerText.text = "Congratulations";
        headerText.color = winColor;
        reasonText.text = reason;
        gameObject.SetActive(true);
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        DialogueSystem.Instance().Reset();
    }
}
