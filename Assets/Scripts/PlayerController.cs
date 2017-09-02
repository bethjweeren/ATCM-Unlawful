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
		INTERACTING
	};

	public float speed = 1;
	public GameObject journalManager;
	public GameObject journalCanvas;
	private State currentState;
	private Rigidbody2D playerRB;
	private Animator animator;
    public float interactRange = 2;
    public LayerMask interactLayer;

	// Use this for initialization
	void Start()
	{
        DialogueSystem.Instance().player = this;
		currentState = State.MAIN;
		playerRB = this.GetComponent<Rigidbody2D>();
		animator = this.GetComponent<Animator>();
	}

	// Update is called every fixed framerate frame
	void FixedUpdate()
	{
		switch (currentState)
		{
			case State.MAIN:
				animator.enabled = true; //Animate sprite
				// It checks every frame if a key is held down and sets velocity and animation accordingly
				// ...Even if both were already set, which isn't ideal I think.
				if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
				{
					animator.SetInteger("Direction", 0); //Animate north sprite
					playerRB.velocity = new Vector2(0, 1) * speed; //Move north
				}
				else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
				{
					animator.SetInteger("Direction", 1); //Animate east sprite
					playerRB.velocity = new Vector2(1, 0) * speed; //Move east
				}
				else if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
				{
					animator.SetInteger("Direction", 2); //Animate south sprite
					playerRB.velocity = new Vector2(0, -1) * speed; //Move south
				}
				else if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
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
					else if (Input.GetKeyDown(KeyCode.I))
					{
						currentState = State.INVENTORY;
						print("Looking through inventory. Press I or ESC to close."); //Replace with inventory code
					}
					else if (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Space))
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
                        RaycastHit2D foundObject = Physics2D.Raycast(transform.position, facingVector, interactRange, interactLayer);
                        if (foundObject)
                        {
                            foundObject.collider.GetComponent<IInteractable>().Interact();
                            currentState = State.INTERACTING;
                        }
					}
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
			case State.INVENTORY:
				if (Input.GetKeyDown(KeyCode.I) || Input.GetKeyDown(KeyCode.Escape))
				{
					currentState = State.MAIN;
					print("Closed inventory."); //Replace with inventory code
				}
				break;
			/*case State.INTERACTING:
				if (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Escape))
				{
					currentState = State.MAIN;
					print("Ended interaction."); //Replace with interaction/NPC discussion code
				}
				break;*/
		}
	}

    public void EndInteraction()
    {
        if(currentState == State.INTERACTING)
        {
            currentState = State.MAIN;
        }
    }
}
