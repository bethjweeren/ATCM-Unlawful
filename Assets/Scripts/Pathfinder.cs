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
	float keyModifier; //km in pseudocode, accounts for cost changes
	HashSet<Vertex> graph; //S in pseudocode, The finite set of vertices of the graph; I chose HashSet cuz the pseudocode just said "Set" and this was close enough I guess.

	// Use this for initialization
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{

	}


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
	//I assume this stands for right-hand side, but I dunno...
	//The D*Lite paper defines the rhs as a
	//"one-step lookahead value based on the g-values and
	//thus better informed than the g-values"
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