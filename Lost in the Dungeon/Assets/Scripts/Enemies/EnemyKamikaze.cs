using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(InputEnemy))]
[RequireComponent(typeof(Attack))]
[RequireComponent(typeof(Attackable))]

public class EnemyKamikaze : Enemy //Enemy inherits from Monobehaviour. Therefore, so does EnemyKnight
{
    public GameObject rushFlash;
    private bool dealDamageCoroutine = false;
    public bool canDealDamage = false;

    protected override void Behaviour()
    {
        if (!dead)
        {
            if (canAttack && !attacking && input.distanceMagnitude < detectionDistance) //If the enemy is not attacking and the distance to the player is less than the attacking distance it means that he shall attack the player
            {
                AttackPlayer(); //Enable 'attacking' variable to trigger te attack animation
            }
            else if (attacking)
            {
                RushTowardsPlayer();
            }
            else //If the player is out of range he goes to idle animation
            {
                anim.SetBool(attackHash, false);
            }
        }
    }

    private void RushTowardsPlayer()
    {
        this.transform.position += (Vector3)attackDirection.normalized * atrib.speed * Time.deltaTime;
        if (!AudioManager.sharedInstance.rushAttack.isPlaying)
            StartCoroutine(PlayRushSoundCoroutine());
        canAttack = false;
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

    public void PlayFlySound()
    {
        AudioManager.sharedInstance.flySound.Play(); //This sound play is more simple because its called in the exact frame of the animation where the wing flaps
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (attacking && collision.gameObject.tag.CompareTo("Player") == 0)
        {
            canDealDamage = false;
            DealDamage();
        }
        if (collision.gameObject.tag.CompareTo("Walls") == 0)
        {
            health.dieEvent?.Invoke();
            AudioManager.sharedInstance.OnEnemyDamageSound += atkb.PlayEnemyCreatureDamageSound;
            AudioManager.sharedInstance.PlayWallCrush();
        }
    }

    private void DealDamage()
    {
        GameManager.sharedInstance.player.GetComponent<Attackable>().Attacked(attackDirection, atrib.damage);
        Instantiate(rushFlash, GameManager.sharedInstance.player.transform);
    }

    public void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag.CompareTo("Player") == 0)
        {
            if (!dealDamageCoroutine)
                StartCoroutine(DealDamageCoroutine());
        }
    }

    public void CanDealDamage()
    {
        canDealDamage = true;
    }

    IEnumerator PlayRushSoundCoroutine()
    {
        AudioManager.sharedInstance.rushAttack.Play();
        yield return new WaitWhile(() => AudioManager.sharedInstance.rushAttack.isPlaying);
    }

    IEnumerator DealDamageCoroutine()
    {
        dealDamageCoroutine = true;
        yield return new WaitUntil(() => canDealDamage);
        canDealDamage = false;
        DealDamage();
        dealDamageCoroutine = false;
    }
}

