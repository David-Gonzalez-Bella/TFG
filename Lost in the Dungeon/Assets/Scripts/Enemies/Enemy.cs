using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum enemyType {standard, creature};

public class Enemy : MonoBehaviour //This contains the IA and the atributes every enemy will have
{
    public enemyType type;
    public Atributes atrib; //Evey enemy has his basic atributes (for a more specific one they'll have them in their own script)
    public int exp;
    public Vector2[] stats;
    protected TriggerSpawner parent;

    public int damage;
    public float speed;

    protected InputEnemy input;
    protected Attack atk; //The enemy can attack our player
    protected Attackable atkb; //The enemy can be attacked by our player
    protected Animator anim;
    protected SpriteRenderer spr;
    protected Health health;
    protected CapsuleCollider2D col;

    protected int attackHash = 0;
    protected int deadHash = 0;
    protected int walkHash = 0;

    protected bool attacking = false;
    protected bool canAttack = false;
    protected bool dead = false;
    protected Vector2 attackDirection;

    [SerializeField] protected float detectionDistance; //From this distance on, the enemy will detect the player
    [SerializeField] protected float attackingDistance; //From this distance on, the enemy will attack the player

    private void Awake()
    {
        input = this.GetComponent<InputEnemy>(); //Now the EnemyKnight knows the direction of the players, in other words, the player´s position regarding its own
        atk = this.GetComponent<Attack>();
        atkb = this.GetComponent<Attackable>();
        anim = this.GetComponent<Animator>();
        col = this.GetComponent<CapsuleCollider2D>();
        spr = this.GetComponentInChildren<SpriteRenderer>();
        health = this.GetComponent<Health>();
        parent = this.gameObject.GetComponentInParent<TriggerSpawner>();
    }

    private void Start()
    {
        if (ContainsParameter("Attack")) { attackHash = Animator.StringToHash("Attack"); }
        if (ContainsParameter("Dead")) { deadHash = Animator.StringToHash("Dead"); }
        if (ContainsParameter("Walk")) { walkHash = Animator.StringToHash("Walk"); }
    }

    private void Update() //Every enemy shall check its behaviour every frame
    {
        if (GameManager.sharedInstance.currentGameState == gameState.inGame)
            Behaviour(); //The knight´s behaviour will be checked every frame. This function will tell the enemy which of his actions are the the most favorable
        speed = atrib.baseSpeed;
        damage = atrib.baseDamage;
    }

    public void OnMouseOver() => CursorManager.sharedInstance.SetCursor(CursorManager.sharedInstance.swordCursor);
    public void OnMouseExit() => CursorManager.sharedInstance.SetCursor(CursorManager.sharedInstance.arrowCursor);

    protected virtual void Behaviour() { }

    public void SetStats(int difficulty)
    {
        int next = difficulty != 8 ? 2 : 0;
        Vector2 newStats = stats[Random.Range(difficulty, difficulty + next)];
        atrib.baseDamage = (int)newStats.x;
        atrib.baseSpeed = newStats.y;
    }

    protected void AttackPlayer()
    {
        int attackChance = UnityEngine.Random.Range(0, 100);
        if (walkHash != 0) { anim.SetBool(walkHash, false); } //When the enemy is in attacking state he will stop walking (in case that this type of enemy walks)
        if (attackChance > 95) //The chance has to be very low, because this method is executing a lot of times per frame, so we must discriminate a lot of numbers
        {
            attackDirection = input.playerDirection; //Just when the enemy decides to attack the direction he is facing will be decided
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
    public void CanAttack()
    {
        canAttack = true;
    }

    public void DissappearEffect()
    {
        Spawner.sharedInstance.InstantiateSpawnEffect(this, this.transform.position);
        AudioManager.sharedInstance.PlayEnemySpawnSound();
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
        parent.enemiesAlive--;
        if (parent.enemiesAlive == 0)
            parent.completed = true;
    }

    private bool ContainsParameter(string parameter)
    {
        foreach (AnimatorControllerParameter param in anim.parameters)
        {
            if (param.name.CompareTo(parameter) == 0)
            {
                return true;
            }
        }
        return false;
    }
}

