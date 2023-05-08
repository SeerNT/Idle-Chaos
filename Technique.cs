using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using NT_Utils.Conversion;
public class Technique : MonoBehaviour
{
    public enum Type
    {
        Spam,
        Hold,
        Passive
    }

    [Header("Objects")]
    public GameObject buyObject;
    public GameObject timeObject;
    public Slider progressBar;
    public Text timer;
    public Text titleText;
    public Text lvlText;
    public Text costText;
    public Button startBut;
    public Button buyBut;
    public Image currencyImg;
    [Header("Settings")]
    public int buyCost = 35;
    public Type techType;
    public float trainStr = 0.1f;
    public int lvl = 0;
    public string title;

    public int gainLvl = 0;
    public int timeLvl = 0;

    public int[] xpIncreases = new int[4];
    public float[] cooldowns = new float[4];

    private float fillSpeed;
    private float unfillSpeed;
    public bool isTraining = false;
    public bool isCooldown = false;
    public float currentTime;

    public float progressValue;

    private bool isHolding = false;
    
    void Start()
    {
        if(lvl != 0)
        {
            startBut.gameObject.GetComponent<Image>().color = Color.green;
        }
        else
        {
            startBut.gameObject.GetComponent<Image>().color = Color.magenta;
        }
        

        titleText.text = title;
        if(this.isTraining || currentTime == cooldowns[timeLvl])
            timer.text = cooldowns[timeLvl].ToString() + "s";
        costText.text = "Cost:" + NumberConversion.AbbreviateNumber(buyCost);

        TextGenerator textGen = new TextGenerator();
        TextGenerationSettings generationSettings = costText.GetGenerationSettings(costText.rectTransform.rect.size);
        float width = textGen.GetPreferredWidth(costText.text, generationSettings);
        float height = textGen.GetPreferredHeight(costText.text, generationSettings);
        costText.GetComponent<RectTransform>().sizeDelta = new Vector2(width, height);

        if (techType != Type.Passive)
        {
            timer.gameObject.SetActive(false);
        }
        else
        {
            timer.gameObject.SetActive(true);
        }
        
        if(lvl != 0)
        {
            buyObject.SetActive(false);
            if (techType == Type.Passive && (this.isTraining || currentTime == cooldowns[timeLvl]))
            {
                timeObject.SetActive(true);
                timer.text = cooldowns[timeLvl].ToString() + "s";
            }
        }
        else
        {
            timeObject.SetActive(false);
        }

        startBut.onClick.AddListener(StartTraining);
        buyBut.onClick.AddListener(Buy);

        fillSpeed = 1 / cooldowns[timeLvl];
        unfillSpeed = 1 / cooldowns[timeLvl];
    }

    void Buy()
    {
        if(GameManager.xp >= buyCost)
        {
            GameManager.xp -= buyCost;
            GameManager.UpdateText();
            lvl += 1;
            buyObject.SetActive(false);
            if(techType == Type.Passive)
            {
                timeObject.SetActive(true);
                timer.text = TimeConversion.AbbreviateTime(cooldowns[timeLvl]);
            }
            startBut.gameObject.GetComponent<Image>().color = Color.green;
        }
    }

    void Update()
    {
        lvlText.text = "Lvl." + lvl.ToString();
        fillSpeed = 1 / cooldowns[timeLvl];
        unfillSpeed = 1 / cooldowns[timeLvl];
        UpdateCostText();
        UpdateCostImg();
        if (isTraining && progressBar.value < 1)
        {
            if(techType == Type.Spam)
            {
                progressBar.value -= trainStr * 2f * Time.deltaTime;
                if (progressBar.value < 0)
                {
                    progressBar.value = 0;
                }
                progressValue = progressBar.value;
            }
            else if(techType == Type.Hold)
            {
                if (isHolding)
                {
                    progressBar.value += trainStr * Time.deltaTime;
                    progressValue = progressBar.value;
                }
                else
                {
                    progressBar.value -= fillSpeed * Time.deltaTime;
                    if (progressBar.value < 0)
                    {
                        progressBar.value = 0;
                    }
                    progressValue = progressBar.value;
                }
            }
            else if(techType == Type.Passive)
            {
                progressBar.value += fillSpeed * Time.deltaTime;
                progressValue = progressBar.value;
                currentTime = (float)Math.Round((1 - progressBar.value) * cooldowns[timeLvl], 1);
                timer.text = TimeConversion.AbbreviateTime(currentTime);
            }
        }
        if (isCooldown) {
            if(techType != Type.Passive)
            {
                progressBar.value -= unfillSpeed * Time.deltaTime;
                currentTime = (float)Math.Round(cooldowns[timeLvl] - ((1 - progressBar.value) * cooldowns[timeLvl]), 1);
                timer.text = TimeConversion.AbbreviateTime(currentTime);
                if (progressBar.value <= 0)
                {
                    progressBar.value = 0;
                    isCooldown = false;
                    startBut.gameObject.GetComponent<Image>().color = Color.green;
                    timer.gameObject.SetActive(false);
                }
                progressValue = progressBar.value;
            }
        }
        if (progressBar.value >= 1 && isTraining)
        {
            isTraining = false;
            IncreaseXp();
        }
    }

