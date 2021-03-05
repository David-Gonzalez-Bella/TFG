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
    private int MAX_ZONES;
    private int COUNT_ZONES = 1;
    private Zone root;
    private Queue<Zone> brotherZones;
    [SerializeField]
    public Zone[] brotherZonesCopy;

    private Queue<Zone> levelChildren;
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
    }

    private void InitializeTree()
    {
        root = Instantiate(zonePrefab, Vector3.zero, Quaternion.identity, GetComponentInChildren<Grid>().transform);
        root.Initialize(0, 2, 3); //The root zone will always have 2 children

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

        #region RoughCode
        //if (insertLevel.leftChild == null)
        //{
        //    if (insertLevel == root)
        //    {
        //        EnableFirstLevelRoads(newZone, 0);
        //        EnqueueBeginSpawnPoints(newZone);
        //    }
        //    else
        //    {
        //        EnableRoads(newZone, canBeExpanded);
        //        EnqueueExpandSpawnPoints(newZone);
        //    }
        //    insertLevel.setLeftChild(newZone);
        //    levelChildren.Enqueue(newZone);
        //}
        //else if (insertLevel.hasRightChild() && insertLevel.rightChild == null) //If not, check if we can insert the new zone as the right child of the current zone
        //{
        //    if (insertLevel == root)
        //    {
        //        EnableFirstLevelRoads(newZone, 1);
        //        EnqueueBeginSpawnPoints(newZone);
        //    }
        //    else
        //    {
        //        EnableRoads(newZone, canBeExpanded);
        //        EnqueueExpandSpawnPoints(newZone);
        //    }
        //    insertLevel.setRightChild(newZone);
        //    levelChildren.Enqueue(newZone);
        //}
        #endregion

        brotherZones.Enqueue(newZone);
        CheckLevelCompleted(insertLevel); //After each node is inserted, we check if we hace completed that level of the tree
    }

    private void ExpandLevel(Zone insertLevel, Zone newZone, int zoneValue)
    {
        if (insertLevel != root)
        {
            if (maxExpand <= 0)
            {
                newZone.Initialize(zoneValue, 0, 1); //0 children

                if (insertLevel.leftChild == null)
                    insertLevel.setLeftChild(newZone);
                else if (insertLevel.hasRightChild() && insertLevel.rightChild == null)
                    insertLevel.setRightChild(newZone);

                EnableRoads(newZone);
                return;
            }

            else if (maxExpand > 1)
            {
                newZone.Initialize(zoneValue, 0, 3); //0, 1, or 2 children
                if (!newZone.isLeaf())
                {
                    maxExpand--;
                    if (newZone.hasRightChild())
                        maxExpand--;
                }
            }

            else if (maxExpand > 0)
            {
                newZone.Initialize(zoneValue, 1, 2); //0 or 1 child
                if (!newZone.isLeaf())
                    maxExpand--;
            }
            if (insertLevel.leftChild == null)
                insertLevel.setLeftChild(newZone);
            else if (insertLevel.hasRightChild() && insertLevel.rightChild == null)
                insertLevel.setRightChild(newZone);

            EnableRoads(newZone);
            EnqueueExpandSpawnPoints(newZone);
        }
        else
        {
            newZone.Initialize(zoneValue, 1, 3);

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
        }
        if (!newZone.isLeaf())
            levelChildren.Enqueue(newZone);
    }

    private void CheckLevelCompleted(Zone insertZone)
    {
        if (insertZone.leftChild != null && ((!insertZone.hasRightChild()) || (insertZone.hasRightChild() && insertZone.rightChild != null))) //If all possible children of the current zone are filled
        {
            levelChildren.Dequeue(); //Move on to the next zone to fill its children
            if (levelChildren.Peek() == null)
            {
                ConnectBrotherZones(); //We create conections between brother nodes(randomly)
                levelChildren.Dequeue();
                levelChildren.Enqueue(null);
                maxExpand = 4;
            }
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
                makeConexion = Random.Range(0, 2);
                if (makeConexion == 0)
                {
                    brother = brotherZones.Peek();
                    distance = (int)(brother.transform.position.x - current.transform.position.x);

                    if (distance == 32)
                        current.rightRoad_near.SetActive(true);

                    else if (distance == 64)
                        current.rightRoad_mid.SetActive(true);

                    else if (distance == 48)
                        current.rightRoad_firstLevel.SetActive(true);

                    else
                        current.rightRoad_far.SetActive(true);

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
        zone.topMidOpening.SetActive(zone.isLeaf());
        zone.topMidRoad.SetActive(!zone.isLeaf());
        zone.topRightOp.SetActive(!zone.hasRightChild());
        zone.topRightRoad.SetActive(zone.hasRightChild());
        zone.downOp.SetActive(false);
        zone.downRoad.SetActive(true);
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

    private void EnqueueExpandSpawnPoints(Zone zone)
    {
        if (zone.isLeaf()) return; //If the zone has no children, then its expand spawn positions are not added to the queue

        Vector2 topMidPoint = new Vector2(zone.transform.position.x, zone.transform.position.y + 20);

        if (spawnPositions.Contains(topMidPoint))
            topMidPoint += new Vector2(32, 0);

        spawnPositions.Enqueue(topMidPoint);

        if (zone.hasRightChild())
            spawnPositions.Enqueue(topMidPoint + new Vector2(32, 0));
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
