using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


/*
 * Sharon Lougheed
 * 
 * I originally wrote this in Java for a project for Algorithms and Data Structures class.
 * 
 */

public class BinaryMinHeap : MonoBehaviour
{
    private static int DEFAULT_CAPACITY = 100;

    private int currentSize; //Number of elements in heap
    public GameObject[] array; //The heap array

    void Start()
    {
        currentSize = 0;
        array = new GameObject[DEFAULT_CAPACITY + 1];
    }

    //Inserts into priority queue, maintaining heap order, allowing duplicates
    public void Insert(GameObject x)
    {
        if (currentSize == array.Length - 1)
            EnlargeArray(array.Length * 2 + 1);

        // Percolate up
        int hole = ++currentSize;
        for (array[0] = x; x.GetComponent<Vertex>() < array[hole / 2].GetComponent<Vertex>(); hole /= 2)
        {
            array[hole] = array[hole / 2];
        }
        array[hole] = x;
    }

    //Removes from queue and rebuilds to maintain order
    public void Remove(GameObject x)
    {
        bool foundIt = false;
        int howMany = 0;
        for (int i = 1; i <= currentSize; i++)
        {
            if (array[i].Equals(x))
            {
                foundIt = true;
                howMany = howMany + 1;
            }
            else
            {
                if (foundIt)
                {
                    array[i - howMany] = array[i];
                    array[i] = null;
                }
            }
        }
        --currentSize;
        BuildHeap();
    }

    //Adds the event to the next available array slot, no percolation
    public void Add(GameObject x)
    {
        if (currentSize == array.Length - 1)
            EnlargeArray(array.Length * 2 + 1);

        int hole = ++currentSize;
        array[hole] = x;
    }

    //Enlarges array to new size
    private void EnlargeArray(int newSize)
    {
        GameObject[] old = array;
        array = new GameObject[newSize];
        for (int i = 0; i < old.Length; i++)
            array[i] = old[i];
    }

    //Finds smallest item in priority queue.
    public GameObject FindMin()
    {
        if (IsEmpty())
            return null;
        return array[1];
    }

    //Remove the smallest item from the priority queue and return it
    //Or returns null if empty
    public GameObject DeleteMin()
    {
        if (IsEmpty())
            return null;

        GameObject minItem = FindMin();
        array[1] = array[currentSize--];
        PercolateDown(1);

        return minItem;
    }

    //Establish heap order property from an arbitrary
    //arrangement of items. Runs in linear time.
    public void BuildHeap()
    {
        for (int i = currentSize / 2; i > 0; i--)
            PercolateDown(i);
    }

    //Return true if priority queue is logically empty, else false
    public bool IsEmpty()
    {
        return currentSize == 0;
    }

    //Make priority queue logically empty
    public void MakeEmpty()
    {
        currentSize = 0;
    }

    //Internal method to percolate down in the heap.
    private void PercolateDown(int hole)
    {
        int child;
        GameObject tmp = array[hole];

        for (; hole * 2 <= currentSize; hole = child)
        {
            child = hole * 2;
            if (child != currentSize && array[child + 1].GetComponent<Vertex>() < (array[child]).GetComponent<Vertex>())
                child++;
            if (array[child].GetComponent<Vertex>() < tmp.GetComponent<Vertex>())
                array[hole] = array[child];
            else
                break;
        }
        array[hole] = tmp;
    }

    public bool Contains(GameObject obj)
    {
        for (int i = 1; i <= currentSize; i++)
        {
            if (array[i].Equals(obj))
            {
                return true;
            }
        }
        return false;
    }

    //Prints the array
    public void PrintArray()
    {
        for (int i = 1; i <= currentSize; i++)
        {
            if (array[i] != null)
            {
                print("bmh vertex: " + i + " " + array[i]);
            }
        }
    }
}
