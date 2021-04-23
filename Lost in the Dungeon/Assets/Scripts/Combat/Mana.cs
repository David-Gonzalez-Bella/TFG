using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Mana : MonoBehaviour
{
    public int baseMana;
    private int currentMana = 0;
    [HideInInspector]
    public float manaRegenTime = 2.0f;
    private float currManaRegenTime = 0.0f;
    public Image manaBar;

    public int CurrentMana
    {
        get
        {
            return currentMana;
        }
        set
        {
            if (value > 0 && value <= baseMana) //If the modifyied health is between 0 and the top health we can have
            {
                currentMana = value;
            }
            else if (value > baseMana) //If the modifyied health exceeds the top health
            {
                currentMana = baseMana;
            }
            else //If the modifyied health is equal or below 0
            {
                currentMana = 0;
            }
            UpdateManaBar();
        }
    }

    private void Start()
    {
        CurrentMana += baseMana;
    }

    private void Update()
    {
        if (currentMana < baseMana) //We will regenerate the mana bar as long as it is not completely filled
        {
            if (currManaRegenTime >= manaRegenTime)
            {
                currManaRegenTime = 0.0f;
                CurrentMana += 1;
            }
            else
            {
                currManaRegenTime += Time.deltaTime;
            }
        }
    }

    public void UpdateManaBar()
    {
        manaBar.fillAmount = (float)CurrentMana / baseMana;
        Bars_Texts.sharedInstance.UpdateManaBarTxt(this);
    }

    public void ModifyMana(int quantity)
    {
        CurrentMana += quantity;
    }

    public void ModifyBaseMana(int quantity)
    {
        int oldBaseMana = baseMana;
        baseMana += quantity;
        CurrentMana = (int)(CurrentMana * baseMana / oldBaseMana);
        Atributes_Texts.sharedInstance.UpdateAtribsTexts(this);
    }
}
