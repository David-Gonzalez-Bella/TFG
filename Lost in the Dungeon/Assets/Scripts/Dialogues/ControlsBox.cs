using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ControlsBox : MonoBehaviour
{
    public TMP_Text content;
    public Dialogue dialogue;
    private int index = 0;

    private void Start()
    {
        index = 0;
    }

    public void StartExplanation()
    {
        content.text = dialogue.lines[index];
    }

    public void NextLine()
    {
        index = (index + 1 >= dialogue.lines.Length ? index : index + 1);
        content.text = dialogue.lines[index];
    }

    public void PreviousLine()
    {
        index = (index - 1 < 0 ? index : index - 1);
        content.text = dialogue.lines[index];
    }
}
