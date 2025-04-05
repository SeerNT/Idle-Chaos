using NT_Utils.Conversion;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PermanentUpgrade : MonoBehaviour
{
    public enum Currency
    {
        QI,
        META
    }

    private Text titleText;
    private Text descText;
    private Text costText;
    private Text amountText;
    private Button buyBut;

    [SerializeField] private Reincarnation reincarnation;

    [Header("Settings")]
    public int increase = 1;
    public int amount = 0;
    public int baseCost = 5;
    public StatUpgrade.Stats stat;
    public Currency currency;
    [Header("Changes")]
    public int bought = 0;
    public float multiplier = 1.55f;
    public double cost = 5;

    void Start()
    {
        titleText = transform.Find("Title").GetComponent<Text>();
        descText = transform.Find("Description").GetComponent<Text>();
        costText = transform.Find("Cost").GetComponent<Text>();
        amountText = transform.Find("Amount").GetComponent<Text>();
        buyBut = transform.Find("BuyBut").GetComponent<Button>();

        titleText.text = stat.ToString();
        descText.text = "Increase stat " + increase.ToString() + "%";
        costText.text = NumberConversion.AbbreviateNumber(cost);
        amountText.text = "Amount:" + NumberConversion.AbbreviateNumber(amount);

        buyBut.onClick.AddListener(Buy);
    }

    void Update()
    {
        costText.text = NumberConversion.AbbreviateNumber(cost);
        amountText.text = "Amount:" + NumberConversion.AbbreviateNumber(amount);

        string name = this.name;
        name = name.Replace("Upg", "");
        //if((int.Parse(name) - 1) != 6 && (int.Parse(name) - 1) != 7)
        Reincarnation.permanentsUpgrades[int.Parse(name) - 1] = (amount * 100);
    }
    void Buy()
    {
        if(currency == Currency.QI)
        {
            if (GameManager.qi >= cost && amount < 35)
            {
                GameManager.qi = (double)(GameManager.qi - cost);
                GameManager.UpdateText();
                bought++;
                cost = baseCost * Math.Pow(multiplier, bought);
                cost = Math.Ceiling(cost);
                costText.text = NumberConversion.AbbreviateNumber(cost);
                amount++;
                string name = this.name;
                name = name.Replace("Upg", "");

                Reincarnation.permanentsUpgrades[int.Parse(name) - 1] = (amount * 100);
                //reincarnation.UpdateBonus(stat, false);
                reincarnation.UpdateBonusForItem(stat, false);
            }
        }
        else if(currency == Currency.META)
        {
            if (GameManager.meta >= cost && amount < 35)
            {
                GameManager.meta = (double)(GameManager.meta - cost);
                GameManager.UpdateText();
                bought++;
                cost = baseCost * Math.Pow(multiplier, bought);
                cost = Math.Ceiling(cost);
                costText.text = NumberConversion.AbbreviateNumber(cost);
                amount++;
                string name = this.name;
                name = name.Replace("Upg", "");
               
                Reincarnation.permanentsUpgrades[int.Parse(name) - 1] = (amount * increase);
                //reincarnation.UpdateBonus(stat, true);
                reincarnation.UpdateBonusForItem(stat, true);
            }
        }
        
    }

    public void Save()
    {
        SaveSystem.SavePermanentUpgrade(amount, bought, cost, baseCost, this.name);
    }

    public void Load()
    {
        PermanentUpgradeData data = SaveSystem.LoadPermanentUpgrade(this.name);

        if (data != null)
        {
            amount = data.amount;
            bought = data.bought;
            cost = data.cost;
            baseCost = data.baseCost;

            costText = transform.Find("Cost").GetComponent<Text>();
            amountText = transform.Find("Amount").GetComponent<Text>();

            costText.text = NumberConversion.AbbreviateNumber(cost);
            amountText.text = "Amount:" + NumberConversion.AbbreviateNumber(amount);
        }
    }
}
