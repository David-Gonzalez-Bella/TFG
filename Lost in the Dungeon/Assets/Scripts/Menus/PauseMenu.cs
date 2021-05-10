using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public Slider musicVolumeBar;
    public Slider sfxVolumeBar;

    private void Start()
    {
        AudioManager am = AudioManager.sharedInstance.GetComponent<AudioManager>();

        am.musicAudioMixer.GetFloat("MusicVolume", out float musicVolume);
        am.sfxAudioMixer.GetFloat("SFX_Volume", out float sfxVolume);

        musicVolumeBar.value = (Mathf.Pow(10, (musicVolume / 20)));
        sfxVolumeBar.value = (Mathf.Pow(10, (sfxVolume / 20)));

        musicVolumeBar.onValueChanged.AddListener(am.SetMusicVolume);
        sfxVolumeBar.onValueChanged.AddListener(am.SetSFX_Volume);
    }

    public void TimeScaleStop() => Time.timeScale = 0.0f;
    public void TimeScaleResume() => Time.timeScale = 1.0f;
}
