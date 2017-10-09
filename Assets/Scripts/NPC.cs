using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//The way NPC movement works right now is still a WIP.
//Pathfinding is still being worked on!!!
public class NPC : MonoBehaviour
{
	public float speed = 3f;
	private float originalSpeed;
	public float waitBetween = 4;
	private Rigidbody2D rb;
	private int positionNum = 0;
	private Vector2 aimPos;
	private Animator animator;
	private float wait;
	private float closeEnough = .05f;
	private Vector2 nextPos;
	public float waitAfterCollision = 30;
	private int xOrY = 0;
	private bool stopped = false;
	private bool getAwayFromPlayer = false;
	GameObject player;
	Vector3 positionAtCollider;
	GameObject next;
	Time_Manager timeManager;
	private GameObject[] positions;  //Positions 
	private Collider2D npcCollider;
	public WaypointSet[] waypointSets; //A list of a list of waypoints (I'm avoiding using the word "path" on purpose because pathfinding is separate)
	private int currentWaypointSet = 0; //We're assuming the sets are in order by time.

	void Awake()
	{
		player = GameObject.FindGameObjectWithTag("Player");
		timeManager = GameObject.FindObjectOfType<Time_Manager>();
	}

	// Use this for initialization
	void Start()
	{
		animator = GetComponent<Animator>();
		animator.SetBool("Walking", false); //Stop animating sprite
		rb = GetComponent<Rigidbody2D>();
		positions = waypointSets[currentWaypointSet].waypoints;
		next = positions[positionNum];
		waitBetween = waypointSets[currentWaypointSet].ticksToWaitAtEachWaypoint;
		wait = waitBetween;
		npcCollider = GetComponent<Collider2D>();
		originalSpeed = speed;
	}

