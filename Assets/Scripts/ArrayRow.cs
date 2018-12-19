using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//For pathfinding
public class ArrayRow
{
    public Vertex[] rowOfVertices;
    public ArrayRow(int columns)
    {
        rowOfVertices = new Vertex[columns];
    }
}