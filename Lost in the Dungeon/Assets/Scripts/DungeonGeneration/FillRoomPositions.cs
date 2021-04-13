using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FillRoomPositions : MonoBehaviour
{
    [Header("Elements lists")]
    public List<Looteable> lootElements;

    [Header("Positions lists")]
    public List<Transform> enemySpawnPositions;
    public List<Transform> lootSpawnPositions;
    public Transform itemSellerPosition;
}
