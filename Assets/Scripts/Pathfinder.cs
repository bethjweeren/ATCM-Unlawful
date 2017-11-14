using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Priority_Queue;

/*
* Uses https://github.com/BlueRaja/High-Speed-Priority-Queue-for-C-Sharp
* Because, unlike Java, C# does not have a built-in priority queue,
* and I ain't got no time to start writing prioritiy queue classes.
* Plus my priority queue class would probably be a copy/paste from a
* textbook or something.
* Rest assured, though, BlueRaja's Priority Queue is released under the
* MIT License, so "it's free to use in all cases: free for personal
* or commercial use, in free or paid software, open or closed source.
* No strings attached!"
* Not really sure I imported it properly though, cuz NuGet wouldn't work,
* but copying some NuGet package files into the assets folder seemed to work.
* -Sharon
*/
public class Pathfinder : MonoBehaviour
{
	SimplePriorityQueue<Vertex> priorityQueue; //U in pseudocode, Maintains the priorities of the vertices
	public GameObject vertexPrefab;
	public Collider2D npcCollider;
	public float distanceBetweenVertices = 0.5f;
	public bool debug = true;

	float keyModifier; //km in pseudocode, accounts for cost changes
	HashSet<GameObject> graph = new HashSet<GameObject>(); //S in pseudocode, The finite set of vertices of the graph; I chose HashSet cuz the pseudocode just said "Set" and this was close enough I guess.
	GameObject[,] vertexArray;
	int numVX, numVY;
	public RectTransform townRectangle; //A rectangle that covers the entire play area; determines how big to make the graph.

	// Use this for initialization
	void Start()
	{
		if (vertexPrefab.GetComponent<Vertex>() == null)
		{
			print("ERROR: Pathfinder.cs needs a vertex prefab object containing the Vertex script!");
		}
		vertexPrefab.GetComponent<Vertex>().npcCollider = npcCollider;
		
		numVX = Mathf.FloorToInt(townRectangle.rect.width / distanceBetweenVertices); //Number of vertices along the x side / width of the rectangle
		numVY = Mathf.FloorToInt(townRectangle.rect.height / distanceBetweenVertices); //Number of vertices along the y side / height of the rectangle
		vertexArray = new GameObject[numVY, numVX];
		MakeGraph();
	}

	// Update is called once per frame
	void Update()
	{

	}

	void MakeGraph()
	{
		print("numVX " + numVX);
		print("numVY " + numVY);
		// (0,0) is the bottom-left of the rectangle
		for (int row = 0; row < numVY; row++)
		{
			for (int col = 0; col < numVX; col++)
			{
				GameObject vp = Instantiate(vertexPrefab, new Vector3(townRectangle.rect.xMin + townRectangle.localPosition.x + (col * distanceBetweenVertices), townRectangle.rect.yMin + townRectangle.localPosition.y + (row * distanceBetweenVertices), 1), Quaternion.identity, transform);
				vp.GetComponent<Vertex>().debug = debug;
				vp.GetComponent<Vertex>().gridX = col;
				vp.GetComponent<Vertex>().gridY = row;
				print("row " + row);
				print("col " + col);
				
				vertexArray[row, col] = vp;
				if (col > 0)
				{
					vp.GetComponent<Vertex>().west = vertexArray[row, col - 1];
					vertexArray[row, col - 1].GetComponent<Vertex>().east = vp;
					//Debug.DrawRay(vp.transform.position, vp.GetComponent<Vertex>().west.transform.position, vp.GetComponent <SpriteRenderer>().color, 100, false);
				}
				if (row > 0)
				{
					vp.GetComponent<Vertex>().south = vertexArray[row - 1, col];
					vertexArray[row - 1, col].GetComponent<Vertex>().north = vp;
					//Debug.DrawRay(vp.transform.position, vp.GetComponent<Vertex>().south.transform.position, vp.GetComponent<SpriteRenderer>().color, 100, false);
				}
				/*
				if (col > 0)
				{
					print(vertexArray[row, col - 1]);
					vp.GetComponent<Vertex>().SetWestVertex(vertexArray[row, col - 1]);
					vertexArray[row, col - 1].GetComponent<Vertex>().SetEastVertex(vp);
				}
				if (row > 0)
				{
					vp.GetComponent<Vertex>().SetSouthVertex(vertexArray[row - 1, col]);
					vertexArray[row - 1, col].GetComponent<Vertex>().SetNorthVertex(vp);
				}
				*/
			}
		}
		/*
		for (float newY = townRectangle.rect.yMin + townRectangle.localPosition.y; newY < townRectangle.rect.yMax + townRectangle.localPosition.y; newY += distanceBetweenVertices)
		{
			for (float newX = townRectangle.rect.xMin + townRectangle.localPosition.x; newX < townRectangle.rect.xMax + townRectangle.localPosition.x; newX += distanceBetweenVertices)
			{
				GameObject vp = Instantiate(vertexPrefab, new Vector3(newX, newY, 1), Quaternion.identity);
				graph.Add(vp);
			}
		}


		/*
		RaycastHit2D hit;
		hit = Physics2D.Raycast(Position, new Vector2(-1, 0), distanceBetweenVertices);
		//Make connections between vertices/nodes
		for (float newY = townRectangle.rect.yMin; newY < townRectangle.rect.yMax; newY += distanceBetweenVertices)
		{
			for (float newX = townRectangle.rect.xMin; newX < townRectangle.rect.xMax; newX += distanceBetweenVertices)
			{
				
			}
		}
		*/
	}

