using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChooseLengthManager : MonoBehaviour
{
    public Button[] buttons;
    public static int DUNGEON_DEPTH = 0;

    private void Start()
    {
        for (int i = 0; i < buttons.Length; i++)
            buttons[i].onClick.AddListener(AudioManager.sharedInstance.PlaySelectSound);
    }

    public void GoToGame(int depth)
    {
        Transitions.sharedInstance.TransitionToGame();
        DUNGEON_DEPTH = depth;
    }

    public void GoToMainMenu() => Transitions.sharedInstance.TransitionToMainMenu();
}
