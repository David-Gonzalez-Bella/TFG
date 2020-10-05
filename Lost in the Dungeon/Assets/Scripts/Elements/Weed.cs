using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weed : Interactive
{
    public Vector3 position;
    public SpriteRenderer minimapWeed;

    private void Start()
    {
        ActivateWeed();
    }

    public override void Interact()
    {
        if (!PanelsMenu.sharedInstance.panelsOpen)
        {
            if (GameManager.sharedInstance.player.GetComponent<PlayerController>().activeMissions.Contains(MissionsManager.sharedInstance.missions["GetWeeds"]))
            {
                AudioManager.sharedInstance.OnWeedCollectedSound += AudioManager.sharedInstance.PlayWeedCollectedSound;
                Missions_Texts.sharedInstance.CheckUpdateMission(MissionsManager.sharedInstance.missions["GetWeeds"]);
                minimapWeed.enabled = false;
                GetComponent<SpriteRenderer>().enabled = false;
                GetComponent<BoxCollider2D>().enabled = false;

            }
        }
    }

    public void ActivateWeed()
    {
        gameObject.transform.position = position;
        GetComponent<SpriteRenderer>().enabled = true;
        GetComponent<BoxCollider2D>().enabled = true;
        GetComponentInChildren<SpriteRenderer>().enabled = true;
    }
}
