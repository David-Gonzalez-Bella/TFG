using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    public Button[] buttons;

    public Slider musicVolumeBar;
    public Slider sfxVolumeBar;

    private void Start()
    {
        AudioManager am = AudioManager.sharedInstance.GetComponent<AudioManager>();

        for (int i = 0; i < buttons.Length; i++)
            buttons[i].onClick.AddListener(AudioManager.sharedInstance.PlaySelectSound);

        am.musicAudioMixer.GetFloat("MusicVolume", out float musicVolume);
        am.sfxAudioMixer.GetFloat("SFX_Volume", out float sfxVolume);

        musicVolumeBar.value = (Mathf.Pow(10, (musicVolume / 20)));
        sfxVolumeBar.value = (Mathf.Pow(10, (sfxVolume / 20)));

        musicVolumeBar.onValueChanged.AddListener(am.SetMusicVolume);
        sfxVolumeBar.onValueChanged.AddListener(am.SetSFX_Volume);
    }

    public void GoToChooseLength() => Transitions.sharedInstance.TransitionToChooseLength();

    public void GoToControls() => Transitions.sharedInstance.TransitionToControls();

    public void QuitGame() => Application.Quit();
}