	/*
	public static Object prefab = Resources.Load("Prefabs/NPCMovement/Node");

	public static Node CreateNode()
	{
		GameObject newObject = Instantiate(prefab) as GameObject;
		Node yourObject = newObject.GetComponent<YourComponent>();
		//do additional initialization steps here
		return yourObject;
	}
	*/

	/*

	//https://cstheory.stackexchange.com/questions/11855/how-do-the-state-of-the-art-pathfinding-algorithms-for-changing-graphs-d-d-l
	//I looked at this C++ implementation to understand the pdf: https://code.google.com/archive/p/dstarlite/
	//However I didn't copy any code over. My implementation is much more similar to the original pseudocode:
	//http://idm-lab.org/bib/abstracts/papers/aaai02b.pdf
	//X.Sun, W.Yeoh and S.Koenig. Moving Target D* Lite. In Proceedings of the International Joint Conference on Autonomous Agents and Multiagent Systems (AAMAS), 67-74, 2010. 
	//
	//D*-Lite (optimized version)
	//Dynamic simple pathfinding (45 or 90 degree angles), faster and less complicated than D*
	
	Vector2 CalculateKey(Vertex vertex)
	{
		//g = estimate of start distance g* of the vertex, RHS = one-step lookahead value
		//According to Wikipedia, "A heuristic function, also called simply a heuristic, is a function that ranks alternatives in search algorithms
		//at each branching step based on available information to decide which branch to follow. For example, it may approximate the exact solution."
		return new Vector2((Mathf.Min(GetStartDistanceEstimate(vertex), GetRHS(vertex)) + heuristic(startVertex, vertex) + keyModifier), (Mathf.Min(GetStartDistanceEstimate(vertex), getRHS(vertex))));
	}

	void Initialize()
	{
		priorityQueue = new SimplePriorityQueue<Vertex>();
		keyModifier = 0;
		//Set S finiteSetofVerticesOfTheGraph 
		foreach (Vertex vertex in graph)
		{
			SetRHS(vertex, float.PositiveInfinity);
			GetStartDistanceEstimate(vertex, float.PositiveInfinity);
		}
		setRHS(goalVertex, 0);
		priorityQueue.Insert(goalVertex, new Vector2(heuristic(startVertex, goalVertex),0))
	}

	void UpdateVertex(vertex)
	{
		if(!anyVertex.Equals(startVertex))
			setRHS(anyVertex, Mathf.Min())
	}

	void ComputeShortestPath()
	{
		while (priorityQueue.First.GetKey() < CalculateKey(startVertex) || getRHS(startVertex)
		{
			anyVertex = priorityQueue.
		}
	}

	//g(s) in pseudocode
	//Yeah, it's a long method name, but at least it's descriptive!
	float GetStartDistanceEstimate(Vertex vertex)
	{

	}

	//rhs(s) in pseudocode
	//Right-hand-side value
	//The D*Lite paper defines the rhs as a
	//"one-step lookahead value based on the g-values and
	//thus better informed than the g-values"
	//g = estimate of start distance g* of the vertex
	//Equal to the cost to the parent of a node plus the cost to travel to that node
	//By comparing this value to the cost to the node we can detect inconsistencies
	//anyVertex is u in pseudocode
	float GetRHS(Vertex vertex)
	{
		if (vertex.Equals(anyVertex))
			return 0;
		else
		{
			return Mathf.Min(,)
		}
	}
	*/
}