using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputEnemy : MonoBehaviour
{
    public Vector2 playerDirection { get; private set; }
    public float horizontalDir
    {
        get
        {
            return playerDirection.x;
        }
    }
    public float verticalDir
    {
        get
        {
            return playerDirection.y;
        }
    }
    public float distanceMagnitude
    {
        get
        {
            return playerDirection.magnitude;
        }
    } 

    private Transform playerPos;

    private void Awake()
    {
        playerPos = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Start()
    {
        SetPlayerDirection();
    }

    private void Update()
    {
        SetPlayerDirection();
    }

    private void SetPlayerDirection()
    {
        playerDirection = playerPos.position - this.gameObject.transform.position;
    }
}
