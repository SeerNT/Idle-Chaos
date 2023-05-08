using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using NT_Utils.Conversion;
public class Train : MonoBehaviour
{
    [Header("Objects")]
    public Slider progressBar;
    public Text titleText;
    public Text costText;
    public Text timer;
    public Text lvlText;
    public Button startBut;
    public Button upgradeBut;
    public Image currencyImg;
    [Header("Settings")]
    public float curLimitTime = 10f;
    public float limitTime = 10f;
    public float baseLimitTime = 10f;
    public int magicIncrease = 1;
    public int lvl = 1;
    public int maxLvl = 22;
    public int baseCost = 5;
    public string title;
    [Header("Changes")]
    public bool isTraining = false;
    public int bought = 0;
    public float multiplier = 1.45f;
    public double cost = 5;
    public bool isWaiting = false;

    private float fillSpeed;

    public float progressValue;
    public bool isSetupPB;

    public int num = 0;

    [Header("Automation")]
    public bool isAuto = false;
    public bool automationOn = true;
    public GameObject automation;
    public GameObject toggleOn;
    public GameObject toggleOff;
    public Button toggleBut;

    private GameObject upgradeImage;

    public bool loaded = false;

    void Start()
    {
        baseLimitTime = limitTime;
        if (isAuto && automationOn)
        {
            toggleOn.gameObject.SetActive(true);
            toggleOff.gameObject.SetActive(false);
            //limitTime = (int)Math.Ceiling(limitTime * 1.2);
        }else if(isAuto && !automationOn)
        {
            toggleOn.gameObject.SetActive(false);
            toggleOff.gameObject.SetActive(true);
        }

        if (loaded)
        {
            if(bought == 0)
            {
                cost = baseCost;
            }
        }
        else
        {
            cost = baseCost;
        }
        
        curLimitTime = limitTime;
        isSetupPB = false;

        startBut.onClick.AddListener(StartTraining);
        upgradeBut.onClick.AddListener(MakeUpgrade);
        
        fillSpeed = 1/limitTime;

        costText.text = "Cost:" + NumberConversion.AbbreviateNumber(cost);

        UpdateTimer();
        UpdateCostImg();
        lvlText.text = "Lvl." + lvl.ToString();
        titleText.text = title;

        toggleBut.onClick.AddListener(ToggleAutomation);

        upgradeImage = transform.Find("Upgrade").Find("Image").gameObject;
    }

