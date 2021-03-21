using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonGenerator : MonoBehaviour
{
    //References
    public Zone zonePrefab;

    //Instantiation positions
    public Queue<Vector2> spawnPositions;

    //Tree generation related atributes
    [SerializeField]
    private int MAX_LEVELS;
    private int COUNT_LEVELS = 1;
    private Zone root;
    private Queue<Zone> brotherZones;
    private Queue<Zone> levelChildren;
    private int maxExpand = 4;

    //[DEBUG]
    //public Zone[] brotherZonesCopy;
    //public Vector2[] spawnPositionsCopy;
    //public Zone[] levelChildrenCopy;

    private void Start()
    {
        InitializeTree();
    }

    //[DEBUG]
    //private void Update()
    //{
    //    brotherZonesCopy = brotherZones.ToArray();
    //    spawnPositionsCopy = spawnPositions.ToArray();
    //    levelChildrenCopy = levelChildren.ToArray();
    //}

    private void InitializeTree()
    {
        root = Instantiate(zonePrefab, Vector3.zero, Quaternion.identity, GetComponentInChildren<Grid>().transform);
        root.Initialize(0, 2, 2); //The root zone will always have 2 children

        spawnPositions = new Queue<Vector2>();
        brotherZones = new Queue<Zone>();
        levelChildren = new Queue<Zone>();

        levelChildren.Enqueue(root);
        levelChildren.Enqueue(null);

        spawnPositions.Enqueue(root.topLeftLongPoint.position);
        spawnPositions.Enqueue(root.topRightLongPoint.position);

        EnableRootRoads();
    }

    public void AddZone(int zoneValue) //We are sure children will be inserted in ascending order
    {
        if (COUNT_LEVELS == MAX_LEVELS) return;

        Zone insertLevel = levelChildren.Peek();
        Zone newZone = Instantiate(zonePrefab, spawnPositions.Dequeue(), Quaternion.identity, GetComponentInChildren<Grid>().transform);
        ExpandLevel(insertLevel, newZone, zoneValue);

        brotherZones.Enqueue(newZone);
        CheckLevelCompleted(insertLevel); //After each node is inserted, we check if we hace completed that level of the tree
    }

    private void ExpandLevel(Zone insertLevel, Zone newZone, int zoneValue)
    {
        int newZonePosX = (int)newZone.transform.position.x;
        int parentPosX = (int)insertLevel.transform.position.x;

        if (insertLevel != root)
        {
            if (maxExpand == 0 || COUNT_LEVELS == MAX_LEVELS - 1)
            {
                newZone.Initialize(zoneValue, 0, 0); //0 children
            }
            else
                AddChildren(newZone);

            if (insertLevel.leftChild == null)
                insertLevel.setLeftChild(newZone);
            else if (insertLevel.hasRightChild() && insertLevel.rightChild == null)
                insertLevel.setRightChild(newZone);
        }
        else
        {
            newZone.Initialize(zoneValue, 2, 2);

            if (insertLevel.leftChild == null)
            {
                EnableFirstLevelRoads(newZone, 0);
                insertLevel.setLeftChild(newZone);
            }

            else if (insertLevel.hasRightChild() && insertLevel.rightChild == null)
            {
                EnableFirstLevelRoads(newZone, 1);
                insertLevel.setRightChild(newZone);
            }
            EnqueueBeginSpawnPoints(newZone);
            levelChildren.Enqueue(newZone);
        }

        //Enable the proper opening depending on the parent's enabled roads towards its children
        if ((insertLevel.rightChildRoad_near.activeSelf ||
            insertLevel.rightChildRoad_far.activeSelf) &&
            newZonePosX > parentPosX)
            newZone.rightChildOp.SetActive(false);

        else if ((insertLevel.leftChildRoad_near.activeSelf ||
            insertLevel.leftChildRoad_far.activeSelf) &&
            newZonePosX < parentPosX)
            newZone.leftChildOp.SetActive(false);

        else
            newZone.downOp.SetActive(false);
    }

    private void CheckLevelCompleted(Zone insertZone)
    {
        if (insertZone.leftChild != null && ((!insertZone.hasRightChild()) || (insertZone.hasRightChild() && insertZone.rightChild != null))) //If all possible children of the current zone are filled
        {
            levelChildren.Dequeue(); //Move on to the next zone to fill its children
            if (levelChildren.Peek() == null)
            {
                if (insertZone != root && COUNT_LEVELS != MAX_LEVELS - 1)
                {
                    int i = 0;
                    Zone[] b = brotherZones.ToArray();

                    //If we have expanded less thatn two zones in this level of the tree the tree (wich means it will end here) we assign children to the current brothers
                    while (maxExpand > 2)
                    {
                        if (i == 0) //Clear spawn positions, since a new children assignment will take place
                        {
                            //ClearChildren(b);
                            spawnPositions.Clear();
                            maxExpand = 4;
                        }
                        AddChildren(b[i]);
                        i = (i + 1) % b.Length;
                    }
                    EnqueueExpandSpawnPoints(b); //We wiil enqueue the necessary spawn points, and we will paint the roads that conect each brother with its children
                }
                //We create conections between brother nodes and prepare everything towards the next zone adition 
                COUNT_LEVELS++; //A new level has been completed
                ConnectBrotherZones();
                levelChildren.Dequeue();
                levelChildren.Enqueue(null);
                maxExpand = 4;
            }
        }
    }

    private void AddChildren(Zone current)
    {
        if (maxExpand > 1)
        {
            current.Initialize(current.getValue(), 0, 2); //0, 1, or 2 children
            if (!current.isLeaf())
            {
                maxExpand--;
                if (current.hasRightChild())
                    maxExpand--;
            }
        }
        else if (maxExpand > 0)
        {
            current.Initialize(current.getValue(), 0, 1); //0 or 1 child
            if (!current.isLeaf())
                maxExpand--;
        }
    }

    private void ConnectBrotherZones() //Each zone can be connected to the one it is next to from left to right 
    {
        if (brotherZones.Count >= 2)
        {
            int makeConexion;
            Zone current;
            Zone brother;
            int distance;
            while (brotherZones.Count > 1)
            {
                current = brotherZones.Dequeue();
                makeConexion = Random.Range(0, 1);
                if (makeConexion == 0)
                {
                    brother = brotherZones.Peek();
                    distance = (int)(brother.transform.position.x - current.transform.position.x);
                    switch (distance)
                    {
                        case 32:
                            current.rightRoad_near.SetActive(true);
                            break;
                        case 64:
                            current.rightRoad_mid.SetActive(true);
                            break;
                        case 48:
                            current.rightRoad_firstLevel.SetActive(true);
                            break;
                        default:
                            current.rightRoad_far.SetActive(true);
                            break;
                    }
                    current.rightOp.SetActive(false);
                    brother.leftOp.SetActive(false);

                    current.setBrother(brother);
                }
            }
        }
        brotherZones.Clear();
    }

    private void EnableRootRoads()
    {
        root.topLeftLongOp.SetActive(false);
        root.topLeftLongRoad.SetActive(true);
        root.topRightLongOp.SetActive(false);
        root.topRightLongRoad.SetActive(true);
    }

    private void EnableFirstLevelRoads(Zone zone, int side)
    {
        bool right = zone.hasRightChild();
        if (side == 0)
        {
            zone.topLeftLongOp.SetActive(false);
            zone.topLeftLongRoad.SetActive(true);
            zone.topRightOp.SetActive(!right);
            zone.topRightRoad.SetActive(right);
        }
        else
        {
            zone.topRightLongOp.SetActive(!right);
            zone.topRightLongRoad.SetActive(right);
            zone.topLeftOp.SetActive(false);
            zone.topLeftRoad.SetActive(true);
        }
    }

    private void EnqueueBeginSpawnPoints(Zone zone)
    {
        if (zone.topLeftLongRoad.activeSelf)
            spawnPositions.Enqueue(zone.topLeftLongPoint.position);
        if (zone.topRightRoad.activeSelf)
            spawnPositions.Enqueue(zone.topRightPoint.position);
        if (zone.topLeftRoad.activeSelf)
            spawnPositions.Enqueue(zone.topLeftPoint.position);
        if (zone.topRightLongRoad.activeSelf)
            spawnPositions.Enqueue(zone.topRightLongPoint.position);
    }

    private void EnqueueExpandSpawnPoints(Zone[] zones)
    {
        int count = 0;
        int totalChildren = 0;
        Zone current, next, prev;

        for (int i = 0; i < zones.Length; i++)
        {
            if (!zones[i].isLeaf())
            {
                totalChildren++;
                if (zones[i].hasRightChild())
                    totalChildren++;
            }
        }

        for (int i = 0; i < zones.Length; i++)
        {
            current = zones[i];
            next = i < zones.Length - 1 ? zones[i + 1] : current;
            prev = i > 0 ? zones[i - 1] : current;

            if (!current.isLeaf())
            {
                levelChildren.Enqueue(current);
                switch (current.transform.position.x)
                {
                    case -48:
                        count = EnqueueTopPositions(current, prev, count, 1);
                        break;
                    case -16:
                        if (count == 0 && totalChildren == 4)
                            count = EnqueueTopPositions(current, next, count, -1);
                        else
                            count = EnqueueTopPositions(current, prev, count, 1);
                        break;
                    case 16:
                        if (i == zones.Length - 1 || zones[zones.Length - 1].isLeaf())
                            count = EnqueueTopPositions(current, prev, count, 1);
                        else
                            count = EnqueueTopPositions(current, next, count, -1);
                        break;
                    case 48:
                        count = EnqueueTopPositions(current, next, count, -1);
                        break;
                }
            }
        }
    }

    private int EnqueueTopPositions(Zone current, Zone adjacent, int count, int direction)
    {
        Vector3 top = current.transform.position + new Vector3(0, 20, 0);
        int currPos = (int)current.transform.position.x;

        if (adjacent != current && //The first and the last zone won't have his top position occupied
           (direction == 1 && spawnPositions.Contains(top) || //When the top possition is occupied, then it is moved one place to the right
           (direction == -1 && (adjacent.transform.position.x - current.transform.position.x == 32) && ((currPos == -16 && !current.hasRightChild()) || (currPos == 16 && adjacent.hasRightChild()))))) //If a node placed to the right next to the current node has two children, then the current node will have its top position moved one placed to the left
        {
            top += new Vector3(32 * direction, 0, 0);
            if (direction == 1)
            {
                //Right Near Road
                current.rightChildRoad_near.SetActive(true);
                current.rightChildRoadOp.SetActive(true);
                current.topRightOp.SetActive(false);
            }
            else
            {
                //Left Near Road
                current.leftChildRoad_near.SetActive(true);
                current.leftChildRoadOp.SetActive(true);
                current.topLeftOp.SetActive(false);
            }
        }
        else
        {
            //Mid Road
            current.topMidRoad.SetActive(true);
            current.topMidOpening.SetActive(false);
        }

        if (!current.hasRightChild())
        {
            spawnPositions.Enqueue(top);
            count++;
        }
        else
        {
            if (direction == -1)
            {
                spawnPositions.Enqueue(top + new Vector3(32 * direction, 0, 0));
                spawnPositions.Enqueue(top);
                if (current.leftChildRoad_near.activeSelf)
                    current.leftChildRoad_far.SetActive(true);
                else
                    current.leftChildRoad_near.SetActive(true);
                current.topLeftOp.SetActive(false);
                current.leftChildRoadOp.SetActive(!current.leftChildRoad_far.activeSelf);
            }
            else
            {
                spawnPositions.Enqueue(top);
                spawnPositions.Enqueue(top + new Vector3(32 * direction, 0, 0));
                if (current.rightChildRoad_near.activeSelf)
                    current.rightChildRoad_far.SetActive(true);
                else
                    current.rightChildRoad_near.SetActive(true);
                current.topRightOp.SetActive(false);
                current.rightChildRoadOp.SetActive(!current.rightChildRoad_far.activeSelf);
            }
            count += 2;
        }
        return count;
    }

    public void PrintHeap(int index, Zone currentZone)
    {
        if (index <= COUNT_LEVELS)
        {
            Debug.Log("- Zone: " + currentZone.getValue());
            string leftChild = currentZone.leftChild == null ? "NO LEFT" : currentZone.leftChild.getValue().ToString();
            Debug.Log("- LeftChild: " + leftChild);
            string rightChild = currentZone.rightChild == null ? "NO RIGHT" : currentZone.rightChild.getValue().ToString();
            Debug.Log("- RightChild: " + rightChild);
            string brother = currentZone.brother == null ? "NO BROTHER" : currentZone.brother.getValue().ToString();
            Debug.Log("- Brother: " + brother);
            Debug.Log("-----------------------------------");

            if (currentZone.leftChild != null)
                PrintHeap(++index, currentZone.leftChild);
            if (currentZone.hasRightChild() && currentZone.rightChild != null)
                PrintHeap(++index, currentZone.rightChild);
        }
    }
}
