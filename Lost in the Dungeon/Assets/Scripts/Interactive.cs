using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System;

public class Interactive : MonoBehaviour
{
    protected Collider2D col;
    //[HideInInspector]
    public PlayerController player;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        col = GetComponent<Collider2D>();
    }

    public virtual void Interact() { }

    public void OnMouseOver()
    {
        if (DialogueBox.sharedInstance.visible.interactable) return;
        if (MenusManager.sharedInstance.mouseOverInteractive) return;
        CursorManager.sharedInstance.SetCursor(CursorManager.sharedInstance.handCursor);
        MenusManager.sharedInstance.MouseOver(true);
    }
    public void OnMouseExit() => StopInteracting();

    private void OnDestroy() => StopInteracting();

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

    private void StopInteracting()
    {
        CursorManager.sharedInstance.SetCursor(CursorManager.sharedInstance.arrowCursor);
        MenusManager.sharedInstance.MouseOver(false);
    }
}
