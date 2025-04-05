using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MetaBuySlot : MonoBehaviour
{
    [SerializeField] private Inventory inventory;
    [SerializeField] private AbilityManager abilityManager;
    [SerializeField] private Text costText;

    public GameItem item;
    public GameAbility ability;
    public double cost;

    private bool isAdded = false;

    void Start()
    {
        transform.GetChild(1).GetComponent<Button>().onClick.AddListener(BuyItem);
    }

    void BuyItem()
    {
        if (GameManager.meta >= cost && this.item != null)
        {
            GameManager.meta -= cost;
            GameManager.UpdateText();

            inventory.AddItem(this.item);
        }
        else if(GameManager.meta >= cost && this.ability != null)
        {
            GameManager.meta -= cost;
            GameManager.UpdateText();
            bool inSlots = false;
            foreach(GameAbility ab in abilityManager.slots)
            {
                if(ab != null)
                {
                    if (ab.id == ability.id)
                    {
                        inSlots = true;
                        break;
                    }
                }
            }
            if(!inSlots) 
                abilityManager.AddAbility(ability);
        }
    }

    void Update()
    {
        UpdateUI();
    }

    void UpdateUI()
    {
        costText.text = cost.ToString();

        if (this.item != null && !this.isAdded)
        {
            string name = this.name;
            name = name.Replace("Slot", "");
            inventory.AddItemToMetaShopBuySlot(this.item, int.Parse(name));
            this.isAdded = true;
        }
    }
}
