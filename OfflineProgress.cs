using System;
using UnityEngine;
using UnityEngine.UI;
using NT_Utils.Conversion;
using System.Collections;

public class OfflineProgress : MonoBehaviour
{
    public Text timeText;

    public Text earningsText;

    public Transform trainings;
    public Transform techniques;
    public Transform statUpgs;
    public Transform growings;
    public Transform searchings;

    public Reincarnation reincarnation;

    public Button closeBut;

    public Battle battle;

    public DailyManager dailyManager;

    public Reroll shop;

    public static bool loaded;

    private double magicAdded;

    public void Start()
    {
        closeBut.onClick.AddListener(CloseScreen);

        StartCoroutine("GetOffline");
    }

    IEnumerator GetOffline()
    {
        yield return new WaitUntil(() => AutoSave.Loaded);

        OfflineData data = SaveSystem.LoadOffline();
        if (data != null)
        {
            this.transform.localPosition = new Vector3(0, 0, 0);

            DateTime date = DateTime.UtcNow;
            float day = date.Day * 24 * 3600;
            float hour = date.Hour * 3600;
            float minute = date.Minute * 60;
            float second = date.Second;
            float total = day + hour + minute + second;
            float res = total - data.total;

            shop.GetOfflineProgress(res);

            TimeSpan time = TimeSpan.FromSeconds(res);

            string timePass = time.Days + "D " + time.Hours + "H " + time.Minutes + "M " + time.Seconds + "S ";
            timeText.text = "It's been " + timePass + "\nsince you've been away";

            GetReincarnationOffline(res);
            GetDailyOffline(res);

            magicAdded = GetMagicOffline(res, true);

            earningsText.text = NumberConversion.AbbreviateNumber(magicAdded) + " Magic\n" + NumberConversion.AbbreviateNumber(GetXpOffline(res)) + " Xp\n" + GetStatsOffline(res);
            GetSeedsOffline(res, true);
            GetProductsOffline(res, true);
            GetManaOffline(res);

            dailyManager.UpdateAfterLoad();
        }
        else
        {
            CloseScreen();
        }
        loaded = true;
    }

    public void UpdateOffline()
    {
        OfflineData data = SaveSystem.LoadOffline();
        if (data != null)
        {
            this.transform.localPosition = new Vector3(0, 0, 0);

            DateTime date = DateTime.UtcNow;
            float day = date.Day * 24 * 3600;
            float hour = date.Hour * 3600;
            float minute = date.Minute * 60;
            float second = date.Second;
            float total = day + hour + minute + second;
            float res = (total + 86400) - data.total;

            shop.GetOfflineProgress(res);

            TimeSpan time = TimeSpan.FromSeconds(res);

            string timePass = time.Days + "D " + time.Hours + "H " + time.Minutes + "M " + time.Seconds + "S ";
            timeText.text = "It's been " + timePass + "\nsince you've been away";

            GetReincarnationOffline(res);
            GetDailyOffline(res);

            magicAdded = GetMagicOffline(res, true);

            earningsText.text = NumberConversion.AbbreviateNumber(magicAdded) + " Magic\n" + NumberConversion.AbbreviateNumber(GetXpOffline(res)) + " Xp\n" + GetStatsOffline(res);
            GetSeedsOffline(res, true);
            GetProductsOffline(res, true);
            GetManaOffline(res);

            dailyManager.UpdateAfterLoad();
        }
        else
        {
            CloseScreen();
        }
        loaded = true;
    }

    public void CloseScreen()
    {
        this.transform.localPosition = new Vector3(-10000, 0, 0);
    }

    public void SaveOffline()
    {
        DateTime date = DateTime.UtcNow;
        float day = date.Day * 24 * 3600;
        float hour = date.Hour * 3600;
        float minute = date.Minute * 60;
        float second = date.Second;
        float total = day + hour + minute + second;
        SaveSystem.SaveOffline(total);
    }

    public double GetMagicOffline(float total, bool addMagic) {
        double magic = 0;

        for (int i = 0; i < trainings.childCount; i++)
        {
            Train train = trainings.GetChild(i).GetComponent<Train>();
            if (train.lvl > 0 && train.isTraining)
            {
                magic = magic + train.SimulateTime(total, addMagic);
            }
        }
        return magic;
    }

    public void GetReincarnationOffline(float total)
    {
        reincarnation.SimulateTime(total);
    }

    public int GetXpOffline(float total)
    {
        int xp = 0;

        for (int i = 0; i < techniques.childCount; i++)
        {
            Technique tech = techniques.GetChild(i).GetComponent<Technique>();
            if (tech.lvl > 0 && tech.techType == Technique.Type.Passive)
            {
                if (tech.isTraining)
                {
                    xp = xp + tech.SimulateTime(total);
                }
            }
        }
        return xp;
    }

