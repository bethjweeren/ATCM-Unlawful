using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Possessions_Manager : MonoBehaviour {

	public int currentMoney = 100;
	public Text moneyText;

	void Start()
	{
		ChangeMoneyText ();
	}

	public void ChangeMoney(int amount)
	{
		currentMoney += amount;
		ChangeMoneyText ();
	}

	public void ChangeMoneyText()
	{
		moneyText.text = "Livres: " + currentMoney.ToString ();
	}
}
