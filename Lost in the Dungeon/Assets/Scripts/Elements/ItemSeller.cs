using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSeller : Interactive
{

    public override void Interact()
    {
        DialogueBox.sharedInstance.StartDialogue("Item seller", DialogueManager.sharedInstance.dialogues["ItemSeller_S"]);
    }
}
