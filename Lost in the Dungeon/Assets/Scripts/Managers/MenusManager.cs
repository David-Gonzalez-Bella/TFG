using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class MenusManager : MonoBehaviour
{
    public static MenusManager sharedInstance { get; private set; }

    [Header("Screens")]
    public GameObject dieScreen;
    public GameObject pauseScreen;
    public GameObject wannaLeaveScreen;
    public GameObject shopScreen;
    public GameObject wannaBuyScreen;
    public GameObject sureToBuyItemScreen;
    public GameObject wannaLeaveShopScreen;

    //Buttons OnClick assignment
    [Header("Buttons")]
    public Button pauseScreen_keepPlaying_btn;
    public Button pauseScreen_goToMenu_btn;
    public Button wannaLeaveScreen_keepPlaying_btn;
    public Button wannaLeaveScreen_goToMenu_btn;
    public Button dieScreen_playAgain_btn;
    public Button dieScreen_goToMenu_btn;
    public Button pauseButton;
    public bool mouseOverInteractive;

    [Header("Extras")]
    public Image dieScreenDarkBackgroundAlpha;
    public RectTransform dieScreenDarkBackgroundSize;
    public TMP_Text dieScreenYouDieTitle;

    private Vector3 pauseScreenScale;
    private Vector3 wannaLeaveScreenScale;
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
        InitializeButtonCallbacks();
        dieScreen.gameObject.SetActive(false);
        pauseScreen.gameObject.SetActive(false);
        wannaLeaveScreen.gameObject.SetActive(false);
    }

    private void Update()
    {
        pauseButton.interactable = GameManager.sharedInstance.currentGameState == gameState.inGame;
    }

    private void InitializeButtonCallbacks()
    {
        AudioManager AM = AudioManager.sharedInstance; //FindObjectOfType<AudioManager>().GetComponent<AudioManager>();
        Transitions TRN = Transitions.sharedInstance;
        Button[] buttons = { pauseScreen_keepPlaying_btn, pauseScreen_goToMenu_btn,
                             wannaLeaveScreen_keepPlaying_btn, wannaLeaveScreen_goToMenu_btn,
                             dieScreen_playAgain_btn, dieScreen_goToMenu_btn, pauseButton };

        for (int i = 0; i < buttons.Length; i++)
            buttons[i].onClick.AddListener(AM.PlaySelectSound);

        dieScreen_playAgain_btn.onClick.AddListener(TRN.TransitionToGame);
        dieScreen_goToMenu_btn.onClick.AddListener(TRN.TransitionToMainMenu);
        pauseScreen_goToMenu_btn.onClick.AddListener(TRN.TransitionToMainMenu);
        wannaLeaveScreen_goToMenu_btn.onClick.AddListener(TRN.TransitionToMainMenu);
    }

    public void PauseGame()
    {
        if (GameManager.sharedInstance.currentGameState == gameState.inGame) //Pause the game
        {
            pauseScreen.gameObject.SetActive(true);
            pauseScreen.GetComponent<Animator>().SetBool(pauseHashCode, true);
            GameManager.sharedInstance.FreezePlayer();
            GameManager.sharedInstance.currentGameState = gameState.pauseScreen;
        }
        else if (GameManager.sharedInstance.currentGameState == gameState.pauseScreen)//Resume
        {
            GameManager.sharedInstance.LeavePauseScreen();
            GameManager.sharedInstance.currentGameState = gameState.inGame;
        }
    }

    public void ShowBuyScreen(int index)
    {
        switch (index)
        {
            case 0:
                wannaBuyScreen.SetActive(true);
                break;
            case 1:
                wannaLeaveShopScreen.SetActive(true);
                break;
            case 2:
                shopScreen.SetActive(true);
                wannaBuyScreen.SetActive(false);
                DialogueBox.sharedInstance.hasBought = true;
                break;
            case 3:
                sureToBuyItemScreen.SetActive(true);
                break;
        }
    }

    public void ExitBuyScreen(int index)
    {
        switch (index)
        {
            case 0:
                wannaBuyScreen.SetActive(false);
                DialogueBox.sharedInstance.StartDialogue("Item seller", 
                    DialogueManager.sharedInstance.dialogues["ItemSeller_S"], 
                    DialogueBox.sharedInstance.dialogue.lines.Length - 1);
                break;
            case 1:
                wannaLeaveShopScreen.SetActive(false);
                break;
            case 2:
                wannaLeaveShopScreen.SetActive(false);
                shopScreen.SetActive(false);
                DialogueBox.sharedInstance.StartDialogue("Item seller",
                    DialogueManager.sharedInstance.dialogues["ItemSeller_S"],
                    DialogueBox.sharedInstance.dialogue.lines.Length - 2);
                break;
            case 3:
                sureToBuyItemScreen.SetActive(false);
                break;
        }
    }

    public void MouseOver(bool over)
    {
        mouseOverInteractive = over;
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
        ColorBlock cb = dieScreen_playAgain_btn.colors;
        cb.normalColor = new Color(1.0f, 1.0f, 1.0f, 0.0f);
        dieScreen_playAgain_btn.colors = cb;

        cb = dieScreen_playAgain_btn.colors;
        cb.normalColor = new Color(1.0f, 1.0f, 1.0f, 0.0f);
        dieScreen_goToMenu_btn.colors = cb;

        dieScreenDarkBackgroundAlpha.color = new Color(0.0f, 0.0f, 0.0f, 0.0f);
        dieScreenDarkBackgroundSize.sizeDelta = new Vector2(1920, 0);
        dieScreenYouDieTitle.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
    }
}
