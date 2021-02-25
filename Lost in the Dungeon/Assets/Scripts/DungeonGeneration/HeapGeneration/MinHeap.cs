using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinHeap
{
    public int MAX_ZONES { get; private set; }
    private int COUNT_ZONES;
    private Zone root;
    private Queue<Zone> brotherZones;
    private Queue<Zone> levelChildren;

    public MinHeap(int MAX)
    {
        MAX_ZONES = MAX;
        COUNT_ZONES = 1; //The root starts in the tree right from the start
        root = new Zone(0, 1, 3);
        brotherZones = new Queue<Zone>();
        levelChildren = new Queue<Zone>();
        levelChildren.Enqueue(root);
        levelChildren.Enqueue(null);
    }

    public Zone getRoot() { return root; }

    public void AddZone(int zoneValue) //We are sure children will be inserted in ascending order
    {
        if (COUNT_ZONES == MAX_ZONES) return;

        Zone newZone = new Zone(zoneValue, 1, 3);
        Zone insertZone = GetInsertZone();

        if (insertZone.leftChild == null)
        {
            insertZone.setLeftChild(newZone);
            AddToQueue(newZone);
        }
        else if (insertZone.hasRightChild() && insertZone.rightChild == null) //If not, check if we can insert the new zone as the right child of the current zone
        {
            insertZone.setRightChild(newZone);
            AddToQueue(newZone);
        }
        CheckLevelCompleted(insertZone);
    }

    private void AddToQueue(Zone z)
    {
        COUNT_ZONES++;
        if (COUNT_ZONES < MAX_ZONES)
        {
            levelChildren.Enqueue(z);
            brotherZones.Enqueue(z);
        }
    }

    private Zone GetInsertZone()
    {
        if (levelChildren.Peek() == null)
        {
            ConnectBrotherZones();//We create conections between brother nodes(randomly)
            levelChildren.Dequeue();
            levelChildren.Enqueue(null);
        }
        return levelChildren.Peek();
    }

    private void CheckLevelCompleted(Zone insertZone)
    {
        if (insertZone.leftChild != null && ((!insertZone.hasRightChild()) || (insertZone.hasRightChild() && insertZone.rightChild != null))) //If all possible children of the current zone are filled
            levelChildren.Dequeue(); //Move on to the next zone to fill its children
    }

    private void ConnectBrotherZones() //Each zone can be connected to the one it is next to from left to right 
    {
        if (brotherZones.Count >= 2)
        {
            int makeConexion;
            Zone currentBrother;
            for (int i = 0; i < brotherZones.Count - 1; i++)
            {
                currentBrother = brotherZones.Dequeue();
                makeConexion = Random.Range(0, 1);
                if (makeConexion == 0)
                    currentBrother.setBrother(brotherZones.Peek());
            }
            brotherZones.Dequeue(); //We remove the last element so the queue is empty
        }
        else
            brotherZones.Clear();
    }

    public void PrintHeap(int index, Zone currentZone)
    {
        if (index <= COUNT_ZONES)
        {
            Debug.Log("- Zone: " + currentZone.difficulty);
            string leftChild = currentZone.leftChild == null ? "NO LEFT" : currentZone.leftChild.difficulty.ToString();
            Debug.Log("- LeftChild: " + leftChild);
            string rightChild = currentZone.rightChild == null ? "NO RIGHT" : currentZone.rightChild.difficulty.ToString();
            Debug.Log("- RightChild: " + rightChild);
            string brother = currentZone.brother == null ? "NO BROTHER" : currentZone.brother.difficulty.ToString();
            Debug.Log("- Brother: " + brother);
            Debug.Log("-----------------------------------");

            if (currentZone.leftChild != null)
                PrintHeap(++index, currentZone.leftChild);
            if (currentZone.hasRightChild() && currentZone.rightChild != null)
                PrintHeap(++index, currentZone.rightChild);
        }
    }
}