	void FixedUpdate()
	{
		speed = originalSpeed * (timeManager.currentTimeSpeed / timeManager.normalTimeSpeed); //Speed up the NPCs when time changes
		if (CheckHour() == waypointSets[(currentWaypointSet + 1) % waypointSets.Length].hour)
		{
			currentWaypointSet = (currentWaypointSet + 1) % waypointSets.Length;
			positions = waypointSets[currentWaypointSet].waypoints;
			positionNum = 0;
			waitBetween = waypointSets[currentWaypointSet].ticksToWaitAtEachWaypoint;
			wait = waitBetween;
		}
		if (!stopped)
		{
			positionAtCollider = new Vector2(npcCollider.bounds.center.x, npcCollider.bounds.center.y);
			//print("col " + positionAtCollider);
			//print("pos " + transform.position);
			if (wait <= 0)
			{
				int directionPicker = UnityEngine.Random.Range(0, 2); //Next direction: East or west? / North or south?
				if (getAwayFromPlayer)
				{
					float playerSpeed = player.GetComponent<PlayerController>().speed;
					if (animator.GetInteger("Direction") == 0 || animator.GetInteger("Direction") == 2) //If moving north or south
					{
						switch (directionPicker)
						{
							case 0:
								animator.SetInteger("Direction", 1); //Animate east sprite
								rb.velocity = new Vector2(1, 0) * playerSpeed;
								break;
							case 1:
								animator.SetInteger("Direction", 3); //Animate west sprite
								rb.velocity = new Vector2(-1, 0) * playerSpeed;
								break;
							case 2:
								if (animator.GetInteger("Direction") == 0) //If moving north
								{
									animator.SetInteger("Direction", 2); //Animate south sprite
									rb.velocity = new Vector2(0, -1) * playerSpeed;
								}
								else //assume south
								{
									animator.SetInteger("Direction", 0); //Animate north sprite
									rb.velocity = new Vector2(0, 1) * playerSpeed;
								}
								break;
						}
					}
					else //Assume moving east or west
					{
						switch (directionPicker)
						{
							case 0:
								animator.SetInteger("Direction", 0); //Animate north sprite
								rb.velocity = new Vector2(0, 1) * playerSpeed;
								break;
							case 1:
								animator.SetInteger("Direction", 2); //Animate south sprite
								rb.velocity = new Vector2(0, -1) * playerSpeed;
								break;
							case 2:
								if (animator.GetInteger("Direction") == 1) //If moving east
								{
									animator.SetInteger("Direction", 3); //Animate west sprite
									rb.velocity = new Vector2(-1, 0) * playerSpeed;
								}
								else //assume west
								{
									animator.SetInteger("Direction", 1); //Animate east sprite
									rb.velocity = new Vector2(1, 0) * playerSpeed;
								}
								break;
						}
					}
					getAwayFromPlayer = false;
					wait = waitAfterCollision + UnityEngine.Random.Range(-waitAfterCollision, waitAfterCollision);
				}
				else
				{
					if (positions.Length > 0)
					{
						next = positions[positionNum];
						nextPos = next.transform.position;
						//If near the goal position
						float xDiff = nextPos.x - positionAtCollider.x;
						float yDiff = nextPos.y - positionAtCollider.y;
						if ((Math.Abs(xDiff) <= closeEnough) && (Math.Abs(yDiff) <= closeEnough))
						{
							rb.velocity = new Vector2(0, 0); //Stop moving
							animator.SetBool("Walking", false); //Stop animating sprite
							if (positionNum >= positions.Length - 1) 
							{
								if (waypointSets[currentWaypointSet].looped) //If at the last position, go the first position if looped
									positionNum = 0;
								else
								{
									animator.SetInteger("Direction", waypointSets[currentWaypointSet].finalDirection);
									animator.SetBool("Walking", false);
									rb.velocity = new Vector2(0, 0);
								}
							}
							else
							{
								positionNum++;
							}
							next = positions[positionNum];
							nextPos = next.transform.position;
							wait = waitBetween;
							//Randomly chooses which to do first, horizontal or vertical movement
							xOrY = UnityEngine.Random.Range(0, 1);
						}

						if (xOrY == 0) //Do horizontal movement first
						{
							if (Math.Abs(xDiff) > closeEnough)
							{
								if (xDiff > 0)
								{
									animator.SetInteger("Direction", 1); //Animate east sprite
									animator.SetBool("Walking", true);
									rb.velocity = new Vector2(1, 0) * speed;
								}
								else if (xDiff < 0)
								{
									animator.SetInteger("Direction", 3); //Animate west sprite
									animator.SetBool("Walking", true);
									rb.velocity = new Vector2(-1, 0) * speed;
								}
							}
							else if (Math.Abs(yDiff) > closeEnough)
							{
								{
									if (yDiff > 0)
									{
										animator.SetInteger("Direction", 0); //Animate north sprite
										animator.SetBool("Walking", true);
										rb.velocity = new Vector2(0, 1) * speed;
									}
									else if (yDiff < 0)
									{
										animator.SetInteger("Direction", 2); //Animate south sprite
										animator.SetBool("Walking", true);
										rb.velocity = new Vector2(0, -1) * speed;
									}
								}
							}
						}
						else //Do vertical movement first
						{
							if (Math.Abs(yDiff) > closeEnough)
							{
								{
									if (yDiff > 0)
									{
										animator.SetInteger("Direction", 0); //Animate north sprite
										animator.SetBool("Walking", true);
										rb.velocity = new Vector2(0, 1) * speed;
									}
									else if (yDiff < 0)
									{
										animator.SetInteger("Direction", 2); //Animate south sprite
										animator.SetBool("Walking", true);
										rb.velocity = new Vector2(0, -1) * speed;
									}
								}
							}
							else if (Math.Abs(xDiff) > closeEnough)
							{
								if (xDiff > 0)
								{
									animator.SetInteger("Direction", 1); //Animate east sprite
									animator.SetBool("Walking", true);
									rb.velocity = new Vector2(1, 0) * speed;
								}
								else if (xDiff < 0)
								{
									animator.SetInteger("Direction", 3); //Animate west sprite
									animator.SetBool("Walking", true);
									rb.velocity = new Vector2(-1, 0) * speed;
								}
							}
						}
					}
				}
			}
			else
			{
				wait--;
			}
		}
	}

