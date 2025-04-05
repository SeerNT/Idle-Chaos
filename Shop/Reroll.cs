using NT_Utils.Conversion;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class Reroll : MonoBehaviour
{
    [SerializeField] private Text timeText;
    [SerializeField] private Transform[] buySlots;
    [SerializeField] private Battle battle;
    private double timeSecs = 0;
    private Timer rerollTimer;

    private void Start()
    {
        rerollTimer = new Timer(DecreaseTime, null, 0, 1000); ;
    }

    private void DecreaseTime(object o)
    {
        timeSecs -= 1;
    }

    private void Update()
    {
        timeText.text = "Will be rerolled in " + TimeConversion.AbbreviateTime(timeSecs);
        if(timeSecs <= 0)
        {
            timeSecs = 1800;
            RerollItems();
        }
    }

    public void RerollItems()
    {
        buySlots[0].GetComponent<BuySlot>().item = GameItem.GetRandomRarityItem(GameItem.Rarity.Common, battle);
        buySlots[0].GetComponent<BuySlot>().isRerolled = true;
        buySlots[1].GetComponent<BuySlot>().item = GameItem.GetRandomRarityItem(GameItem.Rarity.Uncommon, battle);
        buySlots[1].GetComponent<BuySlot>().isRerolled = true;
        buySlots[2].GetComponent<BuySlot>().item = GameItem.GetRandomRarityItem(GameItem.Rarity.Rare, battle);
        buySlots[2].GetComponent<BuySlot>().isRerolled = true;
        buySlots[3].GetComponent<BuySlot>().item = GameItem.GetRandomRarityItem(GameItem.Rarity.Epic, battle);
        buySlots[3].GetComponent<BuySlot>().isRerolled = true;

        System.Random generator = new System.Random(DateTime.Now.Millisecond + DateTime.Now.Second + DateTime.Now.Minute + DateTime.Now.Hour + DateTime.Now.Day);
        
        int i = generator.Next(10);

        if(i < 6)
        {
            buySlots[4].GetComponent<BuySlot>().item = GameItem.GetRandomRarityItem(GameItem.Rarity.Legendary, battle);
        }
        else if(i >= 6 && i < 9)
        {
            buySlots[4].GetComponent<BuySlot>().item = GameItem.GetRandomRarityItem(GameItem.Rarity.Mythic, battle);
        }
        else if (i >= 9)
        {
            buySlots[4].GetComponent<BuySlot>().item = GameItem.GetRandomRarityItem(GameItem.Rarity.Shop, battle);
        }
        buySlots[4].GetComponent<BuySlot>().isRerolled = true;
    }

    public void SaveShop()
    {
        SaveSystem.SaveShop(timeSecs);
    }

    public void LoadShop()
    {
        ShopData data = SaveSystem.LoadShop();

        if (data != null)
        {
            this.timeSecs = data.time;
        }
    }

    public void GetOfflineProgress(double time)
    {
        timeSecs = timeSecs - time;
    }
}
