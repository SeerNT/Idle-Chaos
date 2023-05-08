using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Herb/Default")]
public class Herb : ScriptableObject
{
    public enum Quality
    {
        Common, // 62%
        Uncommon, // 26%
        Rare, // 9%
        Epic, // 2%
        Legendary, // 0.99%
        Mythic, // 0.01%
        NONE
    }

    public string id;
    public string displayName;
    public string description;
    public Quality quality;
    public Sprite icon;

    public float activeTime;
    public float currentActiveTime;
    public bool isActive;

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

    public void Init()
    {
        givenBonuses.Add("HP", bonusHP);
        givenBonuses.Add("ARM", bonusArm);
        givenBonuses.Add("MARM", bonusMgcArm);
        givenBonuses.Add("MP", bonusMP);
        givenBonuses.Add("PP", bonusPP);
        givenBonuses.Add("CR", bonusCrit);
        givenBonuses.Add("PRC", bonusPrc);
        givenBonuses.Add("LC", bonusLuck);
        givenBonuses.Add("AS", bonusAS);
        givenBonuses.Add("MREG", bonusManaReg);
        givenBonuses.Add("HPREG", bonusHpReg);
    }

    public static Quality GetRandomQuality()
    {
        System.Random generator = new System.Random(DateTime.Now.Millisecond);
        Quality rar = Quality.Common;

        if (generator.Next(100000) < 10)
        {
            rar = Quality.Mythic;
        }
        else if (generator.Next(100000) < 999)
        {
            rar = Quality.Legendary;
        }
        else if (generator.Next(100000) < 2000)
        {
            rar = Quality.Epic;
        }
        else if (generator.Next(100000) < 9000)
        {
            rar = Quality.Rare;
        }
        else if (generator.Next(100000) < 26000)
        {
            rar = Quality.Uncommon;
        }
        else if (generator.Next(100000) < 62000)
        {
            rar = Quality.Common;
        }

        return rar;
    }

    public static Herb GetHerbById(int id)
    {
        if (id != 0)
        {
            Herb herb = Resources.Load<Herb>("Creatable/Herbs/Herb" + id.ToString());
            return herb;
        }
        else
        {
            Herb herb = Resources.Load<Herb>("Creatable/Herbs/TEST_HERB");
            return herb;
        }
    }

    public static Color GetQualityColor(Herb herb)
    {
        Color color = Color.black;
        if (herb.quality == Quality.Uncommon)
        {
            color = Color.green;
        }
        if (herb.quality == Quality.Rare)
        {
            color = Color.blue;
        }
        if (herb.quality == Quality.Epic)
        {
            color = Color.magenta;
        }
        if (herb.quality == Quality.Legendary)
        {
            color = Color.yellow;
        }
        if (herb.quality == Quality.Mythic)
        {
            color = Color.red;
        }

        return color;
    }
}
