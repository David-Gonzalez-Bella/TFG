﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonGenerator : MonoBehaviour
{
    //References
    public Zone zonePrefab;
    public TriggerSpawner triggerSpawner;
    public FillRoomPositions fillRoomPositions;
    public ItemSeller itemSeller;
    public LevelExit levelExit;

    //Instantiation positions
    public Queue<Vector2> spawnPositions;

    //Tree generation related atributes
    [SerializeField]
    private int MAX_LEVELS;
    public int COUNT_LEVELS = 1;
    private Zone root;
    private Queue<Zone> brotherZones;
    private Queue<Zone> levelChildren;
    private int maxExpand = 4;
    private bool shopGenerated = false;
    private int shopLevelCounter;

    //Enemy spawn related atributes
    private int numEnemies = 0;

    private void Awake()
    {
        MAX_LEVELS = ChooseLengthManager.DUNGEON_DEPTH;
    }

    private void Start()
    {
        InitializeTree();
        int min, max;
        int difficulty;
        while (COUNT_LEVELS < MAX_LEVELS)
        {
            //First we decide how many enemies the room will have
            if (COUNT_LEVELS < 4)
            {
                min = 1;
                max = 3;
                difficulty = 1;
            }
            else if (COUNT_LEVELS >= 4 && COUNT_LEVELS < 8)
            {
                min = 2;
                max = 4;
                difficulty = 2;
            }
            else
            {
                min = 3;
                max = 5;
                difficulty = 3;
            }
            numEnemies = UnityEngine.Random.Range(min, max + 1);
            AddZone(difficulty);
        }
    }

    private void InitializeTree()
    {
        root = Instantiate(zonePrefab, Vector3.zero, Quaternion.identity,
                                GetComponentInChildren<Grid>().transform);
        root.Initialize(0, 2, 2); //The root zone will always have 2 children

        spawnPositions = new Queue<Vector2>();
        brotherZones = new Queue<Zone>();
        levelChildren = new Queue<Zone>();

        levelChildren.Enqueue(root);
        levelChildren.Enqueue(null);

        spawnPositions.Enqueue(root.topLeftLongPoint.position);
        spawnPositions.Enqueue(root.topRightLongPoint.position);

        numEnemies = UnityEngine.Random.Range(1, 4);
        FillZone(root.topLeftLongPoint.position, root);
        FillZone(root.topRightLongPoint.position, root);
        EnableRootRoads();
        COUNT_LEVELS++;
    }

    public void AddZone(int zoneValue) //We are sure zones will be inserted in ascending order
    {
        if (COUNT_LEVELS == MAX_LEVELS) return;

        Zone insertLevel = levelChildren.Peek();
        Zone newZone = Instantiate(zonePrefab, spawnPositions.Dequeue(), Quaternion.identity,
                                                    GetComponentInChildren<Grid>().transform);
        ExpandLevel(insertLevel, newZone, zoneValue);

        brotherZones.Enqueue(newZone);
        CheckLevelCompleted(insertLevel); //We check if we have completed that level of the tree
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
            else if (insertLevel.hasRightChild() &&
                     insertLevel.rightChild == null)
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

            else if (insertLevel.hasRightChild() &&
                     insertLevel.rightChild == null)
            {
                EnableFirstLevelRoads(newZone, 1);
                insertLevel.setRightChild(newZone);
            }
            EnqueueBeginSpawnPoints(newZone);
            levelChildren.Enqueue(newZone);
        }

        //Enable the proper opening depending on the parent's 
        //enabled roads towards its children
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
        if (insertZone.leftChild != null && ((!insertZone.hasRightChild()) ||
            (insertZone.hasRightChild() && insertZone.rightChild != null)))
        //If all possible children of the current zone are filled
        {
            levelChildren.Dequeue(); //Move on to the next zone to fill its children
            if (levelChildren.Peek() == null)
            {
                Zone[] b = brotherZones.ToArray();
                int i = 0;

                if (insertZone != root && COUNT_LEVELS != MAX_LEVELS - 1)
                {
                    //If we have expanded less thatn two zones in this level of the tree 
                    //the tree (wich means it will end here) we assign children to the current brothers
                    while (maxExpand > 2)
                    {
                        if (i == 0) //Clear spawn positions, since a new children assignment will take place
                        {
                            spawnPositions.Clear();
                            maxExpand = 4;
                        }
                        AddChildren(b[i]);
                        i = (i + 1) % b.Length;
                    }
                    EnqueueExpandSpawnPoints(b); //We wiil enqueue the necessary spawn points, and 
                    //we will paint the roads that conect each brother with its children
                }
                else if (COUNT_LEVELS == MAX_LEVELS - 1)
                {
                    i = 0;
                    int r = 1;
                    while (r == 1)
                    {
                        r = UnityEngine.Random.Range(0, 2);
                        if (r == 0)
                            Instantiate(levelExit, b[i].transform.position, Quaternion.identity,
                                b[i].transform).transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
                        i = (i + 1) % b.Length;
                    }
                }
                //We create conections between brother nodes and prepare everything towards the next adition 
                COUNT_LEVELS++; //A new level has been completed
                ConnectBrotherZones();
                levelChildren.Dequeue();
                levelChildren.Enqueue(null);
                shopGenerated = false;
                shopLevelCounter = 0;
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

    //Each zone can be connected to the one it 
    //is next to, from left to right 
    private void ConnectBrotherZones()
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
                makeConexion = UnityEngine.Random.Range(0, 2);
                if (makeConexion == 0)
                {
                    brother = brotherZones.Peek();
                    distance = (int)(brother.transform.position.x -
                                current.transform.position.x);
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
        {
            spawnPositions.Enqueue(zone.topLeftLongPoint.position);
            FillZone(zone.topLeftLongPoint.position, zone);
        }
        if (zone.topRightRoad.activeSelf)
        {
            spawnPositions.Enqueue(zone.topRightPoint.position);
            FillZone(zone.topRightPoint.position, zone);
        }
        if (zone.topLeftRoad.activeSelf)
        {
            spawnPositions.Enqueue(zone.topLeftPoint.position);
            FillZone(zone.topLeftPoint.position, zone);
        }
        if (zone.topRightLongRoad.activeSelf)
        {
            spawnPositions.Enqueue(zone.topRightLongPoint.position);
            FillZone(zone.topRightLongPoint.position, zone);
        }
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
           (direction == 1 && spawnPositions.Contains(top) || //When the top possition is occupied, then
           (direction == -1 &&                                //it is moved one place to the right
           (adjacent.transform.position.x - current.transform.position.x == 32) &&
           ((currPos == -16 && !current.hasRightChild()) || (currPos == 16 && adjacent.hasRightChild())))))
        //If a node placed to the right next to the current node has two children, then the current node 
        //will have its top position moved one placed to the left
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
        FillZone(top, current);

        if (!current.hasRightChild())
        {
            spawnPositions.Enqueue(top);
            count++;
        }
        else
        {
            Vector3 top2 = top + new Vector3(32 * direction, 0, 0);
            if (direction == -1)
            {
                spawnPositions.Enqueue(top2);
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
                spawnPositions.Enqueue(top2);
                if (current.rightChildRoad_near.activeSelf)
                    current.rightChildRoad_far.SetActive(true);
                else
                    current.rightChildRoad_near.SetActive(true);
                current.topRightOp.SetActive(false);
                current.rightChildRoadOp.SetActive(!current.rightChildRoad_far.activeSelf);
            }
            FillZone(top2, current);
            count += 2;
        }
        return count;
    }

    private void FillZone(Vector3 position, Zone parent)
    {
        fillRoomPositions.transform.position = position;

        //Instantiate item seller
        if (COUNT_LEVELS == 1 || COUNT_LEVELS == 4 || COUNT_LEVELS == 7)
        {
            if (!shopGenerated)
            {
                int r = UnityEngine.Random.Range(0, 2);
                bool lastCheck = COUNT_LEVELS == 1 ?
                     position == root.topRightLongPoint.position :
                     shopLevelCounter == GetNextLevelChildren(brotherZones.ToArray());
                if (r == 0 || lastCheck)
                {
                    ItemSeller t = Instantiate(itemSeller, fillRoomPositions.itemSellerPosition.position,
                        Quaternion.identity, parent.transform);
                    t.ChooseModel(COUNT_LEVELS);
                    shopGenerated = true;
                    MenusManager.sharedInstance.sellerPositions.Add(t.transform);
                    return;
                }
                shopLevelCounter++;
            }
        }
        TriggerSpawner sp = Instantiate(triggerSpawner, position, Quaternion.identity, parent.transform);
        List<Transform> finalEnemySpawnPositions = new List<Transform>(fillRoomPositions.enemySpawnPositions);
        sp.difficulty = COUNT_LEVELS - 1;
        int count = 0;
        int index;
        //Instantiate enemies in random positions
        while (count < numEnemies)
        {
            index = UnityEngine.Random.Range(0, finalEnemySpawnPositions.Count);
            sp.enemyPositions.Add(finalEnemySpawnPositions[index].position);
            finalEnemySpawnPositions.RemoveAt(index);
            count++;
        }
        count = 0;
        //Instantiate loot boxes
        while (count < fillRoomPositions.lootSpawnPositions.Count)
        {
            index = UnityEngine.Random.Range(0, 2);
            GameObject container = GameObject.Find("LootContainer");
            if (index == 0)
            {
                Looteable looteable = fillRoomPositions.lootElements[UnityEngine.Random.Range(0, fillRoomPositions.lootElements.Count)];
                Instantiate(looteable, fillRoomPositions.lootSpawnPositions[count].transform.position, Quaternion.identity, container.transform)   
                .Initialize(1, COUNT_LEVELS + 2, sp);
            }
            count++;
        }
    }

    private int GetNextLevelChildren(Zone[] b)
    {
        int c = 0;
        for (int i = 0; i < b.Length; i++)
        {
            c += b[i].children;
        }
        return c - 1;
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
