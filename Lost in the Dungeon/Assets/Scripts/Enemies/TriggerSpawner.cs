using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TriggerSpawner : MonoBehaviour
{
    public Enemy[] enemies;
    public List<Vector3> enemyPositions;
    public event Action OnEnemyDied;
    public bool playerInside = false;
    public int enemiesAlive = 0;
    public bool completed = false;
    private bool started = false;
    public int difficulty;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag.CompareTo("Player") == 0)
        {
            playerInside = true;
            if (!started)
            {
                started = true;
                Enemy chosenEnemy;
                for (int i = 0; i < enemyPositions.Count; i++)
                {
                    chosenEnemy = enemies[UnityEngine.Random.Range(0, enemies.Length)];
                    Spawner.sharedInstance.SpawnEnemy(chosenEnemy, enemyPositions[i], this, difficulty);
                    enemiesAlive++;
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag.CompareTo("Player") == 0)
        {
            playerInside = false;
        }
    }
}
