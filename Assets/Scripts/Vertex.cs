using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//A vertex/node on the graph that determines whether an NPC can move at a particular location
public class Vertex : MonoBehaviour
{
	public bool debug = true; //If true, enable sprite renderer, else disable
	public Collider2D npcCollider; //The NPC's collider. Needed to determine if the vertex is passable.
    public PathfinderGraph graph;
	private Collider2D thisCollider; //The vertex's collider (a copy of the NPC's collider)
    public bool passable = true; //Is this a wall/barrier?
    public bool known = true; //Does the NPC know about this location? Almost identical to passable.
    public bool unchangeable = false; //Will the passable or known variables stay the same?
	public float weight = 1; //You'd think edges would have weight, but no, I'm putting it in the nodes. Every edge that *points* to this node has this weight.
					  //A higher weight means that the NPC is less likely to walk on this node because it's considered "further away" or more difficult to traverse
					  //This is useful if you want the NPCs to stick to the roads or something. Could probably be set with an area or something.
	//The following GameObjects NEED to have the Vertex script
	public GameObject north;
	public GameObject east;
	public GameObject south;
	public GameObject west;
	public int gridX, gridY;
	public SpriteRenderer sr;
    public Vector2 priority = new Vector2(-1,-1);

    public List<GameObject> pathfinders; //The index of each pathfinder corresponds to the rhs and g values below
    public List<float> rhs; //RHS = right-hand side / one-step lookahead value
    public List<float> g; //g = estimate of start distance g* of the vertex

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
            Pathfinder pf = other.gameObject.GetComponent<Pathfinder>();
            if (pf != null)
            {
                if (Vector3.Distance(other.gameObject.transform.position, transform.position) > pf.closeEnough)
                {
                    pf.startVertexObj = gameObject;
                }
            }
            passable = false;
            graph.graphChanged = true;
            if (!graph.verticesChanged.Contains(gameObject))
            {
                graph.verticesChanged.Add(gameObject);
            }
            if (debug)
			    sr.color = Color.red;
		}
	}

	void OnTriggerExit2D(Collider2D other)
	{
		if (!unchangeable)
		{
			passable = true;
            graph.graphChanged = true;
            if (known && debug)
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

	public GameObject[] GetAdjacentVertices()
	{
		GameObject[] adjacentVertices = { north, east, south, west };
		return adjacentVertices;
	}

    public List<GameObject> GetSuccessors()
    {
        List<GameObject> successors = new List<GameObject>();
        foreach (GameObject adjVertexObj in GetAdjacentVertices())
        {
            if (adjVertexObj != null)
            {
                successors.Add(adjVertexObj);
            }
        }
        return successors;
        /*
        if (passable)
        {
            List<GameObject> successors = new List<GameObject>();
            foreach (GameObject adjVertexObj in GetAdjacentVertices())
            {
                successors.Add(adjVertexObj);
            }
            return successors;
        }
        else
        {
            return null;
        }
        */
    }

    public List<GameObject> GetPredecessors()
    {
        List<GameObject> predecessors = new List<GameObject>();
        foreach(GameObject adjVertexObj in GetAdjacentVertices())
        {
            if (adjVertexObj != null)
            {
                if (adjVertexObj.GetComponent<Vertex>().passable && adjVertexObj.GetComponent<Vertex>().known)
                {
                    predecessors.Add(adjVertexObj);
                }
            }
        }
        return predecessors;
    }

    //Compare priorities
    /*
    public static bool operator ==(Vertex v1, Vertex v2)
    {
        if ((v1.priority.x == v2.priority.x) && (v1.priority.y == v2.priority.y))
            return true;
        else
            return false;
    }

    public static bool operator !=(Vertex v1, Vertex v2)
    {
        if ((v1.priority.x == v2.priority.x) && (v1.priority.y == v2.priority.y))
            return false;
        else
            return true;
    }
    */
    public static bool operator <=(Vertex v1, Vertex v2)
    {
        if ((v1.priority.x < v2.priority.x) || ((v1.priority.x == v2.priority.x) && (v1.priority.y <= v2.priority.y)))
            return true;
        else
            return false;
    }

    public static bool operator >=(Vertex v1, Vertex v2)
    {
        if ((v1.priority.x > v2.priority.x) || ((v1.priority.x == v2.priority[0]) && (v1.priority.y >= v2.priority.y)))
            return true;
        else
            return false;
    }

    public static bool operator <(Vertex v1, Vertex v2)
    {
        if ((v1.priority.x < v2.priority.x) || ((v1.priority.x == v2.priority.x) && (v1.priority.y < v2.priority.y)))
            return true;
        else
            return false;
    }

    public static bool operator >(Vertex v1, Vertex v2)
    {
        if ((v1.priority.x > v2.priority.x) || ((v1.priority.x == v2.priority[0]) && (v1.priority.y > v2.priority.y)))
            return true;
        else
            return false;
    }
}