    public string GetStatsOffline(float total)
    {
        double stat = 0;
        string statText = "";

        double magicWasteSpeed = 0;
        double magicWaste = 0;
        for (int i = 2; i <= 8; i++)
        {
            StatUpgrade upg = statUpgs.GetChild(i).GetComponent<StatUpgrade>();
            if (upg.upgradeLvl > 0)
            {
                if (upg.isUpgrading || upg.isLocked)
                {
                    magicWasteSpeed += upg.upgradeCosts[upg.costLvl];
                    magicWaste += (magicWasteSpeed / upg.statTimeLimits[upg.timeLvl]) * total;
                }
            }
        }
        // 1mg/s 63s wasted 6.3 magic 5
        double magicReserve = GameManager.magic;

        // если магии достаточно для улучшения
        if(magicReserve >= magicWaste)
        { 
            for (int i = 2; i <= 8; i++)
            {
                StatUpgrade upg = statUpgs.GetChild(i).GetComponent<StatUpgrade>();
                if (upg.upgradeLvl > 0)
                {
                    if (upg.isUpgrading || upg.isLocked)
                    {
                        stat = stat + upg.SimulateTime(total);
                        string indexName = statUpgs.GetChild(i).name.Replace("_UPG", "");
                        indexName.Replace("_UPG", "");
                        if(upg.stat == StatUpgrade.Stats.CRT)
                        {
                            stat = stat * 100;
                        }
                        else if(upg.stat == StatUpgrade.Stats.PRC)
                        {
                            stat = stat / 100;
                        }
                        else if (upg.stat == StatUpgrade.Stats.ARM || upg.stat == StatUpgrade.Stats.MRM)
                        {
                            stat = Math.Round(stat, 2);
                        }
                        statText += stat + " " + indexName + "\n";
                    }
                }
            }
        }
        else // если магии недостаточно для улучшения всё время
        {
            // wasted 2 magic 1
            if(magicReserve > 0)
            {
                float reducedTotal = 0;
                for (int i = 2; i <= 8; i++)
                {
                    StatUpgrade upg = statUpgs.GetChild(i).GetComponent<StatUpgrade>();

                    if (upg.upgradeLvl > 0)
                    {
                        if (upg.isUpgrading || upg.isLocked)
                        {
                            if (magicReserve == 0)
                            {
                                reducedTotal += upg.statTimeLimits[upg.timeLvl];
                            }
                            else
                            {
                                reducedTotal += (float)Math.Floor((magicReserve / (magicWasteSpeed / upg.statTimeLimits[upg.timeLvl])));
                            }

                            stat = stat + upg.SimulateTime(reducedTotal);
                            string indexName = statUpgs.GetChild(i).name.Replace("_UPG", "");
                            indexName.Replace("_UPG", "");
                            if (upg.stat == StatUpgrade.Stats.CRT)
                            {
                                stat = stat * 100;
                            }
                            else if (upg.stat == StatUpgrade.Stats.PRC)
                            {
                                stat = stat / 100;
                            }
                            else if (upg.stat == StatUpgrade.Stats.ARM || upg.stat == StatUpgrade.Stats.MRM)
                            {
                                stat = Math.Round(stat, 2);
                            }
                            statText += stat + " " + indexName + "\n";
                            break;
                        }
                    }
                }
            }
            else
            {
                for (int i = 2; i <= 8; i++)
                {
                    StatUpgrade upg = statUpgs.GetChild(i).GetComponent<StatUpgrade>();

                    if (upg.upgradeLvl > 0)
                    {
                        if (upg.isUpgrading)
                        {
                            
                            float reducedTotal = 0;
                            if (total >= upg.statTimeLimits[upg.timeLvl])
                            {
                                reducedTotal = upg.statTimeLimits[upg.timeLvl];
                            }
                            else
                            {
                                reducedTotal = total;
                            }
                            stat = stat + upg.SimulateTime(reducedTotal);
                            string indexName = statUpgs.GetChild(i).name.Replace("_UPG", "");
                            indexName.Replace("_UPG", "");
                            if (upg.stat == StatUpgrade.Stats.CRT)
                            {
                                stat = stat * 100;
                            }
                            else if (upg.stat == StatUpgrade.Stats.PRC)
                            {
                                stat = stat / 100;
                            }
                            else if (upg.stat == StatUpgrade.Stats.ARM || upg.stat == StatUpgrade.Stats.MRM)
                            {
                                stat = Math.Round(stat, 2);
                            }
                            statText += stat + " " + indexName + "\n";
                            break;
                        }
                        else if(upg.isLocked)
                        {
                            stat = stat + upg.SimulateTime(0);
                            break;
                        }
                    }
                }
            }
        }
        
        
        return statText;
    }

    public void GetManaOffline(float total)
    {
        if(battle.player.mana < battle.player.maxMana)
        {
            float manaRegSpeed = battle.player.manaRegSpeed / 100;
            double regAmount = Math.Floor(total / manaRegSpeed);
            battle.player.RegenerateMana(regAmount);
        }
    }

    public void GetDailyOffline(float total)
    {
        DailyManager.currentTime += total;
    }

    public int GetSeedsOffline(float total, bool addSeeds)
    {
        int seeds = 0;

        for (int i = 1; i < searchings.childCount; i++)
        {
            Searching search = searchings.GetChild(i).GetComponent<Searching>();
            if (search.lvl > 0)
            {
                if(search.isGrowing)
                    seeds = seeds + search.SimulateTime(total, addSeeds);
            }
        }
        return seeds;
    }

    public int GetProductsOffline(float total, bool addProducts)
    {
        int products = 0;

        for (int i = 1; i < growings.childCount; i++)
        {
            Growing grow = growings.GetChild(i).GetComponent<Growing>();
            if(grow.isGrowing)
                products = products + grow.SimulateTime(total, addProducts);
        }
        return products;
    }
}
