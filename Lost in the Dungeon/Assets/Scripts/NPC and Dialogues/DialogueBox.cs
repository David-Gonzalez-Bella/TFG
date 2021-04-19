using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class DialogueBox : MonoBehaviour
{
    public static DialogueBox sharedInstance;
    public TMP_Text speaker;
    public TMP_Text content;
    public Dialogue dialogue;
    public CanvasGroup visible;
    public int dialogueIndex = 0;
    public bool talking = false;
    public bool hasBought = false;

    private void Awake()
    {
        if (sharedInstance == null)
            sharedInstance = this;
        visible = GetComponent<CanvasGroup>();
    }

    private void Start()
    {
        MakeVisible(0.0f, false);
    }

    public void StartDialogue(string speakerName, Dialogue npcDialogue, int index = 0)
    {
        dialogueIndex = index;
        MakeVisible(0.8f, true);
        FreezePlayer(npcDialogue);
        speaker.text = "-" + speakerName + ":";
        content.text = dialogue.lines[dialogueIndex];
    }

    public void NextLine()
    {
        bool gonnaBuy = GonnaBuy();
        dialogueIndex = (speaker.text.CompareTo("-Item seller:") == 0 &&
            dialogueIndex == dialogue.lines.Length - 2) ?
            dialogueIndex + 2 :
            dialogueIndex + 1;
        if (gonnaBuy || dialogueIndex >= dialogue.lines.Length)
        {
            MakeVisible(0.0f, false);
            if (!gonnaBuy)
                UnfreezePlayer();
            if (hasBought)
            {
                Spawner.sharedInstance.ItemSellerDissapear();
                GameManager.sharedInstance.player.GetComponent<PlayerController>().itemsLevel++;
                hasBought = false;
            }
            return;
        }
        content.text = dialogue.lines[dialogueIndex];
    }

    private bool GonnaBuy()
    {
        if (speaker.text.CompareTo("-Item seller:") == 0 && dialogueIndex == 2)
        {
            MenusManager.sharedInstance.ShowBuyScreen(0);
            return true;
        }
        return false;
    }

    private void FreezePlayer(Dialogue npcDialogue)
    {
        talking = true;
        GameManager.sharedInstance.FreezePlayer();
        dialogue = npcDialogue;
    }

    private void UnfreezePlayer()
    {
        talking = false;
        dialogueIndex = 0;//dialogue.lines.Length - 1;
        GameManager.sharedInstance.UnfreezePlayer();
    }

    public void MakeVisible(float alpha, bool interactable)
    {
        visible.alpha = alpha;
        visible.interactable = interactable;
    }
}
