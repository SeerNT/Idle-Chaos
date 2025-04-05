using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    [SerializeField] private Transform metaShop;
    [SerializeField] private Transform shop;
    [SerializeField] private GameObject itemStatsObject;
    [SerializeField] private CollectionManager collection;
    [SerializeField] private InventorySlot seaSlot;
    public Transform playerEqp;

    public GameItem[] slots = new GameItem[60];
    public GameItem[] playerSlots = new GameItem[8];
    public GameItem[] buySlots = new GameItem[5];
    public GameItem[] sellSlots = new GameItem[5];

    public GameItem[] availableItems;

    public Dictionary<int, GameItem> itemsPool = new Dictionary<int, GameItem>();

    public void Awake()
    {
        for (int i = 0; i < availableItems.Length; i++)
        {
            itemsPool.Add(i + 1, availableItems[i]);
        }
    }

    public GameObject AddGameObject(int slotId) {
        GameObject go = new GameObject("Item");
        Transform slot = transform.Find("Inv" + slotId);
        
        go.GetComponent<Transform>().SetParent(slot);

        return go;
    }

    public GameObject AddGameObjectForSeaSlot()
    {
        GameObject go = new GameObject("Item");
        Transform slot = seaSlot.transform;
        go.GetComponent<Transform>().SetParent(slot);

        return go;
    }

    public GameObject AddGameObjectForCollection(int slotId)
    {
        GameObject go = new GameObject("Item");
        Transform slot = collection.slots.Find("Inv" + slotId);

        go.GetComponent<Transform>().SetParent(slot);

        return go;
    }

    public GameObject AddGameObjectForMetaShop(int slotId)
    {
        GameObject go = new GameObject("Item");
        Transform slot = null;

        slot = metaShop.Find("UniqueItems").Find("BuyAvailable").Find("Slot" + slotId);


        go.GetComponent<Transform>().SetParent(slot);

        return go;
    }

    public GameObject AddGameObjectForShop(int slotId, bool forBuy)
    {
        GameObject go = new GameObject("Item");
        Transform slot = null;
        if (forBuy)
        {
            slot = shop.Find("BuyAvailable").Find("Slot" + slotId);
        }
        else
        {
            slot = shop.Find("SellAvailable").Find("Slot" + slotId);
        }
        

        go.GetComponent<Transform>().SetParent(slot);

        return go;
    }

    public GameObject AddGameObjectToPlayer(string slotId)
    {
        GameObject go = new GameObject("Item");
        Transform slot = playerEqp.Find(slotId);
        go.GetComponent<Transform>().SetParent(slot);

        return go;
    }

    public void PrepareGameObject(GameObject go, GameItem item) {
        go.AddComponent<Item>();
        go.GetComponent<Item>().item = item;
        go.AddComponent<Image>();
        go.GetComponent<Image>().sprite = go.GetComponent<Item>().item.icon;
        go.AddComponent<CanvasGroup>();
        go.AddComponent<DragDrop>();
        go.GetComponent<DragDrop>().canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
        go.transform.localPosition = Vector3.zero;
        go.GetComponent<RectTransform>().sizeDelta = new Vector2(90, 90);
        go.GetComponent<RectTransform>().localScale = new Vector2(1, 1);
    }

    public void PrepareGameObjectForCollection(GameObject go, GameItem item)
    {
        go.AddComponent<Item>();
        go.GetComponent<Item>().item = item;
        go.AddComponent<Image>();
        go.GetComponent<Image>().sprite = go.GetComponent<Item>().item.icon;
        go.AddComponent<CanvasGroup>();
        //go.AddComponent<DragDrop>();
        //go.GetComponent<DragDrop>().canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
        go.transform.localPosition = Vector3.zero;
        go.GetComponent<RectTransform>().sizeDelta = new Vector2(135, 135);
        go.GetComponent<RectTransform>().localScale = new Vector2(1, 1);
    }

    public bool HasAvailableSlots()
    {
        bool isSuccess = false;
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i] == null)
            {
                isSuccess = true;
                break;
            }
        }
        return isSuccess;
    }

    public bool AddItem(GameItem item) {
        bool isSuccess = false;
        for (int i = 0; i < slots.Length; i++) {
            if (slots[i] == null)
            {
                slots[i] = item;
                GameObject go = AddGameObject(i + 1);
                Transform slot = transform.Find("Inv" + (i+1).ToString());
                PrepareGameObject(go, item);
                slot.GetComponent<InventorySlot>().item = item;
                slot.GetComponent<InventorySlot>().lvl = 1;
                isSuccess = true;
                if(collection.collectionSlots[int.Parse(item.id)-1].item == null)
                {
                    AddItemToCollectionSlot(item, int.Parse(item.id));
                }
                break;
            }
        }
        return isSuccess;
    }

    public bool AddItemToMetaShopBuySlot(GameItem item, int slotId)
    {
        bool isSuccess = false;

        GameObject go = AddGameObjectForMetaShop(slotId);
        PrepareGameObject(go, item);
        isSuccess = true;

        return isSuccess;
    }

    public bool AddItemToShopBuySlot(GameItem item, int slotId)
    {
        bool isSuccess = false;
        if (buySlots[slotId - 1] == null)
        {
            buySlots[slotId - 1] = item;
            GameObject go = AddGameObjectForShop(slotId, true);
            PrepareGameObject(go, item);
            isSuccess = true;
        }
        else
        {
            itemStatsObject.transform.SetParent(this.transform.parent);
            Destroy(shop.Find("BuyAvailable").Find("Slot" + slotId).Find("Item").gameObject);
            buySlots[slotId - 1] = item;
            GameObject go = AddGameObjectForShop(slotId, true);
            PrepareGameObject(go, item);
            isSuccess = true;
        }
        return isSuccess;
    }

    public bool AddItemToShopSellSlot(GameItem item, int slotId)
    {
        bool isSuccess = false;
        if (sellSlots[slotId - 1] == null)
        {
            sellSlots[slotId - 1] = item;
            GameObject go = AddGameObjectForShop(slotId, false);
            PrepareGameObject(go, item);
            isSuccess = true;
        }
        return isSuccess;
    }

    public bool AddItemToSlot(GameItem item, int slotId) {
        bool isSuccess = false;
        if (slots[slotId - 1] == null)
        {
            slots[slotId - 1] = item;
            GameObject go = AddGameObject(slotId);
            PrepareGameObject(go, item);
            isSuccess = true;
        }
        return isSuccess;
    }

    public bool AddItemToCollectionSlot(GameItem item, int slotId)
    {
        bool isSuccess = false;
        if (collection.collectionSlots[slotId-1].item == null)
        {
            collection.collectionSlots[slotId-1].item = item;
            GameObject go = AddGameObjectForCollection(slotId);
            PrepareGameObjectForCollection(go, item);
            isSuccess = true;
        }
        else
        {
            GameObject go = AddGameObjectForCollection(slotId);
            PrepareGameObjectForCollection(go, item);
            isSuccess = true;
        }
        return isSuccess;
    }
    public bool AddItemToSeaSlot(GameItem item)
    {
        bool isSuccess = false;
        seaSlot.item = item;
        GameObject go = AddGameObjectForSeaSlot();
        PrepareGameObject(go, item);
        isSuccess = true;

        return isSuccess;
    }

    public bool AddItemToPlayerSlot(GameItem item, string slot)
    {
        int slotId = GetPlayerSlotId(slot); ;
        bool isSuccess = false;
        if (playerSlots[slotId] == null)
        {
            playerSlots[slotId] = item;
            GameObject go = AddGameObjectToPlayer(slot);
            PrepareGameObject(go, item);
            isSuccess = true;
        }
        return isSuccess;
    }

    public int GetPlayerSlotId(string slot)
    {
        if(slot == "SlotHead")
        {
            return 0;
        }
        else if (slot == "SlotChest")
        {
            return 1;
        }
        else if (slot == "SlotPants")
        {
            return 2;
        }
        else if (slot == "SlotBoots")
        {
            return 3;
        }
        else if (slot == "SlotWeapon")
        {
            return 4;
        }
        else if (slot == "SlotShield")
        {
            return 5;
        }
        else if (slot == "SlotAcc1")
        {
            return 6;
        }
        else
        {
            return -1;
        }
        
    }
}
