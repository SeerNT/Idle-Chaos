using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CollectionManager : MonoBehaviour
{
    [Header("Attachments")]
    [SerializeField] private Button inventoryBut;
    [SerializeField] private Button collectionBut;
    [SerializeField] private Transform inventory;
    [SerializeField] private Transform collection;
    [SerializeField] public Transform slots;
    [SerializeField] private Inventory inventoryManager;

    public InventorySlot[] collectionSlots = new InventorySlot[66];
    void Start()
    {
        inventoryBut.onClick.AddListener(delegate { ChangeWindow(0); });
        collectionBut.onClick.AddListener(delegate { ChangeWindow(1); });
        for(int i = 0; i < collectionSlots.Length; i++)
        {
            collectionSlots[i] = slots.GetChild(i).GetComponent<InventorySlot>();
        }
    }

    void ChangeWindow(int num)
    {
        if(num == 0)
        {
            inventory.localPosition = new Vector3(0, 0, 0);
            collection.localPosition = new Vector3(-100000, 0, 0);
        }
        else if (num == 1)
        {
            inventory.localPosition = new Vector3(-100000, 0, 0);
            collection.localPosition = new Vector3(0, 0, 0);
        }
    }
}
