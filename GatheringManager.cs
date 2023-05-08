using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GatheringManager : MonoBehaviour
{
    [SerializeField] private Transform growingObj;
    [SerializeField] private Transform searchingObj;
    [SerializeField] public GameObject herbStatsObject;

    public Growing[] growingProducts = new Growing[10];
    public Searching[] searchingProducts = new Searching[10];

    public static bool gatheringUnlocked;

    public Herb[] slots = new Herb[5];

    void Awake()
    {
        for(int i = 1; i < growingObj.childCount; i++)
        {
            growingProducts[i-1] = growingObj.GetChild(i).GetComponent<Growing>();
        }
        for (int i = 1; i < searchingObj.childCount; i++)
        {
            searchingProducts[i-1] = searchingObj.GetChild(i).GetComponent<Searching>();
        }
    }

    public void GiveProductSeed(Searching searchObj)
    {
        string name = searchObj.name;
        name = name.Replace("Search", "");
        growingProducts[int.Parse(name) - 1].seedAmount++;
    }

    public void GiveProduct(Growing growingObj)
    {
        string name = growingObj.name;
        name = name.Replace("Grow", "");

        AddHerbToSlot(GetHerb(growingObj), int.Parse(GetHerb(growingObj).id));
    }

    public Herb GetHerb(Growing grow)
    {
        string name = grow.name;
        name = name.Replace("Grow", "");

        Herb herb = Herb.GetHerbById(int.Parse(name));
        herb.quality = grow.quality;

        Transform slot = transform.Find("Herbs").Find("Inv" + name);
        slot.GetComponent<HerbSlot>().herb = herb;

        return herb;
    }

    public HerbSlot GetHerbSlot(InventoryHerb invHerb)
    {
        return invHerb.transform.parent.GetComponent<HerbSlot>();
    }

    public GameObject AddGameObject(int slotId)
    {
        GameObject go = new GameObject("Herb");
        Transform slot = transform.Find("Herbs").Find("Inv" + slotId);

        go.GetComponent<Transform>().SetParent(slot);

        return go;
    }

    public void PrepareGameObject(GameObject go, Herb herb)
    {
        go.AddComponent<InventoryHerb>();
        go.GetComponent<InventoryHerb>().herb = herb;
        go.AddComponent<Image>();
        go.GetComponent<Image>().sprite = go.GetComponent<InventoryHerb>().herb.icon;
        go.AddComponent<CanvasGroup>();
        go.transform.localPosition = Vector3.zero;
        go.GetComponent<RectTransform>().sizeDelta = new Vector2(90, 90);

    }
    
    public bool AddHerbToSlot(Herb herb, int slotId)
    {
        bool isSuccess = false;
        if (slots[slotId - 1] == null)
        {
            slots[slotId - 1] = herb;
            GameObject go = AddGameObject(slotId);
            PrepareGameObject(go, herb);
            isSuccess = true;
        }

        Transform slot = transform.Find("Herbs").Find("Inv" + slotId);
        slot.GetComponent<HerbSlot>().herb = herb;
        slot.GetComponent<HerbSlot>().amount++;
        

        return isSuccess;
    }
}
