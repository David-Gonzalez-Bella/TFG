using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonGenerator : MonoBehaviour
{
    //References
    public Zone zonePrefab;

    //Instantiation positions
    public Queue<Vector2> spawnPositions;
    public Vector2[] spawnPositionsCopy;

    //Tree generation related atributes
    [SerializeField]
    private int MAX_ZONES;
    private int COUNT_ZONES = 1;
    private Zone root;
    private Queue<Zone> brotherZones;
    public Zone[] brotherZonesCopy;

    private Queue<Zone> levelChildren;
    public Zone[] levelChildrenCopy;
    public int maxExpand = 4;


    private void Start()
    {
        InitializeTree();
        //AddZone(1); //When we add a zone to the heap, it is instantiated in the world as well
        //AddZone(2);

        //AddZone(3);
        //AddZone(4);
        //AddZone(5);
        //AddZone(5);

        //AddZone(6);
        //AddZone(6);
        //AddZone(6);
        //AddZone(6);
        //AddZone(6);
        //AddZone(6);
        //AddZone(6);
        //AddZone(6);
        //AddZone(6);
        //AddZone(6);
        //AddZone(6);
        //AddZone(6);
    }

    private void Update()
    {
        brotherZonesCopy = brotherZones.ToArray();
        spawnPositionsCopy = spawnPositions.ToArray();
        levelChildrenCopy = levelChildren.ToArray();
    }

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
        if (COUNT_ZONES == MAX_ZONES) return;

        COUNT_ZONES++;
        Zone insertLevel = levelChildren.Peek();
        Zone newZone = Instantiate(zonePrefab, spawnPositions.Dequeue(), Quaternion.identity, GetComponentInChildren<Grid>().transform);
        ExpandLevel(insertLevel, newZone, zoneValue);

        brotherZones.Enqueue(newZone);
        CheckLevelCompleted(insertLevel); //After each node is inserted, we check if we hace completed that level of the tree
    }

    private void ExpandLevel(Zone insertLevel, Zone newZone, int zoneValue)
    {
        if (insertLevel != root)
        {
            if (maxExpand == 0)
            {
                newZone.Initialize(zoneValue, 0, 0); //0 children
                EnableRoads(newZone);
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
    }

    private void CheckLevelCompleted(Zone insertZone)
    {
        if (insertZone.leftChild != null && ((!insertZone.hasRightChild()) || (insertZone.hasRightChild() && insertZone.rightChild != null))) //If all possible children of the current zone are filled
        {
            Debug.Log("LEVEL COMPLETED");
            levelChildren.Dequeue(); //Move on to the next zone to fill its children
            if (levelChildren.Peek() == null)
            {
                Debug.Log("LETS GO!");
                if (insertZone != root)
                {
                    int i = 0;
                    Zone[] b = brotherZones.ToArray();

                    //If we have expanded less thatn two zones in this level of the tree the tree (wich means it will end here) we assign children to the current brothers
                    while (maxExpand > 2)
                    {
                        Debug.Log("NOT ENOUGH CHILDREN FIXING...");
                        if (i == 0) //Clear spawn positions, since a new children assignment will take place
                        {
                            //ClearChildren(b);
                            spawnPositions.Clear();
                            maxExpand = 4;
                        }
                        AddChildren(b[i]);
                        i = (i + 1) % b.Length;
                    }

                    //We decide the spawn expansion points depending on the children each zone has
                    //In these methods we PAINT each node's final ROADS as well
                    if (maxExpand == 0)
                        EnqueueAllSpawnPoints(b);
                    else
                        EnqueueExpandSpawnPoints(b);
                }

                //We create conections between brother nodes and prepare everything towards the next zone adition 
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
        //EnableRoads(current); [HERE]
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

    private void EnableRoads(Zone zone)
    {
        zone.downOp.SetActive(false);
        zone.downRoad.SetActive(true);

        if (zone.isLeaf()) return;

        if (zone.leftChild.transform.position.x == zone.transform.position.x)
        {
            zone.topMidOpening.SetActive(true);
            zone.rightChildRoad_near.SetActive(zone.hasRightChild());
        }

        else if (zone.leftChild.transform.position.x < zone.transform.position.x)
        {
            if (zone.hasRightChild())
            {
                if (zone.rightChild.transform.position.x == zone.transform.position.x)
                    zone.topMidOpening.SetActive(true);
                else
                    zone.leftChildRoad_far.SetActive(true);
            }
            zone.leftChildRoad_near.SetActive(true);
        }
        else
        {
            zone.rightChildRoad_near.SetActive(true);
            zone.rightChildRoad_far.SetActive(zone.hasRightChild());
        }

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
        zone.downOp.SetActive(false);
        zone.downRoad.SetActive(true);
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
        Zone current, next, prev;
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
            EnableRoads(current);
        }
    }

    public void EnqueueAllSpawnPoints(Zone[] zones)
    {
        Debug.Log("EnqueueAllSpawnPoints");
        Zone current;
        int xPosition = -48;
        int yPosition = (int)zones[0].transform.position.y;
        for (int i = 0; i < 4; i++)
        {
            spawnPositions.Enqueue(new Vector2(xPosition, yPosition + 20));
            xPosition += 32;
        }
        for (int i = 0; i < zones.Length; i++)
        {
            current = zones[i];
            if (!current.isLeaf())
                levelChildren.Enqueue(current);
            EnableRoads(current); 
        }
    }

    private int EnqueueTopPositions(Zone current, Zone adjacent, int count, int direction)
    {
        Debug.Log("EnqueueTopPositions");
        Vector3 top = current.transform.position + new Vector3(0, 20, 0);

        if (adjacent != current && ((direction == 1 && adjacent.hasRightChild()) || (direction == -1 && adjacent.hasRightChild()))) //if the PREVIOUS or the NEXT zone has a right child 
            top += new Vector3(32 * direction, 0, 0);

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
            }
            else
            {
                spawnPositions.Enqueue(top);
                spawnPositions.Enqueue(top + new Vector3(32 * direction, 0, 0));
            }
            count += 2;
        }

        return count;
    }

    public void ClearChildren(Zone[] zones)
    {
        Zone current;
        for (int i = 0; i < zones.Length; i++)
        {
            current = zones[i];
            if (!current.isLeaf())
            {
                current.setLeftChild(null);
                if (current.hasRightChild())
                    current.setRightChild(null);
            }
        }
    }

    public void PrintHeap(int index, Zone currentZone)
    {
        if (index <= COUNT_ZONES)
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
