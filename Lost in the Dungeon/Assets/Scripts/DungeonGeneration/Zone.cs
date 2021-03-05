using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zone : MonoBehaviour
{
    //Prefab related references
    [Header("Openings")]
    public GameObject topMidOpening;
    public GameObject topRightOp;
    public GameObject topRightLongOp;
    public GameObject topLeftOp;
    public GameObject topLeftLongOp;
    public GameObject downOp;
    public GameObject rightOp;
    public GameObject leftOp;

    [Header("Roads")]
    public GameObject topMidRoad;
    public GameObject topRightRoad;
    public GameObject topRightLongRoad;
    public GameObject topLeftRoad;
    public GameObject topLeftLongRoad;
    public GameObject downRoad;
    public GameObject rightRoad_firstLevel;
    public GameObject rightRoad_near;
    public GameObject rightRoad_mid;
    public GameObject rightRoad_far;
    public GameObject leftRoad;

    [Header("Points")]
    public Transform topRightPoint;
    public Transform topRightLongPoint;
    public Transform topLeftPoint;
    public Transform topLeftLongPoint;

    //Atributes
    public int difficulty { get; private set; }
    public Zone brother { get; private set; }
    public Zone leftChild { get; private set; }
    public Zone rightChild { get; private set; }
    public int children { get; private set; }

    //Getters and setters
    public int getValue() { return difficulty; }
    public void setBrother(Zone z) => brother = z;
    public void setLeftChild(Zone z) => leftChild = z;
    public void setRightChild(Zone z) => rightChild = z;

    //Methods
    public bool isLeaf() { return children == 0; }
    public bool hasRightChild() { return children == 2; }
    public void Initialize(int v, int min, int max)
    {
        difficulty = v;
        System.Random r = new System.Random();
        children = r.Next(min, max); //random in the range [min, max)
    }
}

