using NT_Utils.Conversion;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Reincarnation : MonoBehaviour
{
    [Header("Attachments")]
    [SerializeField] private GameObject reincarnationTransit;
    [SerializeField] private Button reincarnateBut;
    [SerializeField] private AutoSave saves;
    [SerializeField] private GameObject reincarnationCurrency;

    [SerializeField] private Text playTimeText;
    [SerializeField] private Text earnMagicText;
    [SerializeField] private Text earnXpText;
    [SerializeField] private Text killedEnemiesText;
    [SerializeField] private Text nextQiText;
    [SerializeField] private Text qiGetText;

    [SerializeField] private Battle battle;
    [SerializeField] private Inventory inventory;
    [SerializeField] private GatheringManager gatheringManager;
    [SerializeField] private Transform playerSlots;
    [SerializeField] private ItemSetManager setManager;
    [SerializeField] private AbilityManager abilityManager;

    // Cycle Statistics
    public static double totalPlayTime;
    public static double totalEarnMagic;
    public static double totalEarnXp;
    public static double totalKilledEnemies;

    public static double[] permanentsUpgrades = new double[8];

    void Start()
    {
        reincarnateBut.onClick.AddListener(Reincarnate);
    }

    void Update()
    {
        playTimeText.text = "TOTAL PLAYED TIME: " + TimeConversion.AbbreviateTime(totalPlayTime, true);
        earnMagicText.text = "TOTAL EARNED MAGIC: " + NumberConversion.AbbreviateNumber(totalEarnMagic);
        earnXpText.text = "TOTAL EARNED XP: " + NumberConversion.AbbreviateNumber(totalEarnXp);
        killedEnemiesText.text = "TOTAL KILLED ENEMIES: " + NumberConversion.AbbreviateNumber(totalKilledEnemies);
        qiGetText.text = "YOU WILL GET " + NumberConversion.AbbreviateNumber(CalculateQi()) + " QI";

        nextQiText.text = "you will earn next qi in " + TimeConversion.AbbreviateTime((3600) - totalPlayTime % 3600, true) + "\n*approximately";

        reincarnationTransit.SetActive(CalculateQi() > 0 || GameManager.qi > 0);
        reincarnationCurrency.SetActive(CalculateQi() > 0 || GameManager.qi > 0);
    }

    public void UpdateBonus(StatUpgrade.Stats stat, bool fromMeta)
    {
        if (stat == StatUpgrade.Stats.PP)
        {
            if (fromMeta)
            {
                if (permanentsUpgrades[6] > 0)
                {
                    battle.player.physicPower = (float)(battle.player.physicPower * 2);
                }
            }
            else
            {
                if (permanentsUpgrades[0] > 0)
                {
                    battle.player.physicPower = (float)(battle.player.physicPower * 2);
                }
            }
            
        }
        if (stat == StatUpgrade.Stats.HP)
        {
            if (fromMeta)
            {
                if (permanentsUpgrades[7] > 0)
                {
                    battle.player.maxHp = (float)((battle.player.maxHp) * 2);
                }
            }
            else
            {
                if (permanentsUpgrades[1] > 0)
                {
                    battle.player.maxHp = (float)((battle.player.maxHp) * 2);
                }
            }
        }
        if(stat == StatUpgrade.Stats.MP)
        {
            if (permanentsUpgrades[2] > 0)
            {
                battle.player.magicPower = (float)((battle.player.magicPower) * 2);
            }
        }
        if(stat == StatUpgrade.Stats.ARM)
        {
            if (permanentsUpgrades[3] > 0)
            {
                battle.player.armor = (float)((battle.player.armor) * 1.8);
               // battle.player.abstractARM = (float)((battle.player.abstractARM) * 1.8);
            }
        }
        if(stat == StatUpgrade.Stats.PRC)
        {
            if (permanentsUpgrades[4] > 0)
            {
                battle.player.pierce = (float)((battle.player.pierce) * 1.5f);
                //battle.player.abstractPRC = (float)((battle.player.abstractPRC) * 1.5f);
            }
        }
        if(stat == StatUpgrade.Stats.MRM)
        {
            if (permanentsUpgrades[5] > 0)
            {
                battle.player.magicArmor = (float)((battle.player.magicArmor) * 1.6f);
                //battle.player.abstractMARM = (float)((battle.player.abstractMARM) * 1.6f);
            }
        }
    }

    void ReturnStats()
    {
        if (permanentsUpgrades[0] > 0 || permanentsUpgrades[6] > 0)
        {
            if(permanentsUpgrades[0] > 0)
            {
                for (int i = 1; i <= (permanentsUpgrades[0] / 100); i++)
                {
                    battle.player.physicPower = (float)((battle.player.physicPower) * 2);
                }
            }
            if(permanentsUpgrades[6] > 0)
            {
                for (int j = 1; j <= (permanentsUpgrades[6] / 100); j++)
                {
                    battle.player.physicPower = (float)((battle.player.physicPower) * 2);
                }
            }
        }
        if (permanentsUpgrades[1] > 0 || permanentsUpgrades[7] > 0)
        {
            if(permanentsUpgrades[1] > 0)
            {
                for (int i = 1; i <= ((int)(permanentsUpgrades[1] / 100)); i++)
                {
                    battle.player.maxHp = (float)((battle.player.maxHp) * 2);
                }
            }
            if (permanentsUpgrades[7] > 0)
            {
                for (int j = 1; j <= ((int)(permanentsUpgrades[7] / 100)); j++)
                {
                    battle.player.maxHp = (float)((battle.player.maxHp) * 2);
                }
            }
        }
        if (permanentsUpgrades[2] > 0)
        {
            for (int i = 1; i <= ((int)(permanentsUpgrades[2] / 100)); i++)
            {
                battle.player.magicPower = (float)((battle.player.magicPower) * 2);
            }
        }
        if (permanentsUpgrades[3] > 0)
        {

            for (int i = 1; i <= ((int)(permanentsUpgrades[3] / 80)); i++)
            {
                battle.player.armor = (float)((battle.player.armor) * 1.8f);
            }
        }
        if (permanentsUpgrades[4] > 0)
        {

            for (int i = 1; i <= ((int)(permanentsUpgrades[4] / 50)); i++)
            {
                battle.player.pierce = (float)((battle.player.pierce) * 1.5f);
            }
        }
        if (permanentsUpgrades[5] > 0)
        {
            for (int i = 1; i <= ((int)(permanentsUpgrades[5] / 60)); i++)
            {
                battle.player.magicArmor = (float)((battle.player.magicArmor) * 1.6f);
            }
        }
    }

    void UpdateStats(GameItem item, bool fromMeta)
    {
        if (item.bonusPP > 0)
        {
            UpdateBonus(StatUpgrade.Stats.PP, fromMeta);
        }
        if (item.bonusHP > 0)
        {
            UpdateBonus(StatUpgrade.Stats.HP, fromMeta);
        }
        if (item.bonusMP > 0)
        {
            UpdateBonus(StatUpgrade.Stats.MP, fromMeta);
        }
        if (item.bonusArm > 0)
        {
            UpdateBonus(StatUpgrade.Stats.ARM, fromMeta);
        }
        if (item.bonusMgcArm > 0)
        {
            UpdateBonus(StatUpgrade.Stats.MRM, fromMeta);
        }
        if (item.bonusPrc > 0)
        {
            UpdateBonus(StatUpgrade.Stats.PRC, fromMeta);
        }
    }

    void UpdateStats(Herb item, bool fromMeta)
    {
        if (item.bonusPP > 0)
        {
            UpdateBonus(StatUpgrade.Stats.PP, fromMeta);
        }
        if (item.bonusHP > 0)
        {
            UpdateBonus(StatUpgrade.Stats.HP, fromMeta);
        }
        if (item.bonusMP > 0)
        {
            UpdateBonus(StatUpgrade.Stats.MP, fromMeta);
        }
        if (item.bonusArm > 0)
        {
            UpdateBonus(StatUpgrade.Stats.ARM, fromMeta);
        }
        if (item.bonusMgcArm > 0)
        {
            UpdateBonus(StatUpgrade.Stats.MRM, fromMeta);
        }
        if (item.bonusPrc > 0)
        {
            UpdateBonus(StatUpgrade.Stats.PRC, fromMeta);
        }
    }

    public void UpdateBonusForItem(StatUpgrade.Stats stat, bool fromMeta)
    {
        int count = 0;
        bool[] haveBonus = new bool[6];
        foreach (GameItem item in inventory.playerSlots)
        {
            if (item != null)
            {
                haveBonus[0] = item.bonusPP > 0;
                haveBonus[1] = item.bonusHP > 0;
                haveBonus[2] = item.bonusMP > 0;
                haveBonus[3] = item.bonusArm > 0;
                haveBonus[4] = item.bonusPrc > 0;
                haveBonus[5] = item.bonusMgcArm > 0;
            }
            else
            {
                count++;
            }
        }
        if (haveBonus[0] && stat == StatUpgrade.Stats.PP)
            UpdateBonus(StatUpgrade.Stats.PP, fromMeta);
        if (haveBonus[1] && stat == StatUpgrade.Stats.HP)
            UpdateBonus(StatUpgrade.Stats.HP, fromMeta);
        if (haveBonus[2] && stat == StatUpgrade.Stats.MP)
            UpdateBonus(StatUpgrade.Stats.MP, fromMeta);
        if (haveBonus[3] && stat == StatUpgrade.Stats.ARM)
            UpdateBonus(StatUpgrade.Stats.ARM, fromMeta);
        if (haveBonus[4] && stat == StatUpgrade.Stats.PRC)
            UpdateBonus(StatUpgrade.Stats.PRC, fromMeta);
        if (haveBonus[5] && stat == StatUpgrade.Stats.MRM)
            UpdateBonus(StatUpgrade.Stats.MRM, fromMeta);

        foreach (bool have in haveBonus)
        {
            
            /*
            if (item.bonusPP > 0 || item.bonusHP > 0 || item.bonusMP > 0 || item.bonusArm > 0 || item.bonusPrc > 0 || item.bonusMgcArm > 0)
            {
                InventorySlot slot = null;
                for (int i = 0; i < playerSlots.childCount; i++)
                {
                    if (playerSlots.GetChild(i).GetComponent<InventorySlot>().item == item)
                    {
                        slot = playerSlots.GetChild(i).GetComponent<InventorySlot>();
                        break;
                    }
                }
                battle.player.UnequipItemPrepared(item, slot.lvl);
                UpdateStats(item, fromMeta);
                battle.player.EquipItem(item, slot.lvl);
                break;
            }
            else
            {
                UpdateStats(item, fromMeta);
            }*/
        }
        bool[] haveBonusHerb = new bool[6];
        foreach(Herb item in gatheringManager.slots)
        {
            if (item != null)
            {
                haveBonusHerb[0] = item.bonusPP > 0;
                haveBonusHerb[1] = item.bonusHP > 0;
                haveBonusHerb[2] = item.bonusMP > 0;
                haveBonusHerb[3] = item.bonusArm > 0;
                haveBonusHerb[4] = item.bonusPrc > 0;
                haveBonusHerb[5] = item.bonusMgcArm > 0;
            }
            else
            {
                count++;
            }
        }
        if (haveBonusHerb[0] && stat == StatUpgrade.Stats.PP)
            UpdateBonus(StatUpgrade.Stats.PP, fromMeta);
        if (haveBonusHerb[1] && stat == StatUpgrade.Stats.HP)
            UpdateBonus(StatUpgrade.Stats.HP, fromMeta);
        if (haveBonusHerb[2] && stat == StatUpgrade.Stats.MP)
            UpdateBonus(StatUpgrade.Stats.MP, fromMeta);
        if (haveBonusHerb[3] && stat == StatUpgrade.Stats.ARM)
            UpdateBonus(StatUpgrade.Stats.ARM, fromMeta);
        if (haveBonusHerb[4] && stat == StatUpgrade.Stats.PRC)
            UpdateBonus(StatUpgrade.Stats.PRC, fromMeta);
        if (haveBonusHerb[5] && stat == StatUpgrade.Stats.MRM)
            UpdateBonus(StatUpgrade.Stats.MRM, fromMeta);

        /*
            foreach(Herb item in gatheringManager.slots)
            {
                if(item != null)
                {
                    if (item.bonusPP > 0 || item.bonusHP > 0 || item.bonusMP > 0 || item.bonusArm > 0 || item.bonusPrc > 0 || item.bonusMgcArm > 0)
                    {
                        battle.player.DisableHerbPrepared(item);
                        UpdateStats(item, fromMeta);
                        battle.player.UseHerb(item);
                    }
                    else
                    {
                        UpdateStats(item, fromMeta);
                    }
                }
                else
                {
                    count++;
                }
            }*/

        if (count == (inventory.playerSlots.Length + gatheringManager.slots.Length))
        {
            UpdateBonus(stat, fromMeta);
        }
        else
        {
            if (!haveBonus[0] && !haveBonusHerb[0] && stat == StatUpgrade.Stats.PP)
            {
                UpdateBonus(StatUpgrade.Stats.PP, fromMeta);
            }
            if (!haveBonus[1] && !haveBonusHerb[1] && stat == StatUpgrade.Stats.HP)
            {
                UpdateBonus(StatUpgrade.Stats.HP, fromMeta);
            }
            if (!haveBonus[2] && !haveBonusHerb[2] && stat == StatUpgrade.Stats.MP)
            {
                UpdateBonus(StatUpgrade.Stats.MP, fromMeta);
            }
            if (!haveBonus[3] && !haveBonusHerb[3] && stat == StatUpgrade.Stats.ARM)
            {
                UpdateBonus(StatUpgrade.Stats.ARM, fromMeta);
            }
            if (!haveBonus[4] && !haveBonusHerb[4] && stat == StatUpgrade.Stats.PRC)
            {
                UpdateBonus(StatUpgrade.Stats.PRC, fromMeta);
            }
            if (!haveBonus[5] && !haveBonusHerb[5] && stat == StatUpgrade.Stats.MRM)
            {
                UpdateBonus(StatUpgrade.Stats.MRM, fromMeta);
            }
        }
        battle.player.CheckStatLimit();
    }

    IEnumerator UpdateBonusesWithWait()
    {
        yield return new WaitUntil(() => AutoSave.Reseted);

        int count = 0;
        foreach (GameItem item in inventory.playerSlots)
        {
            if (item != null)
            {
                InventorySlot slot = null;
                for (int i = 0; i < playerSlots.childCount; i++)
                {
                    if (playerSlots.GetChild(i).GetComponent<InventorySlot>().item == item)
                    {
                        slot = playerSlots.GetChild(i).GetComponent<InventorySlot>();
                        break;
                    }
                }
                if (slot != null)
                {
                    //Debug.Log(battle.player.physicPower + ":" + item.bonusPP);
                    battle.player.UnequipItem(item, slot.lvl);
                }

                //ReturnStats();
                if (slot != null)
                {
                    //Debug.Log(battle.player.physicPower + ":" + item.bonusPP);
                    battle.player.EquipItem(item, slot.lvl);
                    //Debug.Log(battle.player.physicPower + ":" + item.bonusPP);
                }
            }
            else
            {
                count++;
            }
        }
        if(count == inventory.playerSlots.Length)
        {
            ReturnStats();
        }
    }

    void Reincarnate()
    {
        GameManager.qi += CalculateQi();
        GameManager.UpdateText();
        ResetData();
        StartCoroutine(UpdateBonusesWithWait());

        totalPlayTime = 0;
        totalEarnMagic = 0;
        totalEarnXp = 0;
        totalKilledEnemies = 0;
        setManager.GiveBonuses();
    }

    void ResetData()
    {
        battle.enemyHp.transform.GetChild(1).GetChild(0).GetComponent<Image>().color = new Color32(255, 31, 0, 255);
        battle.finalBossAdder.SetActive(false);
        battle.enemyObject.GetComponent<Animator>().enabled = false;
        battle.bossTreat.GetComponent<Text>().text = "BOSS";
        battle.bossTreat.SetActive(false);

        for (int i = 0; i < abilityManager.slots.Length; i++)
        {
            abilityManager.slots[i] = null;
        }

        saves.ResetByReincarnation();
    }

    double CalculateQi()
    {
        double qi = 0;
        double qiFromTime = (int)Math.Floor(totalPlayTime / 3600);

        int enemyTimes = (int)Math.Floor(totalKilledEnemies / 6);
        double qiFromEnemies = 5 * totalKilledEnemies * enemyTimes;

        double magicLimit = 1000;
        double qiFromMagic = 0;

        while(Math.Floor(totalEarnMagic / magicLimit) != 0)
        {
            qiFromMagic++;
            qiFromMagic *= 4;
            magicLimit = magicLimit * 50;
        }

        double xpLimit = 10000;
        double qiFromXp = 0;

        while (Math.Floor(totalEarnXp / xpLimit) != 0)
        {
            qiFromXp += 10;
            qiFromXp *= 2;
            xpLimit = xpLimit * 25;
        }

        qi = qiFromTime + qiFromEnemies + qiFromMagic + qiFromXp;

        // every 1h = 1 qi
        // every 6enemies = 5*totalKilledEnemies
        // every 1000magic = 1 qi, limit*100
        // every 10000xp = 10 qi, limit*25
        return qi;
    }

    public void Save()
    {
        SaveSystem.SaveReincarnation();
    }

    public void Load()
    {
        ReincarnationData data = SaveSystem.LoadReincarnation();

        if (data != null)
        {
            totalPlayTime = data.totalPlayTime;
            totalEarnMagic = data.totalEarnMagic;
            totalEarnXp = data.totalEarnXp;
            totalKilledEnemies = data.totalKilledEnemies;
            permanentsUpgrades = data.permanentsUpgrades;
        }
    }

    public void SimulateTime(float total)
    {
        totalPlayTime += total;
    }
}
