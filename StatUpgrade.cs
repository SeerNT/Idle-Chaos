using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using NT_Utils.Conversion;

public class StatUpgrade : MonoBehaviour
{
    public enum Stats
    {
        PP,
        MP,
        ARM,
        MRM,
        PRC,
        CRT,
        HP,
        AS,
        PD,
        LCK
    }

    private Text locksText;
    private Text lvlText;
    private Text costText;

    public static int locks = 0;
    public static int locksLimit = 1;

    public int[] upgradeCosts = new int[10];
    public float[] statIncreases = new float[10];
    public float[] statTimeLimits = new float[10];
    public int upgradeLvl = 1;

    public int gainLvl = 0;
    public int timeLvl = 0;
    public int costLvl = 0;

    public Slider progressBar;

    private Text timer;

    private float fillSpeed;
    public bool isUpgrading = false;
    public bool isLocked = false;
    public bool readyForUpdate = false;

    private Player player;

    public Stats stat;

    private Button startBut;

    public float progressValue;

    public bool isSetupPB;

    public double currentTime;

    void Start()
    {
        isSetupPB = false;

        costText = this.transform.Find("Cost").GetComponent<Text>();
        locksText = GameObject.Find("Locks").GetComponent<Text>();
        player = GameObject.Find("Battle").GetComponent<Battle>().player;
        progressBar = this.transform.Find("Progress").GetComponent<Slider>();
        timer = this.transform.Find("Time").GetComponent<Text>();
        lvlText = this.transform.Find("Lvl").GetComponent<Text>();
        timer.text = TimeConversion.AbbreviateTime(statTimeLimits[timeLvl]);
        startBut = this.transform.Find("StartBut").GetComponent<Button>();
        startBut.onClick.AddListener(CancelUpgrade);
        fillSpeed = 1 / statTimeLimits[timeLvl];
    }

    void Update()
    {
        locksText.text = "Locks\n" + locks.ToString() + "/" + locksLimit.ToString();
        costText.text = "Cost:" + NumberConversion.AbbreviateNumber(upgradeCosts[costLvl]); 

        lvlText.text = "Lvl." + upgradeLvl.ToString();

        if (isUpgrading && progressBar.value < 1)
        {
            startBut.GetComponent<Image>().color = Color.green;
            if (!SaveSystem.statUpgLoaded || isSetupPB)
            {
                progressBar.value += fillSpeed * Time.deltaTime;
                progressValue = progressBar.value;
            }
            else
            {
                if (isUpgrading)
                {
                    progressBar.value = progressValue;
                }
                
                isSetupPB = true;
            }
            
            currentTime = Math.Round(statTimeLimits[timeLvl] - (progressBar.value * statTimeLimits[timeLvl]), 1);
            timer.text = TimeConversion.AbbreviateTime(currentTime);
        }
        if (progressBar.value >= 1)
        {
            readyForUpdate = true;
            IncreaseStat();
        }
    }

    public void UpdateTime()
    {
        currentTime = Math.Round(statTimeLimits[timeLvl] - (progressBar.value * statTimeLimits[timeLvl]), 1);
        timer.text = TimeConversion.AbbreviateTime(currentTime);
        fillSpeed = 1 / statTimeLimits[timeLvl];
    }

    void IncreaseStat()
    {
        progressBar.value = 0;
        progressValue = 0;
        timer.text = TimeConversion.AbbreviateTime(statTimeLimits[timeLvl]);
        GameObject.Find("Battle").GetComponent<Battle>().player.IncreaseStat(stat, statIncreases[gainLvl]);
        StartUpgrade();
    }

    void CancelUpgrade()
    {
        if (isLocked)
        {
            if (isUpgrading)
            {
                GameManager.magic += upgradeCosts[costLvl];
                GameManager.UpdateText();
            }
            
            isUpgrading = false;
            progressBar.value = 0;
            progressValue = 0;
            timer.text = TimeConversion.AbbreviateTime(statTimeLimits[timeLvl]);
            startBut.GetComponent<Image>().color = Color.yellow;
            StatUpgrade.locks -= 1;
            isLocked = false;
            CancelInvoke();
        }
        else
        {
            if (StatUpgrade.locks < StatUpgrade.locksLimit)
            {
                if (GameManager.magic >= upgradeCosts[costLvl])
                {
                    GameManager.magic -= upgradeCosts[costLvl];
                    GameManager.UpdateText();
                    isUpgrading = true;
                    readyForUpdate = false;
                    startBut.GetComponent<Image>().color = Color.green;
                    if (!isLocked)
                    {
                        StatUpgrade.locks += 1;
                        isLocked = true;
                    }
                }
            }
        }
    }

