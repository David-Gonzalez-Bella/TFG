using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelExit : Interactive
{
    public override void Interact()
    {
        if (!PanelsMenu.sharedInstance.panelsOpen)
        {
            if (GameManager.sharedInstance.player.GetComponent<PlayerController>().currentRoom.enemiesAlive == 0)
            {
                MenusManager.sharedInstance.wannaLeaveScreen.gameObject.SetActive(true);
                MenusManager.sharedInstance.wannaLeaveScreen.GetComponent<Animator>().SetBool(MenusManager.sharedInstance.leaveHashCode, true);
                GameManager.sharedInstance.FreezePlayer();
                GameManager.sharedInstance.currentGameState = gameState.leavingScreen;
            }
            else
            {
                AudioManager.sharedInstance.PlayDenySound();
            }
        }
    }
}