    void ToggleAutomation()
    {
        if (!automationOn)
        {
            if (!isTraining)
            {
                StartTraining();
            }
            toggleOn.gameObject.SetActive(true);
            toggleOff.gameObject.SetActive(false);
            automationOn = true;
            float increaseCoef = (100 + (21-TalantManager.automationSpeed)) / 100;
            limitTime = (float)Math.Round(limitTime * increaseCoef, 1);
        }
        else
        {
            toggleOn.gameObject.SetActive(false);
            toggleOff.gameObject.SetActive(true);
            automationOn = false;
            float increaseCoef = (100 + (21 - TalantManager.automationSpeed)) / 100;
            limitTime = (float)Math.Round(limitTime / increaseCoef, 1);
        }
    }
    // какого хуя увеличваем сука от автоматизации
    void MakeUpgrade()
    {
        int num = int.Parse(this.name.Replace("Train", ""));
        num = num - 1;
        if (num > 0)
        {
            Train previousTrain = transform.parent.Find("Train" + num.ToString()).GetComponent<Train>();

            if (previousTrain.lvl > 0)
            {
                if (GameManager.xp >= cost)
                {
                    if (isTraining)
                    {
                        GameManager.xp = (double)(GameManager.xp - cost);
                        GameManager.UpdateText();
                        float increaseCoef = (100 + (21 - TalantManager.automationSpeed)) / 100;
                        limitTime = (float)Math.Round(limitTime / increaseCoef, 1);
                        curLimitTime = limitTime;
                        bought++;
                        cost = baseCost * Math.Pow(multiplier, bought);
                        cost = Math.Ceiling(cost);
                        costText.text = "Cost:" + NumberConversion.AbbreviateNumber(cost);
                        lvl++;
                        UpdateLvl();

                        isWaiting = true;
                    }
                    else
                    {
                        GameManager.xp = (double)(GameManager.xp - cost);
                        GameManager.UpdateText();

                        bought++;
                        cost = baseCost * Math.Pow(multiplier, bought);
                        cost = Math.Ceiling(cost);
                        costText.text = "Cost:" + NumberConversion.AbbreviateNumber(cost);
                        if (lvl != 0)
                        {
                            float increaseCoef = (100 + (21 - TalantManager.automationSpeed)) / 100;

                            limitTime = (float)Math.Round(limitTime / increaseCoef, 1);
                            curLimitTime = limitTime;
                            UpdateTimer();
                        }
                        lvl++;
                        UpdateLvl();
                    }
                }
            }
        }
        else
        {
            if (GameManager.xp >= cost)
            {
                if (isTraining)
                {
                    GameManager.xp = (double)(GameManager.xp - cost);
                    GameManager.UpdateText();
                    float increaseCoef = (100 + (21 - TalantManager.automationSpeed)) / 100;
                    limitTime = (float)Math.Round(limitTime / increaseCoef, 1);
                    curLimitTime = limitTime;
                    bought++;
                    cost = baseCost * Math.Pow(multiplier, bought);
                    cost = Math.Ceiling(cost);
                    costText.text = "Cost:" + NumberConversion.AbbreviateNumber(cost);
                    lvl++;
                    UpdateLvl();

                    isWaiting = true;
                }
                else
                {
                    GameManager.xp = (double)(GameManager.xp - cost);
                    GameManager.UpdateText();
                    bought++;
                    cost = baseCost * Math.Pow(multiplier, bought);
                    cost = Math.Ceiling(cost);
                    costText.text = "Cost:" + NumberConversion.AbbreviateNumber(cost);
                    if (lvl != 0)
                    {
                        float increaseCoef = (100 + (21 - TalantManager.automationSpeed)) / 100;
                        limitTime = (float)Math.Round(limitTime / increaseCoef, 1);
                        curLimitTime = limitTime;
                        UpdateTimer();
                    }
                    lvl++;
                    UpdateLvl();
                }
            }
        }

        if (lvl >= maxLvl)
        {
            upgradeBut.gameObject.SetActive(false);
            costText.gameObject.SetActive(false);
            currencyImg.gameObject.SetActive(false);
            upgradeImage.SetActive(false);
            lvlText.color = Color.red;
            limitTime = 0.1f;
            curLimitTime = limitTime;
            UpdateTimer();
        }
    }

    public void UpdateTimer() {

        Debug.Log(curLimitTime);
        timer.text = TimeConversion.AbbreviateTime(curLimitTime);
        if (curLimitTime == 0.001f)
            timer.text = "0";
    }

    public void UpdateCostImg()
    {
        switch (costText.text.Length)
        {
            case 6:
                currencyImg.transform.localPosition = new Vector3(-395f, currencyImg.transform.localPosition.y, currencyImg.transform.localPosition.z);
                break; 
            case 7:
                if (!costText.text.Contains("M") && !costText.text.Contains("K"))
                {
                    currencyImg.transform.localPosition = new Vector3(-381f, currencyImg.transform.localPosition.y, currencyImg.transform.localPosition.z);
                }
                else
                {
                    currencyImg.transform.localPosition = new Vector3(-375f, currencyImg.transform.localPosition.y, currencyImg.transform.localPosition.z);
                }
                
                break;
            case 8:
                if (!costText.text.Contains("M") && !costText.text.Contains("K"))
                {
                    currencyImg.transform.localPosition = new Vector3(-371.5f, currencyImg.transform.localPosition.y, currencyImg.transform.localPosition.z);
                }
                else
                {
                    currencyImg.transform.localPosition = new Vector3(-368.6f, currencyImg.transform.localPosition.y, currencyImg.transform.localPosition.z);
                }
                break;
            default:
                currencyImg.gameObject.SetActive(false);
                break;
        }
    }

    public void UpdateLvl()
    {
        lvlText.text = "Lvl." + lvl.ToString();
    }

