using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopMenu : MonoBehaviour
{
    public static ShopMenu sharedInstance { get; private set; }

    public Button[] itemsBuyButtons;
    public Button doneButton;
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
    [HideInInspector]
    public Button itemButton;

    private PlayerController player;
    private bool weaponBought = false;
    private bool spellBought = false;

    private Dictionary<string, GameObject> itemSets;

    private void Awake()
    {
        if (sharedInstance == null)
            sharedInstance = this;

        player = GameManager.sharedInstance.player.GetComponent<PlayerController>();
    }

    private void Start()
    {
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

    public void SetPrice(int price) => itemPrice = price;

    public void SetItem(int index) => chosenItem = index;

    public void SetButton(Button button) => itemButton = button;

    private void InitialiceMenu()
    {
        for (int i = 0; i < itemsBuyButtons.Length; i++)
        {
            if (int.Parse(itemsBuyButtons[i].GetComponentInChildren<TMP_Text>().text) > player.CurrentGold)
                itemsBuyButtons[i].interactable = false;
        }
        warning.SetActive(player.itemsLevel == 0);
        if (player.itemsLevel == 0)
        {
            baseSwordsSet.SetActive(true);
            baseSpellsSet.SetActive(true);
        }
        else
        {
            itemSets[player.weapon + "_" + player.itemsLevel].SetActive(true);
            itemSets[player.spell + "_" + player.itemsLevel].SetActive(true);
        }
    }

    public void UnlockItem()
    {
        PlayerController player = GameManager.sharedInstance.player.GetComponent<PlayerController>();
        player.CurrentGold -= itemPrice;
        itemButton.interactable = false;
        switch (chosenItem)
        {
            case 0: //Fire sword
                player.EvolveFireSword();
                if (player.itemsLevel == 0)
                {
                    DisableAllItems(0, 3);
                    player.weapon = "FireSword";
                }
                break;
            case 1: //Ice sword
                player.EvolveIceSword();
                if (player.itemsLevel == 0)
                {
                    DisableAllItems(0, 3);
                    player.weapon = "IceSword";
                }
                break;
            case 2: //Wind sword
                player.EvolveWindSword();
                if (player.itemsLevel == 0)
                {
                    DisableAllItems(0, 3);
                    player.weapon = "WindSword";
                }
                break;
            case 3: //Fireball
                player.EvolveFireBall();
                if (player.itemsLevel == 0)
                {
                    DisableAllItems(3, 5);
                    player.spell = "Fireball";
                }
                break;
            case 4: //Dash
                player.EvolveDash();
                if (player.itemsLevel == 0)
                {
                    DisableAllItems(3, 5);
                    player.spell = "Dash";
                }
                break;
        }
    }

    public void DisableAllItems(int start, int limit)
    {
        if (start == 0)
            weaponBought = true;
        else
            spellBought = true;
        for (int i = start; i < limit; i++)
            itemsBuyButtons[i].interactable = false;
    }
}
