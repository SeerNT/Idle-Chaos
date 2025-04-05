using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using NT_Utils.Conversion;

public class Growing : MonoBehaviour
{
    [Header("Objects")]
    public Slider progressBar;
    public Text titleText;
    public Text amountText;
    public Text progressText;
    public Text timer;
    public Button startBut;
    public GatheringManager manager;
    [Header("Settings")]
    public float curLimitTime = 10f;
    public float limitTime = 10f;
    public int giveAmount = 1;
    public int baseCost = 1;
    public int seedAmount = 2;
    public int progress = 0;
    public Herb.Quality quality;
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

        startBut.onClick.AddListener(StartGrowing);

        fillSpeed = 1 / limitTime;

        amountText.text = "Seeds:" + NumberConversion.AbbreviateNumber(seedAmount);
        progressText.text = progress.ToString() + "%";

        UpdateTimer();
        titleText.text = title;
    }

    void ToggleAutomation()
    {
        if (!automationOn)
        {
            if (!isGrowing)
            {
                StartGrowing();
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

        amountText.text = "Seeds:" + NumberConversion.AbbreviateNumber(seedAmount);
        progressText.text = progress.ToString() + "%";

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

        if (progressBar.value == 0)
        {

            if (isAuto && automationOn)
            {
                if (this.seedAmount > 0 || progress > 0)
                {
                    StartGrowing();
                }
            }
        }
        if (progressBar.value >= 1)
        {
            isGrowing = false;
            progressBar.value = 0;
            startBut.gameObject.GetComponent<Image>().color = Color.yellow;
            timer.text = limitTime.ToString() + "s";
            progress += 50;
            if(isAuto && automationOn)
            {
                StartGrowing();
            }
        }
    }

    void GiveProduct()
    {
        progress = 0;
        curLimitTime = 0;
        progressBar.value = 0;
        startBut.gameObject.GetComponent<Image>().color = Color.yellow;
        manager.GiveProduct(this);
        if (isAuto && automationOn)
        {
            StartGrowing();
        }
    }

    public void Reset()
    {
        progress = 0;
        curLimitTime = 0;

        progressBar.value = 0;
        startBut.gameObject.GetComponent<Image>().color = Color.yellow;
        automation.SetActive(false);

        progressBar.transform.Find("Background").GetComponent<Image>().color = new Color32(255, 255, 255, 255);
    }

    public void StartGrowing()
    {
        if (progress >= 100)
        {
            GiveProduct();
        }
        else
        {
            if (progress == 0)
            {
                if (this.seedAmount > 0)
                {
                    if (!isGrowing)
                    {
                        seedAmount--;
                        isGrowing = true;
                        startBut.gameObject.GetComponent<Image>().color = Color.red;
                    }
                }
            }
            else
            {
                if (!isGrowing)
                {
                    isGrowing = true;
                    startBut.gameObject.GetComponent<Image>().color = Color.red;
                }
            }
        }
    }

    public void SaveGrowing()
    {
        SaveSystem.SaveGrowing(this, this.name);
    }

    public void LoadGrowing()
    {
        GrowData data = SaveSystem.LoadGrowing(this.name);

        if (data != null)
        {
            loaded = true;

            progressValue = data.progressValue;
            progress = data.progress;

            isGrowing = data.isGrowing;
            isWaiting = data.isWaiting;

            seedAmount = data.seedAmount;
            baseCost = data.baseCost;
            curLimitTime = data.curLimitTime;
            limitTime = data.limitTime;

            automationOn = data.automationOn;
            isAuto = data.isAuto;

            amountText.text = "Seeds:" + NumberConversion.AbbreviateNumber(seedAmount);
            progressText.text = progress.ToString() + "%";

            if (isAuto)
            {
                automation.SetActive(true);
                if (automationOn)
                {
                    toggleOn.gameObject.SetActive(true);
                    toggleOff.gameObject.SetActive(false);
                    if (!isGrowing)
                    {
                        StartGrowing();
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
            }
        }
    }

    public double SimulateTime(float total, bool addProducts, bool isClear)
    {
        double products = 0;
        float left = total;
        double increaseAmount = Math.Floor((total + curLimitTime) / limitTime);
        
        if(progress != 0 && isClear)
        {
            curLimitTime = curLimitTime + total;

            if (addProducts)
            {
                manager.GiveProduct(this, giveAmount);
            }
            products += giveAmount;
            progress = 0;
            isGrowing = true;
            startBut.gameObject.GetComponent<Image>().color = Color.red;
            progressBar.value = 0;
            progressValue = progressBar.value;
            UpdateTimer();
        }
        else if(progress == 0)
        {
            increaseAmount = Math.Floor((total + curLimitTime) / limitTime) / 2;
            if (curLimitTime < left)
            {
                float leftSeconds = (float)((total / (limitTime * 2) - increaseAmount) * limitTime);
                curLimitTime = curLimitTime - leftSeconds;

                if (!isAuto || (isAuto && !automationOn))
                {
                    isGrowing = false;

                    if (addProducts)
                    {
                        progressBar.value = 0;
                        startBut.gameObject.GetComponent<Image>().color = Color.yellow;
                        timer.text = limitTime.ToString() + "s";
                        manager.GiveProduct(this, giveAmount * increaseAmount);
                    }
                    products += giveAmount * increaseAmount;
                    seedAmount -= (int)Math.Floor(giveAmount * increaseAmount) - 1;
                }
                else
                {
                    if (addProducts)
                    {
                        manager.GiveProduct(this, giveAmount * increaseAmount);
                    }
                    products += giveAmount * increaseAmount;
                    seedAmount -= (int)Math.Floor(giveAmount * increaseAmount) - 1;
                    isGrowing = true;
                    startBut.gameObject.GetComponent<Image>().color = Color.red;
                    progressBar.value = (limitTime - curLimitTime) / limitTime;
                    progressValue = progressBar.value;
                    UpdateTimer();
                }
            }
            else if (curLimitTime > left)
            {
                curLimitTime = curLimitTime - left;
                isGrowing = true;
                startBut.gameObject.GetComponent<Image>().color = Color.red;
                progressBar.value = (limitTime - curLimitTime) / limitTime;
                progressValue = progressBar.value;
                UpdateTimer();
            }
        }
        return Math.Floor(products);
    }
}
