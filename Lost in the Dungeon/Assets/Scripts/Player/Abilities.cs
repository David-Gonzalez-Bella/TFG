using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Abilities : MonoBehaviour
{
    //Dash ability
    private Vector2 dashDirection;
    private float dashTime;
    [HideInInspector]
    public float startDashTime = 0.1f;
    [HideInInspector]
    public float dashSpeed = 10;
    [HideInInspector]
    public int dashManaCost = 4;
    [HideInInspector]
    public bool dashing = false;

    //Fireball ability
    public Proyectile fireball;
    private float fireballTime;
    [HideInInspector]
    public float startFireballTime = 1.5f;
    [HideInInspector]
    public int fireballDamage = 1;
    [HideInInspector]
    public float fireballLifeTime = 3.0f;
    [HideInInspector]
    public float fireballSpeed = 5.0f;
    [HideInInspector]
    public int fireballManaCost = 5;
    [HideInInspector]
    public bool throwingFireball = false;

    private Rigidbody2D rb;
    private TrailRenderer trail;
    private Mana mana;

    private float trailColorAlpha = 0.7f;

    private void Awake()
    {
        trail = GetComponent<TrailRenderer>();
        mana = GetComponent<Mana>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        dashTime = startDashTime;
        fireballTime = startFireballTime;
        trail.startColor = new Color(0.64f, 0.11f, 0.0f, 0.5f);
        trail.endColor = new Color(1.0f, 0.77f, 0.48f, 0.5f);
    }

    private void FixedUpdate()
    {
        if (dashing)
        {
            if (dashTime > 0) //If we have not ended dashing
            {
                dashTime -= Time.fixedDeltaTime;
                rb.velocity = dashDirection * dashSpeed;
            }
            else
            {
                if (trailColorAlpha > 0.0f)
                {
                    trailColorAlpha -= 0.1f;
                    trail.startColor = new Color(trail.startColor.r, trail.startColor.g, trail.startColor.b, trailColorAlpha);
                    trail.endColor = new Color(trail.endColor.r, trail.endColor.g, trail.endColor.b, trailColorAlpha);
                }
                else
                {
                    dashTime = startDashTime;
                    trailColorAlpha = 0.7f;
                    trail.enabled = false;
                    trail.startColor = new Color(0.64f, 0.11f, 0.0f, 0.5f);
                    trail.endColor = new Color(1.0f, 0.77f, 0.48f, 0.5f);
                    dashing = false;
                }
            }
        }

        if (throwingFireball)
        {
            if (fireballTime > 0) //If we have not ended throwing the fireball
            {
                fireballTime -= Time.fixedDeltaTime;
            }
            else
            {
                fireballTime = startFireballTime;
                throwingFireball = false;
            }
        }
    }

    public void Dash(Vector2 playerDashDirection)
    {
        dashDirection = playerDashDirection;
        dashing = true;
        trail.enabled = true;
        mana.ModifyMana(-dashManaCost);
    }

    public void ThrowFireball(Attack atck, Vector2 direction, Vector3 position)
    {
        throwingFireball = true;
        atck.ProyectileAttack(fireball, direction, position);
        mana.ModifyMana(-fireballManaCost);
    }
}