	void OnCollisionStay2D(Collision2D collision)
	{
		OnCollisionEnter2D(collision);
	}

	//Ughhhh I don't like this, but I'm still working on actual pathfinding.
	//So this is my  temporary solution.
	//All this does is make the NPC go in a random direction for a short while
	//when it touches something.
	void OnCollisionEnter2D(Collision2D collision)
	{
		if (!stopped)
		{
			xOrY = UnityEngine.Random.Range(0, 1); //Whether start with horizontal or vertical first
			if (collision.otherRigidbody != null)
			{
				if (!collision.gameObject.Equals(player)) //If player bumped into
				{
					int directionPicker = UnityEngine.Random.Range(0, 2); //Next direction: East or west? / North or south?
					if (animator.GetInteger("Direction") == 0 || animator.GetInteger("Direction") == 2) //If moving north or south
					{
						if (animator.GetInteger("Direction") == 0) //If moving north
							this.transform.localPosition.Set(0, -speed, 0); //Move back south by speed
						else if (animator.GetInteger("Direction") == 2) //If moving south
							this.transform.localPosition.Set(0, speed, 0); //Move back north by speed
						switch (directionPicker)
						{
							case 0:
								animator.SetInteger("Direction", 1); //Animate east sprite
								rb.velocity = new Vector2(1, 0) * speed;
								break;
							case 1:
								animator.SetInteger("Direction", 3); //Animate west sprite
								rb.velocity = new Vector2(-1, 0) * speed;
								break;
							case 2:
								if (animator.GetInteger("Direction") == 0) //If moving north
								{
									animator.SetInteger("Direction", 2); //Animate south sprite
									rb.velocity = new Vector2(0, -1) * speed;
								}
								else //assume south
								{
									animator.SetInteger("Direction", 0); //Animate north sprite
									rb.velocity = new Vector2(0, 1) * speed;
								}
								break;
						}
					}
					else //Assume moving east or west
					{
						if (animator.GetInteger("Direction") == 1) //If moving east
							this.transform.localPosition.Set(-speed, 0, 0); //Move back west by speed
						else if (animator.GetInteger("Direction") == 3) //If moving west
							this.transform.localPosition.Set(speed, 0, 0); //Move back east by speed
						switch (directionPicker)
						{
							case 0:
								animator.SetInteger("Direction", 0); //Animate north sprite
								rb.velocity = new Vector2(0, 1) * speed;
								break;
							case 1:
								animator.SetInteger("Direction", 2); //Animate south sprite
								rb.velocity = new Vector2(0, -1) * speed;
								break;
							case 2:
								if (animator.GetInteger("Direction") == 1) //If moving east
								{
									animator.SetInteger("Direction", 3); //Animate west sprite
									rb.velocity = new Vector2(-1, 0) * speed;
								}
								else //assume west
								{
									animator.SetInteger("Direction", 1); //Animate east sprite
									rb.velocity = new Vector2(1, 0) * speed;
								}
								break;
						}
					}
					wait = waitAfterCollision + UnityEngine.Random.Range(-waitAfterCollision, waitAfterCollision);
				}
			}
		}
		else
		{
			rb.velocity = new Vector2(0, 0); //Stop moving
			animator.SetBool("Walking", false); //Stop animating sprite
		}
	}

	public void StopMoving()
	{
		print("Stop moving");
		rb.velocity = new Vector2(0, 0); //Stop moving
		animator.SetBool("Walking", false); //Stop animating sprite
		stopped = true;
	}

	//FixedUpdate takes over
	public void StartMoving()
	{
		print("Start moving");
		stopped = false;
	}

	/*
	void OnDisable()
	{
		//When this Object is disabled, make sure you also cancel the Invoke repeating method
		CancelInvoke();
	}
	*/

	//This ain't efficient
	int CheckHour()
	{
		return timeManager.hour;
	}
}
