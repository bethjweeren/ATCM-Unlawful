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

    void Start()
    {
        gameObject.SetActive(false);
        DialogueSystem.Instance().endGame = this;
    }

    public void LoseGame(string reason)
    {
        headerText.text = "Game Over";
        headerText.color = loseColor;
        reasonText.text = reason;
        gameObject.SetActive(true);
    }

    public void WinGame(string reason)
    {
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
