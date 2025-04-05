using NT_Utils.Conversion;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillConversion : MonoBehaviour
{
    private Text titleText;
    private Text descText;
    private Text costText;
    private Button buyBut;

    [SerializeField] private Battle battle;
    [SerializeField] private SkillManager manager;

    [Header("Settings")]
    public int increase = 1;
    public int baseCost = 5;
    public StatUpgrade.Stats stat;

    public bool isUnlocked;

    void Start()
    {
        titleText = transform.Find("Title").GetComponent<Text>();
        descText = transform.Find("Description").GetComponent<Text>();
        costText = transform.Find("Cost").GetComponent<Text>();
        buyBut = transform.Find("BuyBut").GetComponent<Button>();

        titleText.text = stat.ToString();
        descText.text = "Increase stat " + increase.ToString() + "%";
        costText.text = NumberConversion.AbbreviateNumber(baseCost);

        buyBut.onClick.AddListener(Buy);
    }

    void Update()
    {
        costText.text = NumberConversion.AbbreviateNumber(baseCost);
    }
    void Buy()
    {
        if (manager.skillpoints >= baseCost)
        {
            manager.skillpoints = (int)(manager.skillpoints - baseCost);
            costText.text = NumberConversion.AbbreviateNumber(baseCost);
            Player player = battle.player;

            if(stat == StatUpgrade.Stats.PP)
            {
                float bonusValueProcents = (Mathf.Ceil(player.physicPower * (increase / 100.0f)));
                player.IncreaseStat(StatUpgrade.Stats.PP, bonusValueProcents);
            }
            if (stat == StatUpgrade.Stats.MP)
            {
                float bonusValueProcents = (Mathf.Ceil(player.magicPower * (increase / 100.0f)));
                player.IncreaseStat(StatUpgrade.Stats.MP, bonusValueProcents);
            }
            if (stat == StatUpgrade.Stats.ARM)
            {
                float bonusValueProcents = (Mathf.Ceil(player.armor * (increase / 100.0f)));
                player.IncreaseStat(StatUpgrade.Stats.ARM, bonusValueProcents);
            }
            if (stat == StatUpgrade.Stats.MRM)
            {
                float bonusValueProcents = (Mathf.Ceil(player.magicArmor * (increase / 100.0f)));
                player.IncreaseStat(StatUpgrade.Stats.MRM, bonusValueProcents);
            }
            if (stat == StatUpgrade.Stats.HP)
            {
                float bonusValueProcents = (Mathf.Ceil(player.maxHp * (increase / 100.0f)));
                player.IncreaseStat(StatUpgrade.Stats.HP, bonusValueProcents);
            }
            if (stat == StatUpgrade.Stats.PRC)
            {
                float bonusValueProcents = (Mathf.Ceil(player.pierce * (increase / 100.0f)));
                player.IncreaseStat(StatUpgrade.Stats.PRC, bonusValueProcents);
            }
            if (stat == StatUpgrade.Stats.CRT)
            {
                float bonusValueProcents = (Mathf.Ceil(player.critDmg * (increase / 100.0f)));
                player.IncreaseStat(StatUpgrade.Stats.CRT, bonusValueProcents);
            }
            if (stat == StatUpgrade.Stats.LCK)
            {
                float bonusValueProcents = (Mathf.Ceil(player.luck * (increase / 100.0f)));
                player.IncreaseStat(StatUpgrade.Stats.LCK, bonusValueProcents);
            }
        }
    }

    public void Save()
    {
        SaveSystem.SaveConversion(baseCost, isUnlocked, name);
    }

    public void Load()
    {
        ConversionData data = SaveSystem.LoadConversion(this.name);

        if (data != null)
        {
            this.isUnlocked = data.isUnlocked;
            if (data.isSale)
            {
                this.baseCost = 1;
            }
        }
    }
}
