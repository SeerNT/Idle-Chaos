using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using NT_Utils.Conversion;
using System;

public class GameManager : MonoBehaviour
{
    public static double magic;
    public static double xp;
    public static double gold;
    public static double blood;
    public static double qi;
    public static double meta;

    public static Text magicText;
    public static Text xpText;
    public static Text goldText;
    public Text bloodText;
    public static Text qiText;
    public static Text metaText;
    public static Text chaosText;

    [SerializeField] private Battle battle;
    [SerializeField] private Transform[] ultraTrains = new Transform[8];
    [SerializeField] private Transform[] ultraTechs = new Transform[6];
    [SerializeField] private GameObject mobileTrainPage;
    [SerializeField] private GameObject mobileTechPage;
    [SerializeField] private Transform chaosIncrement;
    [SerializeField] private bool isHolding;
    [SerializeField] private bool isIncreasing;
    [SerializeField] private bool isDecreasing;
    [Header("Event Screen")]
    private ChaosEvent eventChaos;
    [SerializeField] private GameObject iconDrop;
    [SerializeField] private Text eventNameDrop;
    [SerializeField] private Text eventDescDrop;
    [SerializeField] private GameObject eventScreen;
    [SerializeField] private Button closeEventBut;
    [Header("Events")]
    public ChaosEvent[] commonEvents = new ChaosEvent[2];
    public ChaosEvent[] rareEvents = new ChaosEvent[2];
    public ChaosEvent[] ultraEvents = new ChaosEvent[2];
    public ChaosEvent[] trollEvents = new ChaosEvent[2];
    private bool isShowingEvent = false;

    void Awake() {
       // GameManager.xp = 10000;
       // GameManager.gold = 100;
       //  GameManager.magic = 10000;
       // GameManager.blood = 1;
        magicText = GameObject.Find("MagicIncrement").transform.GetChild(0).GetComponent<Text>();
        xpText = GameObject.Find("Xp").GetComponent<Text>();
        goldText = GameObject.Find("Money").GetComponent<Text>();

        qiText = GameObject.Find("QiIncrement").transform.GetChild(0).GetComponent<Text>();
        metaText = GameObject.Find("MetaValue").GetComponent<Text>();
        chaosText = chaosIncrement.GetChild(0).GetComponent<Text>();
        closeEventBut.onClick.AddListener(CloseEvent);
        UpdateText();
    }

    public void HoldingToIncreaseChaos()
    {
        isHolding = true;
        isIncreasing = true;
    }

    public void HoldingToDecreaseChaos()
    {
        isHolding = true;
        isDecreasing = true;
    }

    public void RealeaseIncreasingChaos()
    {
        isHolding = false;
        isIncreasing = false;
    }

    public void RealeaseDecreasingChaos()
    {
        isHolding = false;
        isDecreasing = false;
    }

    public void CloseEvent()
    {
        eventScreen.SetActive(false);
        isShowingEvent = false;
        if(eventChaos.type == ChaosEvent.Type.StunPlayer)
        {
            battle.StunPlayer(eventChaos.duration * 1000);
        }
        if (eventChaos.type == ChaosEvent.Type.BlockPlayerSkills)
        {
            battle.BlockPlayerSkills(eventChaos.duration * 1000);
        }
        if (eventChaos.type == ChaosEvent.Type.WeakenPlayer)
        {
            battle.WeakenPlayer(eventChaos.duration * 1000);
        }
        if (eventChaos.type == ChaosEvent.Type.Minus50PrPP)
        {
            battle.player.IncreaseStat(StatUpgrade.Stats.PP, -battle.player.physicPower / 2);
        }
        
    }

    public void TryGetChaoticItem()
    {
        System.Random generator = new System.Random(DateTime.Now.Millisecond + DateTime.Now.Second + DateTime.Now.Minute + DateTime.Now.Hour + DateTime.Now.Day);
        if (generator.Next(100000000) < (battle.player.chaosFactor * 15000) && battle.player.chaosFactor < 100 && !isShowingEvent)
        {
            GameItem itemDrop = GameItem.GetRandomRarityItem(GameItem.Rarity.Chaotic, battle);
            if (itemDrop != null)
            {
                battle.titleDrop.text = "??? DROPPED ITEM";
                battle.rarityDrop.text = itemDrop.rarity.ToString();
                battle.itemNameDrop.text = itemDrop.displayName;
                battle.itemDescDrop.text = itemDrop.description;
                battle.iconDrop.GetComponent<Image>().sprite = itemDrop.icon;
                battle.dropScreen.SetActive(true);
                battle.inventory.AddItem(itemDrop);
            }
        }
    }