    public void UpdateCostImg()
    {
        switch (costText.text.Length)
        {
            case 6:
                currencyImg.transform.localPosition = new Vector3(43f, 2.4f, currencyImg.transform.localPosition.z);
                break;
            case 7:
                if (!costText.text.Contains("M") && !costText.text.Contains("K"))
                {
                    currencyImg.transform.localPosition = new Vector3(65f, 2.4f, currencyImg.transform.localPosition.z);
                }
                else
                {
                    currencyImg.transform.localPosition = new Vector3(67f, 2.4f, currencyImg.transform.localPosition.z);
                }

                break;
            case 8:
                if (!costText.text.Contains("M") && !costText.text.Contains("K"))
                {
                    currencyImg.transform.localPosition = new Vector3(72f, 2.4f, currencyImg.transform.localPosition.z);
                }
                else
                {
                    currencyImg.transform.localPosition = new Vector3(74f, 2.4f, currencyImg.transform.localPosition.z);
                }
                break;
            case 9:
                currencyImg.transform.localPosition = new Vector3(83.2f, 2.4f, currencyImg.transform.localPosition.z);
                break;
            default:
                currencyImg.gameObject.SetActive(false);
                break;
        }
    }

    void UpdateCostText()
    {
        costText.text = "Cost:" + NumberConversion.AbbreviateNumber(buyCost);
    }

    void IncreaseXp()
    {
        if(techType != Type.Passive)
        {
            startBut.gameObject.GetComponent<Image>().color = Color.red;
            TimeConversion.AbbreviateTime(currentTime);
            timer.gameObject.SetActive(true);
            isCooldown = true;
        }
        else
        {
            progressBar.value = 0;
            currentTime = 0;
            timer.text = TimeConversion.AbbreviateTime(currentTime);
            isCooldown = false;
            progressValue = progressBar.value;
            StartTraining();
        }
        GameManager.xp += xpIncreases[gainLvl];
        Reincarnation.totalEarnXp += xpIncreases[gainLvl];
        GameManager.UpdateText();
    }

    void StartTraining()
    {
        if(lvl != 0)
        {
            if (techType == Type.Spam)
            {
                if (!isCooldown)
                {
                    if (!isTraining)
                    {
                        isTraining = true;
                        startBut.gameObject.GetComponent<Image>().color = Color.green;
                    }
                    if (isTraining)
                    {
                        progressBar.value += trainStr;
                        progressValue = progressBar.value;
                    }
                }
            }
            else if (techType == Type.Passive)
            {
                if (!isCooldown)
                {
                    if (!isTraining)
                    {
                        isTraining = true;
                        startBut.gameObject.GetComponent<Image>().color = Color.yellow;
                    }
                    else
                    {
                        isTraining = false;
                        startBut.gameObject.GetComponent<Image>().color = Color.green;
                    }
                }
            }
        }
        
    }

    public void HoldTraining()
    {
        if(lvl != 0)
        {
            if (techType == Type.Hold)
            {
                if (!isCooldown)
                {
                    if (!isTraining)
                    {
                        isTraining = true;
                        startBut.gameObject.GetComponent<Image>().color = Color.green;
                    }
                    if (isTraining)
                    {
                        startBut.gameObject.GetComponent<Image>().color = Color.yellow;
                        isHolding = true;
                    }
                }
            }
        }
    }

    public void ReleaseTraining()
    {
        if(lvl != 0)
        {
            if (techType == Type.Hold)
            {
                if (!isCooldown)
                {
                    startBut.gameObject.GetComponent<Image>().color = Color.green;
                    isHolding = false;
                }
            }
        } 
    }

    public void Reset()
    {
        progressBar.value = 0;
        if (techType != Type.Passive)
        {
            currentTime = (float)Math.Round(cooldowns[timeLvl], 1);
            timer.text = TimeConversion.AbbreviateTime(currentTime);
            isCooldown = false;
            timer.gameObject.SetActive(false);
            progressValue = progressBar.value;
        }
        if(lvl == 0)
        {
            buyObject.SetActive(true);
            startBut.gameObject.GetComponent<Image>().color = Color.magenta;
            timeObject.gameObject.SetActive(false);
        }

        TextGenerator textGen = new TextGenerator();
        TextGenerationSettings generationSettings = costText.GetGenerationSettings(costText.rectTransform.rect.size);
        float width = textGen.GetPreferredWidth(costText.text, generationSettings);
        float height = textGen.GetPreferredHeight(costText.text, generationSettings);
        costText.GetComponent<RectTransform>().sizeDelta = new Vector2(width, height);

        startBut.gameObject.GetComponent<Image>().color = Color.green;
        costText.text = "Cost:" + NumberConversion.AbbreviateNumber(buyCost);
        UpdateCostImg();
    }

