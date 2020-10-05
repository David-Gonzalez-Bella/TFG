using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActiveButton : MonoBehaviour
{
    private Button button;

    private void Awake()
    {
        button = GetComponent<Button>();
    }
    public void EnableDisableButton(int attributePoints) 
    {
        button.interactable = attributePoints > 0 ? true : false;
    }
}
