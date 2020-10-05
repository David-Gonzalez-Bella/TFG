using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelsMenu : MonoBehaviour
{
    public bool panelsOpen = false;

    public static PanelsMenu sharedInstance { get; private set; }
    private CanvasGroup canvasGroup;

    private void Awake()
    {
        if (sharedInstance == null)
            sharedInstance = this;

        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void OpenClosePanels()
    {
        if (panelsOpen)
        {
            ClosePanels();
        }
        else
        {
            OpenPanels(); 
        }
    }

    private void OpenPanels()
    {
        panelsOpen = true;
        canvasGroup.alpha = 1;
        canvasGroup.blocksRaycasts = true;
        canvasGroup.interactable = true;
        Time.timeScale = 0.0f;
    }
    private void ClosePanels()
    {
        panelsOpen = false;
        canvasGroup.alpha = 0;
        canvasGroup.blocksRaycasts = false;
        canvasGroup.interactable = false;
        Time.timeScale = 1.0f;
    }
}
