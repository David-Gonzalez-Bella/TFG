using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateDungeon : MonoBehaviour
{
    private MinHeap heap = new MinHeap(10);

    void Start()
    {
        heap.AddZone(1);
        heap.AddZone(2);
        heap.AddZone(3);
        heap.AddZone(4);
        heap.PrintHeap(1, heap.getRoot());
    }
}
