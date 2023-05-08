using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MetaBuySlot : MonoBehaviour
{
    [SerializeField] private Inventory inventory;
    [SerializeField] private Text costText;

    public GameItem item;
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
