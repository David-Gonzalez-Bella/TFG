using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using TMPro;

public class Experience : MonoBehaviour
{
    public float nextLevelExp;
    private float currentExp = 0.0f;
    private float porcentageExp = 0.0f;
    public int atributePoints = 0;
    public Image experienceBar;
    public  List <ActiveButton> UI_Buttons;

    [SerializeField] private TMP_Text levelImg;

    public int level = 1;  //We start at level 1
    public float experience //This will manage our experience, respecting our limits and taking into account the current experience we have
    {
        get
        {
            return currentExp;
        }
        set
        {
            currentExp = value;
            porcentageExp = currentExp / nextLevelExp;
            if (currentExp >= nextLevelExp) //If we have exceeded the experience needed to level up
            {
                while (porcentageExp >= 1)
                {
                    currentExp -= nextLevelExp;
                    LevelUp();
                }
            }
            UpdateExperienceBar(); //After any variation in our experience we´ll update the exp bar in the UI
        }
    }

    private void Start()
    {
        nextLevelExp = ExperienceCurve(level + 1); //We start with level 2 as the level we have to achieve
        UpdateExperienceBar();
        Atributes_Texts.sharedInstance.UpdateAtribsTexts(level, atributePoints);
        CheckUnableButtons();
    }

    public float ExperienceCurve(int level) //The level is the "x" of our function. Returns the experience of a concrete level
    {
        float function = Mathf.Log10(level) * 20; //This function stablishes a relationship between level and experience (for each value of level, there will be an amount of experience associated)
        float experience = Mathf.Ceil(function); //our "y" (the experience) depends on the function that involves the "x" (level) value

        return experience;
    }

    private void UpdateExperienceBar()
    {
        experienceBar.fillAmount = porcentageExp;
        Bars_Texts.sharedInstance.UpdateExpBarTxt(this);
    }

    private void LevelUp()
    {
        level++;
        nextLevelExp = ExperienceCurve(level);
        porcentageExp = currentExp / nextLevelExp; ;
        UpdateAtributePoins(1);
        TextHitGenerator.sharedInstance.CreateTextHit(Color.green, this.transform, "LVL UP!");
    }

    public void UpdateAtributePoins(int quantity)
    {
        atributePoints += quantity;
        Atributes_Texts.sharedInstance.UpdateAtribsTexts(level, atributePoints);
        CheckUnableButtons();
    }
    public void CheckUnableButtons()
    {
        foreach (ActiveButton button in UI_Buttons)
        {
            button.EnableDisableButton(atributePoints);
        }
    }

    [ContextMenu("Autofill Buttons")]
    public void AutofillButtons()
    {
        UI_Buttons = FindObjectsOfType<ActiveButton>().Where(t => t.name.Contains("PointUp")).ToList();
    }
}
