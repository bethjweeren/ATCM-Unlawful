using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinder : MonoBehaviour
{

	// Use this for initialization
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{

	}

	//Sources:
	//https://gamedev.stackexchange.com/questions/58963/pathfinding-with-2d-non-grid-based-movement-over-uniform-terrain
	//http://aigamedev.com/open/tutorials/theta-star-any-angle-paths/
	//http://www.jair.org/media/2994/live-2994-5259-jair.pdf
	//https://en.wikipedia.org/wiki/Theta*#Pseudocode
	//Nodes:
	//http://www.jgallant.com/nodal-pathfinding-in-unity-2d-with-a-in-non-grid-based-games/
	//Especially the last link
	//Theta* is supposedly better than A* so that what I'mma use -Sharon
	//NPCs use Vector3s, so input the (X, Y) values as Vector2s
	/*void theta*( start,  goal)
	{
		//This is the main loop:
		currentShortestDistance(start) = 0;
		
		parent(start) = start;

		// Initializing open and closed sets. The open set is initialized 
		// with the start node and an initial cost

		-


		function theta*(start, goal)
		// This main loop is the same as A*
		gScore(start) := 0
		parent(start) := start
		// Initializing open and closed sets. The open set is initialized 
		// with the start node and an initial cost
		open := {}
		open.insert(start, gScore(start) + heuristic(start))
		// gScore(node) is the current shortest distance from the start node to node
		// heuristic(node) is the estimated distance of node from the goal node
		// there are many options for the heuristic such as Euclidean or Manhattan 
		closed := {}
		while open is not empty
			s := open.pop()
			if s = goal
				return reconstruct_path(s)
			closed.push(s)
			for each neighbor of s
			// Loop through each immediate neighbor of s
				if neighbor not in closed
					if neighbor not in open
						// Initialize values for neighbor if it is 
						// not already in the open list
						gScore(neighbor) := infinity
						parent(neighbor) := Null
					update_vertex(s, neighbor)
		return Null
            
    
		function update_vertex(s, neighbor)
			// This part of the algorithm is the main difference between A* and Theta*
			if line_of_sight(parent(s), neighbor)
				// If there is line-of-sight between parent(s) and neighbor
				// then ignore s and use the path from parent(s) to neighbor 
				if gScore(parent(s)) + c(parent(s), neighbor) < gScore(neighbor)
					// c(s, neighbor) is the Euclidean distance from s to neighbor
					gScore(neighbor) := gScore(parent(s)) + c(parent(s), neighbor)
					parent(neighbor) := parent(s)
					if neighbor in open
						open.remove(neighbor)
					open.insert(neighbor, gScore(neighbor) + heuristic(neighbor))
			else
				// If the length of the path from start to s and from s to 
				// neighbor is shorter than the shortest currently known distance
				// from start to neighbor, then update node with the new distance
				if gScore(s) + c(s, neighbor) < gScore(neighbor)
					gScore(neighbor) := gScore(s) + c(s, neighbor)
					parent(neighbor) := s
					if neighbor in open
						open.remove(neighbor)
					open.insert(neighbor, gScore(neighbor) + heuristic(neighbor))

		function reconstruct_path(s)
			total_path = {s}
			// This will recursively reconstruct the path from the goal node 
			// until the start node is reached
			if parent(s) != s
				total_path.push(reconstruct_path(parent(s)))
			else
				return total_path
		}*/
}