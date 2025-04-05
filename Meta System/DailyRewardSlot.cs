using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DailyRewardSlot : MonoBehaviour
{
    [SerializeField] private DailyManager manager;

    private Text dayText;

    public DailyManager.DailyBonus bonus;
    public int day;
    public int value;
    public bool isCollected = false;

    private bool isAdded = false;
    private int loadedState = 0;

    void Start()
    {
        dayText = transform.Find("Day").GetComponent<Text>();
    }

    DailyManager.DailyBonus GetRandomBonus(int day){
        System.Random generator = new System.Random(DateTime.Now.Millisecond + DateTime.Now.Second + DateTime.Now.Minute + DateTime.Now.Hour + DateTime.Now.Day);
        if(day % 5 != 0)
        {
            int i = generator.Next(manager.commonBonuses.Length);
            return manager.commonBonuses[i];
        }else {
            int i = generator.Next(manager.bigBonuses.Length);
            return manager.bigBonuses[i];
        }
    }
    int GetValue(int day)
    {
        return (10 * day);
    }

    void Update()
    {
        UpdateUI();
    }

    void UpdateUI()
    {
        if (!this.isAdded && this.loadedState == -1)
        {
            string name = this.name;
            name = name.Replace("Slot", "");
            this.day = (int)(int.Parse(name) + (Math.Floor((decimal)(DailyManager.dayRow / 5)) * 5));
            this.bonus = GetRandomBonus(day);
            this.value = GetValue(day);
            manager.AddRewardSlot(this.bonus, int.Parse(name));
            this.isAdded = true;
        }
        else if(!this.isAdded && this.loadedState == 1)
        {
            string name = this.name;
            name = name.Replace("Slot", "");
            manager.AddRewardSlot(this.bonus, int.Parse(name));
            this.isAdded = true;
            if (isCollected)
            {
                transform.GetComponent<Image>().color = Color.cyan;
            }
        }
        dayText.text = "DAY " + day.ToString();
    }

    public void Reset()
    {
        string name = this.name;
        name = name.Replace("Slot", "");
        this.day = (int)(int.Parse(name) + (Math.Floor((decimal)(DailyManager.dayRow / 5)) * 5));
        this.bonus = GetRandomBonus(day);
        this.value = GetValue(day);
        manager.RemoveReward(int.Parse(name));
        manager.AddRewardSlot(this.bonus, int.Parse(name));
        this.isAdded = true;
        this.isCollected = false;
    }

    public void Save()
    {
        SaveSystem.SaveDailyReward(day, value, isCollected, this.name, DailyManager.currentTime);
    }

    public void Load()
    {
        DailyRewardData data = SaveSystem.LoadDailyReward(this.name);

        if (data != null)
        {
            string name = this.name;
            name = name.Replace("Inv", "");

            this.day = data.day;
            this.value = data.value;
            this.isCollected = data.isCollected;
            DailyManager.dayRow = data.dayRow;
            DailyManager.currentTime = data.time;
            loadedState = 1;
        }
        else
        {
            loadedState = -1;
            DailyManager.dayRow = 1;
        }
    }
}
