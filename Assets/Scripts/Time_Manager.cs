using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Time_Manager : MonoBehaviour {

	public enum State
	{
		Morning,
		Afternoon,
		Night,
		Paused,
	};

	public static State currentTimeState;
	public static bool isTimeMoving = true;

	void Start()
	{
		currentTimeState = State.Morning;
	}

	void Update()
	{
		TimeKeepingStates ();
	}

	void TimeKeepingStates()
	{
		switch (currentTimeState)
		{
		case State.Morning:

			//enter Morning Code

			break;
		case State.Afternoon:

			//enter Afternoon Code

			break;
		case State.Night:

			//enter Night Code

			break;
		case State.Paused:

			//enter Paused Code

			break;
		}
	}
}
