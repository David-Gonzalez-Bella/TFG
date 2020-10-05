using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Enemy : MonoBehaviour //This contains the IA and the atributes every enemy will have
{
    public Atributes atrib; //Evey enemy has his basic atributes (for a more specific one they'll have them in their own script)
    public int exp;

    private TriggerSpawner parent;

    protected InputEnemy input;
    protected Attack atk; //The EnemyKnight can attack our player
    protected Animator anim;
    protected SpriteRenderer spr;
    protected CapsuleCollider2D col;

    protected int attackHash;
    protected int deadHash;
    protected int walkHash;

    protected bool attacking = false;
    protected bool dead = false;
    protected Vector2 attackDirection;

    [SerializeField] protected float detectionDistance; //From this distance on, the enemy will detect the player
    [SerializeField] protected float attackingDistance; //From this distance on, the enemy will attack the player

    private void Awake()
    {
        input = this.GetComponent<InputEnemy>(); //Now the EnemyKnight knows the direction of the players, in other words, the player´s position regarding its own
        atk = this.GetComponent<Attack>();
        anim = this.GetComponent<Animator>();
        col = this.GetComponent<CapsuleCollider2D>();
        spr = this.GetComponentInChildren<SpriteRenderer>();
        parent = this.gameObject.GetComponentInParent<TriggerSpawner>();
    }

    private void Start()
    {
        attackHash = Animator.StringToHash("Attack");
        deadHash = Animator.StringToHash("Dead");
        walkHash = Animator.StringToHash("Walk");
    }

    private void Update() //Every enemy shall check its behaviour every frame
    {
        if (GameManager.sharedInstance.currentGameState == gameState.inGame)
            Behaviour(); //The knight´s behaviour will be checked every frame
    }

    public void OnMouseOver() => CursorManager.sharedInstance.SetCursor(CursorManager.sharedInstance.swordCursor);
    public void OnMouseExit() => CursorManager.sharedInstance.SetCursor(CursorManager.sharedInstance.arrowCursor);

    protected virtual void Behaviour() { }

    protected void AttackPlayer()
    {
        int attackChance = Random.Range(0, 100);
        anim.SetBool(walkHash, false); //When the enemy is in attacking state he will stop walking
        if (attackChance > 95) //The chance has to be very low, because this method is executing a lot of times per frame, so we must discriminate a lot of numbers
        {
            attackDirection = input.playerDirection; //Just when the knight decides to attack the direction he is facing will be decided
            attacking = true;
            FlipSprite();
            anim.SetTrigger(attackHash);
        }
    }

    protected void ChasePlayer()
    {
        anim.SetBool(walkHash, true);
        FlipSprite();
        this.transform.position += (Vector3)input.playerDirection.normalized * atrib.speed * Time.deltaTime; //We normalize the playerDirection to get the DIRECTION from it, and the magnitude will be que atrib.speed
    }

    //Virtual methods
    public virtual void EnemyAttack() { } //Called during the attack animation (each enemy will attack in his own way)

    public virtual void FlipSprite() { } //Each enemy will flip his sprite in his own way, depending on the direction his sprite sheets look at

    public void EndAttack()
    {
        attacking = false;
    }

    public void DissappearEffect()
    {
        EnemySpawner.sharedInstance.InstantiateSpawnEffect(this, this.transform.position);
        AudioManager.sharedInstance.OnEnemySpawnSound += EnemySpawner.sharedInstance.PlaySpawnSound;
    }

    public void Die() //This function will be called in the "Health" script, thanks to the Unity Event
    {
        dead = true;
        col.enabled = false;
        GameManager.sharedInstance.player.GetComponent<Mana>().CurrentMana += 3;
        anim.SetBool(deadHash, true); //This triggers the animation, and on the last frame the character will be destroyed (so did we configured it in the animator interface)
    }

    public void DropExperience() //Will be called when the enemy dies, as part of the Unity Event "dieEvent" in the Health script
    {
        GameManager.sharedInstance.player.GetComponent<Experience>().experience += exp;
    }

    public void DeadInZone()
    {
        parent.deadEnemies++;
        parent.OnEnemyDied -= this.GetComponent<Enemy>().DeadInZone;
    }
}

