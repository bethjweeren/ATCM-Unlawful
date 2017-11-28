 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

//As a backup, I can try an already-made A* algorithm in C# that's under the MIT License https://github.com/SorcerersApprentice/AStar 
public class Pathfinder : MonoBehaviour
{
    public PathfinderGraph pathfinderGraph;
    BinaryMinHeap priorityQueue; //U in pseudocode, Maintains the priorities of the vertices
    public float speed = 3f;
    float keyModifier; //km in pseudocode, accounts for cost changes
    public GameObject startVertexObj, goalVertexObj; //I would just use Vertex instead of GameObject, but I got a ton of annoying errors earlier when doing that with something else, and I don't have the time to mess with that.
    GameObject lastVertexObj;
    private Collider2D npcCollider;
    private Animator animator;
    private float wait;
    public float closeEnough = .05f;
    public float waitBetween = 10;
    private Rigidbody2D rb;
    Vertex nextVertexObj;

    // Use this for initialization
    void Start()
	{
        npcCollider = GetComponent<Collider2D>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        StartCoroutine(BeginPathfinding());
    }

    IEnumerator BeginPathfinding()
    {
        if (startVertexObj == null || goalVertexObj == null)
        {
            yield return new WaitForSeconds(5f);
            StartCoroutine(BeginPathfinding());
        }
        else
        {
            GameObject lastVertexObj = startVertexObj;
            StartCoroutine(Initialize());
            yield return null;
        }
        
    }

    // Update is called once per frame by NPC
    void FixedUpdate()
    {
        if (nextVertexObj == null)
            return;
        if (Vector3.Distance(startVertexObj.transform.position, goalVertexObj.transform.position) > closeEnough)
        {
            if (wait <= 0)
            {
                int directionPicker = UnityEngine.Random.Range(0, 2); //Next direction: East or west? / North or south?
                if (animator.GetInteger("Direction") == 0 || animator.GetInteger("Direction") == 2) //If moving north or south
                {
                    switch (directionPicker)
                    {
                        case 0:
                            animator.SetInteger("Direction", 1); //Animate east sprite
                            rb.velocity = new Vector2(1, 0) * speed;
                            break;
                        case 1:
                            animator.SetInteger("Direction", 3); //Animate west sprite
                            rb.velocity = new Vector2(-1, 0) * speed;
                            break;
                        case 2:
                            if (animator.GetInteger("Direction") == 0) //If moving north
                            {
                                animator.SetInteger("Direction", 2); //Animate south sprite
                                rb.velocity = new Vector2(0, -1) * speed;
                            }
                            else //assume south
                            {
                                animator.SetInteger("Direction", 0); //Animate north sprite
                                rb.velocity = new Vector2(0, 1) * speed;
                            }
                            break;
                    }
                }
                else //Assume moving east or west
                {
                    switch (directionPicker)
                    {
                        case 0:
                            animator.SetInteger("Direction", 0); //Animate north sprite
                            rb.velocity = new Vector2(0, 1) * speed;
                            break;
                        case 1:
                            animator.SetInteger("Direction", 2); //Animate south sprite
                            rb.velocity = new Vector2(0, -1) * speed;
                            break;
                        case 2:
                            if (animator.GetInteger("Direction") == 1) //If moving east
                            {
                                animator.SetInteger("Direction", 3); //Animate west sprite
                                rb.velocity = new Vector2(-1, 0) * speed;
                            }
                            else //assume west
                            {
                                animator.SetInteger("Direction", 1); //Animate east sprite
                                rb.velocity = new Vector2(1, 0) * speed;
                            }
                            break;
                    }
                }
                wait = waitBetween;
            }
            else
            {
                wait--;
            }
        }
    }

