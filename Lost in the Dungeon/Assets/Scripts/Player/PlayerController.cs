﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Attack))]

public class PlayerController : MonoBehaviour
{
    private float moveX;
    private float moveY;
    private Vector2 movement;
    private bool attack;
    private bool dashing;
    private bool doDash;
    private bool throwFireball;
    private bool attacking;

    [Header("Audio sources")]
    public AudioSource attackAudioSource;
    public AudioSource damgeAudioSource;
    public AudioSource stepsAudioSource;
    public AudioSource dashAudioSource;
    public AudioSource dieAudioSource;
    public AudioSource inventaryAudioSource;

    private int xHashCode;
    private int yHashCode;
    private int runningHashCode;
    private int attackHashCode;

    [HideInInspector]
    public bool dashUnlocked;
    [HideInInspector]
    public bool fireballUnlocked;

    [HideInInspector]
    public InputPlayer input;
    private Rigidbody2D rb;
    [HideInInspector]
    public Animator anim;
    private CapsuleCollider2D col;

    [Header("Animations")]
    public AnimatorOverrideController fireAnimations;
    public AnimatorOverrideController waterAnimations;
    public AnimatorOverrideController windAnimations;

    [Header("Atributes")]
    public Mana mana;
    public Health health;
    public Experience exp;
    public Atributes atrib;
    public LayerMask interactLayer;
    private Attack atck;
    public Abilities abilities;
    private int gold = 0;

    [HideInInspector]
    public int itemsLevel = 0;
    [HideInInspector]
    public ItemSeller interactingItemSeller;
    [HideInInspector]
    public string weapon;
    [HideInInspector]
    public string spell;

    [HideInInspector]
    public int lifeSteal = 0;
    [HideInInspector]
    public int manaVamp = 0;
    [HideInInspector]
    public float dodgeChance = 0;
    [HideInInspector]
    public int galeForce = 0;

    public TriggerSpawner currentRoom;

    public int CurrentGold
    {
        get
        {
            return gold;
        }
        set
        {
            gold = value;
            Bars_Texts.sharedInstance.UpdateGoldTxt(gold);
        }
    }
    public GameObject swordFlash;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        col = GetComponent<CapsuleCollider2D>();
        atck = GetComponent<Attack>();
        abilities = GetComponent<Abilities>();
        mana = GetComponent<Mana>();
        health = GetComponent<Health>();
        exp = GetComponent<Experience>();
        input = GetComponent<InputPlayer>();

        atrib.speedIncrease = 0;
        atrib.damageIncrease = 0;
        atrib.speedCount = atrib.baseSpeed;
    }

    void Start()
    {
        xHashCode = Animator.StringToHash("X"); //Using hash (returns an int) is better than strings, as it is cheaper to compare two ints frame after frame than comparing, with this same frequency, two strings
        yHashCode = Animator.StringToHash("Y");
        runningHashCode = Animator.StringToHash("Running");
        attackHashCode = Animator.StringToHash("Attack");
        ResetPlayerLookAt();
    }

    void Update()
    {

        if (DialogueBox.sharedInstance.talking) return;
        if (GameManager.sharedInstance.currentGameState == gameState.inGame)
        {
            if (!PanelsMenu.sharedInstance.panelsOpen) //You can only move or attack if your inventary is not open
            {
                moveX = input.horizontal;
                moveY = input.vertical;
                attack = input.basicAtk && !MenusManager.sharedInstance.mouseOverInteractive;
                dashing = input.ability1 && dashUnlocked;
                throwFireball = input.ability1 && fireballUnlocked;
            }

            if (moveX != 0 || moveY != 0) //It will update the state only if the character moves. Otherwise, it will stay in the last state it entered (the knight will look at the direction he was lastly told to move to)
            {
                anim.SetFloat(xHashCode, moveX);
                anim.SetFloat(yHashCode, moveY);
                anim.SetFloat(runningHashCode, Mathf.Max(Mathf.Abs(moveX), Mathf.Abs(moveY)));
                if (!stepsAudioSource.isPlaying)
                    StartCoroutine(PlayStepsSound());
            }
            if (attack) //If we pressed the attack button/s
                anim.SetTrigger(attackHashCode);

            if (dashing && !abilities.dashing && mana.CurrentMana >= abilities.dashManaCost)
                doDash = true;

            if (throwFireball && !abilities.throwingFireball && mana.CurrentMana >= abilities.fireballManaCost)
                abilities.ThrowFireball(atck, input.faceDirection, transform.position + (Vector3)input.faceDirection);
        }
        if ((GameManager.sharedInstance.currentGameState == gameState.inGame || GameManager.sharedInstance.currentGameState == gameState.pauseScreen)
            && !PanelsMenu.sharedInstance.panelsOpen)
        {
            if (input.pause)
                MenusManager.sharedInstance.PauseGame();
        }

        if (GameManager.sharedInstance.currentGameState == gameState.inGame || GameManager.sharedInstance.currentGameState == gameState.skillsScreen)
        {
            if (input.inventary)
            {
                PanelsMenu.sharedInstance.OpenClosePanels();
            }
        }
    }

    public void AttackAnimEvent() //Called during the attack animation
    {
        if (attacking) return;
        attacking = true;
        atck.PhysicalAttack(input.faceDirection, atrib.damage, swordFlash); //This will send the direction we are facing ((1, 0), (0, 1), (-1, 0) or (0, -1))
        attackAudioSource.Play();
    }

    public void EndAttackAnimEvent()
    {
        attacking = false;
    }

    private void FixedUpdate()
    {
        if (GameManager.sharedInstance.currentGameState == gameState.inGame)
        {
            if (abilities.dashing) return; //If a dash is taking place we dont want to start a new dash nor to move with the WASD keys

            if (doDash)
            {
                dashAudioSource.Play();
                abilities.Dash(input.faceDirection.normalized);
                doDash = false;
            }
            else
            {
                movement = new Vector2(moveX, moveY) * atrib.speed; //* Time.deltaTime; -> Here i dont multiply by Time.deltaTime, as i am change the rigidbody´s velocity directly. It is not a manual update of the position, as i was doing before
                rb.velocity = movement;
            }
        }
    }

    public void PlayDieAudio()
    {
        dieAudioSource.Play();
    }

    public void PlayInventaryAudio()
    {
        inventaryAudioSource.Play();
    }

    public void ResetPlayerLookAt()
    {
        anim.SetFloat(yHashCode, -1);
    }

    //public RaycastHit2D[] Interactuables() //Returns the objects the player can interact with (that means, those that are within its CircleCastAll range)
    //{
    //    RaycastHit2D[] interactuables = Physics2D.CircleCastAll(this.transform.position, col.size.x, InputPlayer.sharedInstance.faceDirection.normalized, 1.0f, interactLayer);
    //    return interactuables;
    //}

    public Collider2D[] Interactuables() //Returns the objects the player can interact with (that means, those that are within its CircleCastAll range)
    {
        Collider2D[] interactuables = Physics2D.OverlapCircleAll(this.transform.position, col.size.x, interactLayer);
        return interactuables;
    }

    IEnumerator PlayStepsSound()
    {
        stepsAudioSource.Play();
        yield return new WaitWhile(() => stepsAudioSource.isPlaying);
    }
}
