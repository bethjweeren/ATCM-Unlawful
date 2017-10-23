using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

// Controls player movement and handles player sprite animations
public class PlayerController : MonoBehaviour
{
	// Different screens a player can be in
	public enum State
	{
		MAIN,
		JOURNAL,
		INVENTORY,
		INTERACTING,
		PAUSED,
		MENU
	};

	public float speed = 1;
	public float sprintValue = 1;
	public Journal_Manager journal_manager;
	public ItemsManager itemsManager;
	public GameObject itemCanvas;
	public GameObject journalCanvas;
	public GameObject enableOnStart;
	public GameObject screenMenuStarting;
	public GameObject screenMenu;
	public GameObject screenControls;
	public GameObject screenCredits;
	public State currentState;

	public Button journalButton;
	public float interactRange = 1;
	public LayerMask interactLayer;
	public Transform interactRayOrigin;
	public Time_Manager time_manager;

	public static float playerY;

	public GameObject sleepingNPCNotice;

	private Rigidbody2D playerRB;
	private Collider2D playerCollider;
	private Animator animator;
	
	//public bool isInTutorial = false;
	//public Tutorial tutorial;

	// Use this for initialization
	void Start()
	{
        ClueList test = new ClueList();
        Provider.GetInstance().player = this;
        journalCanvas.SetActive (false);
		itemCanvas.SetActive (false);
		journal_manager = journalCanvas.GetComponent<Journal_Manager> ();
		journalButton.onClick.AddListener(ToggleJournal);
		currentState = State.MAIN;
		playerRB = GetComponent<Rigidbody2D>();
		animator = GetComponent<Animator>();
        playerCollider = GetComponent<Collider2D>();
		animator.SetBool("Walking", false); //Stop animating sprite
		playerRB.velocity = new Vector2(0, 0); //Don't move
		screenMenuStarting.SetActive(true); //Starting menu gets in the way, keep disabled in scene and this will enable it
		enableOnStart.SetActive(true); //Enable all the intrusive UI things on start, because they get in the way in the scene
        //DialogueSystem.Instance().CreateJournalEntry("[Red] wanted to talk to me", CharacterID.RED, "talkToRed");
    }

	// Update is called every fixed framerate frame
	void Update()
	{
		//If you press CTRL + SHIFT + R the game/scene reloads
		if ((Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)) && (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) && Input.GetKey(KeyCode.R))
		{
			SceneManager.LoadScene(SceneManager.GetActiveScene().name); //Reloads current scene
		}

		playerY = transform.position.y + playerCollider.offset.y;

