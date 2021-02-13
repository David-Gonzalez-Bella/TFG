using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : Interactive
{
    public string npcName;
    private Dialogue npcDialogue;
    public bool npcMission;
    public bool find;
    public string missionId;

    public override void Interact()
    {
        if (PanelsMenu.sharedInstance.panelsOpen) return;

        //If its the first interaction, the character will speak
        PlayInteractSound();

        //Write the npc´s dialogue
        string npcNoSpacesName = npcName.Replace(" ", "");
        DialogueBox.sharedInstance.dialogueIndex = 0;

        //At first, we set the NPC's dialogue to his start dialogue
        if (npcDialogue == null)
            npcDialogue = DialogueManager.sharedInstance.dialogues[npcNoSpacesName + "_S"];

        //If the NPC gives the player a mission, then it is assigned and its state is checked
        if (npcMission)
        {
            Missions_Texts.sharedInstance.AddMission(MissionsManager.sharedInstance.missions[missionId]);

            if (MissionsManager.sharedInstance.missions[missionId].completed && npcDialogue != DialogueManager.sharedInstance.dialogues[npcNoSpacesName + "_E"])
            {
                npcDialogue = DialogueManager.sharedInstance.dialogues[npcNoSpacesName + "_E"];
                Missions_Texts.sharedInstance.ClearMission(MissionsManager.sharedInstance.missions[missionId]); //The mission will be marked as "cleared" by painting it in green
                GameManager.sharedInstance.player.GetComponent<Experience>().experience += MissionsManager.sharedInstance.missions[missionId].exp; //Once we clear a mission, we will recieve XP
                if (MissionsManager.sharedInstance.missions[missionId].id.CompareTo("FirstMission") == 0)//Check if its the mission that clears the path
                {
                    Rocks.sharedInstance.ClearPath();
                }
            }
        }

        //If the NPC is ment to be found as part of a mission then the mission in question is completed, since the character has been found
        if (find && GameManager.sharedInstance.player.GetComponent<PlayerController>().activeMissions.Contains(MissionsManager.sharedInstance.missions[missionId]))//(npcDialogue.id.CompareTo("Mr.Chopy_S") == 0 && GameManager.sharedInstance.player.GetComponent<PlayerController>().activeMissions.Contains(MissionsManager.sharedInstance.missions[missionId]))
        {
            MissionsManager.sharedInstance.missions[missionId].completed = true;
        }

        //The dialogue starts
        DialogueBox.sharedInstance.StartDialogue(npcName, npcDialogue);
    }

    private void PlayInteractSound()
    {
        if (DialogueBox.sharedInstance.talking) return;

        switch (gameObject.tag)
        {
            case ("NPC"):
                AudioManager.sharedInstance.PlayTalkSoundNPC();
                break;
            case ("Cat"):
                AudioManager.sharedInstance.PlayTalkSoundCat();
                break;
        }
    }

    public void ResetDialogueNPC()
    {
        npcDialogue = null;
    }
}
