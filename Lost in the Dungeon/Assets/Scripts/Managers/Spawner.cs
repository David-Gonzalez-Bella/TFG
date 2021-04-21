using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    //Singleton
    public static Spawner sharedInstance { get; private set; }

    //References
    public GameObject enemySpawnEffect;
    public GameObject itemSellerDissapearEffect;

    private void Awake()
    {
        if (sharedInstance == null)
            sharedInstance = this;
    }

    #region Enemy spawn
    public void SpawnEnemy(Enemy enemy, Vector3 position, TriggerSpawner parent, int difficulty)
    {
        StartCoroutine(SpawnEnemyCoroutine(enemy, position, parent, difficulty));
    }

    public void InstantiateSpawnEffect(Enemy enemy, Vector3 position)
    {
        Instantiate(enemySpawnEffect, position + new Vector3(0f, -0.5f, 0f), Quaternion.identity);
        AudioManager.sharedInstance.PlayEnemySpawnSound();
    }

    IEnumerator SpawnEnemyCoroutine(Enemy enemy, Vector3 position, TriggerSpawner parent, int difficulty)
    {
        InstantiateSpawnEffect(enemy, position);
        yield return new WaitForSeconds(0.7f);
        Instantiate(enemy, position, Quaternion.identity, parent.transform).SetStats(difficulty);
    }
    #endregion

    #region Item seller dissapear
    public void ItemSellerDissapear()
    {
        StartCoroutine(SpawnItemSellerDissapear());
    }

    public void InstantiateItemSellerDissapearEffect(GameObject itemSeller)
    {
        Instantiate(itemSellerDissapearEffect, itemSeller.transform.position, Quaternion.identity);
        AudioManager.sharedInstance.PlayEnemySpawnSound();
    }

    IEnumerator SpawnItemSellerDissapear()
    {
        GameObject itemSeller = GameManager.sharedInstance.player.GetComponent<PlayerController>().interactingItemSeller.gameObject;
        InstantiateItemSellerDissapearEffect(itemSeller);
        yield return new WaitForSeconds(0.5f);
        Destroy(itemSeller);
    }
    #endregion
}
