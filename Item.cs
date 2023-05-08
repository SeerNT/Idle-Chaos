using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Item : MonoBehaviour, IDropHandler
{
    [SerializeField] private Inventory inventory;

    public GameItem item;

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null)
        {
            eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = GetComponent<RectTransform>().anchoredPosition;
            string name = eventData.pointerDrag.name;
            if (name.Contains("Inv")) {
                name = name.Replace("Inv", "");
                inventory.AddItemToSlot(this.item, int.Parse(name));
            }
        }
    }
}
