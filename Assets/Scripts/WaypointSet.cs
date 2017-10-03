using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//A a set/list of objects that represent the positions the NPC travels through starting at a given time. It's basically a given path.
//I don't call it a "path" though because the actual path through these waypoints is determined using a pathfinding algorithm.
//The path will change, but it will always go through the waypoints (at least, if the NPC has time to visit all of them
//before the NPC controller switches to a different set of waypoints.
public class WaypointSet : MonoBehaviour
{
	public GameObject[] waypoints;  //Positions
	public int hour;
	public int minute;
	public bool looped = false; //After you reach the end, go back to the beginning? Or just stay there?
	public int ticksToWaitAtEachWaypoint = 4; //Yeah... ticks, not in-game time. Maybe this will change to in-game time.
	public int finalDirection = 2; //IF looped is false, this determines what direction the NPC is facing when they stop. 0 = North, 1 = East, 2 = South, 3 = West

	// Use this for initialization
	void Start ()
	{
		if (((hour < 6) && (hour > 24)) || ((minute < 0) && (minute > 60)))
		{
			print("The time for waypoint set " + name + " is invalid.");
		}

		foreach (GameObject obj in waypoints)
		{
			Renderer rend = obj.GetComponent<Renderer>();
			rend.enabled = false;
		}
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}
}
