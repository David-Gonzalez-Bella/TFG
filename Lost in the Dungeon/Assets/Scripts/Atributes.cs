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

    private int speedIncrease = 0;
    private int damageIncrease = 0;

    public float speed { get { return baseSpeed + speedIncrease; } }
    public int damage { get { return baseDamage + damageIncrease; } }

    public void ModifyBaseSpeed()
    {
        baseSpeed++;
        Atributes_Texts.sharedInstance.UpdateAtribsTexts(baseDamage, baseSpeed);
    }
    public void ModifyBaseDamage()
    {
        baseDamage++;
        Atributes_Texts.sharedInstance.UpdateAtribsTexts(baseDamage, baseSpeed);
    }
}
