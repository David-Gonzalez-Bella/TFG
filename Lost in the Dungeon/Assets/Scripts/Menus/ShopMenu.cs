using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopMenu : MonoBehaviour
{
    public static ShopMenu sharedInstance { get; private set; }

    public Button doneButton;
    public Button buyItemButton;
    public TMP_Text goldText;
    public GameObject warning;

    public GameObject baseSwordsSet;
    public GameObject baseSpellsSet;
    public GameObject fireSwordSet_1;
    public GameObject fireSwordSet_2;
    public GameObject iceSwordSet_1;
    public GameObject iceSwordSet_2;
    public GameObject windSwordSet_1;
    public GameObject windSwordSet_2;
    public GameObject fireballSet_1;
    public GameObject fireballSet_2;
    public GameObject dashSet_1;
    public GameObject dashSet_2;

    private int itemPrice;
    private int chosenItem;
    private int chosenImprovement = 0;
    [HideInInspector]
    public Button itemButton;

    private GameObject activeSwordsSet;
    private GameObject activeSpellsSet;

    private PlayerController player;
    private bool weaponBought = false;
    private bool spellBought = false;

    private Dictionary<string, GameObject> itemSets;
    private Dictionary<string, System.Action> itemImprovements;

    private void Awake()
    {
        if (sharedInstance == null)
            sharedInstance = this;

        player = GameManager.sharedInstance.player.GetComponent<PlayerController>();
    }

    private void Start()
    {
        buyItemButton.onClick.AddListener(AudioManager.sharedInstance.PlaySpendGoldSound);

        itemSets = new Dictionary<string, GameObject>();
        itemSets.Add("FireSword_1", fireSwordSet_1);
        itemSets.Add("FireSword_2", fireSwordSet_2);
        itemSets.Add("IceSword_1", iceSwordSet_1);
        itemSets.Add("IceSword_2", iceSwordSet_2);
        itemSets.Add("WindSword_1", windSwordSet_1);
        itemSets.Add("WindSword_2", windSwordSet_2);
        itemSets.Add("Fireball_1", fireballSet_1);
        itemSets.Add("Fireball_2", fireballSet_2);
        itemSets.Add("Dash_1", dashSet_1);
        itemSets.Add("Dash_2", dashSet_2);

        itemImprovements = new Dictionary<string, System.Action>();
        itemImprovements.Add("FireSword_0_0", UnlockFireSword);
        itemImprovements.Add("FireSword_1_0", ImproveFireSword_1_0);
        itemImprovements.Add("FireSword_1_1", ImproveFireSword_1_1);
        itemImprovements.Add("FireSword_1_2", ImproveFireSword_1_2);
        itemImprovements.Add("FireSword_2_0", ImproveFireSword_1_0);
        itemImprovements.Add("FireSword_2_1", ImproveFireSword_1_1);
        itemImprovements.Add("FireSword_2_2", ImproveFireSword_1_2);

        itemImprovements.Add("IceSword_0_0", UnlockIceSword);
        itemImprovements.Add("IceSword_1_0", ImproveIceSword_1_0);
        itemImprovements.Add("IceSword_1_1", ImproveIceSword_1_1);
        itemImprovements.Add("IceSword_1_2", ImproveIceSword_1_2);
        itemImprovements.Add("IceSword_2_0", ImproveIceSword_1_0);
        itemImprovements.Add("IceSword_2_1", ImproveIceSword_1_1);
        itemImprovements.Add("IceSword_2_2", ImproveIceSword_1_2);

        itemImprovements.Add("WindSword_0_0", UnlockWindSword);
        itemImprovements.Add("WindSword_1_0", ImproveWindSword_1_0);
        itemImprovements.Add("WindSword_1_1", ImproveWindSword_1_1);
        itemImprovements.Add("WindSword_1_2", ImproveWindSword_1_2);
        itemImprovements.Add("WindSword_2_0", ImproveWindSword_1_0);
        itemImprovements.Add("WindSword_2_1", ImproveWindSword_1_1);
        itemImprovements.Add("WindSword_2_2", ImproveWindSword_1_2);

        itemImprovements.Add("Fireball_0_0", UnlockFireBall);
        itemImprovements.Add("Fireball_1_0", ImproveFireBall_1_0);
        itemImprovements.Add("Fireball_1_1", ImproveFireBall_1_1);
        itemImprovements.Add("Fireball_2_0", ImproveFireBall_1_0);
        itemImprovements.Add("Fireball_2_1", ImproveFireBall_1_1);

        itemImprovements.Add("Dash_0_0", UnlockDash);
        itemImprovements.Add("Dash_0_1", ImproveDash_1_0);
        itemImprovements.Add("Dash_1_0", ImproveDash_1_1);
        itemImprovements.Add("Dash_1_1", ImproveDash_1_0);
        itemImprovements.Add("Dash_1_2", ImproveDash_1_1);
    }

    private void Update()
    {
        doneButton.interactable = ((player.itemsLevel == 0 && weaponBought && spellBought) || player.itemsLevel > 0);
        goldText.text = GameManager.sharedInstance.player.GetComponent<PlayerController>().CurrentGold.ToString();
    }

    private void OnEnable()
    {
        InitialiceMenu();
    }

    private void OnDisable()
    {
        activeSwordsSet.SetActive(false);
        activeSpellsSet.SetActive(false);
    }

    public void SetPrice(int price) => itemPrice = price;
    public void SetItem(int index) => chosenItem = index;
    public void SetImprovement(int improvement) => chosenImprovement = improvement;
    public void SetWeapon(string weapon) => player.weapon = weapon;
    public void SetSpell(string spell) => player.spell = spell;

    public void SetButton(Button button) => itemButton = button;

    private void InitialiceMenu()
    {
        warning.SetActive(player.itemsLevel == 0);
        if (player.itemsLevel == 0)
        {
            activeSwordsSet = baseSwordsSet;
            activeSpellsSet = baseSpellsSet;
        }
        else
        {
            activeSwordsSet = itemSets[player.weapon + "_" + player.itemsLevel];
            activeSpellsSet = itemSets[player.spell + "_" + player.itemsLevel];
        }
        activeSwordsSet.SetActive(true);
        activeSpellsSet.SetActive(true);
        EnableAvailableItems();
    }

    private void EnableAvailableItems()
    {
        foreach (Button b in activeSwordsSet.GetComponentsInChildren<Button>())
        {
            if (int.Parse(b.GetComponentInChildren<TMP_Text>().text) > player.CurrentGold)
                b.interactable = false;
        }
        foreach (Button b in activeSpellsSet.GetComponentsInChildren<Button>())
        {
            if (int.Parse(b.GetComponentInChildren<TMP_Text>().text) > player.CurrentGold)
                b.interactable = false;
        }
    }

    public void DisableAllSwords()
    {
        weaponBought = true;
        foreach (Button b in activeSwordsSet.GetComponentsInChildren<Button>())
            b.interactable = false;
    }
    public void DisableAllSpells()
    {
        spellBought = true;
        foreach (Button b in activeSpellsSet.GetComponentsInChildren<Button>())
            b.interactable = false;
    }

    public void UnlockItem(int weaponType)
    {
        player.CurrentGold -= itemPrice;
        itemButton.interactable = false;
        if (chosenItem == 0)
            itemImprovements[player.weapon + "_" + player.itemsLevel + "_" + chosenImprovement].Invoke();
        else
            itemImprovements[player.spell + "_" + player.itemsLevel + "_" + chosenImprovement].Invoke();
        EnableAvailableItems();
    }

    #region Unlock items and improvements

    //Level 0
    public void UnlockFireSword()
    {
        player.anim.runtimeAnimatorController = player.fireAnimations as RuntimeAnimatorController;
        player.atrib.ModifyDamage();
        DisableAllSwords();
    }
    public void UnlockIceSword()
    {
        player.anim.runtimeAnimatorController = player.waterAnimations as RuntimeAnimatorController;
        player.mana.ModifyBaseMana(5);
        DisableAllSwords();
    }
    public void UnlockWindSword()
    {
        player.anim.runtimeAnimatorController = player.windAnimations as RuntimeAnimatorController;
        player.atrib.ModifySpeed();
        DisableAllSwords();
    }
    public void UnlockDash()
    {
        player.dashUnlocked = true;
        DisableAllSpells();
    }
    public void UnlockFireBall()
    {
        player.fireballUnlocked = true;
        DisableAllSpells();
    }

    //Level 1
    public void ImproveFireSword_1_0() => player.atrib.ModifyDamage();
    public void ImproveFireSword_1_1() => player.health.ModifyBaseHealth(5);
    public void ImproveFireSword_1_2() => player.lifeSteal = 1;

    public void ImproveIceSword_1_0() => player.mana.ModifyBaseMana(5);
    public void ImproveIceSword_1_1() => player.mana.manaRegenTime = 1.5f;
    public void ImproveIceSword_1_2() => player.manaVamp = 1;

    public void ImproveWindSword_1_0() => player.atrib.ModifySpeed(0.2f);
    public void ImproveWindSword_1_1() => player.extraGold = 1;
    public void ImproveWindSword_1_2() => player.galeForce = 1;

    public void ImproveFireBall_1_0() => player.abilities.fireballDamage++;
    public void ImproveFireBall_1_1() => player.abilities.fireballSpeed += 0.75f;

    public void ImproveDash_1_0() => player.abilities.dashSpeed += 2.5f;
    public void ImproveDash_1_1() => player.abilities.dashManaCost--;

    //Level 2
    public void ImproveFireSword_2_0() => player.atrib.ModifyDamage(2);
    public void ImproveFireSword_2_1() => player.health.ModifyBaseHealth(10);
    public void ImproveFireSword_2_2() => player.lifeSteal = 2;

    public void ImproveIceSword_2_0() => player.mana.ModifyBaseMana(10);
    public void ImproveIceSword_2_1() => player.mana.manaRegenTime = 1.0f;
    public void ImproveIceSword_2_2() => player.manaVamp = 2;

    public void ImproveWindSword_2_0() => player.atrib.ModifySpeed(0.4f);
    public void ImproveWindSword_2_1() => player.extraGold = 2;
    public void ImproveWindSword_2_2() => player.galeForce = 2;

    public void ImproveFireBall_2_0() => player.abilities.fireballDamage += 2;
    public void ImproveFireBall_2_1() => player.abilities.fireballSpeed += 0.75f;

    public void ImproveDash_2_0() => player.abilities.dashSpeed += 2.5f;
    public void ImproveDash_2_1() => player.abilities.dashManaCost--;

    #endregion
}
