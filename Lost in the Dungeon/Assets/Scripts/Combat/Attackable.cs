using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Health))]
public class Attackable : MonoBehaviour
{
    private Health myHealth;
    private Rigidbody2D rb;
    private SpriteRenderer spr;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spr = GetComponentInChildren<SpriteRenderer>();
    }

    private void Start()
    {
        myHealth = GetComponent<Health>(); //Each attackable object will have his own health (baseHealth and currentHealth)
    }

    public void Attacked(Vector2 attackDirection, int damage)
    {
        StartCoroutine(TakeDamage(damage));
        rb.AddForce(attackDirection * 200, ForceMode2D.Impulse); //When attacked, the object will be pushed back as well
        if (this.tag == "Player") { PlayPlayerDamageSound(); } //If the player is attacked
        else //If an enemy is attacked
        {
            switch (this.GetComponent<Enemy>().type)
            {
                case enemyType.standard:
                    AudioManager.sharedInstance.OnEnemyDamageSound += PlayEnemyDamageSound;
                    break;
                case enemyType.creature:
                    AudioManager.sharedInstance.OnEnemyDamageSound += PlayEnemyCreatureDamageSound;
                    break;
            }
        }
    }

    //Coroutines
    IEnumerator TakeDamage(int damage)
    {
        myHealth.ModifyHealth(-damage); //When attacked, the object will lose 1HP
        spr.color = Color.red;
        TextHitGenerator.sharedInstance.CreateTextHit(Color.red, this.transform, (-damage).ToString());
        yield return new WaitForSeconds(0.15f);
        spr.color = Color.white;
    }

    public void PlayPlayerDamageSound()
    {
        GameManager.sharedInstance.player.GetComponent<PlayerController>().damgeAudioSource.Play();
    }

    public void PlayEnemyDamageSound()
    {
        AudioManager.sharedInstance.enemyDamage.Play();
        AudioManager.sharedInstance.OnEnemyDamageSound -= PlayEnemyDamageSound;
    }

    public void PlayEnemyCreatureDamageSound()
    {
        AudioManager.sharedInstance.enemyCreatureDamage.Play();
        AudioManager.sharedInstance.OnEnemyDamageSound -= PlayEnemyCreatureDamageSound;
    }
}
