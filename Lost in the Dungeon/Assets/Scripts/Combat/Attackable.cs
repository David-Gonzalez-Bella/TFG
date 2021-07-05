using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Health))]
public class Attackable : MonoBehaviour
{
    private Health myHealth;
    private Rigidbody2D rb;
    private SpriteRenderer spr;
    private int extraForce = 0;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spr = GetComponentInChildren<SpriteRenderer>();
    }

    private void Start()
    {
        myHealth = GetComponent<Health>(); //Each attackable object will have his own health (baseHealth and currentHealth)
    }

    public void Attacked(Vector2 attackDirection, float damage)
    {

        if (gameObject.GetComponent<PlayerController>() != null)
        {
            PlayerController player = gameObject.GetComponent<PlayerController>();
            if (player.dodgeChance > 0)
            {
                float c = Random.Range(0.0f, 1.0f);
                if (c < player.dodgeChance)
                {
                    StartCoroutine(Dodge());
                    return;
                }
            }
            else
                PlayPlayerDamageSound();

        }
        else if (gameObject.GetComponent<Enemy>() != null)
        {
            PlayerController player = GameManager.sharedInstance.player.GetComponent<PlayerController>();
            if (player.lifeSteal > 0)
                player.health.ModifyHealth(player.lifeSteal);
            if (player.manaVamp > 0)
                player.mana.ModifyMana(player.manaVamp);
            if (player.galeForce > 0)
                extraForce = player.galeForce * 60;
            switch (this.GetComponent<Enemy>().type)
            {
                case enemyType.standard:
                    AudioManager.sharedInstance.PlayEnemyDamageSound();
                    break;
                case enemyType.creature:
                    AudioManager.sharedInstance.PlayEnemyCreatureDamageSound();
                    break;
            }
        }
        StartCoroutine(TakeDamage(damage));
        rb.AddForce(attackDirection * (20 + extraForce), ForceMode2D.Impulse); //When attacked, the object will be pushed back as well
    }

    //Coroutines
    IEnumerator Dodge()
    {
        spr.color = new Color(0.98f, 0.93f, 0.65f);
        TextHitGenerator.sharedInstance.CreateTextHit(Color.yellow, this.transform, "DODGE!");
        yield return new WaitForSeconds(0.15f);
        spr.color = Color.white;
    }

    IEnumerator TakeDamage(float damage)
    {
        myHealth.ModifyHealth(-damage); //When attacked, the object will lose 1HP
        spr.color = Color.red;
        TextHitGenerator.sharedInstance.CreateTextHit(Color.red, this.transform, (-damage).ToString());
        yield return new WaitForSeconds(0.15f);
        spr.color = Color.white;
    }

    public void PlayPlayerDamageSound()
    {
        GetComponent<PlayerController>().damgeAudioSource.Play();
    }
}