    public void TryGetChaosEvent()
    {
        System.Random generator = new System.Random(DateTime.Now.Millisecond + DateTime.Now.Second + DateTime.Now.Minute + DateTime.Now.Hour + DateTime.Now.Day);
        if(generator.Next(100000000) < (battle.player.chaosFactor * 10000) && battle.player.chaosFactor < 100 && !isShowingEvent)
        {
            eventChaos = ChaosEvent.GetRandomEvent(this);
            if (eventChaos != null)
            {
                eventNameDrop.text = eventChaos.displayName;
                eventNameDrop.color = ChaosEvent.GetRarityColor(eventChaos);
                eventDescDrop.text = eventChaos.description;
                iconDrop.GetComponent<Image>().sprite = eventChaos.icon;
                eventScreen.SetActive(true);
                isShowingEvent = true;
            }
        } 
    }

    public void Update()
    {
        if(battle.player.chaosFactor >= 1)
        {
            if (Application.platform == RuntimePlatform.Android)
            {
                ultraTrains[4].gameObject.SetActive(true);
                ultraTrains[5].gameObject.SetActive(true);
                ultraTrains[6].gameObject.SetActive(true);
                ultraTrains[7].gameObject.SetActive(true);
                ultraTechs[3].gameObject.SetActive(true);
                ultraTechs[4].gameObject.SetActive(true);
                ultraTechs[5].gameObject.SetActive(true);
                mobileTrainPage.SetActive(true);
                mobileTechPage.SetActive(true);
            }
            else
            {
                ultraTrains[0].gameObject.SetActive(true);
                ultraTrains[1].gameObject.SetActive(true);
                ultraTrains[2].gameObject.SetActive(true);
                ultraTrains[3].gameObject.SetActive(true);
                ultraTechs[0].gameObject.SetActive(true);
                ultraTechs[1].gameObject.SetActive(true);
                ultraTechs[2].gameObject.SetActive(true);
            }  

            
            chaosIncrement.gameObject.SetActive(true);
            if (!CutsceneManager.isShowing)
            {
                TryGetChaosEvent();
                TryGetChaoticItem();
            }
        }

        magic = Math.Round(magic, 1);
        xp = Math.Round(xp, 1);

        if (isHolding)
        {
            if (isIncreasing)
            {
                battle.player.chaosFactor += battle.player.chaosFactor * 0.018f / 5000;
            }
            if (isDecreasing && battle.player.chaosFactor > 0)
            {
                battle.player.chaosFactor -= battle.player.chaosFactor * 0.018f / 5000;
                if(battle.player.chaosFactor < 0)
                {
                    battle.player.chaosFactor = 0;
                }
            }
        }
        magicText.text = NumberConversion.AbbreviateNumber(magic, 1);
        xpText.text = NumberConversion.AbbreviateNumber(xp, 1);
        goldText.text = NumberConversion.AbbreviateNumber(gold);
        if (TalantManager.bloodUnlocked)
        {
            bloodText.text = NumberConversion.AbbreviateNumber(blood);
        }
           
        if (qi > 0)
            qiText.text = NumberConversion.AbbreviateNumber(qi);
        metaText.text = NumberConversion.AbbreviateNumber(meta);
        chaosText.text = String.Format("{0:0.000000}", battle.player.chaosFactor);;
    }

    public static void SaveCurrency()
    {
        SaveSystem.SaveCurrency();
    }

    public static void LoadCurrency()
    {
        CurrencyData data = SaveSystem.LoadCurrency();

        if(data != null)
        {
            magic = data.magic;
            xp = data.xp;
            gold = data.gold;
            blood = data.blood;
            qi = data.qi;
            meta = data.meta;
            magicText = GameObject.Find("MagicIncrement").transform.GetChild(0).GetComponent<Text>();
            xpText = GameObject.Find("Xp").GetComponent<Text>();
            goldText = GameObject.Find("Money").GetComponent<Text>();
            //if(qi > 0)
                //qiText = GameObject.Find("QiIncrement").transform.GetChild(0).GetComponent<Text>();
            metaText = GameObject.Find("MetaValue").GetComponent<Text>();
            UpdateText();
        }
    }

    public static void UpdateText() {
        if(qi > 0 && qiText != null)
            qiText.text = NumberConversion.AbbreviateNumber(qi);
        metaText.text = NumberConversion.AbbreviateNumber(meta);
    }
}

