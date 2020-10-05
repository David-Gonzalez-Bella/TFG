using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueBox : MonoBehaviour
{
    public static DialogueBox sharedInstance;
    public TMP_Text speaker;
    public TMP_Text content;
    private Dialogue dialogue;
    public CanvasGroup visible;
    public int dialogueIndex = 0;
    public bool talking = false;

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

    public void StartDialogue(string speakerName, Dialogue npcDialogue)
    {
        MakeVisible(0.8f, true);
        FreezePlayer(npcDialogue);
        speaker.text = "-" + speakerName + ":";
        content.text = dialogue.lines[dialogueIndex];
    }

    public void NextLine()
    {
        dialogueIndex++;
        if (dialogueIndex >= dialogue.lines.Length)
        {
            MakeVisible(0.0f, false);
            UnfreezePlayer();
            return;
        }
        content.text = dialogue.lines[dialogueIndex]; 
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
        dialogueIndex = dialogue.lines.Length - 1;
        GameManager.sharedInstance.UnfreezePlayer();
    }

    public void MakeVisible(float alpha, bool interactable)
    {
        visible.alpha = alpha;
        visible.interactable = interactable;
    }
}
