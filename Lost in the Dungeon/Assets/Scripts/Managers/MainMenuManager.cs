using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    public Button[] buttons;

    private void Start()
    {
        for (int i = 0; i < buttons.Length; i++)
            buttons[i].onClick.AddListener(AudioManager.sharedInstance.PlaySelectSound);
    }

    public void GoToGame() => Transitions.sharedInstance.TransitionToGame();

    public void QuitGame() => Application.Quit();
}
