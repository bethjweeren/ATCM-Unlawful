using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
	public float speed = 3f;
	public GameObject[] positions;	//Positions 
	public float waitBetween = 4;
	
	private Rigidbody2D rb;
	private int positionNum = 0;
	private Vector2 aimPos;
	private Animator animator;
	private float wait;
	private float closeEnough = .05f;
	private Vector2 nextPos;

	GameObject next;
	
	void Awake()
	{
		//player = GameObject.FindWithTag("Player");
	}

	// Use this for initialization
	void Start()
	{
		animator = this.GetComponent<Animator>();
		animator.SetBool("Walking", false); //Stop animating sprite
		foreach (GameObject obj in positions)
		{
			Renderer rend = obj.GetComponent<Renderer>();
			rend.enabled = false;
		}
		rb = this.GetComponent<Rigidbody2D>();
		next = positions[positionNum];
		wait = waitBetween;
	}

	void FixedUpdate()
	{
		if (wait <= 0)
		{
			if (positions.Length > 0)
			{
				next = positions[positionNum];
				nextPos = next.transform.position;
				//If near the goal position
				float xDiff = nextPos.x - transform.position.x;
				float yDiff = nextPos.y - transform.position.y;
				if ((Math.Abs(xDiff) <= closeEnough) && (Math.Abs(yDiff) <= closeEnough))
				{
					rb.velocity = new Vector2(0,0); //Stop moving
					animator.SetBool("Walking", false); //Stop animating sprite
					if (positionNum >= positions.Length - 1) //If at the last position, go the first position
					{
						positionNum = 0;
					}
					else
					{
						positionNum++;
					}
					next = positions[positionNum];
					nextPos = next.transform.position;
					wait = waitBetween;
				}

				//Will always move horizontally before vertically
				//This will almost definitely change when I add in the pathfinding aspect
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
		}
		else
		{
			wait--;
		}
	}

	void OnDisable()
	{
		//When this Object is disabled, make sure you also cancel the Invoke repeating method
		CancelInvoke();
	}
}
