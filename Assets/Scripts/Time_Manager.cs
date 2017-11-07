using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Time_Manager : MonoBehaviour
{

	public enum State
	{
		Morning,
		Afternoon,
		Night,
		Rest,
		Paused,
	};

	public State currentTimeState;
	public State pastTimeState;

	public int hour, hourStart = 6, day, afternoonStart = 12, nightStart = 18, morningStart = 6, restStart = 0;
	public float hrDegree = 10f, timerOffset = 10f /*, minDegree = 6f*/;
	public float timer, timeCrunch, minute, currentTimeSpeed, normalTimeSpeed, fastTimeSpeed, pauseEffectTimer, timerHand, minuteCounter;
	public GameObject hourHand, /* minuteHand, */ hourHand_red,/* minuteHand_red, */ hourHand_black /*, minuteHand_black*/;
	public Transform restPlace;
	public GameObject Player, nightShade, afternoonShade, homeButton;
	private PlayerController playerController;
	public bool isGoing = true;
	public string outOfTimeReason;
	public Journal_Manager journal_Manager;
	public Possessions_Manager moneyManager;
	//private WaitForSeconds pauseEffectDuration = new WaitForSeconds(.5f);
	//private NPC[] npcs; //To freeze/unfreeze NPCs (so they don't have to check on every update)
	private GameObject[] npcs; //To freeze/unfreeze NPCs (so they don't have to check on every update)
	bool alreadyFrozeNPCs = false;
	public AudioSource timeShift, endOfDay;
	public Text dayText;

	void Start()
	{
		pauseEffectTimer = 0;
		currentTimeState = State.Morning;
		hour = hourStart;
		pastTimeState = currentTimeState;
		currentTimeSpeed = normalTimeSpeed;
		playerController = Player.GetComponent<PlayerController>();

		afternoonShade.SetActive(false);
		nightShade.SetActive(false);
		timeShift.Play();
	}

	void Awake()
	{
		npcs = GameObject.FindGameObjectsWithTag("NPC");
	}

	void FixedUpdate()
	{
		timerHand++;
	}

	void Update()
	{
		TimeKeepingStates();
		UpdateClock();
		//Debug.Log (hour + " : " + minute + " _ " + day);
		if (day >= 4) {
			Provider.GetInstance().gameOver.LoseGame(outOfTimeReason);
            this.enabled = false;
		}

	}

	void TimeKeepingStates()
	{
		switch (currentTimeState)
		{
			case State.Morning:
				MorningCode();
				break;

			case State.Afternoon:
				AfternoonCode();
				break;

			case State.Night:
				NightCode();
				break;

			case State.Rest:
				RestCode();
				break;

			case State.Paused:
				PausedCode();
				break;
		}
	}

	void UpdateTime(bool isTimeMoving, float timeSpeed)
	{
		if (isTimeMoving)
		{
			if (timer >= timeCrunch)
			{
				timer = 0;
				minute += (timeSpeed * Time.deltaTime);
				minuteCounter += (timeSpeed * Time.deltaTime);
			}
			if (minute >= 60)
			{
				minute = 0;
				hour++;
			}
			if (minuteCounter >= 1160)
			{
                //SkipRestPeriod();
            }
			if (hour >= 24)
			{
				hour = 0;
				//moneyManager.ChangeMoney (100);
				//ResetBins();
				day++;
				if (day == 1)
				{
					dayText.text = "First Day";
				}
				else if (day == 2)
				{
					dayText.text = "Second Day";
				}
				else //Day 3
				{
					dayText.text = "Final Day";
				}
			}
		}
	}

	void MorningCode()
	{
		NormalCode();
		UpdateTime(true, currentTimeSpeed);
		pastTimeState = State.Morning;
		homeButton.SetActive(false);
		//enter morning code

		if (hour >= afternoonStart)
		{
			timeShift.Play();
			afternoonShade.SetActive(true);
			currentTimeState = State.Afternoon;
		}
		SwitchToPausedState();
	}

	void AfternoonCode()
	{
		NormalCode();
		UpdateTime(true, currentTimeSpeed);
		pastTimeState = State.Afternoon;

		//enter afternoon code

		if (hour >= nightStart)
		{
			timeShift.Play();
			afternoonShade.SetActive(false);
			nightShade.SetActive(true);
            currentTimeState = State.Night;
		}
		SwitchToPausedState();
	}

	void NightCode()
	{
		NormalCode();
		UpdateTime(true, currentTimeSpeed);
		pastTimeState = State.Night;

        //enter night code

        if (hour == morningStart)
		{
			endOfDay.Play();
			//skipRest.SetActive(true);
			currentTimeState = State.Rest;
			pastTimeState = State.Rest;
		}
		SwitchToPausedState();
	}

	void RestCode()
	{
		//enter rest code

		//move the player
		//Player.transform.position = restPlace.position;
		//stop player
		/*
		if (isGoing)
		{
			playerController.StopInput();
			isGoing = false;
		}
		*/
		homeButton.SetActive(true);
		if (minuteCounter >= 1160)
        {
            UpdateTime(false, currentTimeSpeed);
        }
        else
        {
            UpdateTime(true, currentTimeSpeed);
        }

        //UpdateTime(false, currentTimeSpeed);
        //SwitchToPausedState();
    }

	void PausedCode()
	{

		pauseEffectTimer += Time.deltaTime;

		if (Mathf.RoundToInt(pauseEffectTimer) % 2 == 0)
		{
			SwitchToRed();
		}
		else
		{
			SwitchToBlack();
		}

		UpdateTime(false, currentTimeSpeed);
		if (Input.GetKeyDown(KeyCode.P))
		{
			pauseEffectTimer = 0;
			hourHand_red.SetActive(false);
			//minuteHand_red.SetActive (false);
			hourHand_black.SetActive(true);
			//minuteHand_black.SetActive (true);
			Debug.Log("trying to resume");
			currentTimeState = pastTimeState;
			UpdateTime(true, currentTimeSpeed);
		}
	}

	public void SwitchToPausedState()
	{
		if (Input.GetKeyDown(KeyCode.P))
		{
			pastTimeState = currentTimeState;
			currentTimeState = State.Paused;
			if (!alreadyFrozeNPCs) //It was calling it hundred of times...
				FreezeNPCs();
		}
	}

	void NormalCode()
	{
		ChangeTimeSpeed();
	}

	void ChangeTimeSpeed()
	{
		/*
		if (Input.GetKeyDown(KeyCode.Keypad6))
		{
			currentTimeSpeed = fastTimeSpeed;
		}

		if (Input.GetKeyDown(KeyCode.Keypad5))
		{
			currentTimeSpeed = normalTimeSpeed;
		}
		*/
	}

	void UpdateClock()
	{
		hourHand.transform.localRotation = Quaternion.Euler(0f, 0f, (-minuteCounter * hrDegree) + timerOffset);
		//minuteHand.transform.localRotation = Quaternion.Euler(0f, 0f, (-minute * minDegree) + 90f);
	}

	public void SkipRestPeriod()
	{
		Debug.Log("skip rest period");
		timeShift.Play();
		nightShade.SetActive(false);
		homeButton.SetActive(false);
		currentTimeState = State.Morning;
		hour = 6;
		minute = 0;
		minuteCounter = 0;
		UpdateTime(true, currentTimeSpeed);
		playerController.ResumeInput();
		Debug.Log("skipped");
	}

	public void LeavePauseState()
	{
		pauseEffectTimer = 0;
		hourHand_red.SetActive(false);
		//minuteHand_red.SetActive (false);
		hourHand_black.SetActive(true);
		//minuteHand_black.SetActive (true);
		Debug.Log("resuming");
		currentTimeState = pastTimeState;
		UpdateTime(true, currentTimeSpeed);
		UnfreezeNPCs();
	}

	public void MakeMorning()
	{
		nightShade.SetActive(false);
		afternoonShade.SetActive(false);
		currentTimeState = State.Morning;
		pastTimeState = State.Morning;
		hour = 6;
		minute = 0;
	}

	public void MakeAfternoon()
	{
		nightShade.SetActive(false);
		afternoonShade.SetActive(true);
		currentTimeState = State.Afternoon;
		pastTimeState = State.Afternoon;
		hour = 12;
		minute = 0;
	}

	public void MakeNight()
	{
		nightShade.SetActive(true);
		afternoonShade.SetActive(false);
		currentTimeState = State.Night;
		pastTimeState = State.Night;
		hour = 12;
		minute = 0;
	}

	public void MakeRest()
	{
		nightShade.SetActive(true);
		afternoonShade.SetActive(false);
		currentTimeState = State.Rest;
		pastTimeState = State.Rest;
		hour = 18;
		minute = 0;
	}

	//Need this for in-game Menu (esc in PlayerController)
	public void ForcePausedState()
	{
		//pastTimeState = currentTimeState;
		currentTimeState = State.Paused;
		if (alreadyFrozeNPCs == false)
			FreezeNPCs();
	}

	void SwitchToRed()
	{
		hourHand_red.SetActive(true);
		//minuteHand_red.SetActive (true);
		hourHand_black.SetActive(false);
		//minuteHand_black.SetActive (false);
	}

	void SwitchToBlack()
	{
		hourHand_red.SetActive(false);
		//minuteHand_red.SetActive (false);
		hourHand_black.SetActive(true);
		//minuteHand_black.SetActive (true);
	}

	void ResetBins()
	{
		GameObject[] bins = GameObject.FindGameObjectsWithTag("Bin");
		foreach (GameObject bin in bins)
		{
			bin.GetComponent<Bin>().AssignRandomAmount();
		}
	}

	public void FreezeNPCs()
	{
		alreadyFrozeNPCs = true;
		foreach (GameObject npc in GameObject.FindGameObjectsWithTag("NPC"))
		{
			print(npc.GetComponent<NPC>().name + " freeze");
			npc.GetComponent<NPC>().StopMoving();
		}
	}

	public void UnfreezeNPCs()
	{
		foreach (GameObject npc in GameObject.FindGameObjectsWithTag("NPC"))
		{
			print(npc.GetComponent<NPC>().name + " unfreeze");
			npc.GetComponent<NPC>().StartMoving();
		}
		alreadyFrozeNPCs = false;
	}

	/*
	void OnEnable()
	{
		UnfreezeNPCs();
	}
	*/
}