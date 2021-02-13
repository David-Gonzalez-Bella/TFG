using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(InputEnemy))]
[RequireComponent(typeof(Attack))]
[RequireComponent(typeof(Attackable))]
[RequireComponent(typeof(Proyectile))]

public class EnemyWizard : Enemy //Enemy inherits from Monobehaviour. Therefore, so does EnemyWizard
{
    public Proyectile fireball; //Reference to the Fireball prefab
    public Transform shootOrigin;

    protected override void Behaviour()
    {
        if (!dead)
        {
            if (!attacking && input.distanceMagnitude < attackingDistance) //If the enemy is not attacking and the distance to the player is less than the attacking distance it means that he shall attack the player
            {
                AttackPlayer();
            }
            else if (!attacking && (input.distanceMagnitude < detectionDistance) && parent.playerInside)
            {
                ChasePlayer();
            }
            else //If the player is out of range he goes to idle animation
            {
                anim.SetBool(walkHash, false);
            }
        }
    }

    public override void EnemyAttack() //The wizard overrides the method to attack, as he attacks in his own way
    {
        atk.ProyectileAttack(fireball, input.playerDirection, shootOrigin); //Instead of "attackDirection" we send "input.playerDirection" because we want the fireballs to track more than the knight's attacks do
        AudioManager.sharedInstance.PlayFireballSound();
    }

    public override void FlipSprite() //Each enemy will flip his sprite in his own way, depending on the direction his sprite sheets look at
    {
        if (input.horizontalDir < 0 && spr.flipX)
        {
            spr.flipX = false;
            shootOrigin.localPosition *= Vector2.left;
        }
        else if (input.horizontalDir > 0 && !spr.flipX)
        {
            spr.flipX = true;
            shootOrigin.localPosition *= Vector2.left;
        }
        else if (input.horizontalDir > 0 && spr.flipX)
        {
            spr.flipX = true;
        }
        else
        {
            spr.flipX = false;
        }
    }
}
