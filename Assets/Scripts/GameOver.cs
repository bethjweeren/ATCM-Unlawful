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

	public void LoseGame(string reason)
    {
		if (loseSound != null)
			loseSound.Play();
		headerText.text = "Game Over";
        headerText.color = loseColor;
        reasonText.text = reason;
        gameObject.SetActive(true);
    }

    public void WinGame(string reason)
    {
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
    }
}
