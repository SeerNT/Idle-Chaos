using NT_Utils.Conversion;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkipUpgrade : MonoBehaviour
{
    private Text titleText;
    private Text descText;
    private Text costText;
    private Button buyBut;
    [SerializeField] private AutoSave saveSystem;

    [Header("Settings")]
    public int cost = 5;
    public int valueTimeHours = 0;

    void Start()
    {
        titleText = transform.Find("Title").GetComponent<Text>();
        descText = transform.Find("Description").GetComponent<Text>();
        costText = transform.Find("Cost").GetComponent<Text>();
        buyBut = transform.Find("BuyBut").GetComponent<Button>();

        costText.text = NumberConversion.AbbreviateNumber(cost);

        buyBut.onClick.AddListener(Buy);
    }

    void Update()
    {
        costText.text = NumberConversion.AbbreviateNumber(cost);
    }
    void Buy()
    {
        if (GameManager.meta >= cost)
        {
            GameManager.meta = (int)(GameManager.meta - cost);
            GameManager.UpdateText();
            if(valueTimeHours == 12)
                saveSystem.SkipTime12Hours();
            else
            {
                saveSystem.SkipTime();
            }
        }
    }
}