		switch (currentState)
		{
		case State.MAIN:
			//TO-DO Use Axis instead of GetKey
			//TO-DO Only change animator and velocity when keys pressed or released, not held down

			//If ANY key was just pressed, change animation and velocity
			//So it doesn't change every frame
			if ((Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) ||
					((Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) && !(Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))))
			{
				animator.SetInteger("Direction", 0); //Animate north sprite
				animator.SetBool("Walking", true);
				playerRB.velocity = new Vector2(0, 1) * speed * sprintValue; //Move north
			}
			else if ((Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)) || ((Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) && !(Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))))
			{
				animator.SetInteger("Direction", 1); //Animate east sprite
				animator.SetBool("Walking", true);
				playerRB.velocity = new Vector2(1, 0) * speed * sprintValue; //Move east
			}
			else if ((Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)) || ((Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) && !(Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))))
			{
				animator.SetInteger("Direction", 2); //Animate south sprite
				animator.SetBool("Walking", true);
				playerRB.velocity = new Vector2(0, -1) * speed * sprintValue; //Move south
			}
			else if ((Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow)) || ((Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) && !(Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))))
			{
				animator.SetInteger("Direction", 3); //Animate west sprite
				animator.SetBool("Walking", true);
				playerRB.velocity = new Vector2(-1, 0) * speed * sprintValue; //Move west
			}
			else
			{
				if (Input.GetKeyDown(KeyCode.M))
				{
					if (time_manager.currentTimeState != Time_Manager.State.Rest) {
						time_manager.ForcePausedState ();
					}
					currentState = State.JOURNAL;
					StopMoving();
					print("Looking at map. Press M or ESC or Space to close."); //Replace with map code
					journalCanvas.SetActive(true);
					journal_manager.BringUpMap();
					FreezeNPCs();
				}
				else if (Input.GetKeyDown(KeyCode.J))
				{
					if (time_manager.currentTimeState != Time_Manager.State.Rest) {
						time_manager.ForcePausedState ();
					}
					currentState = State.JOURNAL;
					StopMoving();
					print("Looking at journal. Press J or ESC to close."); //Replace with journal code
					journalCanvas.SetActive(true);
					FreezeNPCs();
				}
				/*
				else if (Input.GetKeyDown(KeyCode.I))
				{
					currentState = State.INVENTORY;
					StopMoving();
					itemCanvas.SetActive (true);
					print("Looking through inventory. Press I or ESC to close."); //Replace with inventory code
					FreezeNPCs();
				}
				*/
				else if (Input.GetKeyDown(KeyCode.Escape))
				{
					if (time_manager.currentTimeState != Time_Manager.State.Rest)
					{
						time_manager.ForcePausedState ();
					}
					currentState = State.MENU;
					StopMoving();
					screenMenu.SetActive(true);
					FreezeNPCs();
				}
			}
			/*
                if (Input.GetKeyDown(KeyCode.LeftShift))
                {
                    sprintValue = 2;
                }
                else if (Input.GetKeyUp(KeyCode.LeftShift))
                {
                    sprintValue = 1;
                }
				*/

			if (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Space))
			{
				int facing = animator.GetInteger("Direction");
				Vector2 facingVector;
				switch (facing)
				{
					case 0:
						facingVector = Vector2.up;
						break;
					case 1:
						facingVector = Vector2.right;
						break;
					case 2:
						facingVector = Vector2.down;
						break;
					case 3:
						facingVector = Vector2.left;
						break;
					default:
						Debug.Log("Invalid facing dirction recieved from animator component");
						facingVector = Vector2.zero;
						break;
				}
				Debug.DrawLine(interactRayOrigin.position, (Vector2)interactRayOrigin.position + interactRange * facingVector);
				RaycastHit2D foundObject = Physics2D.Raycast(interactRayOrigin.position, facingVector, interactRange, interactLayer);
				if (foundObject)
				{
					if (time_manager.currentTimeState != Time_Manager.State.Rest)
					{
						BeginInteraction(foundObject.collider.GetComponent<IInteractable>());
					}
					else
					{
						print("interacting at night");
						if (foundObject.collider.gameObject.CompareTag("NPC"))
						{
							print("found sleeping npc");
							sleepingNPCNotice.SetActive(true);
						}
						else
						{
							BeginInteraction(foundObject.collider.GetComponent<IInteractable>());
						}
					}
				}
				else
				{

						GameObject[] interactableTagged = GameObject.FindGameObjectsWithTag("Interactable");
						GameObject[] npcTagged = GameObject.FindGameObjectsWithTag("NPC");
						GameObject[] interacts = new GameObject[interactableTagged.Length + npcTagged.Length];
						for (int i = 0; i < interactableTagged.Length; i++)
						{
							interacts[i] = interactableTagged[i];
						}
						for (int i = 0; i < npcTagged.Length; i++)
						{
							interacts[i+interactableTagged.Length] = npcTagged[i];
						}
						GameObject nearest = null;
						float distance = Mathf.Infinity;
						foreach (GameObject g in interacts)
						{
							if(Vector2.Distance(transform.position, g.transform.position) < distance)
							{
								nearest = g;
								distance = Vector2.Distance(transform.position, g.transform.position);
							}
						}
						if (nearest != null && distance < interactRange * 0.75f) //If the player misses the raycast but there is an interactable object nearby
						{
							if(Mathf.Abs(transform.position.y - nearest.transform.position.y) > Mathf.Abs(transform.position.x - nearest.transform.position.x))
							{
								if(transform.position.y > nearest.transform.position.y)
								{
									animator.SetInteger("Direction", 2); //Face south
								}
								else
								{
									animator.SetInteger("Direction", 0); //Face north
								}
							}
							else
							{
								if (transform.position.x > nearest.transform.position.x)
								{
									animator.SetInteger("Direction", 3); //Face west
								}
								else
								{
									animator.SetInteger("Direction", 1); //Face east
								}
							}
							if (time_manager.currentTimeState != Time_Manager.State.Rest)
							{
                                BeginInteraction(nearest.GetComponent<IInteractable>());
                            }
							else if (nearest.CompareTag("NPC"))
							{
								print("found sleeping npc 2");
								sleepingNPCNotice.SetActive(true);
							}
							else
							{
                                BeginInteraction(nearest.GetComponent<IInteractable>());
                            }
						}
				}
			}
			else if (!AnyMovementKey())
			{
				StopMoving();
			}
			break;

		case State.JOURNAL:
			if (Input.GetKeyDown(KeyCode.J) || Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.M) || Input.GetKeyDown(KeyCode.Space)) //Pressing "J" to close the journal might not work if typing
			{
				if (time_manager.currentTimeState != Time_Manager.State.Rest)
				{
					time_manager.LeavePauseState ();
				}
				currentState = State.MAIN;
				journalCanvas.SetActive(false);
				UnfreezeNPCs();
			}
			break;

		case State.INVENTORY:
			if (Input.GetKeyDown(KeyCode.I) || Input.GetKeyDown(KeyCode.Escape))
			{
				itemCanvas.SetActive (false);
				currentState = State.MAIN;
				UnfreezeNPCs();
			}
			break;

		case State.INTERACTING:
			//if(isInTutorial && Input.GetKeyDown(KeyCode.Space)){
			//	tutorial.Close();
			//}
			/*
				if (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Escape))
				{
					currentState = State.MAIN;
					print("Ended interaction."); //Replace with interaction/NPC discussion code
				}
				*/
			break;

		case State.MENU:
			if (time_manager.currentTimeState != Time_Manager.State.Rest) {
				time_manager.ForcePausedState ();
			}
			if (Input.GetKeyDown(KeyCode.Escape))
			{
				if (time_manager.currentTimeState != Time_Manager.State.Rest) {
					time_manager.LeavePauseState ();
				}
				screenMenu.SetActive(false);
				screenControls.SetActive(false);
				screenCredits.SetActive(false);
				currentState = State.MAIN;
			}
			break;

		case State.PAUSED:
			break;
		}
	}

	public void StopMoving()
	{
		animator.SetBool("Walking", false); //Stop animating sprite
		playerRB.velocity = new Vector2(0, 0); //Don't move
	}

	bool AnyMovementKeyDown()
	{
		return (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.RightArrow));
	}

	bool AnyMovementKey()
	{
		return (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.RightArrow));
	}

	public void EndInteraction()
	{
		if (currentState == State.INTERACTING)
		{
			if (time_manager.currentTimeState != Time_Manager.State.Rest) {
				time_manager.LeavePauseState ();
			}
			currentState = State.MAIN;
			UnfreezeNPCs();
		}
        journalButton.gameObject.SetActive(true);
	}

    public void BeginInteraction(IInteractable other)
    {
        currentState = State.INTERACTING;
        StopMoving();
        time_manager.ForcePausedState();
        journalButton.gameObject.SetActive(false);
        other.Interact();
        //FreezeNPCs(); They should already be frozen by time manager
    }

	//Similar to EndInteraction, but need this for the Menu, specifically ButtonPlay.cs
	public void SwitchToMainState()
	{
		currentState = State.MAIN;
	}

	public void SetSpeed(float newSpeed)
	{
		speed = newSpeed;
	}

	void ToggleJournal()
	{
		if (currentState == State.MAIN) {
			if (time_manager.currentTimeState != Time_Manager.State.Rest) {
				time_manager.ForcePausedState ();
			}
			currentState = State.JOURNAL;
			journalCanvas.SetActive (true);
		} else {
			if (time_manager.currentTimeState != Time_Manager.State.Rest) {
				time_manager.LeavePauseState ();
			}
			currentState = State.MAIN;
			journalCanvas.SetActive(false);
		}
	}

	public void StopInput()
	{
		animator.SetBool("Walking", false);
		currentState = State.PAUSED;
	}

	public void ResumeInput()
	{
		currentState = State.MAIN;
	}

	public void SayHello(){
		Debug.Log ("hello");
		currentState = State.MAIN;
	}

	//I have the player change the NPCs' state as needed because it's WAY fewer checks
	//than having every NPC check the player's state every update.
	public void FreezeNPCs()
	{

		//foreach (NPC npcController in npcs)
		//{
		//	npcController.StopMoving();
		//}

	}

	//I have the player change the NPCs' state as needed because it's WAY fewer checks
	//than having every NPC check the player's state every update.
	public void UnfreezeNPCs()
	{

		//foreach (NPC npcController in npcs)
		//{
		//	npcController.StartMoving();
		//}

	}

	void OnEnable()
	{
		//UnfreezeNPCs();
	}

	void OnDisable()
	{
		//FreezeNPCs();
	}
}