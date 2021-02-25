using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(InputEnemy))]
[RequireComponent(typeof(Attack))]
[RequireComponent(typeof(Attackable))]

public class EnemyKnight : Enemy //Enemy inherits from Monobehaviour. Therefore, so does EnemyKnight
{
    public GameObject swordFlash;

    protected override void Behaviour()
    {
        if (dead) return;

        if (!attacking && input.distanceMagnitude < attackingDistance && parent.playerInside) //If the enemy is not attacking and the distance to the player is less than the attacking distance it means that he shall attack the player
            AttackPlayer();

        else if (!attacking && input.distanceMagnitude < detectionDistance && parent.playerInside)
            ChasePlayer();

        else //If the player is out of range he goes to idle animation
            anim.SetBool(walkHash, false);
    }

    public override void EnemyAttack() //The knight overrides the method to attack, as he attacks in his own way
    {
        atk.PhysicalAttack(attackDirection, atrib.damage, swordFlash);
        AudioManager.sharedInstance.PlaySwordSound();
    }
    public override void FlipSprite() //Each enemy will flip his sprite in his own way, depending on the direction his sprite sheets look at
    {
        if (input.horizontalDir < 0)
        {
            spr.flipX = true;
        }
        else
        {
            spr.flipX = false;
        }
    }

    public void PlayStepsSound()
    {
        AudioManager.sharedInstance.stepsSound.Play(); //This sound play is more simple because its called in the exact frame of the animation where the enemy steps
    }
}
