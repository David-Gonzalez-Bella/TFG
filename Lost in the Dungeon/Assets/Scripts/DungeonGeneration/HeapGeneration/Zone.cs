using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zone
{
    //Atributes
    public int difficulty { get; private set; }
    public Zone brother { get; private set; }
    public Zone leftChild { get; private set; }
    public Zone rightChild { get; private set; }
    public int children { get; private set; }

    //Constructor
    public Zone(int d, int min, int max)
    {
        System.Random r = new System.Random();
        difficulty = d;
        brother = null;
        leftChild = null;
        rightChild = null;
        children = r.Next(min, max); //random in the range [min, max)
    }

    //Getters and setters
    public void setBrother(Zone z) => brother = z;
    public void setLeftChild(Zone z) => leftChild = z;
    public void setRightChild(Zone z) => rightChild = z;

    //Methods
    public bool hasRightChild() { return children == 2; }
}
