using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSeller : Interactive
{

    public override void Interact()
    {
        if (DialogueBox.sharedInstance.visible.alpha != 0.0f) return;
        if (MenusManager.sharedInstance.wannaBuyScreen.activeSelf) return;
        if (MenusManager.sharedInstance.shopScreen.activeSelf) return;

        DialogueBox.sharedInstance.StartDialogue("Item seller", DialogueManager.sharedInstance.dialogues["ItemSeller_S"]);
        player.interactingItemSeller = this;
    }
}
