﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//A vertex/node on the graph that determines whether an NPC can move at a particular location
public class Vertex : MonoBehaviour
{
	public bool debug = true; //If true, enable sprite renderer, else disable
	public Collider2D npcCollider; //The NPC's collider. Needed to determine if the vertex is passable.
	private Collider2D thisCollider; //The vertex's collider (a copy of the NPC's collider)
	bool passable = true; //Is this a wall/barrier?
	bool known = true; //Does the NPC know about this location? Almost identical to passable.
	bool unchangeable = false; //Will the passable or known variables stay the same?
	float weight = 1; //You'd think edges would have weight, but no, I'm putting it in the nodes. Every edge that *points* to this node has this weight.
					  //A higher weight means that the NPC is less likely to walk on this node because it's considered "further away" or more difficult to traverse
					  //This is useful if you want the NPCs to stick to the roads or something. Could probably be set with an area or something.
	//The following GameObjects NEED to have the Vertex script
	public GameObject north;
	public GameObject east;
	public GameObject south;
	public GameObject west;
	public int gridX, gridY;
	public SpriteRenderer sr;

	// Use this for initialization
	void Start ()
	{
		//If debug on, show vertices
		sr = GetComponent<SpriteRenderer>();
		if(debug)
			sr.enabled = true;
		else
			sr.enabled = false;

		//If the vertex is changeable, make a collider that's a copy of the NPC's collider
		if (!unchangeable)
		{
			if (npcCollider is BoxCollider2D)
			{
				//Sets collider to NPC's collider
				thisCollider = gameObject.AddComponent<BoxCollider2D>();
				thisCollider.isTrigger = true;
				((BoxCollider2D)thisCollider).size = new Vector2(((BoxCollider2D)npcCollider).size.x * (npcCollider.gameObject.transform.localScale.x), ((BoxCollider2D)npcCollider).size.y * (npcCollider.gameObject.transform.localScale.y));

				//Check if colliding with anything
				Collider2D[] overlappingColliders = new Collider2D[20];
				ContactFilter2D cf = new ContactFilter2D();
				cf.NoFilter();
				thisCollider.OverlapCollider(cf, overlappingColliders);
				foreach(Collider2D oc in overlappingColliders)
				{
					if ((oc != null) && !(oc.isTrigger))
					{
						Rigidbody2D rb = oc.GetComponentInParent<Rigidbody2D>();
						if (rb != null)
						{
							passable = false;
							//If rigidbody is static, make unchangeable
							if (rb.bodyType.Equals(RigidbodyType2D.Static))
							{

								unchangeable = true;
								break;
							}
						}
					}
				}
			}
			else if (npcCollider is CircleCollider2D)
			{
				//Sets collider to NPC's collider
				thisCollider = gameObject.AddComponent<CircleCollider2D>();
				thisCollider.isTrigger = true;
				((CircleCollider2D)thisCollider).radius  = ((CircleCollider2D)npcCollider).radius * (npcCollider.gameObject.transform.localScale.x);

				//Check if colliding with anything
				Collider2D[] overlappingColliders = new Collider2D[20];
				ContactFilter2D cf = new ContactFilter2D();
				cf.NoFilter();
				thisCollider.OverlapCollider(cf, overlappingColliders);
				foreach (Collider2D oc in overlappingColliders)
				{
					if ((oc != null) && !(oc.isTrigger))
					{
						Rigidbody2D rb = oc.GetComponentInParent<Rigidbody2D>();
						if (rb != null)
						{
							passable = false;
							//If rigidbody is static, make unchangeable
							if (rb.bodyType.Equals(RigidbodyType2D.Static))
							{
								unchangeable = true;
								break;
							}
						}
					}
				}
			}
			else
			{
				print("Vertex " + name + " is missing a valid collider.");
				unchangeable = true;
			}
			if (passable && known)
			{
				sr.color = Color.green;
			}
			else
			{
				sr.color = Color.red;
			}
		}
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if (!unchangeable)
		{
			passable = false;
			sr.color = Color.red;
		}
	}

	void OnTriggerExit2D(Collider2D other)
	{
		if (!unchangeable)
		{
			passable = true;
			if (known)
				sr.color = Color.green;
		}
	}

	void OnDrawGizmosSelected()
	{
		if (debug)
		{
			if (north != null)
			{
				Gizmos.color = north.GetComponent<SpriteRenderer>().color;
				Gizmos.DrawLine(transform.position, north.transform.position);
			}
			if (east != null)
			{
				Gizmos.color = east.GetComponent<SpriteRenderer>().color;
				Gizmos.DrawLine(transform.position, east.transform.position);
			}
			if (south != null)
			{
				Gizmos.color = south.GetComponent<SpriteRenderer>().color;
				Gizmos.DrawLine(transform.position, south.transform.position);
			}
			if (west != null)
			{
				Gizmos.color = west.GetComponent<SpriteRenderer>().color;
				Gizmos.DrawLine(transform.position, west.transform.position);
			}
		}
	}

	public GameObject[] getAdjacentVertices()
	{
		GameObject[] adjacentVertices = { north, east, south, west };
		return adjacentVertices;
	}
}