    IEnumerator Pathfind()
    {
        if (Vector3.Distance(startVertexObj.transform.position, goalVertexObj.transform.position) > closeEnough)
        {
            /* if (g(sstart) = infinity) then there is no known path */

            startVertexObj = GetMinSuccessorVertex(startVertexObj);
            print("what");
            if (pathfinderGraph.graphChanged)
            {
                pathfinderGraph.graphChanged = false;
                keyModifier = keyModifier + Heuristic(lastVertexObj.GetComponent<Vertex>(), startVertexObj.GetComponent<Vertex>());
                lastVertexObj = startVertexObj;
                foreach (GameObject vertexObj in pathfinderGraph.verticesChanged)
                {
                    vertexObj.GetComponent<Vertex>().passable = !vertexObj.GetComponent<Vertex>().passable;
                    Vertex vertex = vertexObj.GetComponent<Vertex>();
                    int vertexIndex = vertex.pathfinders.IndexOf(gameObject);
                    print("vertex changed = " + vertexObj);
                    foreach (GameObject vertexAdjObj in vertex.GetAdjacentVertices())
                    {
                        Vertex vertexAdj = vertexAdjObj.GetComponent<Vertex>();
                        int vertexAdjIndex = vertexAdj.pathfinders.IndexOf(gameObject);
                        float costOld = Cost(vertexAdj, vertexObj.GetComponent<Vertex>());
                        vertexObj.GetComponent<Vertex>().passable = !vertexObj.GetComponent<Vertex>().passable;

                        if (costOld > Cost(vertexAdj, vertex))
                        {
                            if (!vertexAdjObj.Equals(goalVertexObj))
                            {
                                vertexAdjObj.GetComponent<Vertex>().rhs[vertexAdjIndex] = Mathf.Min(vertexAdj.rhs[vertexAdjIndex], Cost(vertexAdj, vertex) + vertex.g[vertexIndex]);
                            }
                        }
                        else if (vertexAdj.rhs[vertexAdjIndex] == costOld + vertexAdj.g[vertexIndex])
                        {
                            if (!vertexAdjObj.Equals(goalVertexObj))
                            {
                                GameObject minSuccessorVertexObj = GetMinSuccessorVertex(vertexAdjObj);
                                Vertex minSuccessorVertex = minSuccessorVertexObj.GetComponent<Vertex>();
                                int minSuccessorIndex = minSuccessorVertex.pathfinders.IndexOf(gameObject);
                                vertexAdjObj.GetComponent<Vertex>().rhs[vertexAdjIndex] = Cost(vertexAdj, minSuccessorVertex) + minSuccessorVertex.g[minSuccessorIndex];
                            }
                        }
                    }
                }
                print("i am in this one");
                pathfinderGraph.verticesChanged = new List<GameObject>();
                ComputeShortestPath();
            }
        }
        print("i am here");
        priorityQueue.PrintArray();
        yield return new WaitForSeconds(10f);
        StartCoroutine(Pathfind());
        yield return null;
    }

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
        int index = vertex.pathfinders.IndexOf(gameObject);
        //g = estimate of start distance g* of the vertex, RHS = right-hand side / one-step lookahead value
        //According to Wikipedia, "A heuristic function, also called simply a heuristic, is a function that ranks alternatives in search algorithms
        //at each branching step based on available information to decide which branch to follow. For example, it may approximate the exact solution."
        //return [min(g(s); rhs(s)) + h(sstart; s) + km; min(g(s); rhs(s))];
        return new Vector2((Mathf.Min(vertex.g[index], vertex.rhs[index]) + Heuristic(startVertexObj.GetComponent<Vertex>(), vertex) + keyModifier), (Mathf.Min(vertex.g[index], vertex.rhs[index])));
	}

	IEnumerator Initialize()
	{
        priorityQueue = GetComponent<BinaryMinHeap>();
        keyModifier = 0;
        //for all vertices in S, rhs(vertex) = g(vertex) = infinity

        if (pathfinderGraph.GetVertexArray() == null || startVertexObj == null || goalVertexObj == null)
        {
            yield return new WaitForSeconds(5f);
            StartCoroutine(Initialize());
        }
        else
        {
            foreach (GameObject vertexObj in pathfinderGraph.GetVertexArray())
            {
                //Vertex v = vertexObj.GetComponent<Vertex>();
                vertexObj.GetComponent<Vertex>().pathfinders.Add(gameObject);
                vertexObj.GetComponent<Vertex>().rhs.Insert(vertexObj.GetComponent<Vertex>().pathfinders.IndexOf(gameObject), float.PositiveInfinity);
                vertexObj.GetComponent<Vertex>().g.Insert(vertexObj.GetComponent<Vertex>().pathfinders.IndexOf(gameObject), float.PositiveInfinity);
            }
            Vertex gv = goalVertexObj.GetComponent<Vertex>();
            goalVertexObj.GetComponent<Vertex>().rhs[gv.pathfinders.IndexOf(gameObject)] = 0;
            goalVertexObj.GetComponent<Vertex>().priority = new Vector2(Heuristic(startVertexObj.GetComponent<Vertex>(), gv), 0);
            priorityQueue.Insert(goalVertexObj);
            ComputeShortestPath();
            StartCoroutine(Pathfind());
        }
        yield return null;

    }

    //moves from the current vertex s, starting at sstart, to any successor s0 that minimizes c(s; s0 ) + g(s0) until sgoal is reached (ties can be broken arbitrarily)
	void UpdateVertex(GameObject vertexObj)
	{
        Vertex vertex = vertexObj.GetComponent<Vertex>();
        int index = vertex.pathfinders.IndexOf(gameObject);
        float g = vertex.g[index];
        float rhs = vertex.rhs[index];
        bool contained = priorityQueue.Contains(vertexObj);
        //if (g(u) != rhs(u) AND u in U
        //then U.Update(u; CalculateKey(u));
        if (g != rhs && contained)
        {
            vertexObj.GetComponent<Vertex>().priority = CalculateKey(vertex); //Why use GetComponent<Vertex> again? Because I'm paranoid of errors I encountered before when trying to change things inside other scripts, that's why. Will try cleaning up later.
            priorityQueue.BuildHeap(); //Must do this after changing priority to maintain order
        }
        //else if (g(u) != rhs(u) AND u NOT in U
        //then U.Insert(u; CalculateKey(u));
        else if (g != rhs && !contained)
        {
            vertexObj.GetComponent<Vertex>().priority = CalculateKey(vertex);
            priorityQueue.Insert(vertexObj);
        }
        //else if (g(u) = rhs(u) AND u in U
        //then U.Remove(u);
        else if (g == rhs && contained)
        {
            priorityQueue.Remove(vertexObj);
            print("removed a vertex:");
            priorityQueue.PrintArray();
        }
    }

	void ComputeShortestPath()
	{
        print("Does this even get called?");
        Vector2 topKey = priorityQueue.FindMin().GetComponent<Vertex>().priority;
        Vector2 calculatedKey = CalculateKey(startVertexObj.GetComponent<Vertex>());
        Vertex startVertex = startVertexObj.GetComponent<Vertex>();
        int startVertexIndex = startVertex.pathfinders.IndexOf(gameObject); //Each Vertex stores rhs and g values in arrays because there are multiple pathfinders and only one graph, so this index points to the appropriate rhs and g values
        while (((topKey.x < calculatedKey.x) || ((topKey.x == calculatedKey.x) && (topKey.y < calculatedKey.y))) || (startVertex.rhs[startVertexIndex] != startVertex.g[startVertexIndex]))
        {
            GameObject vertexObj = priorityQueue.FindMin();
            Vertex vertex = vertexObj.GetComponent<Vertex>();
            int vertexIndex = vertex.pathfinders.IndexOf(gameObject); //Which index is rhs and g in Vertex object?
            Vector2 oldKey = vertex.priority;
            Vector2 newKey = CalculateKey(vertex);
            //if (kold < knew)
            if ((oldKey.x < newKey.x) || (oldKey.x == newKey.x) && (oldKey.y < newKey.y))
            {
                vertexObj.GetComponent<Vertex>().priority = newKey; //I hate calling GetComponent<Vertex> so often, but that trying to avoid an old error...
                priorityQueue.BuildHeap(); //Have to do this to maintain order after updating a priority
            }
            //else if (g(u) > rhs(u))
            else if (vertex.g[vertexIndex] > vertex.rhs[vertexIndex])
            {
                //g(u) = rhs(u);
                vertexObj.GetComponent<Vertex>().g[vertexIndex] = vertexObj.GetComponent<Vertex>().rhs[vertexIndex]; //*Complains about calling GetComponent<Vertex>() all the friggin time*
                //U.Remove(u);
                priorityQueue.Remove(vertexObj);
                //or all s in Pred(u)
                List<GameObject> predecessors = vertex.GetPredecessors();
                foreach (GameObject preVertexObj in predecessors)
                {
                    Vertex preVertex = preVertexObj.GetComponent<Vertex>();
                    int preVertexIndex = preVertexObj.GetComponent<Vertex>().pathfinders.IndexOf(gameObject); //Which rhs and g?
                    if (!preVertexObj.Equals(goalVertexObj))
                    {
                        //rhs(s) = min(rhs(s); c(s; u) + g(u));
                        preVertexObj.GetComponent<Vertex>().rhs[preVertexIndex] = Mathf.Min(preVertex.rhs[preVertexIndex], (Cost(preVertex, vertex) + vertex.g[vertexIndex]));
                    }
                    UpdateVertex(preVertexObj);
                }
            }
            else
            {
                float gOld = vertex.g[vertexIndex];
                vertexObj.GetComponent<Vertex>().g[vertexIndex] = float.PositiveInfinity;
                List<GameObject> predecessors = vertex.GetPredecessors();
                predecessors.Add(vertexObj);
                foreach (GameObject preVertexObj in predecessors)
                {
                    Vertex preVertex = preVertexObj.GetComponent<Vertex>();
                    int preVertexIndex = preVertexObj.GetComponent<Vertex>().pathfinders.IndexOf(gameObject); //Which rhs and g?
                    if ((preVertex.rhs[preVertexIndex] == Cost(preVertex, vertex) + gOld) || preVertex.Equals(vertex))
                    {
                        if (!preVertexObj.Equals(goalVertexObj))
                        {
                            GameObject minSuccessorVertexObj = GetMinSuccessorVertex(vertexObj);
                            Vertex minSuccessorVertex = minSuccessorVertexObj.GetComponent<Vertex>();
                            int minSuccessorVertexIndex = minSuccessorVertex.pathfinders.IndexOf(gameObject);
                            preVertexObj.GetComponent<Vertex>().rhs[preVertexIndex] = Cost(vertex, minSuccessorVertex) + minSuccessorVertex.g[minSuccessorVertexIndex];
                        }
                    }
                    UpdateVertex(preVertexObj);
                }
            }
            print("printing pq");
            priorityQueue.PrintArray();
            //Do this stuff again cuz I couldn't do it on the while line without making it insanely long and calling GetComponent<Vertex> over and over
            //Honestly, would rather use Vertex instead of GameObject, but, again, trying to avoid stupid errors I ran into a couple weeks ago. Will try to clean up later.
            topKey = priorityQueue.FindMin().GetComponent<Vertex>().priority;
            calculatedKey = CalculateKey(startVertexObj.GetComponent<Vertex>());
        }
	}

    GameObject GetMinSuccessorVertex(GameObject vertexObj)
    {
        Vertex vertex = vertexObj.GetComponent<Vertex>();
        List <GameObject> successors = vertex.GetSuccessors();
        GameObject min = successors[0];
        Vertex minVertex = successors[0].GetComponent<Vertex>();
        int minVertexIndex = minVertex.pathfinders.IndexOf(gameObject);
        float minCost = Cost(vertex, minVertex) + minVertex.g[minVertexIndex];
        for (int i = 1; i< successors.Count; i++)
        {
            Vertex successorVertex = successors[i].GetComponent<Vertex>();
            int successorVertexIndex = successorVertex.pathfinders.IndexOf(gameObject);
            float thisCost = Cost(vertex, successorVertex) + successorVertex.g[successorVertexIndex];
            if (thisCost < minCost)
            {
                min = successors[i];
                minCost = thisCost;
            }
            /*
            if (vertexObjs[i].GetComponent<Vertex>() < min.GetComponent<Vertex>())
                min = vertexObjs[i];
            */

        }
        return min;
    }

    float Cost(Vertex a, Vertex b)
    {
        if (b.passable)
        {
            float xDiff = Mathf.Abs(a.gridX - b.gridX);
            float yDiff = Mathf.Abs(a.gridY - b.gridY);
            return (xDiff + yDiff);
        }
        else
        {
            return float.PositiveInfinity;
        }
    }

    /*
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
	float GetRHS(GameObject vertexObj)
	{
		return vertexObj.GetComponent<Vertex>().rhs;
	}

    void SetRHS(GameObject vertexObj)
    {
        if (vertexObj.Equals(startVertexObj))
        {
            vertexObj.GetComponent<Vertex>().rhs = 0;
        }
        else
        {
            return Mathf.Min(,)

        }
    }
    */

    /* void Dstar::getSucc(state u,list<state> &s)
     * --------------------------
     * Returns a list of successor states for state u, since this is an
     * 8-way graph this list contains all of a cells neighbours. Unless
     * the cell is occupied in which case it has no successors. 
     */

    /* void Dstar::getPred(state u,list<state> &s)
     * --------------------------
     * Returns a list of all the predecessor states for state u. Since
     * this is for an 8-way connected graph the list contails all the
     * neighbours for state u. Occupied neighbours are not added to the
     * list.
     */

    //Modified from Dstar.cpp
    //James Neufeld (neufeld@cs.ualberta.ca)
    /* double Dstar::heuristic(state a, state b)
     * --------------------------
     * Pretty self explanitory, the heristic we use is the 8-way distance
     * scaled by a constant C1 (should be set to <= min cost).
     */
    float Heuristic(Vertex a, Vertex b)
    {
        float C1 = 1;      // cost of an unseen cell
        return FourCondist(a, b) * C1;
    }

    //Modified from Dstar.cpp to be 4-way distance because we have 90 degree movements not 45
    //James Neufeld (neufeld@cs.ualberta.ca)
    /* double Dstar::eightCondist(state a, state b) 
     * --------------------------
     * Returns the 8-way distance between state a and state b.
     */
    float FourCondist(Vertex a, Vertex b)
    {
        return (Mathf.Abs(a.gridX - b.gridX) + Mathf.Abs(a.gridY - b.gridY));
        /*
        float temp;
        float min = Mathf.Abs(a.gridX - b.gridX);
        float max = Mathf.Abs(a.gridY - b.gridY);
        if (min > max)
        {
            temp = min;
            min = max;
            max = temp;
        }
        return ((Mathf.Sqrt(2) - 1.0f) * min + max);
        */
    }
}