using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Atributes_Texts : MonoBehaviour
{
    public static Atributes_Texts sharedInstance;

    [SerializeField] private Atributes playerAtribs;
    [SerializeField] private Health playerHealth;
    [SerializeField] private Mana playerMana;
    [SerializeField] private TMP_Text lvlAtribText;
    [SerializeField] private TMP_Text attackAtribTxt;
    [SerializeField] private TMP_Text velocityAtribTxt;
    [SerializeField] private TMP_Text magicAtribTxt;
    [SerializeField] private TMP_Text healthAtribTxt;
    [SerializeField] private TMP_Text atribPointsTxt;


    private void Awake()
    {
        if(sharedInstance == null)
        {
            sharedInstance = this;
        }
    }

    private void Start()
    {
        UpdateAtribsTexts(playerAtribs.damage, playerAtribs.speed);
        UpdateAtribsTexts(playerHealth);
        UpdateAtribsTexts(playerMana);
    }

    public void UpdateAtribsTexts(int level, int atribPoints)
    {
        lvlAtribText.text = level.ToString() + " -";
        atribPointsTxt.text = atribPoints.ToString();
    }
    public void UpdateAtribsTexts(int atckPoints, float velocityPoints) //These will be updated from the Atributes script
    {
        attackAtribTxt.text = (atckPoints).ToString();
        velocityAtribTxt.text = ((int)velocityPoints).ToString();
    }
    public void UpdateAtribsTexts(Health health)
    {
        healthAtribTxt.text = health.baseHealth.ToString();
    }
    public void UpdateAtribsTexts(Mana mana)
    {
        magicAtribTxt.text = mana.baseMana.ToString();
    }
}
