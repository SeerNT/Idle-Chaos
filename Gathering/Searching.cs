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
    [Header("Automation")]
    public bool isAuto = true;
    public bool automationOn = true;
    public GameObject automation;
    public GameObject toggleOn;
    public GameObject toggleOff;
    public Button toggleBut;

    private float fillSpeed;

    public float progressValue;
    public bool isSetupPB;

    public bool loaded = false;

    void Start()
    {
        if (isAuto && automationOn)
        {
            toggleOn.gameObject.SetActive(true);
            toggleOff.gameObject.SetActive(false);
        }
        else if (isAuto && !automationOn)
        {
            toggleOn.gameObject.SetActive(false);
            toggleOff.gameObject.SetActive(true);
        }
        toggleBut.onClick.AddListener(ToggleAutomation);

        curLimitTime = limitTime;
        isSetupPB = false;

        startBut.onClick.AddListener(StartSearching);

        fillSpeed = 1 / limitTime;

        lvlText.text = "Lvl:" + lvl.ToString();
        chanceText.text = seedChance.ToString() + "%";

        UpdateTimer();
        titleText.text = title;
    }

    void ToggleAutomation()
    {
        if (!automationOn)
        {
            if (!isGrowing)
            {
                StartSearching();
            }
            toggleOn.gameObject.SetActive(true);
            toggleOff.gameObject.SetActive(false);
            automationOn = true;
            float increaseCoef = (100 + (21 - TalantManager.herbAutomationSpeed)) / 100;
            if (curLimitTime == 0.001f || curLimitTime == 0)
                progressBar.transform.Find("Background").GetComponent<Image>().color = new Color32(0, 255, 13, 255);
            limitTime = (float)Math.Round(limitTime * increaseCoef, 2);
        }
        else
        {
            toggleOn.gameObject.SetActive(false);
            toggleOff.gameObject.SetActive(true);
            automationOn = false;
            float increaseCoef = (100 + (21 - TalantManager.herbAutomationSpeed)) / 100;
            progressBar.transform.Find("Background").GetComponent<Image>().color = new Color32(255, 255, 255, 255);
            limitTime = (float)Math.Round(limitTime / increaseCoef, 2);
        }
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

        if (isAuto)
        {
            automation.SetActive(true);
            toggleOn.gameObject.SetActive(automationOn);
            toggleOff.gameObject.SetActive(!automationOn);
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
        if (isAuto && automationOn)
        {
            StartSearching();
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
        automation.SetActive(false);
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

            isAuto = data.isAuto;
            automationOn = data.automationOn;

            lvlText.text = "Lvl:" + lvl.ToString();
            chanceText.text = seedChance.ToString() + "%";

            if (isAuto)
            {
                automation.SetActive(true);
                if (automationOn)
                {
                    toggleOn.gameObject.SetActive(true);
                    toggleOff.gameObject.SetActive(false);
                    if (!isGrowing)
                    {
                        StartSearching();
                    }
                }
                else
                {
                    toggleOn.gameObject.SetActive(false);
                    toggleOff.gameObject.SetActive(true);
                }
            }

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

    public double SimulateTime(float total, bool addSeeds)
    {
        double seeds = 0;
        float left = total;
        double increaseAmount = Math.Floor((total + curLimitTime) / limitTime);

        if (curLimitTime < left)
        {
            float leftSeconds = (float)((((total + curLimitTime) / limitTime) - increaseAmount) * limitTime);
            curLimitTime = curLimitTime + leftSeconds;
            if (!isAuto || (isAuto && !automationOn))
            {
                isGrowing = false;

                if (addSeeds)
                {
                    progressBar.value = 0;
                    startBut.gameObject.GetComponent<Image>().color = Color.yellow;
                    timer.text = limitTime.ToString() + "s";
                }
                seeds += giveAmount * increaseAmount;
                for(int i = 0; i < increaseAmount; i++)
                {
                    System.Random generator = new System.Random(DateTime.Now.Millisecond);
                    if (generator.Next(100) < seedChance)
                    {
                        manager.GiveProductSeed(this);
                    }
                }
            }
            else
            {
                seeds += giveAmount * increaseAmount;
                for (int i = 0; i < increaseAmount; i++)
                {
                    System.Random generator = new System.Random(DateTime.Now.Millisecond);
                    if (generator.Next(100) < seedChance)
                    {
                        manager.GiveProductSeed(this);
                    }
                }
                isGrowing = true;
                startBut.gameObject.GetComponent<Image>().color = Color.red;
                progressBar.value = (limitTime - curLimitTime) / limitTime;
                progressValue = progressBar.value;
                UpdateTimer();
            }
        }
        else if (curLimitTime != limitTime)
        {
            curLimitTime = curLimitTime - left;
            isGrowing = true;
            startBut.gameObject.GetComponent<Image>().color = Color.red;
            progressBar.value = (limitTime - curLimitTime) / limitTime;
            progressValue = progressBar.value;
            UpdateTimer();
        }

        return seeds;
    }
}
