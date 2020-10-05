using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System;

public class Interactive : MonoBehaviour
{
    protected Collider2D col;
    public PlayerController player;

    private void Start()
    {
        col = GetComponent<Collider2D>();
    }

    public void OnMouseOver()
    {
        if(!DialogueBox.sharedInstance.visible.interactable)
            CursorManager.sharedInstance.SetCursor(CursorManager.sharedInstance.handCursor);
    }
    public void OnMouseExit() => CursorManager.sharedInstance.SetCursor(CursorManager.sharedInstance.arrowCursor);
    public void OnMouseDown() //The interface´s method must be implemented (ir occurs when we click it)*/
    {
        foreach (Collider2D interactObj in player.Interactuables())
        {
            if(interactObj.gameObject == this.gameObject) //If this gameObject is within the collection of interactuable objects detected by the CircleCastAll...
            {
                Interact();
            }
        }   
    }

    public virtual void Interact()  { }
}
