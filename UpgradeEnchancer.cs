using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using NT_Utils.Conversion;
using System;

public class UpgradeEnchancer : MonoBehaviour
{
    public enum Stats
    {
        PP,
        MP,
        ARM,
        MRM,
        PRC,
        CRT,
        HP
    }

    public enum Enchance
    {
        Gain,
        Time,
        Cost
    }

    public string[] costs;
    public Enchance[] upgradeTypes;

    public string[] descriptions;

    public int level = 0;
    public bool isActivated = false;

    void Start()
    {
        transform.Find("LVL1").Find("BuyBut").GetComponent<Button>().onClick.AddListener(delegate { Buy(1); });
        transform.Find("LVL2").Find("BuyBut").GetComponent<Button>().onClick.AddListener(delegate { Buy(2); });
        transform.Find("LVL3").Find("BuyBut").GetComponent<Button>().onClick.AddListener(delegate { Buy(3); });
        transform.Find("LVL4").Find("BuyBut").GetComponent<Button>().onClick.AddListener(delegate { Buy(4); });

        UpdateCostText();

        transform.Find("LVL1").Find("Description").GetComponent<Text>().text = descriptions[0];
        transform.Find("LVL2").Find("Description").GetComponent<Text>().text = descriptions[1];
        transform.Find("LVL3").Find("Description").GetComponent<Text>().text = descriptions[2];
        transform.Find("LVL4").Find("Description").GetComponent<Text>().text = descriptions[3];

        if (!isActivated)
            DeactivateOnScreen();
    }

    void UpdateUI()
    {
        string findObjName = this.transform.name.Replace("_Upgrader", "");
        findObjName = findObjName + "_UPG";
        GameObject findObj = this.transform.parent.Find(findObjName).gameObject;
        int num = findObj.GetComponent<StatUpgrade>().upgradeLvl;
        for (int i = 1; i < num; i++)
        {
            transform.Find("LVL" + i.ToString()).Find("BuyBut").gameObject.SetActive(false);
            transform.Find("LVL" + i.ToString()).Find("BoughtText").gameObject.SetActive(true);
            transform.Find("LVL" + i.ToString()).Find("Title").GetComponent<Text>().color = Color.green;
        }
        UpdateCostText();
    }

    void Buy(int num)
    {
        if (num == (level+1))
        {
            if (!costs[level].Contains("e"))
            {
                if (GameManager.magic >= int.Parse(costs[level]))
                {
                    GameManager.magic -= int.Parse(costs[level]);
                    GameManager.UpdateText();
                    level += 1;
                    transform.Find("LVL" + num.ToString()).Find("BuyBut").gameObject.SetActive(false);
                    transform.Find("LVL" + num.ToString()).Find("BoughtText").gameObject.SetActive(true);
                    transform.Find("LVL" + num.ToString()).Find("Title").GetComponent<Text>().color = Color.green;
                    ExecuteUpgrade(upgradeTypes[level - 1]);
                }
            }
            else
            {
                if (GameManager.magic >= double.Parse(ScienceNumber.ConvertFromScience(costs[level])))
                {
                    GameManager.magic -= double.Parse(ScienceNumber.ConvertFromScience(costs[level]));
                    GameManager.UpdateText();
                    level += 1;
                    transform.Find("LVL" + num.ToString()).Find("BuyBut").gameObject.SetActive(false);
                    transform.Find("LVL" + num.ToString()).Find("BoughtText").gameObject.SetActive(true);
                    transform.Find("LVL" + num.ToString()).Find("Title").GetComponent<Text>().color = Color.green;
                    ExecuteUpgrade(upgradeTypes[level - 1]);
                }
            }
        }
    }

    void UpdateCostText()
    {
        for (int i = 1; i <= (transform.childCount - 2); i++)
        {
            transform.GetChild(i).Find("Cost").GetComponent<Text>().text = NumberConversion.AbbreviateNumber(double.Parse(costs[i - 1]));
        }
    }

    void ExecuteUpgrade(Enchance enchance)
    {
        if(enchance == Enchance.Gain)
        {
            string findObjName = this.transform.name.Replace("_Upgrader", "");
            findObjName = findObjName + "_UPG";
            GameObject findObj = this.transform.parent.Find(findObjName).gameObject;

            findObj.GetComponent<StatUpgrade>().upgradeLvl += 1;
            findObj.GetComponent<StatUpgrade>().gainLvl += 1;
        }
        if (enchance == Enchance.Time)
        {
            string findObjName = this.transform.name.Replace("_Upgrader", "");
            findObjName = findObjName + "_UPG";
            GameObject findObj = this.transform.parent.Find(findObjName).gameObject;

            findObj.GetComponent<StatUpgrade>().upgradeLvl += 1;
            findObj.GetComponent<StatUpgrade>().timeLvl += 1;
            if (!findObj.GetComponent<StatUpgrade>().isUpgrading)
            {
                findObj.GetComponent<StatUpgrade>().UpdateTime();
            }
        }
        if (enchance == Enchance.Cost)
        {
            string findObjName = this.transform.name.Replace("_Upgrader", "");
            findObjName = findObjName + "_UPG";
            GameObject findObj = this.transform.parent.Find(findObjName).gameObject;

            findObj.GetComponent<StatUpgrade>().upgradeLvl += 1;
            findObj.GetComponent<StatUpgrade>().costLvl += 1;
        }
    }

    public void ActivateOnScreen()
    {
        this.transform.localPosition = new Vector3(-588.98f, 304,0);
        isActivated = true;
    }

    public void DeactivateOnScreen()
    {
        this.transform.localPosition = new Vector3(-10000, 304, 0);
        isActivated = false;
    }

    public void SaveUpgradeEnchance()
    {
        string nm = this.name.Replace("_Upgrader", "");

        SaveSystem.SaveUpgradeEnchance(level, isActivated, nm);
    }

    public void LoadUpgradeEnchance()
    {
        string nm = this.name.Replace("_Upgrader", "");
        UpgradeEnchanceData data = SaveSystem.LoadUpgradeEnchance(nm);

        if (data != null)
        {
            this.level = data.lvl;
            this.isActivated = data.isActivated;
            UpdateUI();
        }
        else
        {
            this.level = 1;
            this.isActivated = false;
            DeactivateOnScreen();
            level = 0;
            for(int num = 1; num < 4; num++)
            {
                transform.Find("LVL" + num.ToString()).Find("BuyBut").gameObject.SetActive(true);
                transform.Find("LVL" + num.ToString()).Find("BoughtText").gameObject.SetActive(false);
                transform.Find("LVL" + num.ToString()).Find("Title").GetComponent<Text>().color = Color.red;
            }
        }
    }
}
