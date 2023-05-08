using NT_Utils.Conversion;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class TempUpgrade : MonoBehaviour
{
    private Text titleText;
    private Text descText;
    private Text costText;
    private Text amountText;
    private Text timeText;
    private Button buyBut;
    private Button useBut;

    [SerializeField] private Reincarnation reincarnation;

    [Header("Settings")]
    public int increase = 1;
    public int amount = 0;
    public int baseCost = 5;
    public float curLimitTime = 10f;
    public float limitTime = 10f;
    public bool isActive = false;
    public StatUpgrade.Stats stat;

    private CancellationTokenSource tokenSource;

    void Start()
    {
        titleText = transform.Find("Title").GetComponent<Text>();
        descText = transform.Find("Description").GetComponent<Text>();
        costText = transform.Find("Cost").GetComponent<Text>();
        amountText = transform.Find("Amount").GetComponent<Text>();
        timeText = transform.Find("Time").GetComponent<Text>();
        buyBut = transform.Find("BuyBut").GetComponent<Button>();
        useBut = transform.Find("UseBut").GetComponent<Button>();

        titleText.text = stat.ToString();
        descText.text = "Increase stat " + increase.ToString() + "%";
        costText.text = NumberConversion.AbbreviateNumber(baseCost);
        amountText.text = "Amount:" + NumberConversion.AbbreviateNumber(amount);
        timeText.text = "Time: " + TimeConversion.AbbreviateTime(curLimitTime);

        buyBut.onClick.AddListener(Buy);
        useBut.onClick.AddListener(Use);
    }

    void Update()
    {
        costText.text = NumberConversion.AbbreviateNumber(baseCost);
        amountText.text = "Amount:" + NumberConversion.AbbreviateNumber(amount);
        timeText.text = "Time: " + TimeConversion.AbbreviateTime(curLimitTime);

        string name = this.name;
        name = name.Replace("Upg", "");
    }

    void Buy()
    {
        if (GameManager.meta >= baseCost)
        {
            GameManager.meta = (int)(GameManager.meta - baseCost);
            GameManager.UpdateText();
            costText.text = NumberConversion.AbbreviateNumber(baseCost);
            amount++;
        }
    }

    public async Task TaskDelayWithCancel(CancellationToken token)
    {
        Debug.Log(limitTime);
        await Task.Delay((int)(limitTime * 1000), token).ContinueWith(t => DisableBonus());
    }

    void Use()
    {
        if (amount > 0)
        {
            amount--;
            if (tokenSource != null)
            {
                tokenSource.Cancel();
                curLimitTime = -10;
                CancelInvoke();
                DisableBonus();
            }

            GiveBonus();

            InvokeRepeating("IncreaseTime", 0, 1);
            tokenSource = new CancellationTokenSource();
        }
    }

    void GiveBonus()
    {
        string name = this.name;
        name = name.Replace("Upg", "");

        //Reincarnation.permanentsUpgrades[int.Parse(name) - 1] += (increase);
        //reincarnation.UpdateBonuses();
    }

    void DisableBonus()
    {
        string name = this.name;
        name = name.Replace("Upg", "");

        //Reincarnation.permanentsUpgrades[int.Parse(name) - 1] -= (increase);
        //reincarnation.UpdateBonuses();

        curLimitTime = limitTime;
    }

    public void IncreaseTime()
    {
        curLimitTime--;
        if(curLimitTime <= 0)
        {
            DisableBonus();
            CancelInvoke();
        }
    }

    /*
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
    }*/
}
