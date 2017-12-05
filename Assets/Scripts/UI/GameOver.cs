using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOver : MonoBehaviour
{

	public Text headerText;
	public Text reasonText;
	public Color winColor;
	public Color loseColor;
	public AudioClip winSound;
	public AudioClip loseSound;
	public GameObject clock;
	public GameObject journal;
	public GameObject inventory;
    public GameObject map;
    public GameObject sleepText;
	public GameObject returnHome;
    public AudioSource audioSource;

	public void LoseGame(string reason)
	{
        gameObject.SetActive(true);
        clock.SetActive(false);
		journal.SetActive(false);
		inventory.SetActive(false);
        map.SetActive(false);
        returnHome.SetActive(false);
        if (loseSound != null)
        {
            audioSource.clip = loseSound;
            audioSource.Play();
        }
		headerText.text = "Game Over";
		headerText.color = loseColor;
		reasonText.text = reason;
	}

	public void WinGame(string reason)
	{
        gameObject.SetActive(true);
        clock.SetActive(false);
		journal.SetActive(false);
		inventory.SetActive(false);
        map.SetActive(false);
        returnHome.SetActive(false);
        if (winSound != null)
        {
            audioSource.clip = winSound;
            audioSource.Play();
        }
		headerText.text = "Congratulations";
		headerText.color = winColor;
		reasonText.text = reason;
	}

	public void Restart()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
		DialogueSystem.Instance().Reset();
	}
}
