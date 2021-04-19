using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSeller : Interactive
{
    public GameObject baseSeller;
    public GameObject midSeller;
    public GameObject advancedSeller;
    public int requiredLevel;

    public void ChooseModel(int type)
    {
        switch (type)
        {
            case 1:
                baseSeller.SetActive(true);
                requiredLevel = 0;
                break;
            case 4:
                midSeller.SetActive(true);
                requiredLevel = 1;
                break;
            case 7:
                advancedSeller.SetActive(true);
                requiredLevel = 2;
                break;
        }
    }

    public override void Interact()
    {
        if (DialogueBox.sharedInstance.visible.alpha != 0.0f) return;
        if (MenusManager.sharedInstance.wannaBuyScreen.activeSelf) return;
        if (MenusManager.sharedInstance.shopScreen.activeSelf) return;

        if(player.itemsLevel < requiredLevel)
            DialogueBox.sharedInstance.StartDialogue("Item seller", DialogueManager.sharedInstance.dialogues["ItemSeller_N"]);
        else
            DialogueBox.sharedInstance.StartDialogue("Item seller", DialogueManager.sharedInstance.dialogues["ItemSeller_S"]);
        player.interactingItemSeller = this;
    }
}
