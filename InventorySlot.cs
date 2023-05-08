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
        if (this.isLoaded && this.item != null && !this.isAdded && !this.name.Contains("Slot"))
        {
            
            string name = this.name;
            name = name.Replace("Inv", "");
            inventory.AddItemToSlot(this.item, int.Parse(name));
            this.isAdded = true;
        }
        else if(this.isLoaded && this.item != null && !this.isAdded && this.name.Contains("Slot"))
        {
            string name = this.name;
            name = name.Replace("Inv", "");
            inventory.AddItemToPlayerSlot(this.item, name);
            this.isAdded = true;
        }
    }

    public void SaveItem()
    {
        SaveSystem.SaveItem(item, this.name, this.lvl);
    }

    public void LoadItem()
    {
        ItemData data = SaveSystem.LoadItem(this.name);

        if (data != null)
        {
            if(data.itemId != 0 && data.itemId != -1)
            {
                this.item = inventory.availableItems[data.itemId - 1];
                this.lvl = data.lvl;
                this.isLoaded = true;
            }       
        }
    }
}
