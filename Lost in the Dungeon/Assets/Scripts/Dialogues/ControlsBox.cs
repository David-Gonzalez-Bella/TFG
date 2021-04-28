using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ControlsBox : MonoBehaviour
{
    public Button nextButton;
    public Button prevButton;
    public TMP_Text content;
    public Dialogue dialogue;
    private int index = 0;

    private void Start()
    {
        index = 0;
    }

    private void Update()
    {
        nextButton.interactable = (index + 1) < (dialogue.lines.Length);
        prevButton.interactable = index - 1 >= 0;
    }

    public void StartExplanation()
    {
        content.text = dialogue.lines[index];
    }

    public void NextLine()
    {
        index++;
        SetText();
    }

    public void PreviousLine()
    {
        index--;
        SetText();
    }

    public void SetText()
    {
        content.text = dialogue.lines[index];
    }
}
