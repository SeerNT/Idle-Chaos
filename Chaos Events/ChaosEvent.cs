using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ChaosEvents/Default")]
public class ChaosEvent : ScriptableObject
{
    public enum Rarity
    {
        Common, // 69%
        Rare, // 21%
        Ultra, // 8%
        Trolling, // 2%
        NONE
    }

    public enum Type
    {
        StunPlayer,
        BlockPlayerSkills,
        WeakenPlayer,
        Minus50PrPP,
        NONE
    }

    public string id;
    public string displayName;
    public string description;
    public Rarity rarity;
    public Sprite icon;
    public Type type;

    public float value = 0;
    public float duration = 0;

    public static ChaosEvent GetRandomEvent(GameManager manager)
    {
        ChaosEvent chEvent = null;

        System.Random generator = new System.Random(DateTime.Now.Millisecond);
        Rarity rar = Rarity.Common;
        if (generator.Next(100) < 2)
        {
            rar = Rarity.Trolling;
        }
        else if (generator.Next(100) < 8)
        {
            rar = Rarity.Ultra;
        }
        else if (generator.Next(100) < 21)
        {
            rar = Rarity.Rare;
        }
        else if (generator.Next(100) < 69)
        {
            rar = Rarity.Common;
        }

        if (rar == Rarity.Common)
        {
            int i = generator.Next(manager.commonEvents.Length);
            chEvent = manager.commonEvents[i];
        }
        else if (rar == Rarity.Rare)
        {
            int i = generator.Next(manager.rareEvents.Length);
            chEvent = manager.rareEvents[i];
        }
        else if (rar == Rarity.Ultra)
        {
            int i = generator.Next(manager.ultraEvents.Length);
            chEvent = manager.ultraEvents[i];
        }
        else if (rar == Rarity.Trolling)
        {
            int i = generator.Next(manager.trollEvents.Length);
            chEvent = manager.trollEvents[i];
        }

        return chEvent;
    }

    public static ChaosEvent GetRandomRarityEvent(ChaosEvent rar, Battle battle)
    {
        System.Random generator = new System.Random(DateTime.Now.Millisecond + DateTime.Now.Second + DateTime.Now.Minute + DateTime.Now.Hour + DateTime.Now.Day);
        ChaosEvent item = null;
        /*
        if (rar == Rarity.Common)
        {
            int i = generator.Next(battle.commonItems.Length);
            item = battle.commonItems[i];
        }
        else if (rar == Rarity.Uncommon)
        {
            int i = generator.Next(battle.uncommonItems.Length);
            item = battle.uncommonItems[i];
        }
        else if (rar == Rarity.Rare)
        {
            int i = generator.Next(battle.rareItems.Length);
            item = battle.rareItems[i];
        }
        else if (rar == Rarity.Epic)
        {
            int i = generator.Next(battle.epicItems.Length);
            item = battle.epicItems[i];
        }
        else if (rar == Rarity.Legendary)
        {
            int i = generator.Next(battle.legendaryItems.Length);
            item = battle.legendaryItems[i];
        }
        else if (rar == Rarity.Mythic)
        {
            int i = generator.Next(battle.mythicItems.Length);
            item = battle.mythicItems[i];
        }
        else if (rar == Rarity.Shop)
        {
            int i = generator.Next(battle.shopItems.Length);
            item = battle.shopItems[i];
        }
        else
        {
            item = GetRandomItem(battle);
        }*/
        return item;
    }

    public static ChaosEvent GetEventById(int id)
    {
        if (id != 0)
        {
            ChaosEvent item = Resources.Load<ChaosEvent>("Creatable/ChaosEvents/ChaosEvent" + id.ToString());
            return item;
        }
        else
        {
            ChaosEvent item = Resources.Load<ChaosEvent>("Creatable/ChaosEvents/TEST_EVENT");
            return item;
        }
    }

    public static Color GetRarityColor(ChaosEvent chEvent)
    {
        Color color = Color.red;

        if (chEvent.rarity == ChaosEvent.Rarity.Rare)
        {
            color = Color.blue;
        }
        if (chEvent.rarity == ChaosEvent.Rarity.Ultra)
        {
            color = Color.magenta;
        }
        if (chEvent.rarity == ChaosEvent.Rarity.Trolling)
        {
            color = Color.yellow;
        }

        return color;
    }
}
