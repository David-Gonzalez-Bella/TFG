using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TriggerSpawner : MonoBehaviour
{
    public Enemy[] enemies;
    public Vector3[] enemyPossitions;
    public event Action OnEnemyDied;
    public bool playerInside = false;
    public int enemiesAlive = 0;

    private void OnValidate()
    {
        if (enemyPossitions.Length != enemies.Length)
            enemyPossitions = new Vector3[enemies.Length]; //We will have the same amount of possitions and players
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag.CompareTo("Player") == 0)
        {
            playerInside = true;
            if (enemiesAlive == 0)
            {
                for (int i = 0; i < enemies.Length; i++)
                {
                    EnemySpawner.sharedInstance.SpawnEnemy(enemies[i], enemyPossitions[i], this);
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