    public void SaveTech()
    {
        SaveSystem.SaveTechnique(this, this.name);
    }

    public void LoadTech()
    {
        TechniqueData data = SaveSystem.LoadTechnique(this.name);

        if (data != null)
        {
            this.progressValue = data.progressValue;

            this.isTraining = data.isTraining;
            this.isCooldown = data.isCooldown;

            this.lvl = data.lvl;
            this.gainLvl = data.gainLvl;
            this.timeLvl = data.timeLvl;
            this.currentTime = data.currentTime;

            lvlText.text = "Lvl." + lvl.ToString();

            if (this.isTraining)
            {
                progressBar.value = progressValue;
                timer.text = TimeConversion.AbbreviateTime(currentTime);
                startBut.gameObject.GetComponent<Image>().color = Color.yellow;
                timer.gameObject.SetActive(false);
            }
            else if(!this.isTraining && currentTime < cooldowns[timeLvl] && techType == Type.Passive)
            {
                progressBar.value = progressValue;
                timer.text = TimeConversion.AbbreviateTime(currentTime);
                startBut.gameObject.GetComponent<Image>().color = Color.green;
                timer.gameObject.SetActive(true);
            }
            else
            {
                if(lvl > 0)
                {
                    startBut.gameObject.GetComponent<Image>().color = Color.green;
                    if (techType != Type.Passive)
                    {
                        currentTime = (float)Math.Round(cooldowns[timeLvl], 1);
                        timer.text = TimeConversion.AbbreviateTime(currentTime);
                        timer.gameObject.SetActive(isCooldown);
                        if (isCooldown)
                        {
                            startBut.gameObject.GetComponent<Image>().color = Color.red;
                        }
                        progressValue = progressBar.value;
                    }
                    else
                    {
                        progressValue = progressBar.value;
                        if (!this.isTraining && currentTime < cooldowns[timeLvl] && techType == Type.Passive)
                        {
                            timer.text = TimeConversion.AbbreviateTime(currentTime);
                            startBut.gameObject.GetComponent<Image>().color = Color.green;
                            timer.gameObject.SetActive(true);
                        }
                        else
                        {
                            currentTime = (float)Math.Round((1 - progressBar.value) * cooldowns[timeLvl], 1);
                            timer.text = TimeConversion.AbbreviateTime(currentTime);
                            timer.gameObject.SetActive(true);
                        } 
                    }
                }
                else
                {
                    buyObject.SetActive(true);
                    startBut.gameObject.GetComponent<Image>().color = Color.magenta;
                    timeObject.gameObject.SetActive(false);
                }
            }
        }
    }

    public int SimulateTime(float total)
    {
        int xp = 0;
        float left = total;
        if (!this.isTraining && currentTime < cooldowns[timeLvl] && techType == Type.Passive)
        {
            progressBar.value = progressValue;
            timer.text = TimeConversion.AbbreviateTime(currentTime);
            startBut.gameObject.GetComponent<Image>().color = Color.green;
            timer.gameObject.SetActive(true);
        }
        else if (currentTime < left)
        {
            double increaseAmount = Math.Floor(left / cooldowns[timeLvl]);
            float leftSeconds = (float)Math.Round(left - (increaseAmount * cooldowns[timeLvl]), 2);
            currentTime = currentTime + leftSeconds;

            double i = increaseAmount;
            while (i != 0)
            {
                i--;
                GameManager.xp += xpIncreases[gainLvl];
                Reincarnation.totalEarnXp += xpIncreases[gainLvl];
                GameManager.UpdateText();
                xp += xpIncreases[gainLvl];
            }

            isTraining = true;
            startBut.gameObject.GetComponent<Image>().color = Color.yellow;
            progressBar.value = (cooldowns[timeLvl] - currentTime) / cooldowns[timeLvl];
            progressValue = progressBar.value;
            timer.text = TimeConversion.AbbreviateTime(currentTime);
        }
        else if (currentTime != cooldowns[timeLvl])
        {
            currentTime = currentTime - left;
            isTraining = true;
            startBut.gameObject.GetComponent<Image>().color = Color.yellow;
            progressBar.value = (cooldowns[timeLvl] - currentTime) / cooldowns[timeLvl];
            progressValue = progressBar.value;
            timer.text = TimeConversion.AbbreviateTime(currentTime);
        }
        return xp;
    }
}
