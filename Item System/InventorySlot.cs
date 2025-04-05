using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    [SerializeField] private Inventory inventory;

    public GameItem item;
    private bool isAdded = false;
    private bool isLoaded = false;
    public int lvl = 1;

    void Update()
    {
        UpdateUI();
    }

    void UpdateUI()
    {
        if (transform.parent.name.Contains("CollectionItems"))
        {
            if (this.item != null && !this.isAdded)
            {
                string name = this.name;
                name = name.Replace("Inv", "");
                inventory.AddItemToCollectionSlot(this.item, int.Parse(name));
                this.isAdded = true;
            }
        }
        else if (transform.name.Contains("SpecialSlot"))
        {
            if (this.isLoaded && this.item != null && !this.isAdded)
            {
                inventory.AddItemToSeaSlot(this.item);
                this.isAdded = true;
            }
        }
        else
        {
            if (this.isLoaded && this.item != null && !this.isAdded && !this.name.Contains("Slot"))
            {

                string name = this.name;
                name = name.Replace("Inv", "");
                inventory.AddItemToSlot(this.item, int.Parse(name));
                this.isAdded = true;
            }
            else if (this.isLoaded && this.item != null && !this.isAdded && this.name.Contains("Slot"))
            {
                string name = this.name;
                name = name.Replace("Inv", "");
                inventory.AddItemToPlayerSlot(this.item, name);
                this.isAdded = true;
            }
        }
    }

    public void SaveItem()
    {
        SaveSystem.SaveItem(item, this.name, this.lvl, transform.parent.name.Contains("CollectionItems"));
    }

    public void LoadItem()
    {
        if (transform.parent.name.Contains("CollectionItems"))
        {
            string name = this.name;
            name = name.Replace("Inv", "");
            ItemData data = SaveSystem.LoadItem(name, transform.parent.name.Contains("CollectionItems"));

            if (data != null)
            {
                if (data.itemId != 0 && data.itemId != -1)
                {
                    this.item = inventory.availableItems[data.itemId - 1];
                    this.lvl = data.lvl;
                    this.isLoaded = true;
                }
            }
        }
        else
        {
            ItemData data = SaveSystem.LoadItem(this.name);

            if (data != null)
            {
                if (data.itemId != 0 && data.itemId != -1)
                {
                    this.item = inventory.availableItems[data.itemId - 1];
                    this.lvl = data.lvl;
                    this.isLoaded = true;
                }
            }
        }
    }
}
