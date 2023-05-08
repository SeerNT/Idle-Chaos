using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using NT_Utils.Conversion;
using System;

public class TechniqueUpgrade : MonoBehaviour
{
    public enum Enchance
    {
        Gain,
        Time,
        Ability
    }

    public string[] costs;
    public Enchance[] upgradeTypes;

    public string[] descriptions;

    public int level = 0;
    public bool isActivated = false;

    [Header("Ability")]
    public Bonus.Type bonusType;
    public string bonusValue;
    public Bonus bonus;

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
        GameObject findObj = this.transform.parent.parent.gameObject;
        int num = findObj.GetComponent<Technique>().lvl;
        for (int i = 1; i < num; i++)
        {
            transform.Find("LVL" + i.ToString()).Find("BuyBut").gameObject.SetActive(false);
            transform.Find("LVL" + i.ToString()).Find("BoughtText").gameObject.SetActive(true);
            transform.Find("LVL" + i.ToString()).Find("Title").GetComponent<Text>().color = Color.green;
        }
        UpdateCostText();
    }

    void UpdateCostText()
    {
        for(int i = 1; i <= (transform.childCount-2); i++)
        {
            transform.GetChild(i).Find("Cost").GetComponent<Text>().text = NumberConversion.AbbreviateNumber(double.Parse(costs[i - 1]));
        }
    }

    void Buy(int num)
    {
        if (num == (level + 1))
        {
            GameObject findObj = this.transform.parent.parent.gameObject;
            if(findObj.GetComponent<Technique>().lvl != 0)
            {
                if (!costs[level].Contains("e"))
                {
                    if (GameManager.xp >= int.Parse(costs[level]))
                    {
                        GameManager.xp -= int.Parse(costs[level]);
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
                    if (GameManager.xp >= double.Parse(ScienceNumber.ConvertFromScience(costs[level])))
                    {
                        GameManager.xp -= double.Parse(ScienceNumber.ConvertFromScience(costs[level]));
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
    }

    void ExecuteUpgrade(Enchance enchance)
    {
        if (enchance == Enchance.Gain)
        {
            GameObject findObj = this.transform.parent.parent.gameObject;

            findObj.GetComponent<Technique>().lvl += 1;
            findObj.GetComponent<Technique>().gainLvl += 1;
        }
        if (enchance == Enchance.Time)
        {
            GameObject findObj = this.transform.parent.parent.gameObject;

            findObj.GetComponent<Technique>().lvl += 1;
            findObj.GetComponent<Technique>().timeLvl += 1;
        }
        if(enchance == Enchance.Ability)
        {
            bonus = new Bonus(bonusType, bonusValue);
            bonus.GetBonus();
        }
    }

    public void ActivateOnScreen()
    {
        this.transform.localPosition = new Vector3(-576.4f, 575.1f, 0);
        isActivated = true;
    }

    public void DeactivateOnScreen()
    {
        this.transform.localPosition = new Vector3(-10000, 575.1f, 0);
        isActivated = false;
    }
    public void SaveUpgrade()
    {
        string nm = this.transform.parent.parent.name;

        SaveSystem.SaveTechUpgrade(this, nm);
    }

    public void LoadUpgrade()
    {
        string nm = this.transform.parent.parent.name;
        UpgradeTechData data = SaveSystem.LoadUpgradeTech(nm);

        if (data != null)
        {
            this.level = data.level;
            this.isActivated = data.isActivated;

            if (this.isActivated)
            {
                ActivateOnScreen();
            }

            UpdateUI();
        }
        else
        {
            this.level = 1;
            this.isActivated = false;
            DeactivateOnScreen();
            level = 0;
            for (int num = 1; num < 4; num++)
            {
                transform.Find("LVL" + num.ToString()).Find("BuyBut").gameObject.SetActive(true);
                transform.Find("LVL" + num.ToString()).Find("BoughtText").gameObject.SetActive(false);
                transform.Find("LVL" + num.ToString()).Find("Title").GetComponent<Text>().color = Color.red;
            }
        }
    }
}
