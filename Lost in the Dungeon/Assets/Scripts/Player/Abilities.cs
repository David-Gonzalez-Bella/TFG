using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Abilities : MonoBehaviour
{
    public float dashSpeed;
    private float dashTime = 0.0f;
    public float startDashTime;
    private Vector2 dashDirection;
    [HideInInspector] public int dashManaCost = 5;
    [HideInInspector] public bool dashing = false;

    private Rigidbody2D rb;
    private TrailRenderer trail;
    private Mana mana;

    private float trailColorAlpha = 0.7f;

    private void Awake()
    {
        trail = GetComponent<TrailRenderer>();
        mana = GetComponent<Mana>();
    }

    private void Start()
    {
        dashTime = startDashTime;
        trail.startColor = new Color(0.64f, 0.11f, 0.0f, 0.5f);
        trail.endColor = new Color(1.0f, 0.77f, 0.48f, 0.5f);
    }

    private void FixedUpdate()
    {
        if (dashing)
        {
            if (dashTime <= 0) //If we have ended dashing
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
            else
            {
                dashTime -= Time.fixedDeltaTime;
                rb.velocity = dashDirection * dashSpeed;
            }
        }
    }

    public void Dash(Vector2 playerDashDirection, Rigidbody2D playerRb)
    {
        if (rb == null) { rb = playerRb; }
        dashDirection = playerDashDirection;
        dashing = true;
        trail.enabled = true;
        mana.ModifyMana(-dashManaCost);
    }
}

