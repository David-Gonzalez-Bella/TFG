using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AudioManager : MonoBehaviour
{
    public static AudioManager sharedInstance;

    public AudioSource swordAttack;
    public AudioSource fireballAttack;
    public AudioSource rushAttack;
    public AudioSource stepsSound;
    public AudioSource flySound;
    public AudioSource backgroundMusic;
    public AudioSource enemyDamage;
    public AudioSource wallCrush;
    public AudioSource enemyCreatureDamage;
    public AudioSource enemySpawn;
    public AudioSource collectWeed;
    public AudioSource selectSound;
    public AudioSource gameOverSound;
    public AudioSource miauSound;
    public AudioSource[] salutesNPC;

    private void Awake()
    {
        if (sharedInstance == null)
            sharedInstance = this;
        else
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
    }

    #region Play Sounds Functions
    public void PlaySwordSound() => swordAttack.Play();

    public void PlayFireballSound() => fireballAttack.Play();

    public void PlayWeedCollectedSound() => collectWeed.Play();

    public void PlaySelectSound() => selectSound.Play();

    public void PlayGameOverSound() => gameOverSound.Play();

    public void PlayWallCrush() => wallCrush.Play();

    public void PlayEnemyDamageSound() => enemyDamage.Play();

    public void PlayEnemyCreatureDamageSound() => enemyCreatureDamage.Play();

    public void PlayEnemySpawnSound() => enemySpawn.Play();

    public void PlayTalkSoundCat() => miauSound.Play();

    public void PlayTalkSoundNPC()
    {
        int random = UnityEngine.Random.Range(0, 3);
        salutesNPC[random].Play();
    }
    #endregion
}
