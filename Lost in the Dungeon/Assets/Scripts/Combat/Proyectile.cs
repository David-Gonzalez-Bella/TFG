using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Proyectile : MonoBehaviour
{
    public Vector2 direction; //This will be the direction towards our player
    public float speed;
    public GameObject explosionEffect;
    public Atributes wizardAtrib;
    private Rigidbody2D rb;

    private string objective;
    private float lifeTime;
    private int damage;

    private void Awake()
    {
        PlayerController player = GameManager.sharedInstance.player.GetComponent<PlayerController>();
        if (wizardAtrib == player.atrib)
        {
            objective = "Enemy";
            lifeTime = player.abilities.fireballLifeTime;
            damage = player.abilities.fireballDamage;
            speed = player.abilities.fireballSpeed;
        }
        else
        {
            objective = "Player";
            lifeTime = 3.0f;
            damage = wizardAtrib.damage;
        }
        rb = this.GetComponent<Rigidbody2D>();
        rb.velocity = direction.normalized * speed;
    }

    private void Start()
    {
        Destroy(this.gameObject, lifeTime); //The proyectiles will be destroyed after 3s if they dont collide  
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(objective))
        {
            Destroy(this.gameObject);
            collision.gameObject.GetComponent<Attackable>().Attacked(direction, damage);
        }

        if (collision.CompareTag("Walls") || collision.CompareTag("Proyectile"))
        {
            Destroy(this.gameObject);
        }
    }

    private void OnDestroy()
    {
        Instantiate(explosionEffect, transform.position, Quaternion.identity);
        AudioManager.sharedInstance.PlayExplosionSound();
    }
}
