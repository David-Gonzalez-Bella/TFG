using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Scripting.APIUpdating;

public enum gameState { mainMenu, inGame, deadScreen, leavingScreen, pauseScreen, inventaryScreen }

public class GameManager : MonoBehaviour
{
    public static GameManager sharedInstance { get; private set; }
    public Transform proyectilesContiner;
    public Vector2 playerSpawnPoint;

    public GameObject player { get; private set; }
    public GameObject playerDieEffect;
    public Camera mainCamera;
    public gameState currentGameState;

    private void Awake()
    {
        if (sharedInstance == null)
            sharedInstance = this;
    }
    private void Start()
    {
        currentGameState = gameState.inGame;
        player = GameObject.FindGameObjectWithTag("Player");
    }

    public void LeaveGame()
    {
        Application.Quit();
    }

    public void PlayerDie()
    {
        AudioManager.sharedInstance.PlayGameOverSound();
        FreezePlayer();
        StartCoroutine(PlayerDieCorroutine());
    }

    public void LeaveWannaLeaveScreen()
    {
        MenusManager.sharedInstance.wannaLeaveScreen.GetComponent<Animator>().SetBool(MenusManager.sharedInstance.leaveHashCode, false);
        MenusManager.sharedInstance.ResetWannaLeaveScreenScale();
        MenusManager.sharedInstance.wannaLeaveScreen.gameObject.SetActive(false);

        UnfreezePlayer();
        currentGameState = gameState.inGame;
    }

    public void LeavePauseScreen()
    {
        MenusManager.sharedInstance.pauseScreen.GetComponent<Animator>().SetBool(MenusManager.sharedInstance.pauseHashCode, false);
        MenusManager.sharedInstance.ResetPauseScreenScale();
        MenusManager.sharedInstance.pauseScreen.gameObject.SetActive(false);

        Time.timeScale = 1.0f;
        UnfreezePlayer();
        currentGameState = gameState.inGame;
    }

    public void FreezePlayer()
    {
        player.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
        player.GetComponent<Animator>().SetFloat("Running", 0.0f);
    }

    public void UnfreezePlayer()
    {
        player.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
        player.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    IEnumerator PlayerDieCorroutine()
    {
        MenusManager.sharedInstance.dieScreen.gameObject.SetActive(true);
        MenusManager.sharedInstance.dieScreen.GetComponent<Animator>().SetBool(MenusManager.sharedInstance.dieHashCode, true);
        player.GetComponent<CapsuleCollider2D>().enabled = false;
        Instantiate(playerDieEffect, player.transform.position + new Vector3(0f, -0.5f, 0f), Quaternion.identity);
        player.GetComponent<PlayerController>().PlayDieAudio();
        currentGameState = gameState.deadScreen;
        yield return new WaitForSeconds(0.5f);
        foreach (SpriteRenderer spr in player.GetComponentsInChildren<SpriteRenderer>()) { spr.enabled = false; } //We disable the minimap sprite and the main screen sprite of the player
    }
}
