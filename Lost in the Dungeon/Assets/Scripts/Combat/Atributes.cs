﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Atributes")]
public class Atributes : ScriptableObject //Attributes is only a bunch of data, so theres no need for this class to inherit from MonoBehaviour. Instead it will be an ScriptableObject, so any gameobject can have an "instance" of this data and choose its values for itself
{
    [Tooltip("Character´s speed")]
    public float baseSpeed;

    [Tooltip("Character´s damage")]
    public  int baseDamage;

    [HideInInspector]
    public float speedIncrease;
    [HideInInspector]
    public int damageIncrease;

    public float speed { get { return baseSpeed + speedIncrease; } }
    public int damage { get { return baseDamage + damageIncrease; } }

    [HideInInspector]
    public float speedCount;

    public void ModifySpeed(float quantity = 0.2f)
    {
        speedIncrease += quantity;
        Atributes_Texts.sharedInstance.UpdateAtribsTexts(damage, ++speedCount);
    }
    public void ModifyDamage(int quantity = 1)
    {
        damageIncrease += quantity;
        Atributes_Texts.sharedInstance.UpdateAtribsTexts(damage, speedCount);
    }
}
