using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Item/Default")]
public class GameItem : ScriptableObject
{
    public enum Rarity
    {
        Common, // 62%
        Uncommon, // 26%
        Rare, // 9%
        Epic, // 2%
        Legendary, // 0.99%
        Mythic, // 0.01%
        Shop,
        Meta,
        Chaotic,
        NONE
    }
    public enum Type
    {
        Head,
        Chest,
        Pants,
        Boots,
        Weapon,
        Shield,
        Accessory
    }

    public string id;
    public string displayName;
    public string description;
    public Rarity rarity;
    public Sprite icon;
    public Sprite dragIcon;
    public Type type;
    public ItemSet itemSet;

    public float baseCost = 0;
    public float bonusHP = 0;
    public float bonusArm = 0;
    public float bonusMgcArm = 0;
    public float bonusPP = 0;
    public float bonusMP = 0;
    public float bonusCrit = 0;
    public float bonusPrc = 0;
    public float bonusLuck = 0;
    public float bonusAS = 0;
    public float bonusManaReg = 0;
    public float bonusHpReg = 0;
    public string[] specialBonus;
    public Dictionary<string, float> givenBonuses = new Dictionary<string, float>();

    public void InitItem() {
        if (bonusHP != 0) { givenBonuses.Add("HP", bonusHP); }
        if (bonusArm != 0) { givenBonuses.Add("ARM", bonusArm); }
        if (bonusMgcArm != 0) { givenBonuses.Add("MARM", bonusMgcArm); }
        if (bonusMP != 0) { givenBonuses.Add("MP", bonusMP); }
        if (bonusPP != 0) { givenBonuses.Add("PP", bonusPP); }
        if (bonusCrit != 0) { givenBonuses.Add("CR", bonusCrit); }
        if (bonusPrc != 0) { givenBonuses.Add("PRC", bonusPrc); }
        if (bonusLuck != 0) { givenBonuses.Add("LC", bonusLuck); }
        if (bonusAS != 0) { givenBonuses.Add("AS", bonusAS); }
        if (bonusManaReg != 0) { givenBonuses.Add("MREG", bonusManaReg); }
        if (bonusHpReg != 0) { givenBonuses.Add("HPREG", bonusHpReg); }
    }

    public bool WillDrop() {
        bool isSuccess = false;
        
        
        return isSuccess;
    }

    public static GameItem GetRandomItem(Battle battle) {
        System.Random generator = new System.Random(DateTime.Now.Millisecond);
        Rarity rar = Rarity.Common;
        GameItem item = null;
        if (generator.Next(100000) < 10)
        {
            rar = Rarity.Mythic;
        }
        else if (generator.Next(100000) < 999)
        {
            rar = Rarity.Legendary;
        }
        else if (generator.Next(100000) < 2000)
        {
            rar = Rarity.Epic;
        }
        else if (generator.Next(100000) < 9000)
        {
            rar = Rarity.Rare;
        }
        else if (generator.Next(100000) < 26000)
        {
            rar = Rarity.Uncommon;
        }
        else if (generator.Next(100000) < 62000) {
            rar = Rarity.Common;
        }
        
        if (rar == Rarity.Common) {
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

        return item;
    }

    public static GameItem GetRandomRarityItem(Rarity rar, Battle battle)
    {
        System.Random generator = new System.Random(DateTime.Now.Millisecond + DateTime.Now.Second + DateTime.Now.Minute + DateTime.Now.Hour + DateTime.Now.Day);
        GameItem item = null;

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
        else if (rar == Rarity.Chaotic)
        {
            int i = generator.Next(battle.chaoticItems.Length);
            item = battle.chaoticItems[i];
        }
        else
        {
            item = GetRandomItem(battle);
        }
        return item;
    }

    public static GameItem GetItemById(int id)
    {
        if(id != 0)
        {
            GameItem item = Resources.Load<GameItem>("Creatable/Items/Item" + id.ToString());
            return item;
        }
        else
        {
            GameItem item = Resources.Load<GameItem>("Creatable/Items/TEST_ITEM");
            return item;
        }
    }

    public static Color GetRarityColor(GameItem item) {
        Color color = Color.black;
        if (item.rarity == GameItem.Rarity.Uncommon)
        {
            color = Color.green;
        }
        if (item.rarity == GameItem.Rarity.Rare)
        {
            color = Color.blue;
        }
        if (item.rarity == GameItem.Rarity.Epic)
        {
            color = Color.magenta;
        }
        if (item.rarity == GameItem.Rarity.Legendary)
        {
            color = Color.yellow;
        }
        if (item.rarity == GameItem.Rarity.Mythic)
        {
            color = Color.red;
        }
        if (item.rarity == GameItem.Rarity.Chaotic)
        {
            color = new Color32(101, 67, 33, 255);
        }
        if (item.rarity == GameItem.Rarity.Shop)
        {
            color = new Color(1,0.53f,0.039f);
        }
        if (item.rarity == GameItem.Rarity.Meta)
        {
            color = new Color(0.47f, 0.1255f, 0.37255f);
        }
        return color;
    }
}
