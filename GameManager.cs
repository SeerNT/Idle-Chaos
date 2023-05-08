using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using NT_Utils.Conversion;

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
    public static Text bloodText;
    public static Text qiText;
    public static Text metaText;

    void Awake() {
       // GameManager.xp = 10000;
       // GameManager.gold = 100;
       //  GameManager.magic = 10000;
       // GameManager.blood = 1;
        magicText = GameObject.Find("MagicIncrement").transform.GetChild(0).GetComponent<Text>();
        xpText = GameObject.Find("Xp").GetComponent<Text>();
        goldText = GameObject.Find("Money").GetComponent<Text>();
        bloodText = GameObject.Find("Blood").GetComponent<Text>();
        qiText = GameObject.Find("Qi").GetComponent<Text>();
        metaText = GameObject.Find("MetaValue").GetComponent<Text>();
        UpdateText();
    }

    public void Update()
    {
        magicText.text = NumberConversion.AbbreviateNumber(magic);
        xpText.text = NumberConversion.AbbreviateNumber(xp);
        goldText.text = NumberConversion.AbbreviateNumber(gold);
        bloodText.text = NumberConversion.AbbreviateNumber(blood);
        qiText.text = NumberConversion.AbbreviateNumber(qi);
        metaText.text = NumberConversion.AbbreviateNumber(meta);
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
            if(TalantManager.bloodUnlocked)
                bloodText = GameObject.Find("Blood").GetComponent<Text>();
            if(qi > 0)
                qiText = GameObject.Find("Qi").GetComponent<Text>();
            metaText = GameObject.Find("MetaValue").GetComponent<Text>();
            UpdateText();
        }
    }

    public static void UpdateText() {
        
        magicText.text = NumberConversion.AbbreviateNumber(magic);
        xpText.text = NumberConversion.AbbreviateNumber(xp);
        goldText.text = NumberConversion.AbbreviateNumber(gold);
        if (TalantManager.bloodUnlocked)
            bloodText.text = NumberConversion.AbbreviateNumber(blood);
        if(qi > 0)
            qiText.text = NumberConversion.AbbreviateNumber(qi);
        metaText.text = NumberConversion.AbbreviateNumber(meta);
    }
}