    void StartUpgrade()
    {
        if (!isUpgrading || readyForUpdate)
        {
            if(StatUpgrade.locks < StatUpgrade.locksLimit || isLocked)
            {
                if (GameManager.magic >= upgradeCosts[costLvl])
                {
                    GameManager.magic -= upgradeCosts[costLvl];
                    GameManager.UpdateText();
                    isUpgrading = true;
                    readyForUpdate = false;
                    startBut.GetComponent<Image>().color = Color.green;

                    if (!isLocked)
                    {
                        StatUpgrade.locks += 1;
                        isLocked = true;
                    }
                }
                else if(isLocked)
                {
                    isUpgrading = false;
                    readyForUpdate = true;
                    Invoke("StartUpgrade", 1);
                }
            }
        }
        else if (isUpgrading == true || isLocked)
        {
            GameManager.magic += upgradeCosts[costLvl];
            GameManager.UpdateText();
            isUpgrading = false;
            progressBar.value = 0;
            progressValue = 0;
            timer.text = TimeConversion.AbbreviateTime(statTimeLimits[timeLvl]);
            startBut.GetComponent<Image>().color = Color.yellow;
            StatUpgrade.locks -= 1;
            isLocked = false;
        }
    }

    public void StartUpgFrom(double timeLeft)
    {
        GameManager.UpdateText();
        
        startBut.GetComponent<Image>().color = Color.green;
       
        if(timeLeft != 0)
        {
            timer.text = TimeConversion.AbbreviateTime(timeLeft);
            progressBar.value = (float)((statTimeLimits[timeLvl] - timeLeft) / 10);
            isUpgrading = true;
            readyForUpdate = false;
        }
        else
        {
            progressBar.value = 0;
            progressValue = 0;
            timer.text = TimeConversion.AbbreviateTime(statTimeLimits[timeLvl]);
            isUpgrading = false;
            readyForUpdate = true;
            StartUpgrade();
        }
    }

    public double SimulateTime(float total)
    {
        double increaseAmount = Math.Floor((total + currentTime) / statTimeLimits[timeLvl]);

        if(increaseAmount == 0)
        {
            StartUpgFrom(total);
        }
        else
        {
            if (GameManager.magic != 0)
            {
                GameManager.magic -= upgradeCosts[costLvl] * increaseAmount;
                
                double timeLeft = (((total + currentTime) / statTimeLimits[timeLvl]) - increaseAmount) * statTimeLimits[timeLvl];
                if (GameManager.magic != 0)
                {
                    if (timeLeft > 0)
                    {
                        StartUpgFrom(timeLeft);
                    }
                }
                else
                {
                    StartUpgFrom(0);
                }
            }
            else
            {
                progressBar.value = 0;
                progressValue = 0;
                timer.text = TimeConversion.AbbreviateTime(statTimeLimits[timeLvl]);

                isUpgrading = false;
                Invoke("StartUpgrade", 1);
            }
        }
        GameObject.Find("Battle").GetComponent<Battle>().player.IncreaseStat(stat, (int)(increaseAmount * statIncreases[gainLvl]));
        double permanentUpgCoef = 1;
        if(stat == Stats.PP)
        {
            permanentUpgCoef = (100 + Reincarnation.permanentsUpgrades[0] + Reincarnation.permanentsUpgrades[6]) / 100;
        }
        if (stat == Stats.MP)
        {
            permanentUpgCoef = (100 + Reincarnation.permanentsUpgrades[2]) / 100;
        }
        if (stat == Stats.ARM)
        {
            permanentUpgCoef = (100 + Reincarnation.permanentsUpgrades[3]) / 100;
        }
        if (stat == Stats.MRM)
        {
            permanentUpgCoef = (100 + Reincarnation.permanentsUpgrades[5]) / 100;
        }
        if (stat == Stats.PRC)
        {
            permanentUpgCoef = (100 + Reincarnation.permanentsUpgrades[4]) / 100;
        }
        if (stat == Stats.HP)
        {
            permanentUpgCoef = (100 + Reincarnation.permanentsUpgrades[1] + Reincarnation.permanentsUpgrades[7]) / 100;
        }
        return increaseAmount * statIncreases[gainLvl] * permanentUpgCoef;
    }

    public void SaveStatUpgrade()
    {
        SaveSystem.SaveStatUpgrade(this, stat.ToString());
    }

    public void LoadStatUpgrade()
    {
        StatUpgradeData data = SaveSystem.LoadStatUpgrade(stat.ToString());

        if (data != null)
        {
            this.progressValue = data.progressValue;

            this.isUpgrading = data.isUpgrading;
            this.isLocked = data.isLocked;
            this.readyForUpdate = data.readyForUpdate;
            this.gainLvl = data.gainLvl;
            this.timeLvl = data.timeLvl;
            this.costLvl = data.costLvl;
            this.upgradeLvl = data.upgradeLvl;

            StatUpgrade.locks = data.locks;
            StatUpgrade.locksLimit = data.locksLimit;
        }
    }

    public void ResetStatUpgrade()
    {
        this.progressValue = 0;

        this.isUpgrading = false;
        this.isLocked = false;
        this.readyForUpdate = false;

        this.gainLvl = 0;
        this.timeLvl = 0;
        this.costLvl = 0;
        this.upgradeLvl = 1;

        StatUpgrade.locks = 0;
        StatUpgrade.locksLimit = 1;

        isUpgrading = false;
        progressBar.value = 0;
        progressValue = 0;
        timer.text = TimeConversion.AbbreviateTime(statTimeLimits[timeLvl]);
        startBut.GetComponent<Image>().color = Color.yellow;
        SaveStatUpgrade();
    }
}
