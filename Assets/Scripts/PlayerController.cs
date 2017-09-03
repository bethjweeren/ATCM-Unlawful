using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Controls player movement and handles player sprite animations
public class PlayerController : MonoBehaviour
{
	// Different screens a player can be in
	// Was originally going to have "still" and "walking" but there was too much overlap
	enum State
	{
		MAIN,
		MAP,
		JOURNAL,
		INVENTORY,
		INTERACTING,
		MENU
	};

	public float speed = 1;
	public GameObject journalManager;
	public GameObject journalCanvas;
	public GameObject screenMenu;
	public GameObject screenControls;
	public GameObject screenCredits;
	private State currentState;
	private Rigidbody2D playerRB;
	private Animator animator;

	// Use this for initialization
	void Start()
	{
		currentState = State.MENU;
		playerRB = this.GetComponent<Rigidbody2D>();
		animator = this.GetComponent<Animator>();
		animator.enabled = false; //Stop animating sprite
		playerRB.velocity = new Vector2(0, 0); //Don't move
		screenMenu.SetActive(true); //Start with menu enabled, because it's not fun to keep it enabled
	}

	// Update is called every fixed framerate frame
	void Update()
	{
		switch (currentState)
		{
			case State.MAIN:
				animator.enabled = true; //Animate sprite

				//TO-DO Use Axis instead of GetKey
				//TO-DO Only change animator and velocity when keys pressed or released, not held down

				//If ANY key was just pressed, change animation and velocity
				//So it doesn't change every framevvvvvvvvvvvvvvvvvvvvvvvvvv
				if (Input.anyKeyDown)
				{
					if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
					{
						animator.SetInteger("Direction", 0); //Animate north sprite
						playerRB.velocity = new Vector2(0, 1) * speed; //Move north
					}
					else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
					{
						animator.SetInteger("Direction", 1); //Animate east sprite
						playerRB.velocity = new Vector2(1, 0) * speed; //Move east
					}
					else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
					{
						animator.SetInteger("Direction", 2); //Animate south sprite
						playerRB.velocity = new Vector2(0, -1) * speed; //Move south
					}
					else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
					{
						animator.SetInteger("Direction", 3); //Animate west sprite
						playerRB.velocity = new Vector2(-1, 0) * speed; //Move west
					}
					else
					{
						animator.enabled = false; //Stop animating sprite
						playerRB.velocity = new Vector2(0, 0); //Don't move

						if (Input.GetKeyDown(KeyCode.M))
						{
							currentState = State.MAP;
							print("Looking at map. Press M or ESC or Space to close."); //Replace with map code
						}
						else if (Input.GetKeyDown(KeyCode.J))
						{
							currentState = State.JOURNAL;
							print("Looking at journal. Press J or ESC to close."); //Replace with journal code
							journalManager.SetActive(true);
							journalCanvas.SetActive(true);

						}
						/*
						else if (Input.GetKeyDown(KeyCode.I))
						{
							currentState = State.INVENTORY;
							print("Looking through inventory. Press I or ESC to close."); //Replace with inventory code
						}
						else if (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Space))
						{
							currentState = State.INTERACTING;
							print("Looking or talking. Press E or Space or ESC to end interaction."); //Replace with interaction code
						}
						*/
						else if (Input.GetKeyDown(KeyCode.Escape))
						{
							currentState = State.MENU;
							screenMenu.SetActive(true);
						}
					}
				}

				else if (!(Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.RightArrow)))
				{
					animator.enabled = false; //Stop animating sprite
					playerRB.velocity = new Vector2(0, 0); //Don't move
				}
				break;
			case State.MAP:
				if (Input.GetKeyDown(KeyCode.M) || Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Space))
				{
					currentState = State.MAIN;
					print("Closed map"); //Replace with map code
				}
				break;
			case State.JOURNAL:
				if (Input.GetKeyDown(KeyCode.J) || Input.GetKeyDown(KeyCode.Escape)) //Pressing "J" to close the journal might not work if typing
				{
					currentState = State.MAIN;
					journalManager.SetActive(false);
					journalCanvas.SetActive(false);
					print("Closed journal."); //Replace with journal code
				}
				break;
			/*
			case State.INVENTORY:
				if (Input.GetKeyDown(KeyCode.I) || Input.GetKeyDown(KeyCode.Escape))
				{
					currentState = State.MAIN;
					print("Closed inventory."); //Replace with inventory code
				}
				break;
			case State.INTERACTING:
				if (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Escape))
				{
					currentState = State.MAIN;
					print("Ended interaction."); //Replace with interaction/NPC discussion code
				}
				break;
			*/
			case State.MENU:
				if (Input.GetKeyDown(KeyCode.Escape))
				{
					screenMenu.SetActive(false);
					screenControls.SetActive(false);
					screenCredits.SetActive(false);
					currentState = State.MAIN;
				}
				else if (!screenMenu.activeInHierarchy && !screenControls.activeInHierarchy && !screenCredits.activeInHierarchy)
				{
					currentState = State.MAIN;
				}
				break;
		}
	}
}
