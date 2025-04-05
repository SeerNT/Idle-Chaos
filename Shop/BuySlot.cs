using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuySlot : MonoBehaviour
{
    [SerializeField] private Inventory inventory;
    [SerializeField] private Text costText;

    public GameItem item;
    public double cost;
    public bool isRerolled = false;
    private bool isAdded = false;

    void Start()
    {
        transform.GetChild(1).GetComponent<Button>().onClick.AddListener(BuyItem);
    }

    void BuyItem()
    {
        if(GameManager.gold >= cost && this.item != null) {
            if (inventory.HasAvailableSlots())
            {
                GameManager.gold -= cost;
                GameManager.UpdateText();
                inventory.AddItem(this.item);
            }
        }
    }

    void Update()
    {
        UpdateUI();
    }

    void UpdateUI()
    {
        if (item != null)
        {
            cost = item.baseCost;
        }
        else
        {
            cost = 0;
        }

        costText.text = cost.ToString();

        if (this.item != null && !this.isAdded)
        {
            string name = this.name;
            name = name.Replace("Slot", "");
            inventory.AddItemToShopBuySlot(this.item, int.Parse(name));
            this.isAdded = true;
        }
        if(this.isRerolled && this.item != null)
        {
            string name = this.name;
            name = name.Replace("Slot", "");
            inventory.AddItemToShopBuySlot(this.item, int.Parse(name));
            this.isAdded = true;
            this.isRerolled = false;
        }
    }

    public void SaveItem()
    {
        SaveSystem.SaveItem(item, this.name + "Buy", 1);
    }

    public void LoadItem()
    {
        ItemData data = SaveSystem.LoadItem(this.name + "Buy");

        if (data != null)
        {
            if (data.itemId != -1)
            {
                this.item = inventory.availableItems[data.itemId - 1];;
            }
        }
    }
}
