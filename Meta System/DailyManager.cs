using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using NT_Utils.Conversion;

public class DailyManager : MonoBehaviour
{
    public enum DailyBonus
    {
        META,
        MAGIC,
        XP,
        BIG_META,
        BIG_MAGIC,
        BIG_XP,
        TIMESKIP
    }

    [SerializeField] private Button collectBut;
    [SerializeField] private Text currentDayText;
    [SerializeField] private Text dailyTimerText;
    [SerializeField] private Transform rewardsObj;
    [SerializeField] private AutoSave saveSystem;

    public static int dayRow;
    public static double currentTime;

    public DailyBonus[] commonBonuses = new DailyBonus[3];
    public DailyBonus[] bigBonuses = new DailyBonus[4];

    public Dictionary<DailyBonus, Sprite> icons = new Dictionary<DailyBonus, Sprite>();

    private int inGameTime;

    private bool readyToSkip = false;

    void Awake()
    {
        foreach(DailyBonus bonus in Enum.GetValues(typeof(DailyBonus)))
        {
            icons.Add(bonus, Resources.Load<Sprite>("RewardIcons/" + bonus.ToString()));
        }
        collectBut.onClick.AddListener(Collect);
        InvokeRepeating("IncreaseTime", 0f, 1f);
    }

    void IncreaseTime()
    {
        inGameTime++;
        currentTime += 1;
    }

    void Collect()
    {
        DailyRewardSlot slot = null;
        if (dayRow % 5 != 0)
            slot = rewardsObj.Find("Slot" + (dayRow - Math.Floor((decimal)(dayRow / 5)) * 5).ToString()).GetComponent<DailyRewardSlot>();
        else
        {
            if (dayRow > 5)
            {
                slot = rewardsObj.Find("Slot" + Math.Floor(dayRow / (decimal)(dayRow / 5)).ToString()).GetComponent<DailyRewardSlot>();
            }
            else
            {
                slot = rewardsObj.Find("Slot" + dayRow.ToString()).GetComponent<DailyRewardSlot>();
            }
        }
        if (!slot.isCollected)
        {
            slot.transform.GetComponent<Image>().color = Color.cyan;
            slot.isCollected = true;
            inGameTime = 0;
            GiveBonus(slot.bonus, slot.value);
        }
    }

    void GiveBonus(DailyBonus bonus, int value)
    {
        if(bonus == DailyBonus.META)
        {
            GameManager.meta = 10 + GameManager.meta + GameManager.meta * (value / 100.0f);
        }
        if (bonus == DailyBonus.MAGIC)
        {
            GameManager.magic += 100 + GameManager.magic * (value / 100.0f);
        }
        if (bonus == DailyBonus.XP)
        {
            GameManager.xp += 100 + GameManager.xp * (value / 100.0f);
        }
        if (bonus == DailyBonus.BIG_META)
        {
            GameManager.meta += 100 + GameManager.meta * (value / 100.0f);
        }
        if (bonus == DailyBonus.BIG_MAGIC)
        {
            GameManager.magic += 10000 + GameManager.magic * (value / 100.0f);
        }
        if (bonus == DailyBonus.BIG_XP)
        {
            GameManager.xp += 10000 + GameManager.xp * (value / 100.0f);
        }
        GameManager.UpdateText();
        if (bonus == DailyBonus.TIMESKIP)
        {
            readyToSkip = true;
        }
    }

    void Skip()
    {
        saveSystem.SkipTime();
    }

