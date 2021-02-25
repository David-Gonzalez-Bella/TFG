using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class Transitions : MonoBehaviour
{
    public static Transitions sharedInstance { get; private set; }

    private Animator anim;
    private Canvas canvas;
    private int toGameTriggerHash;
    private int toMainMenuTriggerHash;

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
        anim = GetComponentInChildren<Animator>();
        canvas = GetComponent<Canvas>();
    }

    private void Start()
    {
        canvas.sortingOrder = 0;
        canvas.enabled = false;
        toGameTriggerHash = Animator.StringToHash("TransitionToGame");
        toMainMenuTriggerHash = Animator.StringToHash("TransitionToMainMenu");
    }

    public void GoToMainMenu() => SceneManager.LoadScene(0);
    public void GoToGame() => SceneManager.LoadScene(1);

    public void TransitionToGame()
    {
        PrepareTransition();
        anim.SetTrigger(toGameTriggerHash);
    }

    public void TransitionToMainMenu()
    {
        PrepareTransition();
        anim.SetTrigger(toMainMenuTriggerHash);
    }

    private void PrepareTransition()
    {
        canvas.enabled = true;
        canvas.sortingOrder = 3;
    }

    public void EndFadeTransition()
    {
        canvas.sortingOrder = 0;
        canvas.enabled = false;
    }

    //public void EndFadeTransitionToGame()
    //{
    //    canvas.sortingOrder = 0;
    //    canvas.enabled = false;
    //    GameManager.sharedInstance.currentGameState = gameState.inGame;
    //}

    //public void EndFadeTransitionToMenu()
    //{
    //    canvas.sortingOrder = 0;
    //    canvas.enabled = false;
    //    GameManager.sharedInstance.currentGameState = gameState.mainMenu;
    //}

}
