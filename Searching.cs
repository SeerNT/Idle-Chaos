using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using NT_Utils.Conversion;

public class Searching : MonoBehaviour
{
    [Header("Objects")]
    public Slider progressBar;
    public Text titleText;
    public Text lvlText;
    public Text chanceText;
    public Text timer;
    public Button startBut;
    public GatheringManager manager;
    [Header("Settings")]
    public float curLimitTime = 10f;
    public float limitTime = 10f;
    public int giveAmount = 1;
    public int lvl = 1;
    public int seedChance = 85;
    public string title;
    [Header("Changes")]
    public bool isGrowing = false;
    public bool isWaiting = false;

    private float fillSpeed;

    public float progressValue;
    public bool isSetupPB;

    public bool loaded = false;

    void Start()
    {
        curLimitTime = limitTime;
        isSetupPB = false;

        startBut.onClick.AddListener(StartSearching);

        fillSpeed = 1 / limitTime;

        lvlText.text = "Lvl:" + lvl.ToString();
        chanceText.text = seedChance.ToString() + "%";

        UpdateTimer();
        titleText.text = title;
    }

    public void UpdateTimer()
    {
        timer.text = TimeConversion.AbbreviateTime(curLimitTime);
    }

    void Update()
    {
        if (!isGrowing)
        {
            isWaiting = false;
            curLimitTime = limitTime;
            UpdateTimer();

        }

        fillSpeed = 1 / limitTime;

        lvlText.text = "Lvl:" + lvl.ToString();
        chanceText.text = seedChance.ToString() + "%";

        if (isGrowing && progressBar.value < 1)
        {
            if (!SaveSystem.trainLoaded || isSetupPB)
            {
                progressBar.value += fillSpeed * Time.deltaTime;
                progressValue = progressBar.value;
            }
            else
            {
                if (isGrowing)
                {
                    progressBar.value = progressValue;
                }

                isSetupPB = true;
            }
            curLimitTime = (float)Math.Round(limitTime - (progressBar.value * limitTime), 1);
            UpdateTimer();
        }
        if (progressBar.value >= 1)
        {
            isGrowing = false;
            GiveProduct();
        }
    }

    void GiveProduct()
    {
        progressBar.value = 0;
        startBut.gameObject.GetComponent<Image>().color = Color.yellow;
        timer.text = limitTime.ToString() + "s";
        curLimitTime = 0;
        System.Random generator = new System.Random(DateTime.Now.Millisecond);
        if (generator.Next(100) < seedChance)
        {
            manager.GiveProductSeed(this);
        }
    }

    public void StartSearching()
    {
        if (this.lvl > 0)
        {
            if (!isGrowing)
            {
                isGrowing = true;
                startBut.gameObject.GetComponent<Image>().color = Color.red;
            }
        }
    }

    public void Reset()
    {
        progressBar.value = 0;
        startBut.gameObject.GetComponent<Image>().color = Color.yellow;
        timer.text = limitTime.ToString() + "s";
        curLimitTime = 0;
    }

    public void SaveSearching()
    {
        SaveSystem.SaveSearching(this, this.name);
    }

    public void LoadSearching()
    {
        SearchData data = SaveSystem.LoadSearching(this.name);

        if (data != null)
        {
            loaded = true;

            progressValue = data.progressValue;
            lvl = data.lvl;

            isGrowing = data.isGrowing;
            isWaiting = data.isWaiting;

            seedChance = data.seedChance;
            giveAmount = data.giveAmount;
            curLimitTime = data.curLimitTime;
            limitTime = data.limitTime;

            lvlText.text = "Lvl:" + lvl.ToString();
            chanceText.text = seedChance.ToString() + "%";

            if (this.isGrowing)
            {
                UpdateTimer();
                startBut.gameObject.GetComponent<Image>().color = Color.red;
            }
            else
            {
                timer.text = limitTime.ToString() + "s";
                curLimitTime = 0;
            }
        }
    }

    public int SimulateTime(float total, bool addSeeds)
    {
        int seeds = 0;
        float left = total;

        if ((limitTime - (limitTime-curLimitTime)) < left)
        {
            isGrowing = false;
            if (addSeeds)
                GiveProduct();
            seeds += giveAmount;
        }
        else if (curLimitTime != limitTime)
        {
            curLimitTime = curLimitTime - left;
            isGrowing = true;
            startBut.gameObject.GetComponent<Image>().color = Color.red;
            progressBar.value = curLimitTime / limitTime;
            progressValue = progressBar.value;
            UpdateTimer();
        }
        

        return seeds;
    }
}
