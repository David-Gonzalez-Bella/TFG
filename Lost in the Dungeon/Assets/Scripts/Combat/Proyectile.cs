using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Proyectile : MonoBehaviour
{
    public Vector2 direction; //This will be the direction towards our player
    public float speed;
    private Rigidbody2D rb;
    public Atributes wizardAtrib;


    private void Awake()
    {
        rb = this.GetComponent<Rigidbody2D>();
        rb.velocity = direction.normalized * speed;
    }

    private void Start()
    {
        Destroy(this.gameObject, 3.0f); //The proyectiles will be destroyed after 3s if they dont collide  
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Destroy(this.gameObject);
            collision.gameObject.GetComponent<Attackable>().Attacked(direction, wizardAtrib.damage);
        }
    }
}
