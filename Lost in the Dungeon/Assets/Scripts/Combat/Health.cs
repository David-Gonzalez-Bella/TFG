using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class Health : MonoBehaviour
{
    public int baseHealth;
    private int currentHealth = 0;
    public Image healthBar;
    public UnityEvent dieEvent; //This is not really necessary, we could do it with a reference to the GameObject´s animator, but its usefull to have an example of UnityEvents

    public int CurrentHealth
    {
        get
        {
            return currentHealth;
        }
        set
        {
            if (value > 0 && value <= baseHealth) //If the modifyied health is between 0 and the top health we can have
            {
                currentHealth = value;
            }
            else if (value > baseHealth) //If the modifyied health exceeds the top health
            {
                currentHealth = baseHealth;
            }
            else //If the modifyied health is equal or below 0
            {
                currentHealth = 0;
                if (gameObject.GetComponent<Enemy>() != null) gameObject.GetComponent<Enemy>().DeadInZone();
                dieEvent?.Invoke(); //The '?' check is just to avoid a possible error in case that the envent wasn´t dropped in the editor, but it will always invoke 
            }
            UpdateHealthBar();
        }
    }

    void Start()
    {
        CurrentHealth += baseHealth;
    }

    public void ModifyHealth(float quantity) //This metod will be used to take damage or to heal
    {
        CurrentHealth += (int)quantity;
    }

    public void DestroyCharacter() //Called during the character´s death animation
    {
        Destroy(this.gameObject);
    }

    public void UpdateHealthBar()
    {
        if (healthBar != null)
        {
            healthBar.fillAmount = (float)CurrentHealth / baseHealth;
            Bars_Texts.sharedInstance.UpdateHealthBarTxt(this);
        }
    }

    public void ModifyBaseHealth(int quantity)
    {
        int oldBaseHealth = baseHealth;
        baseHealth += quantity;
        CurrentHealth = (int)(CurrentHealth * baseHealth / oldBaseHealth);
        Atributes_Texts.sharedInstance.UpdateAtribsTexts(this);
    }

    public IEnumerator HealEffect()
    {
        GetComponentInChildren<SpriteRenderer>().color = Color.green;
        yield return new WaitForSeconds(0.25f);
        GetComponentInChildren<SpriteRenderer>().color = Color.white;
    }
}
