using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Time_Manager : MonoBehaviour {

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
	private float hrDegree = 30f, minDegree = 6f;
	public float timer, timeCrunch, minute, currentTimeSpeed, normalTimeSpeed, fastTimeSpeed;
	public Transform hourHand, minuteHand, restPlace;
	public GameObject Player, skipRest, nightShade, afternoonShade;
	private PlayerController playerController;

	void Start()
	{
		currentTimeState = State.Morning;
		hour = hourStart;
		pastTimeState = currentTimeState;
		currentTimeSpeed = normalTimeSpeed;
		playerController = Player.GetComponent<PlayerController> ();

		Button skipRest_Btn = skipRest.GetComponent<Button> ();
		skipRest_Btn.onClick.AddListener (SkipRestPeriod);

		skipRest.SetActive (false);
		afternoonShade.SetActive (false);
		nightShade.SetActive (false);
	}

	void Update()
	{
		TimeKeepingStates ();
		UpdateClock ();
		//Debug.Log (hour + " : " + minute + " _ " + day);
	}

	void TimeKeepingStates()
	{
		switch (currentTimeState)
		{
		case State.Morning:
			MorningCode ();
			break;

		case State.Afternoon:
			AfternoonCode ();
			break;

		case State.Night:
			NightCode ();
			break;

		case State.Rest:
			RestCode ();
			break;

		case State.Paused:
			PausedCode ();
			break;
		}
	}

	void UpdateTime(bool isTimeMoving, float timeSpeed)
	{
		if (isTimeMoving) {
			if (timer >= timeCrunch) {
				timer = 0;
				minute += (timeSpeed * Time.deltaTime);
			}
			if (minute >= 60) {
				minute = 0;
				hour++;
			}
			if (hour >= 24) {
				hour = 0;
				day++;
			}
		}
	}

	void MorningCode(){
		NormalCode ();
		UpdateTime (true, currentTimeSpeed);

		//enter morning code

		if (hour >= afternoonStart) {
			afternoonShade.SetActive (true);
			currentTimeState = State.Afternoon;
		}
		SwitchToPausedState ();
	}

	void AfternoonCode(){
		NormalCode ();
		UpdateTime (true, currentTimeSpeed);

		//enter afternoon code

		if (hour >= nightStart) {
			afternoonShade.SetActive (false);
			nightShade.SetActive (true);
			currentTimeState = State.Night;
		}
		SwitchToPausedState ();
	}

	void NightCode(){
		NormalCode ();
		UpdateTime (true, currentTimeSpeed);

		//enter night code

		if (hour == morningStart) {
			skipRest.SetActive (true);
			currentTimeState = State.Rest;
		}
		SwitchToPausedState ();
	}

	void RestCode(){
		//enter rest code
		//move the player
		Player.transform.position = restPlace.position;
		//stop player
		playerController.StopInput ();

		UpdateTime (false, currentTimeSpeed);
		SwitchToPausedState ();
	}

	void PausedCode(){

		//enter paused code

		UpdateTime (false, currentTimeSpeed);
		if (Input.GetKeyDown(KeyCode.P)) {
			Debug.Log ("trying to resume");
			currentTimeState = pastTimeState;
			UpdateTime (true, currentTimeSpeed);
		}
	}

	public void SwitchToPausedState(){
		if (Input.GetKeyDown(KeyCode.P)) {
			pastTimeState = currentTimeState;
			currentTimeState = State.Paused;
		}
	}

	void NormalCode(){
		ChangeTimeSpeed ();
	}

	void ChangeTimeSpeed(){
		if (Input.GetKeyDown (KeyCode.Keypad6)) {
			currentTimeSpeed = fastTimeSpeed;
		}

		if (Input.GetKeyDown (KeyCode.Keypad5)) {
			currentTimeSpeed = normalTimeSpeed;
		}
	}

	void UpdateClock(){
		hourHand.localRotation = Quaternion.Euler(0f, 0f, (-hour * hrDegree) + 90f);
		minuteHand.localRotation = Quaternion.Euler(0f, 0f, (-minute * minDegree) + 90f);
	}

	void SkipRestPeriod(){
		nightShade.SetActive (false);
		currentTimeState = State.Morning;
		hour = 6;
		minute = 0;
		skipRest.SetActive (false);
		UpdateTime (true, currentTimeSpeed);
		playerController.ResumeInput ();
	}

	public void LeavePauseState(){
		currentTimeState = pastTimeState;
		UpdateTime (true, currentTimeSpeed);
	}

	public void MakeMorning(){
		nightShade.SetActive (false);
		afternoonShade.SetActive (false);
		currentTimeState = State.Morning;
		pastTimeState = State.Morning;
		hour = 6;
		minute = 0;
	}

	public void MakeAfternoon(){
		nightShade.SetActive (false);
		afternoonShade.SetActive (true);
		currentTimeState = State.Afternoon;
		pastTimeState = State.Afternoon;
		hour = 12;
		minute = 0;
	}

	public void MakeNight(){
		nightShade.SetActive (true);
		afternoonShade.SetActive (false);
		currentTimeState = State.Night;
		pastTimeState = State.Night;
		hour = 12;
		minute = 0;
	}

	public void MakeRest(){
		nightShade.SetActive (true);
		afternoonShade.SetActive (false);
		currentTimeState = State.Rest;
		pastTimeState = State.Rest;
		hour = 18;
		minute = 0;
	}

	//Need this for in-game Menu (esc in PlayerController)
	public void ForcePausedState()
	{
		pastTimeState = currentTimeState;
		currentTimeState = State.Paused;
	}
}
