using System.Collections;
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
    private bool inventary;
    private bool pause;
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

    private InputPlayer input;
    private Rigidbody2D rb;
    private Animator anim;
    private CapsuleCollider2D col;

    public Atributes atrib;
    public LayerMask interactLayer;
    private Attack atck;
    private Abilities abilities;
    public Mana mana;
    public Health health;
    public Experience exp;
    public List<Mission> activeMissions = new List<Mission>();

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
            if (!PanelsMenu.sharedInstance.panelsOpen) //You can onlu move or attack if your inventary is not open
            {
                moveX = input.horizontal;
                moveY = input.vertical;
                attack = input.basicAtk && !MenusManager.sharedInstance.mouseOverInteractive;
                dashing = input.ability1;
            }
            inventary = input.inventary; //You can always open and close your inventary once you are playing

            if (moveX != 0 || moveY != 0) //It will update the state only if the character moves. Otherwise, it will stay in the last state it entered (the knight will look at the direction he was lastly told to move to)
            {
                anim.SetFloat(xHashCode, moveX);
                anim.SetFloat(yHashCode, moveY);
                anim.SetFloat(runningHashCode, Mathf.Abs(moveX) + Mathf.Abs(moveY));
                if (!stepsAudioSource.isPlaying)
                    StartCoroutine(PlayStepsSound());
            }
            if (attack) //If we pressed the attack button/s
                anim.SetTrigger(attackHashCode);

            if (dashing && !abilities.dashing && mana.CurrentMana >= abilities.dashManaCost)
                doDash = true;

            if (inventary)
            {
                PlayInventaryAudio();
                PanelsMenu.sharedInstance.OpenClosePanels();
            }
        }
        if (GameManager.sharedInstance.currentGameState == gameState.inGame || GameManager.sharedInstance.currentGameState == gameState.pauseScreen)
        {
            pause = input.pause;
            if (pause)
                MenusManager.sharedInstance.PauseGame();
        }
    }

    public void AttackAnimEvent() //Called during the attack animation
    {
        atck.PhysicalAttack(input.faceDirection, atrib.damage, swordFlash); //This will send the direction we are facing ((1, 0), (0, 1), (-1, 0) or (0, -1))
        attackAudioSource.Play();
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
