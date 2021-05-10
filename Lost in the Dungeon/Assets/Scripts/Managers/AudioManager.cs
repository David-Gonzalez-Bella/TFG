using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager sharedInstance;

    //Audio Mixers
    [Header("Audio Mixers")]
    public AudioMixer musicAudioMixer;
    public AudioMixer sfxAudioMixer;

    [Header("Audios")]
    public AudioSource swordAttack;
    public AudioSource fireballAttack;
    public AudioSource rushAttack;
    public AudioSource stepsSound;
    public AudioSource backgroundMusic;
    public AudioSource enemyDamage;
    public AudioSource wallCrush;
    public AudioSource enemyCreatureDamage;
    public AudioSource enemySpawn;
    public AudioSource selectSound;
    public AudioSource gameOverSound;
    public AudioSource explosion;
    public AudioSource breakBox;
    public AudioSource interactBox;
    public AudioSource spendGold;
    public AudioSource denySound;
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

    #region Mange volume Bars Functions
    public void SetMusicVolume(float volume)
    {
        musicAudioMixer.SetFloat("MusicVolume", Mathf.Log10(volume) * 20); //We have to convert float to db, and that is done with the logarithm and *20 operation

    }

    public void SetSFX_Volume(float volume)
    {
        sfxAudioMixer.SetFloat("SFX_Volume", Mathf.Log10(volume) * 20); //We have to convert float to db, and that is done with the logarithm and *20 operation
    }


    #endregion

    #region Play Sounds Functions
    public void PlaySwordSound() => swordAttack.Play();

    public void PlayFireballSound() => fireballAttack.Play();

    public void PlaySelectSound() => selectSound.Play();

    public void PlayGameOverSound() => gameOverSound.Play();

    public void PlayWallCrush() => wallCrush.Play();

    public void PlayEnemyDamageSound() => enemyDamage.Play();

    public void PlayEnemyCreatureDamageSound() => enemyCreatureDamage.Play();

    public void PlayEnemySpawnSound() => enemySpawn.Play();

    public void PlayExplosionSound() => explosion.Play();

    public void PlaySpendGoldSound() => spendGold.Play();

    public void PlayBreakBoxSound() => breakBox.Play();

    public void PlayInteractBoxSound() => interactBox.Play();

    public void PlayDenySound() => denySound.Play();

    public void PlayTalkSoundNPC()
    {
        int random = UnityEngine.Random.Range(0, 3);
        salutesNPC[random].Play();
    }
    #endregion
}
