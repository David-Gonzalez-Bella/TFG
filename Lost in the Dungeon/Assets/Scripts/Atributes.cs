using System.Collections;
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
    public int speedIncrease;
    [HideInInspector]
    public int damageIncrease;

    public float speed { get { return baseSpeed + speedIncrease; } }
    public int damage { get { return baseDamage + damageIncrease; } }

    public void ModifySpeed()
    {
        speedIncrease++;
        Atributes_Texts.sharedInstance.UpdateAtribsTexts(damage, speed);
    }
    public void ModifyDamage()
    {
        damageIncrease++;
        Atributes_Texts.sharedInstance.UpdateAtribsTexts(damage, speed);
    }
}
