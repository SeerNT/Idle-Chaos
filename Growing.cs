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

    private float fillSpeed;

    public float progressValue;
    public bool isSetupPB;

    public bool loaded = false;

    void Start()
    {
        curLimitTime = limitTime;
        isSetupPB = false;

        startBut.onClick.AddListener(StartGrowing);

        fillSpeed = 1 / limitTime;

        amountText.text = "Seeds:" + NumberConversion.AbbreviateNumber(seedAmount);
        progressText.text = progress.ToString() + "%";

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
        if (progressBar.value >= 1)
        {
            isGrowing = false;
            progressBar.value = 0;
            startBut.gameObject.GetComponent<Image>().color = Color.yellow;
            timer.text = limitTime.ToString() + "s";
            progress += 50;
            
        }
    }

    void GiveProduct()
    {
        progress = 0;
        curLimitTime = 0;
        progressBar.value = 0;
        startBut.gameObject.GetComponent<Image>().color = Color.yellow;
        manager.GiveProduct(this);
    }

    public void Reset()
    {
        progress = 0;
        curLimitTime = 0;
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

            amountText.text = "Seeds:" + NumberConversion.AbbreviateNumber(seedAmount);
            progressText.text = progress.ToString() + "%";

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

    public int SimulateTime(float total, bool addProducts)
    {
        int products = 0;
        float left = total;

        if ((limitTime - (limitTime - curLimitTime)) < left)
        {
            isGrowing = false;
            if (addProducts)
                GiveProduct();
            products += giveAmount;
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

        return products;
    }
}
