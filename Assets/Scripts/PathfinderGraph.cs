using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//The graph that pathfinder.cs uses
public class PathfinderGraph : MonoBehaviour
{
    public GameObject vertexPrefab;
	public Collider2D npcCollider;
	public float distanceBetweenVertices = 0.5f;
	public bool debug = true;
	public bool createOnPlay = true; //Can just copy this object from the previous play so the game doesn't lag every time it starts. After you do that, set this variable to false.
    public RectTransform townRectangle; //A rectangle that covers the entire play area; determines how big to make the graph.

    //The following variables are only public so Pathfinder can access them easily.
    public bool graphChanged;
    public List<GameObject> verticesChanged = new List<GameObject>();
    public GameObject[,] vertexArray; //2D Array
    private int numVX, numVY;

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

        if (vertexArray != null)
        {
            createOnPlay = false;
            print("PathfinderGraph - Automatically disabled createOnPlay due to existing vertexArray.");
        }
        else
        {
            print("PathfinderGraph - vertexArray is null");
        }
        if (createOnPlay)   //If don't already have a graph copied over from a previous play, make a new graph
        {
            print("PathfinderGraph - Making graph...");
            vertexArray = new GameObject[numVY, numVX];
            MakeGraph();
            print("PathfinderGraph - ...done.");
        }
    }

	// Update is called once per frame
	void Update()
	{

	}

    //Create all of the vertices and add them to the 2D array
	void MakeGraph()
	{
		print("numVX " + numVX);
		print("numVY " + numVY);
		// (0,0) is the bottom-left of the rectangle
		for (int row = 0; row < numVY; row++)
		{
			for (int col = 0; col < numVX; col++)
			{
                //Create a new vertex in the graph
                //vp = a new instance of the vertex prefab
				GameObject vp = Instantiate(vertexPrefab, new Vector3(townRectangle.rect.xMin + townRectangle.localPosition.x + (col * distanceBetweenVertices), townRectangle.rect.yMin + townRectangle.localPosition.y + (row * distanceBetweenVertices), 1), Quaternion.identity, transform);
                vp.GetComponent<Vertex>().graph = this;
                vp.GetComponent<Vertex>().debug = debug;
				vp.GetComponent<Vertex>().gridX = col; //Width = number of columns
				vp.GetComponent<Vertex>().gridY = row; //Height = number of rows
				//print("row " + row);
				//print("col " + col);
				
				vertexArray[row, col] = vp;
				if (col > 0)
				{
					vp.GetComponent<Vertex>().west = vertexArray[row, col - 1]; //Set the west vertex gameobject
					vertexArray[row, col - 1].GetComponent<Vertex>().east = vp; //Set the east vertex gameobject
                }
				if (row > 0)
				{
					vp.GetComponent<Vertex>().south = vertexArray[row - 1, col]; //Set the south vertex gameobject
                    vertexArray[row - 1, col].GetComponent<Vertex>().north = vp; //Set the north vertex gameobject
                }
			}
		}
	}

    public GameObject[,] GetVertexArray()
    {
        //I don't know anymore, just trying to get the actual vertices instead of null objects, ugh
        GameObject[,] copyOfVertexArray = new GameObject[vertexArray.GetLength(0), vertexArray.GetLength(1)];
        for (int a = 0; a < vertexArray.GetLength(0); a++)
        {
            for (int b = 0;b < vertexArray.GetLength(1); b++)
            {
                copyOfVertexArray[a, b] = vertexArray[a, b];
            }
        }
        return copyOfVertexArray;
    }
}