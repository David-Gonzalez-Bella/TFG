using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MenusManager : MonoBehaviour
{
    public static MenusManager sharedInstance { get; private set; }

    public GameObject mainMenu;
    public GameObject dieScreen;
    public GameObject pauseScreen;
    public GameObject wannaLeaveScreen;

    private Vector3 pauseScreenScale;
    private Vector3 wannaLeaveScreenScale;
    public Button dieScreenPlayAgain;
    public Button dieScreenGoToMenu;
    public Button pauseButton;
    public Image dieScreenDarkBackgroundAlpha;
    public RectTransform dieScreenDarkBackgroundSize;
    public TMP_Text dieScreenYouDieTitle;

    [HideInInspector] public int pauseHashCode;
    [HideInInspector] public int leaveHashCode;
    [HideInInspector] public int dieHashCode;

    private void Awake()
    {
        if (sharedInstance == null)
            sharedInstance = this;

        pauseHashCode = Animator.StringToHash("Pause");
        leaveHashCode = Animator.StringToHash("Leaving");
        dieHashCode = Animator.StringToHash("DeadMenu");
        pauseScreenScale = pauseScreen.GetComponent<RectTransform>().localScale;
        wannaLeaveScreenScale = wannaLeaveScreen.GetComponent<RectTransform>().localScale;
    }

    private void Start()
    {
        dieScreen.gameObject.SetActive(false);
        pauseScreen.gameObject.SetActive(false);
        wannaLeaveScreen.gameObject.SetActive(false);
    }

    private void Update()
    {
        pauseButton.interactable = GameManager.sharedInstance.currentGameState == gameState.inGame;
    }

    public void PauseGame()
    {
        if(GameManager.sharedInstance.currentGameState == gameState.inGame) //Pause the game
        {
            Debug.Log("PAUSE");
            pauseScreen.gameObject.SetActive(true);
            pauseScreen.GetComponent<Animator>().SetBool(pauseHashCode, true);
            GameManager.sharedInstance.FreezePlayer();
            GameManager.sharedInstance.currentGameState = gameState.pauseScreen;
        }
        else if(GameManager.sharedInstance.currentGameState == gameState.pauseScreen)//Resume
        {
            Debug.Log("resume");
            GameManager.sharedInstance.LeavePauseScreen();
            GameManager.sharedInstance.currentGameState = gameState.inGame;
        }

    }

    public void ResetWannaLeaveScreenScale()
    {
        wannaLeaveScreenScale = new Vector3(0.001f, 0.001f, 1.0f);
    }

    public void ResetPauseScreenScale()
    {
        pauseScreenScale = new Vector3(0.001f, 0.001f, 1.0f);
    }

    public void ResetDieScreenValues()
    {
        ColorBlock cb = dieScreenPlayAgain.colors;
        cb.normalColor = new Color(1.0f, 1.0f, 1.0f, 0.0f);
        dieScreenPlayAgain.colors = cb;

        cb = dieScreenPlayAgain.colors;
        cb.normalColor = new Color(1.0f, 1.0f, 1.0f, 0.0f);
        dieScreenGoToMenu.colors = cb;

        dieScreenDarkBackgroundAlpha.color = new Color(0.0f, 0.0f, 0.0f, 0.0f);
        dieScreenDarkBackgroundSize.sizeDelta = new Vector2(1920, 0);
        dieScreenYouDieTitle.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
    }
}
