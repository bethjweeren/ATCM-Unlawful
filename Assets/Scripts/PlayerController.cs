using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Controls player movement and handles player sprite animations
public class PlayerController : MonoBehaviour
{
	// Different screens a player can be in
	// Was originally going to have "still" and "walking" but there was too much overlap
	enum State
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
	public GameObject journalCanvas;
	public GameObject enableOnStart;
	public GameObject screenMenuStarting;
	public GameObject screenMenu;
	public GameObject screenControls;
	public GameObject screenCredits;
	private State currentState;
	private Rigidbody2D playerRB;
    private Collider2D playerCollider;
	private Animator animator;
	public Button jounalButton;
    public float interactRange = 1;
    public LayerMask interactLayer;
    public Transform interactRayOrigin;
	public Time_Manager time_manager;

    public static float playerY;

    // Use this for initialization
    void Start()
	{
        DialogueSystem.Instance().player = this;
        journalCanvas.SetActive (false);
		journal_manager = journalCanvas.GetComponent<Journal_Manager> ();
		jounalButton.onClick.AddListener (ToggleJournal);
		currentState = State.MAIN;
		playerRB = this.GetComponent<Rigidbody2D>();
		animator = this.GetComponent<Animator>();
        playerCollider = this.GetComponent<Collider2D>();
		animator.SetBool("Walking", false); //Stop animating sprite
		playerRB.velocity = new Vector2(0, 0); //Don't move
		screenMenuStarting.SetActive(true); //Starting menu gets in the way, keep disabled in scene and this will enable it
		enableOnStart.SetActive(true); //Enable all the intrusive UI things on start, because they get in the way in the scene
	}

	// Update is called every fixed framerate frame
	void Update()
	{
        playerY = transform.position.y + playerCollider.offset.y;

        switch (currentState)
		{
			case State.MAIN:
                //TO-DO Use Axis instead of GetKey
                //TO-DO Only change animator and velocity when keys pressed or released, not held down

                //If ANY key was just pressed, change animation and velocity
                //So it doesn't change every frame
                if ((Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) || ((Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) && !(Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))))
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
                        currentState = State.JOURNAL;
                        StopMoving();
                        print("Looking at map. Press M or ESC or Space to close."); //Replace with map code
                        journalCanvas.SetActive(true);
                        journal_manager.BringUpMap();
                    }
                    else if (Input.GetKeyDown(KeyCode.J))
                    {
                        currentState = State.JOURNAL;
                        StopMoving();
                        print("Looking at journal. Press J or ESC to close."); //Replace with journal code
                        journalCanvas.SetActive(true);
                    }
                    /*
                    else if (Input.GetKeyDown(KeyCode.I))
                    {
                        currentState = State.INVENTORY;
                        StopMoving();
                        print("Looking through inventory. Press I or ESC to close."); //Replace with inventory code
                    }
                    */

                    else if (Input.GetKeyDown(KeyCode.Escape))
                    {
                        currentState = State.MENU;
                        StopMoving();
						time_manager.ForcePausedState();
						screenMenu.SetActive(true);
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
                        currentState = State.INTERACTING;
                        StopMoving();
                        foundObject.collider.GetComponent<IInteractable>().Interact();
                    }
                    else
                    {
                        GameObject[] interacts = GameObject.FindGameObjectsWithTag("Interactable");
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
                                    Debug.Log("south");
                                }
                                else
                                {
                                    animator.SetInteger("Direction", 0); //Face north
                                    Debug.Log("north");
                                }
                            }
                            else
                            {
                                if (transform.position.x > nearest.transform.position.x)
                                {
                                    animator.SetInteger("Direction", 3); //Face west
                                    Debug.Log("west");
                                }
                                else
                                {
                                    animator.SetInteger("Direction", 1); //Face east
                                    Debug.Log("east");
                                }
                            }
                            currentState = State.INTERACTING;
                            StopMoving();
                            nearest.GetComponent<IInteractable>().Interact();
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
					currentState = State.MAIN;
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
					time_manager.LeavePauseState();
					currentState = State.MAIN;
				}
				break;

			case State.PAUSED:
			break;
		}
	}

    void StopMoving()
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
            currentState = State.MAIN;
        }
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
			currentState = State.JOURNAL;
			journalCanvas.SetActive (true);
		} else {
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

}