    void LateUpdate()
    {
        if (readyToSkip)
        {
            readyToSkip = false;
            Invoke("Skip", 2);
        }
        currentDayText.text = "Day " + dayRow.ToString();
        DailyRewardSlot slot = null;
        if (dayRow != 0)
        {
            if (dayRow % 5 != 0)
                slot = rewardsObj.Find("Slot" + (dayRow - Math.Floor((decimal)(dayRow / 5)) * 5).ToString()).GetComponent<DailyRewardSlot>();
            else
            {
                if (dayRow > 5)
                {
                    slot = rewardsObj.Find("Slot" + Math.Floor(dayRow / (decimal)(dayRow / 5)).ToString()).GetComponent<DailyRewardSlot>();
                }
                else
                {
                    slot = rewardsObj.Find("Slot" + dayRow.ToString()).GetComponent<DailyRewardSlot>();
                }
            }
            if (slot.isCollected)
            {
                if (currentTime >= 86400 && currentTime < 172800)
                {
                    dayRow += 1;
                    currentTime = 0;
                    if (dayRow % 5 == 1)
                    {
                        ResetDaily();
                    }
                }
                else if (currentTime >= 172800)
                {
                    dayRow = 1;
                    currentTime = 0;
                    if (dayRow % 5 == 1)
                    {
                        ResetDaily();
                    }
                }
            }
        }

        dailyTimerText.gameObject.SetActive(dayRow != 0);
        dailyTimerText.text = TimeConversion.AbbreviateTime(86400 - currentTime);

        if((86400 - currentTime) <= 0)
        {
            dayRow = 1;
            currentTime = 0;
            if (dayRow % 5 == 1)
            {
                ResetDaily();
            }
        }
    }

    public void UpdateAfterLoad()
    {
        DailyRewardSlot slot = null;
        if (dayRow != 0)
        {
            if (dayRow % 5 != 0)
                slot = rewardsObj.Find("Slot" + (dayRow - Math.Floor((decimal)(dayRow / 5)) * 5).ToString()).GetComponent<DailyRewardSlot>();
            else
            {
                if (dayRow > 5)
                {
                    slot = rewardsObj.Find("Slot" + Math.Floor(dayRow / (decimal)(dayRow / 5)).ToString()).GetComponent<DailyRewardSlot>();
                }
                else
                {
                    slot = rewardsObj.Find("Slot" + dayRow.ToString()).GetComponent<DailyRewardSlot>();
                }
            }

            if (slot.isCollected)
            {
                if (currentTime >= 86400 && currentTime < 172800)
                {
                    dayRow += 1;
                    if (dayRow % 5 == 1)
                    {
                        ResetDaily();
                    }
                }
                else if (currentTime >= 172800)
                {
                    dayRow = 1;
                }
                else
                {
                    dayRow = 1;
                }
            }
            else
            {
                dayRow = 1;
                ResetDaily();
            }
        }
        else
        {
            dayRow = 1;
        }
        
    }

    public void ResetDaily()
    {
        for(int i = 0; i < rewardsObj.childCount; i++)
        {
            rewardsObj.Find("Slot" + (i + 1).ToString()).GetComponent<DailyRewardSlot>().Reset();
            if(i != 4)
            {
                rewardsObj.Find("Slot" + (i + 1).ToString()).GetComponent<Image>().color = new Color(0.8588f, 0.8313f, 0.4235f);
            }
            else
            {
                rewardsObj.Find("Slot" + (i + 1).ToString()).GetComponent<Image>().color = new Color(1, 0.8313f, 0);
            }
        }
    }

    public void RemoveReward(int slotId)
    {
        Transform slot = transform.Find("Rewards").Find("Slot" + slotId).Find("Reward");
        Destroy(slot.gameObject);
    }

    public void PrepareGameObject(GameObject go, DailyBonus bonus)
    {
        go.AddComponent<Image>();
        go.GetComponent<Image>().sprite = GetIconByBonus(bonus);
        go.AddComponent<CanvasGroup>();
        go.transform.localPosition = Vector3.zero;
        go.GetComponent<RectTransform>().sizeDelta = new Vector2(90, 90);
        go.GetComponent<RectTransform>().localScale = new Vector2(1, 1);
    }

    public GameObject AddGameObjectForMeta(int slotId)
    {
        GameObject go = new GameObject("Reward");
        Transform slot = null;

        slot = transform.Find("Rewards").Find("Slot" + slotId);

        go.GetComponent<Transform>().SetParent(slot);

        return go;
    }

    public void AddRewardSlot(DailyBonus bonus, int slotId)
    {
        GameObject go = AddGameObjectForMeta(slotId);
        PrepareGameObject(go, bonus);
    }

    public Sprite GetIconByBonus(DailyBonus bonus)
    {
        Sprite icon = null;

        icons.TryGetValue(bonus, out icon);

        return icon;
    }
}
