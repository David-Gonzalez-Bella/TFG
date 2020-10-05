using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AudioManager : MonoBehaviour
{
    public static AudioManager sharedInstance;

    public AudioSource swordAttack;
    public AudioSource fireballAttack;
    public AudioSource stepsSound;
    public AudioSource backgroundMusic;
    public AudioSource enemyDamage;
    public AudioSource enemySpawn;
    public AudioSource collectWeed;
    public AudioSource selectSound;
    public AudioSource gameOverSound;
    public AudioSource miauSound;
    public AudioSource[] salutesNPC;

    public event Action OnPlaySwordSound;
    public event Action OnPlayFireballSound;
    public event Action OnStepsSound;
    public event Action OnEnemyDamageSound;
    public event Action OnEnemySpawnSound;
    public event Action OnWeedCollectedSound;
    public event Action OnPlayerDieSound;
    public event Action OnTalkSoundNPC;


    private void Awake()
    {
        if (sharedInstance == null)
            sharedInstance = this;
    }

    private void Update()
    {
        OnPlaySwordSound?.Invoke();
        OnPlayFireballSound?.Invoke();
        OnStepsSound?.Invoke();
        OnEnemyDamageSound?.Invoke();
        OnEnemySpawnSound?.Invoke();
        OnWeedCollectedSound?.Invoke();
        OnPlayerDieSound?.Invoke();
        OnTalkSoundNPC?.Invoke();
    }

    public void PlayWeedCollectedSound()
    {
        collectWeed.Play();
        OnWeedCollectedSound -= PlayWeedCollectedSound;
    }

    public void PlaySelectSound()
    {
        selectSound.Play();
    }

    public void PlayGameOverSound()
    {
        gameOverSound.Play();
    }

    public void PlayTalkSoundNPC()
    {
        int random = UnityEngine.Random.Range(0, 3);
        salutesNPC[random].Play();
        OnTalkSoundNPC -= PlayTalkSoundNPC;
    }

    public void PlayTalkSoundCat()
    {
        miauSound.Play();
        OnTalkSoundNPC -= PlayTalkSoundCat;
    }
}
