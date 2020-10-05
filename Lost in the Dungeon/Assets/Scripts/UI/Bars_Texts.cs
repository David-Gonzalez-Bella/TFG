using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Bars_Texts : MonoBehaviour
{
    public static Bars_Texts sharedInstance;

    [SerializeField] private TMP_Text healthBarTxt;
    [SerializeField] private TMP_Text magicBarTxt;
    [SerializeField] private TMP_Text expBarTxt;

    private void Awake()
    {
        if(sharedInstance == null)
        {
            sharedInstance = this;
        }
    }

    //Overloaded method to update all bars texts
    public void UpdateHealthBarTxt(Health health)
    {
        healthBarTxt.text = health.CurrentHealth.ToString() + " / " + health.baseHealth.ToString();
    }
    public void UpdateManaBarTxt(Mana mana)
    {
        magicBarTxt.text = mana.CurrentMana.ToString() + " / " + mana.baseMana.ToString();
    }
    public void UpdateExpBarTxt(Experience exp)
    {
        expBarTxt.text = exp.experience.ToString() + " / " + exp.nextLevelExp.ToString();
    }
}
