using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Scripting.APIUpdating;

public enum gameState { mainMenu, inGame, deadScreen, leavingScreen, pauseScreen }

public class GameManager : MonoBehaviour
{
    public static GameManager sharedInstance { get; private set; }
    public Transform proyectilesContiner;
    public Vector2 playerSpawnPoint;

    public GameObject player { get; private set; }
    public GameObject playerDieEffect;
    public GameObject mainMenu;

    public Camera mainCamera;

    public gameState currentGameState;

    private void Awake()
    {
        if (sharedInstance == null)
            sharedInstance = this;
        currentGameState = gameState.mainMenu;
    }
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        ScaleCamera();
    }

    //private void Update()
    //{
    //    Debug.Log(currentGameState);
    //}

    private void ScaleCamera()
    {
        Vector2 screenResolution = new Vector2(Screen.width, Screen.height);
        float srcWidth = Screen.width;
        float srcHeight = Screen.height;

        float DEVICE_SCREEN_ASPECT = srcWidth / srcHeight;
        mainCamera.aspect = DEVICE_SCREEN_ASPECT;
    }

    private void ResetStats()
    {
        //Set the base stats to its original values
        player.GetComponent<PlayerController>().atrib.baseDamage = 1;
        player.GetComponent<PlayerController>().atrib.baseSpeed = 4;
        player.GetComponent<PlayerController>().health.baseHealth = 10;
        player.GetComponent<PlayerController>().mana.baseMana = 20;
        player.GetComponent<PlayerController>().exp.level = 1;
        player.GetComponent<PlayerController>().exp.nextLevelExp = player.GetComponent<PlayerController>().exp.ExperienceCurve(player.GetComponent<PlayerController>().exp.level + 1);
        player.GetComponent<PlayerController>().exp.experience = 0;
        player.GetComponent<PlayerController>().exp.atributePoints = 0;
        Atributes_Texts.sharedInstance.UpdateAtribsTexts(player.GetComponent<PlayerController>().exp.level, player.GetComponent<PlayerController>().exp.atributePoints);
        player.GetComponent<PlayerController>().exp.CheckUnableButtons();
        player.GetComponent<PlayerController>().health.CurrentHealth = player.GetComponent<PlayerController>().health.baseHealth;
        player.GetComponent<PlayerController>().mana.CurrentMana = player.GetComponent<PlayerController>().mana.baseMana;
        player.GetComponent<PlayerController>().ResetPlayerLookAt();
    }

    private void ResetMissions()
    {
        player.GetComponent<PlayerController>().activeMissions.Clear();
        MissionsManager.sharedInstance.ResetMissionsProgress();
    }

    private void ResetDialoguesNPCs()
    {
        foreach(GameObject npc in GameObject.FindGameObjectsWithTag("NPC"))
        {
            npc.GetComponent<NPC>().ResetDialogueNPC();
        }
    }

    private void ResetTexts()
    {
        //Set the atributes texts to the base stats
        Atributes_Texts.sharedInstance.UpdateAtribsTexts(player.GetComponent<PlayerController>().atrib.damage, player.GetComponent<PlayerController>().atrib.speed);
        Atributes_Texts.sharedInstance.UpdateAtribsTexts(player.GetComponent<PlayerController>().health);
        Atributes_Texts.sharedInstance.UpdateAtribsTexts(player.GetComponent<PlayerController>().mana);

        //Set the mission texts to empty
        Missions_Texts.sharedInstance.EmptyMissions();
        Missions_Texts.sharedInstance.missionIndex = 0;
    }

    private void DestroyAllEnemies()
    {
        foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            Destroy(enemy);
        }
        foreach (GameObject spawner in GameObject.FindGameObjectsWithTag("Spawner"))
        {
            spawner.GetComponent<TriggerSpawner>().playerInside = false;
            spawner.GetComponent<TriggerSpawner>().deadEnemies = 0;
            spawner.GetComponent<BoxCollider2D>().enabled = true;
        }
    }

    private void ResetRocks()
    {
        Rocks.sharedInstance.BlockPath();
        Rocks.sharedInstance.pathCleared = false;
    }

    private void ResetWeeds()
    {
        foreach (GameObject weed in GameObject.FindGameObjectsWithTag("Weed"))
        {
            weed.GetComponent<Weed>().ActivateWeed();
        }
    }

    public void LeaveGame()
    {
        Application.Quit();
    }

    public void StartGame()
    {
        ResetMissions();
        ResetRocks();
        ResetWeeds();
        ResetStats();
        ResetDialoguesNPCs();
        ResetTexts();
        DestroyAllEnemies();
        LeavePauseScreen();
        LeaveWannaLeaveScreen();
        LeaveDeadScreen();
        foreach (SpriteRenderer spr in player.GetComponentsInChildren<SpriteRenderer>()) { spr.enabled = true; } //We now enable the minimap sprite and the main screen sprite of the player
        player.transform.position = playerSpawnPoint;
        player.GetComponent<CapsuleCollider2D>().enabled = true;
        mainMenu.SetActive(false);
    }

    public void ShowMainMenu()
    {
        LeaveDeadScreen();
        LeaveWannaLeaveScreen();
        mainMenu.SetActive(true);
    }

    public void PlayerDie()
    {
        AudioManager.sharedInstance.PlayGameOverSound();
        FreezePlayer();
        StartCoroutine(PlayerDieCorroutine());
    }

    private void LeaveDeadScreen()
    {
        MenusManager.sharedInstance.dieScreen.GetComponent<Animator>().SetBool(MenusManager.sharedInstance.dieHashCode, false);
        MenusManager.sharedInstance.ResetDieScreenValues();
        MenusManager.sharedInstance.dieScreen.gameObject.SetActive(false);
    }

    public void LeaveWannaLeaveScreen()
    {
        MenusManager.sharedInstance.wannaLeaveScreen.GetComponent<Animator>().SetBool(MenusManager.sharedInstance.leaveHashCode, false);
        MenusManager.sharedInstance.ResetWannaLeaveScreenScale();
        MenusManager.sharedInstance.wannaLeaveScreen.gameObject.SetActive(false);

        UnfreezePlayer();
        //currentGameState = gameState.inGame;
    }

    public void LeavePauseScreen()
    {
        MenusManager.sharedInstance.pauseScreen.GetComponent<Animator>().SetBool(MenusManager.sharedInstance.pauseHashCode, false);
        MenusManager.sharedInstance.ResetPauseScreenScale();
        MenusManager.sharedInstance.pauseScreen.gameObject.SetActive(false);

        Time.timeScale = 1.0f;
        UnfreezePlayer();
        //currentGameState = gameState.inGame;
    }

    public void GameStateToGame()
    {
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
        AudioManager.sharedInstance.OnPlayerDieSound += player.GetComponent<PlayerController>().PlayDieAudio;
        currentGameState = gameState.deadScreen;
        yield return new WaitForSeconds(0.5f);
        foreach (SpriteRenderer spr in player.GetComponentsInChildren<SpriteRenderer>()) { spr.enabled = false; } //We disable the minimap sprite and the main screen sprite of the player
    }
}