    void Update() {
        if (lvl > 0 && AutoSave.Loaded)
        {
            if(lvl >= maxLvl)
            {
                upgradeBut.gameObject.SetActive(false);
                costText.gameObject.SetActive(false);
                currencyImg.gameObject.SetActive(false);
                upgradeImage.SetActive(false);
                lvlText.color = Color.red;
            }

            if (isWaiting)
            {
                if (!isTraining)
                {
                    isWaiting = false;
                    curLimitTime = limitTime;
                    UpdateTimer();
                }
            }

            fillSpeed = 1 / limitTime;

            if (isTraining && progressBar.value < 1)
            {
                if (!SaveSystem.trainLoaded || isSetupPB)
                {
                    progressBar.value += fillSpeed * Time.deltaTime;
                    progressValue = progressBar.value;
                }
                else
                {
                    if (isTraining)
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
                isTraining = false;
                IncreaseMagic();
            }
            UpdateCostImg();
        }
    }

    void IncreaseMagic() {
        progressBar.value = 0;
        startBut.gameObject.GetComponent<Image>().color = Color.yellow;
        timer.text = limitTime.ToString() + "s";

        GameManager.magic += magicIncrease * (1 + GameManager.qi);
        Reincarnation.totalEarnMagic += magicIncrease;
        GameManager.UpdateText();

        if (isAuto && automationOn)
        {
            float increaseCoef = (100 + (21 - TalantManager.automationSpeed)) / 100;
            //limitTime = (float)Math.Round(limitTime * increaseCoef, 1);
            StartTraining();
        }
    }

    public void StartTraining() {
        if(this.lvl > 0)
        {
            if (!isTraining)
            {
                isTraining = true;
                startBut.gameObject.GetComponent<Image>().color = Color.red;
            }
        }
    }

    public double SimulateTime(float total, bool addMagic)
    {
        double magic = 0;
        float left = total;
        
        if (curLimitTime < left)
        {
            double increaseAmount = Math.Floor(left / limitTime);
            float leftSeconds = (float)Math.Round(left - (increaseAmount * limitTime), 2); 
            curLimitTime = curLimitTime + leftSeconds;
            if (!isAuto || (isAuto && !automationOn))
            {
                isTraining = false;

                if (addMagic)
                {
                    progressBar.value = 0;
                    startBut.gameObject.GetComponent<Image>().color = Color.yellow;
                    timer.text = limitTime.ToString() + "s";
                    GameManager.magic += magicIncrease * (1 + GameManager.qi); ;
                    Reincarnation.totalEarnMagic += magicIncrease * (1 + GameManager.qi); ;
                    GameManager.UpdateText();
                }
                magic += magicIncrease * (1 + GameManager.qi); ;
            }
            else
            {
                double i = increaseAmount;
                while (i != 0)
                {
                    i--;
                    if (addMagic)
                    {
                        GameManager.magic += magicIncrease * (1 + GameManager.qi); ;
                        Reincarnation.totalEarnMagic += magicIncrease * (1 + GameManager.qi); ;
                        GameManager.UpdateText();
                    }
                    magic += magicIncrease * (1 + GameManager.qi); ;
                }

                isTraining = true;
                startBut.gameObject.GetComponent<Image>().color = Color.red;
                progressBar.value = (limitTime - curLimitTime) / limitTime;
                progressValue = progressBar.value;
                UpdateTimer();
            }
        }
        else if (curLimitTime != limitTime)
        {
            curLimitTime = curLimitTime - left;
            isTraining = true;
            startBut.gameObject.GetComponent<Image>().color = Color.red;
            progressBar.value = (limitTime-curLimitTime) / limitTime;
            progressValue = progressBar.value;
            UpdateTimer();
        }
        return magic;
    }
    
    public void Reset()
    {
        progressBar.value = 0;
        startBut.gameObject.GetComponent<Image>().color = Color.yellow;
        timer.text = limitTime.ToString() + "s";
        costText.text = "Cost:" + NumberConversion.AbbreviateNumber(cost);
        automation.SetActive(false);
        UpdateCostImg();
    }

    public void SaveTraining()
    {
        SaveSystem.SaveTraining(this, this.name);
    }

    public void LoadTraining()
    {
        TrainData data = SaveSystem.LoadTraining(this.name);

        if (data != null)
        {
            loaded = true;

            progressValue = data.progressValue;
            
            isTraining = data.isTraining;
            isWaiting = data.isWaiting;

            lvl = data.lvl;
            bought = data.bought;
            cost = data.cost;
            baseCost = data.baseCost;
            curLimitTime = data.curLimitTime;
            limitTime = data.limitTime;
            isAuto = data.isAuto;
            automationOn = data.automationOn;

            if (isAuto)
            {
                automation.SetActive(true);
                if (automationOn)
                {
                    toggleOn.gameObject.SetActive(true);
                    toggleOff.gameObject.SetActive(false);
                    if (!isTraining)
                    {
                        StartTraining();
                    }
                }
                else
                {
                    toggleOn.gameObject.SetActive(false);
                    toggleOff.gameObject.SetActive(true);
                }
            }
            
            costText.text = "Cost:" + cost.ToString();
            lvlText.text = "Lvl." + lvl.ToString();

            if (this.isTraining)
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
}
