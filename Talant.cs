﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Talant : MonoBehaviour, IPointerClickHandler
{
    public enum Type
    {
        None,
        Train1_AUTO,
        Train2_AUTO,
        ThreeProcentIncreaseSpeed,
        PlusFifty,
        HalfPower,
        Praise_GodSkill,
        UnlockBlood,
        UnlockGather,
        LawStarterPack,
        Sacrifice_GodSkill,
        UnlockMint,
        UnlockDandelion,
        AddChances,
        Train3_AUTO,
        Train4_AUTO,
        Train5_AUTO,
        Train6_AUTO,
        Train7_AUTO,
        Train8_AUTO,
        SevenProcentIncreaseSpeed,
        TenProcentIncreaseSpeed,
        AutomationMoment
    }

    public Battle battle;

    [Header("Talant Information")]
    public string description;
    public string title;
    public string bonus;
    public bool isBought;
    public Sprite image;
    public static Talant previousTalant;
    public Type bonusType;
    [Header("Information Objects")]
    public GameObject talantInfo;
    public Text titleText;
    public Text boughtText;
    public Text descriptionText;
    public Text bonusText;
    public Image talantIcon;
    public Text talantPoints;
    public Button buyBut;
    [Header("Requirements")]
    public GameObject requirementObj;
    public GameObject requirementListObj;
    public Text reqTitle;
    public bool hasRequirements;
    public int playerLvlReq;
    public Talant[] talantReq;

    private Text[] requirementsTexts = new Text[4];
    private int reqIt = 0;
    private int reqCompleted = 0;
    private bool reqCanBuy = false;
    [Header("Exclusives")]
    public GameObject exclusiveObj;
    public GameObject exclusiveListObj;
    public bool hasExclusives;
    public Talant[] talantExclusives;

    private Text[] exclusivesTexts = new Text[3];
    private int exclusiveIt = 0;
    private int exclusiveCompleted = 0;
    private bool excCanBuy = false;

    [Header("Other")]
    public bool isActivated;

    public TalantManager talantManager;
    public AbilityManager abilityManager;

    [SerializeField] private GameObject godSlots;
    [SerializeField] private GameObject bloodCurrency;
    [SerializeField] private GameObject gatherTransit;
    [SerializeField] private GameObject mintGrow;
    [SerializeField] private GameObject mintSearch;
    [SerializeField] private GameObject dandeGrow;
    [SerializeField] private GameObject dandeSearch;
    [SerializeField] private GameObject searchings;
    [SerializeField] private Transform trainings;

    /*
    public void SetupAfterSaveUi()
    {
        talantInfo.transform.localPosition = Vector3.zero;

        titleText.text = title;
        descriptionText.text = description;
        bonusText.text = bonus;

        if (isBought)
        {
            talantIcon.color = Color.white;
            boughtText.gameObject.SetActive(true);
            buyBut.gameObject.SetActive(false);
            talantPoints.gameObject.SetActive(false);
        }
        else
        {
            if (exclusiveWith != null && exclusiveWith.GetComponent<Talant>().isBought)
            {
                buyBut.gameObject.SetActive(false);
                talantPoints.gameObject.SetActive(false);
                boughtText.gameObject.SetActive(false);
            }
            else
            {
                boughtText.gameObject.SetActive(false);
                buyBut.gameObject.SetActive(true);
                talantPoints.gameObject.SetActive(true);
            }
            talantIcon.color = Color.gray;
        }
        talantIcon.sprite = image;
        talantInfo.transform.Find("Icon").GetComponent<Image>().sprite = image;
        talantPoints.text = "Points:" + talantManager.talantPoints.ToString();

        buyBut.onClick.AddListener(Buy);
        requirementObj.SetActive(true);
        if (hasRequirements)
        {
            canBuy = false;

            reqIt = 0;
            reqCompleted = 0;


            if (playerLvlReq != 0)
            {
                GameObject reqObj = new GameObject("Req" + (reqIt + 1));
                reqObj.AddComponent<Text>();

                reqObj.GetComponent<Text>().font = Resources.Load("BebasNeue-Regular") as Font;
                reqObj.GetComponent<Text>().fontStyle = FontStyle.Bold;
                reqObj.GetComponent<Text>().fontSize = 39;
                reqObj.GetComponent<Text>().color = Color.red;
                reqObj.GetComponent<Text>().text = "-PLAYER LVL " + playerLvlReq;

                reqObj.gameObject.transform.SetParent(requirementListObj.transform);
                reqObj.transform.localPosition = new Vector3(-100, -62 + (-48 * reqIt), 0);

                reqObj.GetComponent<RectTransform>().sizeDelta = new Vector2(258, 64);

                requirementsTexts[reqIt] = reqObj.GetComponent<Text>();
                reqObj = null;
                reqIt++;
            }
            if (talantReq != null)
            {
                GameObject reqObj = new GameObject("Req" + (reqIt + 1));
                reqObj.AddComponent<Text>();

                reqObj.GetComponent<Text>().font = Resources.Load("BebasNeue-Regular") as Font;
                reqObj.GetComponent<Text>().fontStyle = FontStyle.Bold;
                reqObj.GetComponent<Text>().fontSize = 39;
                reqObj.GetComponent<Text>().color = Color.red;
                reqObj.GetComponent<Text>().text = "-" + talantReq.title;

                reqObj.gameObject.transform.SetParent(requirementListObj.transform);
                reqObj.transform.localPosition = new Vector3(-100, -62 + (-48 * reqIt), 0);

                reqObj.GetComponent<RectTransform>().sizeDelta = new Vector2(258, 64);

                requirementsTexts[reqIt] = reqObj.GetComponent<Text>();
                reqObj = null;
                reqIt++;
            }

            CheckRequirements();
        }
        else
        {
            requirementObj.SetActive(false);
        }

        if (this.isActivated)
        {
            talantInfo.SetActive(true);
        }
    }
    */
    public void SetupUi()
    {
        talantInfo.transform.localPosition = Vector3.zero;

        titleText.text = title;
        descriptionText.text = description;
        bonusText.text = bonus;

        requirementObj.SetActive(hasRequirements);
        exclusiveObj.SetActive(hasExclusives);

        SetupRequirements();
        SetupExclusives();

        if (isBought)
        {
            talantIcon.color = Color.white;
            boughtText.gameObject.SetActive(true);
            buyBut.gameObject.SetActive(false);
            talantPoints.gameObject.SetActive(false);
        }
        else
        {
            if(!hasRequirements && !hasExclusives)
            {
                buyBut.gameObject.SetActive(true);
            }
            else
            {
                if (!hasExclusives)
                {
                    excCanBuy = true;
                }
                buyBut.gameObject.SetActive(reqCanBuy && excCanBuy);
            }
            boughtText.gameObject.SetActive(false);
            talantPoints.gameObject.SetActive(true);
            talantIcon.color = Color.gray;
        }
        talantIcon.sprite = image;
        talantInfo.transform.Find("Icon").GetComponent<Image>().sprite = image;
        talantPoints.text = "Points:" + talantManager.talantPoints.ToString();

        buyBut.onClick.AddListener(Buy);

        if (this.isActivated)
        {
            talantInfo.SetActive(true);
        }
    }

    private void SetupRequirements()
    {
        if (hasRequirements)
        {
            reqCanBuy = false;

            reqIt = 0;
            reqCompleted = 0;


            if (playerLvlReq != 0)
            {
                GameObject reqObj = new GameObject("Req" + (reqIt + 1));
                reqObj.AddComponent<Text>();

                reqObj.GetComponent<Text>().font = Resources.Load("BebasNeue-Regular") as Font;
                reqObj.GetComponent<Text>().fontStyle = FontStyle.Bold;
                reqObj.GetComponent<Text>().fontSize = 39;
                reqObj.GetComponent<Text>().color = Color.red;
                reqObj.GetComponent<Text>().text = "-PLAYER LVL " + playerLvlReq;

                reqObj.gameObject.transform.SetParent(requirementListObj.transform);
                reqObj.transform.localPosition = new Vector3(-100, -62 + (-48 * reqIt), 0);

                reqObj.GetComponent<RectTransform>().sizeDelta = new Vector2(258, 64);

                requirementsTexts[reqIt] = reqObj.GetComponent<Text>();
                reqObj = null;
                reqIt++;
            }
            if (talantReq != null)
            {
                for (int i = 0; i < talantReq.Length; i++)
                {
                    GameObject reqObj = new GameObject("Req" + (reqIt + 1));
                    reqObj.AddComponent<Text>();

                    reqObj.GetComponent<Text>().font = Resources.Load("BebasNeue-Regular") as Font;
                    reqObj.GetComponent<Text>().fontStyle = FontStyle.Bold;
                    reqObj.GetComponent<Text>().fontSize = 39;
                    reqObj.GetComponent<Text>().color = Color.red;
                    reqObj.GetComponent<Text>().text = "-" + talantReq[i].title;

                    reqObj.gameObject.transform.SetParent(requirementListObj.transform);
                    reqObj.transform.localPosition = new Vector3(-100, -62 + (-48 * reqIt), 0);

                    reqObj.GetComponent<RectTransform>().sizeDelta = new Vector2(258, 64);

                    requirementsTexts[reqIt] = reqObj.GetComponent<Text>();
                    reqObj = null;
                    reqIt++;
                }
            }

            CheckRequirements();
        }
        else
        {
            requirementObj.SetActive(false);
        }
    }

    private void SetupExclusives()
    {
        if (hasExclusives)
        {
            excCanBuy = false;

            exclusiveIt = 0;
            exclusiveCompleted = 0;

            for (int i = 0; i < talantExclusives.Length; i++)
            {
                GameObject exlusiveObj = new GameObject("Exc" + (exclusiveIt + 1));
                exlusiveObj.AddComponent<Text>();

                exlusiveObj.GetComponent<Text>().font = Resources.Load("BebasNeue-Regular") as Font;
                exlusiveObj.GetComponent<Text>().fontStyle = FontStyle.Bold;
                exlusiveObj.GetComponent<Text>().fontSize = 39;
                exlusiveObj.GetComponent<Text>().color = Color.cyan;
                exlusiveObj.GetComponent<Text>().text = "-" + talantExclusives[i].title;

                exlusiveObj.gameObject.transform.SetParent(exclusiveListObj.transform);
                exlusiveObj.transform.localPosition = new Vector3(-100, -62 + (-48 * exclusiveIt), 0);

                exlusiveObj.GetComponent<RectTransform>().sizeDelta = new Vector2(258, 64);

                exclusivesTexts[exclusiveIt] = exlusiveObj.GetComponent<Text>();

                exlusiveObj = null;
                exclusiveIt++;
            }

            CheckExclusives();
        }
        else
        {
            exclusiveObj.SetActive(false);
        }
    }

    public void ChangeUi()
    {
        talantInfo.transform.localPosition = Vector3.zero;

        titleText.text = title;
        descriptionText.text = description;
        bonusText.text = bonus;

        if (isBought)
        {
            talantIcon.color = Color.white;
            boughtText.gameObject.SetActive(true);
            buyBut.gameObject.SetActive(false);
            talantPoints.gameObject.SetActive(false);
        }
        else
        {
            if (!hasRequirements && !hasExclusives)
            {
                buyBut.gameObject.SetActive(true);
            }
            else
            {
                buyBut.gameObject.SetActive(reqCanBuy && excCanBuy);
            }
            boughtText.gameObject.SetActive(false);
            talantPoints.gameObject.SetActive(true);
            talantIcon.color = Color.gray;

        }
        talantIcon.sprite = image;
        talantInfo.transform.Find("Icon").GetComponent<Image>().sprite = image;
        talantPoints.text = "Points:" + talantManager.talantPoints.ToString();

        buyBut.onClick.AddListener(Buy);
    }

    public void CheckRequirements()
    {
        if (!reqCanBuy)
        {
            if (battle.player.lvl >= playerLvlReq)
            {
                requirementsTexts[0].color = Color.green;
                reqCompleted = 1;
            }
            for (int i = 0; i < talantReq.Length; i++)
            {
                if (talantReq[i].isBought)
                {
                    reqCompleted++;
                    requirementsTexts[i].color = Color.red;
                }
            }
            /*
            if (battle.player.lvl >= playerLvlReq && requirementsTexts[0] != null)
            {
                requirementsTexts[0].color = Color.green;
                reqCompleted = 1;
                if (talantReq.isBought)
                {
                    requirementsTexts[1].color = Color.green;
                    reqCompleted = 2;
                }
            }
            else if (talantReq.isBought && requirementsTexts[1] != null)
            {
                requirementsTexts[1].color = Color.green;
                reqCompleted = 1;
                if (battle.player.lvl >= playerLvlReq)
                {
                    requirementsTexts[0].color = Color.green;
                    reqCompleted = 2;
                }
            }*/
        }

        if (reqCompleted == reqIt)
        {
            reqCanBuy = true;
            reqTitle.color = new Color32(26, 171, 0, 255);
        }
        else
        {
            reqCanBuy = false;
        }
    }

    public void CheckExclusives()
    {
        if (!excCanBuy)
        {
            if (exclusiveObj.activeSelf)
            {
                for (int i = 0; i < talantExclusives.Length; i++)
                {
                    if (talantExclusives[i].isBought)
                    {
                        exclusiveCompleted++;
                        exclusivesTexts[i].color = Color.red;
                    }
                }
            }
        }

        excCanBuy = (exclusiveCompleted == 0);

    }

    public void OnPointerClick(PointerEventData eventData)
    {
        TryActivateTalantInfo();
    }

    public void TryActivateTalantInfo()
    {
        if(exclusiveListObj.transform.childCount != 0){
            for (int i = 0; i < exclusiveListObj.transform.childCount; i++)
            {
                Destroy(exclusiveListObj.transform.GetChild(i).gameObject);
            }
            exclusiveIt = 0;
        }
        if (requirementListObj.transform.childCount != 0)
        {
            for (int i = 0; i < requirementListObj.transform.childCount; i++)
            {
                Destroy(requirementListObj.transform.GetChild(i).gameObject);
            }
            reqIt = 0;
        }

        if (!talantInfo.activeSelf)
        {
            isActivated = true;
            buyBut.onClick.RemoveAllListeners();
            previousTalant = this;
            talantInfo.SetActive(true);
            SetupUi();
        }
        else
        {
            if (previousTalant != null)
            {
                if (!isActivated)
                {
                    isActivated = true;
                    previousTalant.isActivated = false;
                    buyBut.onClick.RemoveAllListeners();
                    previousTalant = this;
                    talantInfo.SetActive(true);
                    SetupUi();
                }
                else
                {
                    isActivated = false;
                    buyBut.onClick.RemoveAllListeners();
                    talantInfo.SetActive(false);
                }
            }
            else
            {
                isActivated = false;
                buyBut.onClick.RemoveAllListeners();
                talantInfo.SetActive(false);
            }
        }
    }

    public void Buy()
    {
        if (hasRequirements)
        {
            CheckRequirements();
        }
        else
        {
            reqCanBuy = true;
        }

        if (hasExclusives)
        {
            CheckExclusives();
        }
        else
        {
            excCanBuy = true;
        }

        if (talantManager.talantPoints >= 1 && !isBought)
        {
            if (reqCanBuy && excCanBuy)
            {
                talantManager.talantPoints = talantManager.talantPoints - 1;
                talantManager.OnPointsChange();
                this.GetComponent<Image>().color = Color.white;
                isBought = true;

                TryGetTalantBonus();
                ChangeUi();
            }
            /*
            if(hasRequirements && canBuy)
            {
                if (talantReq != null)
                {
                    if (talantReq.isBought)
                    {
                        talantManager.talantPoints = talantManager.talantPoints - 1;
                        talantManager.OnPointsChange();
                        this.GetComponent<Image>().color = Color.white;
                        isBought = true;

                        TryGetTalantBonus();
                        ChangeUi();
                    }
                }
                else
                {
                    talantManager.talantPoints = talantManager.talantPoints - 1;
                    talantManager.OnPointsChange();
                    if (exclusiveWith == null)
                        this.GetComponent<Image>().color = Color.white;
                    isBought = true;

                    TryGetTalantBonus();
                    ChangeUi();
                }
            }
            else if(!hasRequirements)
            {
                if (talantReq != null)
                {
                    if (talantReq.isBought)
                    {
                        talantManager.talantPoints = talantManager.talantPoints - 1;
                        talantManager.OnPointsChange();
                        if (exclusiveWith == null)
                            this.GetComponent<Image>().color = Color.white;
                        isBought = true;

                        TryGetTalantBonus();
                        ChangeUi();
                    }
                }
                else
                {
                    talantManager.talantPoints = talantManager.talantPoints - 1;
                    talantManager.OnPointsChange();
                    if (exclusiveWith == null)
                        this.GetComponent<Image>().color = Color.white;
                    isBought = true;

                    TryGetTalantBonus();
                    ChangeUi();
                }
            }
            */
        }
    }

    public void TryGetTalantBonus()
    {
        if (bonusType == Type.AutomationMoment)
        {
            for(int i = 0; i < trainings.childCount; i++)
            {
                if(trainings.GetChild(i).GetComponent<Train>().lvl == trainings.GetChild(i).GetComponent<Train>().maxLvl)
                {
                    trainings.GetChild(i).GetComponent<Train>().limitTime = 0.001f;
                    trainings.GetChild(i).GetComponent<Train>().curLimitTime = 0.001f;
                }
            }
        }
        if (bonusType == Type.Train1_AUTO)
        {
            Train train = battle.transform.parent.Find("Trainings").GetChild(0).GetComponent<Train>();
            train.isAuto = true;
            train.automationOn = false;
            train.automation.SetActive(true);
            train.toggleOn.gameObject.SetActive(false);
            train.toggleOff.gameObject.SetActive(true);
        }
        if (bonusType == Type.Train2_AUTO)
        {
            Train train = battle.transform.parent.Find("Trainings").GetChild(1).GetComponent<Train>();
            train.isAuto = true;
            train.automationOn = false;
            train.automation.SetActive(true);
            train.toggleOn.gameObject.SetActive(false);
            train.toggleOff.gameObject.SetActive(true);
        }
        if (bonusType == Type.Train3_AUTO)
        {
            Train train = battle.transform.parent.Find("Trainings").GetChild(2).GetComponent<Train>();
            train.isAuto = true;
            train.automationOn = false;
            train.automation.SetActive(true);
            train.toggleOn.gameObject.SetActive(false);
            train.toggleOff.gameObject.SetActive(true);
        }
        if (bonusType == Type.Train4_AUTO)
        {
            Train train = battle.transform.parent.Find("Trainings").GetChild(3).GetComponent<Train>();
            train.isAuto = true;
            train.automationOn = false;
            train.automation.SetActive(true);
            train.toggleOn.gameObject.SetActive(false);
            train.toggleOff.gameObject.SetActive(true);
        }
        if (bonusType == Type.Train5_AUTO)
        {
            Train train = battle.transform.parent.Find("Trainings").GetChild(4).GetComponent<Train>();
            train.isAuto = true;
            train.automationOn = false;
            train.automation.SetActive(true);
            train.toggleOn.gameObject.SetActive(false);
            train.toggleOff.gameObject.SetActive(true);
        }
        if (bonusType == Type.Train6_AUTO)
        {
            Train train = battle.transform.parent.Find("Trainings").GetChild(5).GetComponent<Train>();
            train.isAuto = true;
            train.automationOn = false;
            train.automation.SetActive(true);
            train.toggleOn.gameObject.SetActive(false);
            train.toggleOff.gameObject.SetActive(true);
        }
        if (bonusType == Type.Train7_AUTO)
        {
            Train train = battle.transform.parent.Find("Trainings").GetChild(6).GetComponent<Train>();
            train.isAuto = true;
            train.automationOn = false;
            train.automation.SetActive(true);
            train.toggleOn.gameObject.SetActive(false);
            train.toggleOff.gameObject.SetActive(true);
        }
        if (bonusType == Type.Train8_AUTO)
        {
            Train train = battle.transform.parent.Find("Trainings").GetChild(7).GetComponent<Train>();
            train.isAuto = true;
            train.automationOn = false;
            train.automation.SetActive(true);
            train.toggleOn.gameObject.SetActive(false);
            train.toggleOff.gameObject.SetActive(true);
        }
        if (bonusType == Type.ThreeProcentIncreaseSpeed)
        {
            TalantManager.automationSpeed += 3;
        }
        if (bonusType == Type.SevenProcentIncreaseSpeed)
        {
            TalantManager.automationSpeed += 7;
        }
        if (bonusType == Type.TenProcentIncreaseSpeed)
        {
            TalantManager.automationSpeed += 10;
        }
        if (bonusType == Type.PlusFifty)
        {
            battle.player.IncreaseStat(StatUpgrade.Stats.PP, 50);
            battle.player.IncreaseStat(StatUpgrade.Stats.MP, 50);
            battle.player.IncreaseStat(StatUpgrade.Stats.CRT, 0.5f);
            battle.player.IncreaseStat(StatUpgrade.Stats.AS, 50);
            battle.player.IncreaseStat(StatUpgrade.Stats.HP, 50);
        }
        if(bonusType == Type.HalfPower)
        {
            battle.player.IncreaseStat(StatUpgrade.Stats.PP, battle.player.physicPower / 2);
        }
        if(bonusType == Type.Praise_GodSkill)
        {
            abilityManager.AddAbilityToBattleGodSlots(AbilityManager.GetGodAbilityById(1));
            godSlots.SetActive(true);
        }
        if (bonusType == Type.UnlockBlood)
        {
            bloodCurrency.SetActive(true);
            TalantManager.bloodUnlocked = true;
        }
        if (bonusType == Type.UnlockGather)
        {
            gatherTransit.SetActive(true);
            GatheringManager.gatheringUnlocked = true;
        }
        if (bonusType == Type.LawStarterPack)
        {
            battle.player.IncreaseStat(StatUpgrade.Stats.PP, 800);
            battle.player.IncreaseStat(StatUpgrade.Stats.MP, 800);
            battle.player.IncreaseStat(StatUpgrade.Stats.CRT, 0.1f);
            battle.player.IncreaseStat(StatUpgrade.Stats.AS, 25);
            battle.player.IncreaseStat(StatUpgrade.Stats.HP, 1200);
            GameManager.gold += 300;
            GameManager.UpdateText();
        }
        if (bonusType == Type.Sacrifice_GodSkill)
        {
            abilityManager.AddAbilityToBattleGodSlots(AbilityManager.GetGodAbilityById(2));
        }
        if (bonusType == Type.UnlockMint)
        {
            mintGrow.SetActive(true);
            mintSearch.SetActive(true);
        }
        if (bonusType == Type.UnlockDandelion)
        {
            dandeGrow.SetActive(true);
            dandeSearch.SetActive(true);
        }
        if (bonusType == Type.AddChances)
        {
            for(int i = 1; i < searchings.transform.childCount; i++)
            {
                searchings.transform.GetChild(i).GetComponent<Searching>().seedChance += 10;
            }
        }
    }

    public void Reset()
    {
        SetupExclusives();
        SetupRequirements();
        isActivated = false;
        buyBut.onClick.RemoveAllListeners();
        talantInfo.SetActive(false);
    }

    public void SaveTalant()
    {
        SaveSystem.SaveTalant(isBought, isActivated, talantManager.talantPoints, this.name);
    }

    public void LoadTalant()
    {
        TalantData data = SaveSystem.LoadTalant(this.name);

        if (data != null)
        {
            this.isBought = data.isBought;
            this.isActivated = data.isActivated;

            talantManager.talantPoints = data.talantPoints;
            talantManager.OnPointsChange();

            if (isBought)
            {
                this.GetComponent<Image>().color = Color.white;
                talantIcon.color = Color.white;
                boughtText.gameObject.SetActive(true);
                buyBut.gameObject.SetActive(false);
                talantPoints.gameObject.SetActive(false);
                if (bonusType == Type.Praise_GodSkill)
                {
                    godSlots.SetActive(true);
                }
                else if (bonusType == Type.UnlockBlood)
                {
                    bloodCurrency.SetActive(true);
                    TalantManager.bloodUnlocked = true;
                }
                else if (bonusType == Type.UnlockGather)
                {
                    gatherTransit.SetActive(true);
                    GatheringManager.gatheringUnlocked = true;
                }
                else if (bonusType == Type.UnlockMint)
                {
                    mintGrow.SetActive(true);
                    mintSearch.SetActive(true);
                }
                else if (bonusType == Type.UnlockDandelion)
                {
                    dandeGrow.SetActive(true);
                    dandeSearch.SetActive(true);
                }
            }
            else
            {
                this.GetComponent<Image>().color = Color.gray;
                talantIcon.color = Color.gray;
                boughtText.gameObject.SetActive(false);
                buyBut.gameObject.SetActive(true);
                talantPoints.gameObject.SetActive(true);
                if (bonusType == Type.Praise_GodSkill)
                {
                    godSlots.SetActive(false);
                }
                else if (bonusType == Type.UnlockBlood)
                {
                    bloodCurrency.SetActive(false);
                    TalantManager.bloodUnlocked = false;
                }
                else if (bonusType == Type.UnlockGather)
                {
                    gatherTransit.SetActive(false);
                    GatheringManager.gatheringUnlocked = false;
                }
                else if (bonusType == Type.UnlockMint)
                {
                    mintGrow.SetActive(false);
                    mintSearch.SetActive(false);
                }
                else if (bonusType == Type.UnlockDandelion)
                {
                    dandeGrow.SetActive(false);
                    dandeSearch.SetActive(false);
                }
            }

            if (this.isActivated)
                SetupUi();
        }
        else
        {
            if (bonusType == Type.Praise_GodSkill)
            {
                godSlots.SetActive(false);
            }
            else if (bonusType == Type.UnlockBlood)
            {
                bloodCurrency.SetActive(false);
                TalantManager.bloodUnlocked = false;
            }
            else if (bonusType == Type.UnlockGather)
            {
                gatherTransit.SetActive(false);
                GatheringManager.gatheringUnlocked = false;
            }
            else if (bonusType == Type.UnlockMint)
            {
                mintGrow.SetActive(false);
                mintSearch.SetActive(false);
            }
            else if (bonusType == Type.UnlockDandelion)
            {
                dandeGrow.SetActive(false);
                dandeSearch.SetActive(false);
            }
        }
    }
}
