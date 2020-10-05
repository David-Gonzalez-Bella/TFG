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
    public int deadEnemies = 0;

    private void OnValidate()
    {
        if (enemyPossitions.Length != enemies.Length)
            enemyPossitions = new Vector3[enemies.Length]; //We will have the same amount of possitions and players
    }

    private void Update()
    {
        OnEnemyDied?.Invoke(); //Constantly checking if an anemy died
        if (deadEnemies >= enemies.Length)
        {
            this.GetComponent<BoxCollider2D>().enabled = true;
            deadEnemies = 0;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag.CompareTo("Player") == 0)
        {
            if (!playerInside)
            {
                for (int i = 0; i < enemies.Length; i++)
                {
                    EnemySpawner.sharedInstance.SpawnEnemy(enemies[i], enemyPossitions[i], this);
                }
                playerInside = true;
                this.GetComponent<BoxCollider2D>().enabled = false;
            }
            else
            {
                playerInside = !playerInside;
            }
        }
    }
}
