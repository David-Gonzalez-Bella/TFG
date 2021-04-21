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
    public TMP_Text warning;
    private int itemPrice;
    private int chosenItem;
    [HideInInspector]
    public Button itemButton;

    private PlayerController player;
    private bool weaponBought = false;
    private bool spellBought = false;

    private void Awake()
    {
        if (sharedInstance == null)
            sharedInstance = this;
    }

    private void Start()
    {
        player = GameManager.sharedInstance.player.GetComponent<PlayerController>();
        for (int i = 0; i < itemsBuyButtons.Length; i++)
        {
            if (int.Parse(itemsBuyButtons[i].GetComponentInChildren<TMP_Text>().text) > player.CurrentGold)
                itemsBuyButtons[i].interactable = false;
        }
        warning.enabled = player.itemsLevel == 0;
    }

    private void Update()
    {
        doneButton.interactable = ((player.itemsLevel == 0 && weaponBought && spellBought) || player.itemsLevel != 0);
        goldText.text = GameManager.sharedInstance.player.GetComponent<PlayerController>().CurrentGold.ToString();
    }

    public void SetPrice(int price) => itemPrice = price;

    public void SetItem(int index) => chosenItem = index;

    public void SetButton(Button button) => itemButton = button;

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
                    DisableAllItems(0, 3);
                break;
            case 1: //Ice sword
                player.EvolveIceSword();
                if (player.itemsLevel == 0)
                    DisableAllItems(0, 3);
                break;
            case 2: //Wind sword
                player.EvolveWindSword();
                if (player.itemsLevel == 0)
                    DisableAllItems(0, 3);
                break;
            case 3: //Fireball
                player.EvolveFireBall();
                if (player.itemsLevel == 0)
                    DisableAllItems(3, 5);
                break;
            case 4: //Dash
                player.EvolveDash();
                if (player.itemsLevel == 0)
                    DisableAllItems(3, 5);
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
