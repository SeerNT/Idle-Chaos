using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SeaManager : MonoBehaviour
{
    [Header("Attachments")]
    [SerializeField] private Text battleText;
    [SerializeField] private Text progressText;
    [SerializeField] private Text remainingText;
    [SerializeField] private Text accelerationText;
    [SerializeField] private Text xpDropText;
    [SerializeField] private Slider progressBar;
    [SerializeField] private Battle battle;
    [SerializeField] private SkillManager skillManager;
    [SerializeField] private TalantManager talantManager;
    [SerializeField] private InventorySlot itemChooseSlot;

    [Header("Settings Attachments")]
    [SerializeField] private Image currentItem;
    [SerializeField] private Dropdown itemDropSetting;
    [SerializeField] private Dropdown rarityDropSetting;
    [SerializeField] private Dropdown autoLvlSetting;

    [SerializeField] private Dropdown commonItemSetting;
    [SerializeField] private Dropdown uncommonItemSetting;
    [SerializeField] private Dropdown rareItemSetting;
    [SerializeField] private Dropdown epicItemSetting;
    [SerializeField] private Dropdown legendaryItemSetting;
    [SerializeField] private Dropdown mythicItemSetting;

    [SerializeField] private GameObject[] recycleSettings = new GameObject[6];

    private double baseXpDrop = 100;
    private double xpDrop = 100;

    private double baseTime = 7200; //7200
    private double remainingTime;
    private double timeSpent = 0;
    private int enemyNum = 0;

    void Start()
    {
        recycleSettings[0].transform.GetChild(1).GetComponent<Dropdown>().onValueChanged.AddListener(delegate { ChangeText(0); });
        recycleSettings[1].transform.GetChild(1).GetComponent<Dropdown>().onValueChanged.AddListener(delegate { ChangeText(1); });
        recycleSettings[2].transform.GetChild(1).GetComponent<Dropdown>().onValueChanged.AddListener(delegate { ChangeText(2); });
        recycleSettings[3].transform.GetChild(1).GetComponent<Dropdown>().onValueChanged.AddListener(delegate { ChangeText(3); });
        recycleSettings[4].transform.GetChild(1).GetComponent<Dropdown>().onValueChanged.AddListener(delegate { ChangeText(4); });
        recycleSettings[5].transform.GetChild(1).GetComponent<Dropdown>().onValueChanged.AddListener(delegate { ChangeText(5); });

        InvokeRepeating("StartAutoBattle", 0, 0.1f);
        if(enemyNum == 0)
        {
            NextBattlePhase();
        }
    }

    void ChangeText(int num)
    {
        Dropdown drop = recycleSettings[num].transform.GetChild(1).GetComponent<Dropdown>();
        Text bonusText = recycleSettings[num].transform.GetChild(0).GetComponent<Text>();

        if (num == 0)
        {
            bonusText.text = "+10% " + drop.options[drop.value].text + " PER ITEM";
        }
        if (num == 1)
        {
            bonusText.text = "+15% " + drop.options[drop.value].text + " PER ITEM";
        }
        if (num == 2)
        {
            bonusText.text = "+20% " + drop.options[drop.value].text + " PER ITEM";
        }
        if (num == 3)
        {
            bonusText.text = "+25% " + drop.options[drop.value].text + " PER ITEM";
        }
        if (num == 4)
        {
            bonusText.text = "+45% " + drop.options[drop.value].text + " PER ITEM";
        }
        if (num == 5)
        {
            bonusText.text = "+80% " + drop.options[drop.value].text + " PER ITEM";
        }
    }

    void Update()
    {
        if (itemChooseSlot.item != null)
            currentItem.sprite = itemChooseSlot.item.icon;
        else
        {
            currentItem.sprite = null;
            itemDropSetting.value = 0;
        }
            
    }

    void StartAutoBattle()
    {
        timeSpent += 0.1;
        if (battle.player.chaosFactor / 10 < 1)
        {
            progressBar.value = (float)(timeSpent / (baseTime - (baseTime * (battle.player.chaosFactor / 10))));
            progressText.text = Mathf.Round((float)(timeSpent * 100 / (baseTime - (baseTime * (battle.player.chaosFactor / 10))))).ToString() + "%";
        }
        else
        {
            progressBar.value = (float)(timeSpent / (baseTime - (baseTime * 0.90f)));
            progressText.text = Mathf.Round((float)(timeSpent * 100 / (baseTime - (baseTime * 0.90f)))).ToString() + "%";
        }
        
        remainingTime -= 0.1;
        remainingText.text = "remaining: " + NT_Utils.Conversion.TimeConversion.AbbreviateTime(remainingTime);

        if(progressBar.value >= 1)
        {
            GiveReward();
            NextBattlePhase();
        }
    }
    
    void UtilizeItem(GameItem item)
    {
        Dropdown setting = null;
        float increaseValue = 0;
        if(item.rarity == GameItem.Rarity.Common)
        {
            setting = commonItemSetting;
            increaseValue = 0.1f;
        }
        if (item.rarity == GameItem.Rarity.Uncommon)
        {
            setting = uncommonItemSetting;
            increaseValue = 0.15f;
        }
        if (item.rarity == GameItem.Rarity.Rare)
        {
            setting = rareItemSetting;
            increaseValue = 0.20f;
        }
        if (item.rarity == GameItem.Rarity.Epic)
        {
            setting = epicItemSetting;
            increaseValue = 0.25f;
        }
        if (item.rarity == GameItem.Rarity.Legendary)
        {
            setting = legendaryItemSetting;
            increaseValue = 0.45f;
        }
        if (item.rarity == GameItem.Rarity.Mythic)
        {
            setting = mythicItemSetting;
            increaseValue = 0.80f;
        }

        if (setting.options[setting.value].text == "PP")
        {
            battle.player.IncreaseStat(StatUpgrade.Stats.PP, battle.player.physicPower * increaseValue);
        }
        if (setting.options[setting.value].text == "HP")
        {
            battle.player.IncreaseStat(StatUpgrade.Stats.HP, battle.player.maxHp * increaseValue);
        }
        if (setting.options[setting.value].text == "CRT")
        {
            battle.player.IncreaseStat(StatUpgrade.Stats.CRT, battle.player.critDmg * increaseValue);
        }
    }

    void GiveReward()
    {
        GameManager.xp += xpDrop;
        Reincarnation.totalEarnXp += xpDrop;

        if (TalantManager.bloodUnlocked)
            GameManager.blood += (1 + Mathf.Floor(enemyNum / 10));
        GameManager.UpdateText();

        GameItem itemDrop = GameItem.GetRandomItem(battle);
        if (itemDrop != null)
        {
            if(itemDropSetting.options[itemDropSetting.value].text == "RANDOM")
            {
                if (rarityDropSetting.options[rarityDropSetting.value].text == itemDrop.rarity.ToString().ToUpper())
                    battle.inventory.AddItem(itemDrop);
                else if (rarityDropSetting.options[rarityDropSetting.value].text == "ANY")
                    battle.inventory.AddItem(itemDrop);
                else
                    UtilizeItem(itemDrop);
            }
            else if (itemDropSetting.options[itemDropSetting.value].text == "SLOT")
            {
                if(itemChooseSlot.item != null)
                {
                    if (itemDrop == itemChooseSlot.item)
                    {
                        if (autoLvlSetting.options[autoLvlSetting.value].text == "NO")
                            battle.inventory.AddItem(itemDrop);
                        else if (autoLvlSetting.options[autoLvlSetting.value].text == "YES")
                        {
                            if(itemChooseSlot.lvl < 100)
                            {
                                itemChooseSlot.lvl++;
                            }
                        }
                    }
                }
            }
        }
    }

    void NextBattlePhase()
    {
        accelerationText.text = "CHAOS FACTOR ACCELERATION +" + Math.Round(battle.player.chaosFactor * 10).ToString() + "%";
        if(battle.player.chaosFactor / 10 < 1)
        {
            remainingTime = baseTime - (baseTime * (battle.player.chaosFactor / 10));
        }
        else
        {
            remainingTime = baseTime - baseTime * 0.90f;
        }
        
        timeSpent = 0;

        if((enemyNum+1) >= battle.names.Count)
        {
            enemyNum = 1;
            baseXpDrop = baseXpDrop * 1.2;
            xpDrop = baseXpDrop;
        }
        else
        {
            enemyNum++;

            xpDrop *= 1.8;
        }

        if(enemyNum >= 19)
        {
            battleText.text = "Battle #" + enemyNum.ToString() + " " + battle.names[enemyNum];
        }
        else
        {
            battleText.text = "Battle #" + enemyNum.ToString() + " " + battle.names[enemyNum - 1];
        }
       
        xpDropText.text = "WILL DROP " + NT_Utils.Conversion.NumberConversion.AbbreviateNumber(Math.Round(xpDrop)) + " XP";
    }

    public void SimulateTime(float total)
    {
        float left = total;
        // 5 3 25
        if ((timeSpent + left) < baseTime)
        {
            timeSpent += left;
        }
        else if ((timeSpent + left) >= baseTime)
        {
            double increaseAmount = Math.Floor((total + timeSpent) / baseTime);
            
            /*
            double cycles = Math.Floor(increaseAmount / battle.names.Count);
            if (cycles != 0)
                enemyNum = (int)cycles % battle.names.Count;
            else
                enemyNum += (int)increaseAmount;
                double xpSum2 = (xpDrop * (Math.Pow(1.2f, battle.names.Count - enemyNum) - 1)) / (1.2f - 1);
                GameManager.xp += xpSum2;
                Reincarnation.totalEarnXp += xpSum2;
            if (enemyNum >= 19)
            {
                battleText.text = "Battle #" + enemyNum.ToString() + " " + battle.names[enemyNum];
            }
            else
            {
                battleText.text = "Battle #" + enemyNum.ToString() + " " + battle.names[enemyNum - 1];
            }

            // 417 
            for (int i = 0; i < cycles; i++)
            {
                double xpSum = (xpDrop * (Math.Pow(1.2f, battle.names.Count - enemyNum) - 1)) / (1.2f - 1);
                xpSum = xpSum * (1.8f * (i + 1));
                xpDrop = (xpDrop * Math.Pow(1.2f, battle.names.Count - enemyNum - 1)) * (1.8f * (i + 1)); ;
                GameManager.xp += xpSum;
                Reincarnation.totalEarnXp += xpSum;
            }*/
            xpDropText.text = "WILL DROP " + NT_Utils.Conversion.NumberConversion.AbbreviateNumber(Math.Round(xpDrop)) + " XP";
            float leftSeconds = (float)((((total + timeSpent) / baseTime) - increaseAmount) * baseTime);

            timeSpent = timeSpent + leftSeconds;
            // с прогрессией должно быть
            
            GameManager.UpdateText();
            if (TalantManager.bloodUnlocked)
                GameManager.blood += (1 + Mathf.Floor(enemyNum / 10) * increaseAmount);
        }
    }

    public void Reset()
    {
        CancelInvoke("StartAutoBattle");
        SaveSystem.ResetSeaBattle();
        SaveSystem.ResetSeaFishing();
        SaveSystem.ResetSeaRecycle();
    }

    public void Save()
    {
        if(itemChooseSlot != null)
        {
            if(itemChooseSlot.item != null)
            {
                SaveSystem.SaveSeaFishing(itemDropSetting.value, rarityDropSetting.value, autoLvlSetting.value, itemChooseSlot);
            }
            else
            {
                SaveSystem.SaveSeaFishing(itemDropSetting.value, rarityDropSetting.value, autoLvlSetting.value, null);
            }
        }
        else
        {
            SaveSystem.SaveSeaFishing(itemDropSetting.value, rarityDropSetting.value, autoLvlSetting.value, null);
        }

        SaveSystem.SaveSeaBattle(xpDrop, remainingTime, timeSpent, enemyNum);
        SaveSystem.SaveSeaRecycle(commonItemSetting.value, uncommonItemSetting.value, rareItemSetting.value, epicItemSetting.value, legendaryItemSetting.value, mythicItemSetting.value);
    }

    public void Load()
    {
        SeaBattleData data = SaveSystem.LoadSeaBattle();

        if (data != null)
        {
            this.xpDrop = data.xpDrop;
            this.remainingTime = data.remainingTime;
            this.timeSpent = data.timeSpent;
            this.enemyNum = data.enemyNum;

            if (enemyNum >= 19)
            {
                battleText.text = "Battle #" + enemyNum.ToString() + " " + battle.names[enemyNum];
            }
            else
            {
                battleText.text = "Battle #" + enemyNum.ToString() + " " + battle.names[enemyNum - 1];
            }

            xpDropText.text = "WILL DROP " + NT_Utils.Conversion.NumberConversion.AbbreviateNumber(Math.Round(xpDrop)) + " XP";
            accelerationText.text = "CHAOS FACTOR ACCELERATION +" + (battle.player.chaosFactor * 10).ToString() + "%";
        }

        SeaFishingData data2 = SaveSystem.LoadSeaFishing();

        if (data2 != null)
        {
            this.itemDropSetting.value = data2.itemDropSetting;
            this.rarityDropSetting.value = data2.rarityDropSetting;
            this.autoLvlSetting.value = data2.autoLvlSetting;

            if(data2.id != 0)
            {
                this.itemChooseSlot.item = GameItem.GetItemById(data2.id);
                this.itemChooseSlot.lvl = data2.lvl;
            }
        }

        SeaRecycleData data3 = SaveSystem.LoadSeaRecycle();

        if (data3 != null)
        {
            this.commonItemSetting.value = data3.common;
            this.uncommonItemSetting.value = data3.uncommon;
            this.rareItemSetting.value = data3.rare;
            this.epicItemSetting.value = data3.epic;
            this.legendaryItemSetting.value = data3.legendary;
            this.mythicItemSetting.value = data3.mythic;
            ChangeText(0);
            ChangeText(1);
            ChangeText(2);
            ChangeText(3);
            ChangeText(4);
            ChangeText(5);
        }
    }
}
