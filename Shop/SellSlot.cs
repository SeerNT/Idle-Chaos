using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SellSlot : MonoBehaviour
{
    [SerializeField] private Inventory inventory;
    [SerializeField] private Text costText;

    public GameItem item;
    private double cost;
    private bool isAdded = false;
    public int lvl = 1;

    void Start()
    {
        transform.GetChild(1).GetComponent<Button>().onClick.AddListener(SellItem);
    }

    void SellItem()
    {
        if (this.item != null)
        {
            GameManager.gold += cost;
            GameManager.UpdateText();

            this.item = null;
            GameObject itemObj = transform.Find("Item").gameObject;
            Destroy(itemObj);
        }
    }

    void Update()
    {
        UpdateUI();
    }

    void UpdateUI()
    {
        if(item != null)
        {
            cost = Math.Floor(item.baseCost - (item.baseCost * 0.1));
        }
        else
        {
            cost = 0;
        }
        
        costText.text = cost.ToString();
        if (this.item != null && !this.isAdded && transform.Find("Item") == null)
        {
            string name = this.name;
            name = name.Replace("Slot", "");
            inventory.AddItemToShopSellSlot(this.item, int.Parse(name));
            this.isAdded = true;
        }
    }

    public void SaveItem()
    {
        SaveSystem.SaveItem(item, this.name + "Sell", this.lvl);
    }

    public void LoadItem()
    {
        ItemData data = SaveSystem.LoadItem(this.name + "Sell");

        if (data != null)
        {
            if (data.itemId != -1)
            {
                this.item = inventory.availableItems[data.itemId - 1]; 
                this.lvl = data.lvl;
            }
        }
    }
}
