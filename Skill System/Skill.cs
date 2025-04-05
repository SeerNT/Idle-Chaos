using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Skill : MonoBehaviour, IPointerClickHandler
{
    [Header("Skill Information")]
    public string description;
    public string title;
    public int cost;
    public int upgradeCost;
    public bool isChosen;
    public int lvl;
    public int lvlCap;
    public Sprite image;
    public GameObject exclusiveWith;
    public Skill[] requiredSkills;
    [Header("Information Objects")]
    public GameObject skillInfo;
    public Text titleText;
    public Text descriptionText;
    public Image skillIcon;
    public Text lvlText;
    public Text skillPoints;
    public Text costText;
    public Button spendBut;
    public GameObject skillProgressionObject;

    public SkillManager skillManager;
    public SkillProgression skillProgress;

    public bool isActivated;

    public Button closeBut;

    public void Start()
    {
        closeBut.onClick.AddListener(CloseSkillProg);
    }

    public void SetupUi()
    {
        if (this.transform.parent.name.Contains("3"))
        {
            skillInfo.transform.localPosition = Vector3.zero;
            skillProgress.transform.localPosition = new Vector3(461.91f, -52.59f, 0);
            closeBut.transform.localPosition = new Vector3(850f, 401.9f, 0);
        }
        else
        {
            skillInfo.transform.localPosition = new Vector3(568.9f, 0, 0);
            skillProgress.transform.localPosition = new Vector3(1032, -49, 0);
            closeBut.transform.localPosition = new Vector3(1430.09f, 401.9f, 0);
        }
        closeBut.gameObject.SetActive(true);

        titleText.text = title;
        descriptionText.text = description;
        lvlText.text = "Lvl." + lvl.ToString();
        if (lvl > 0)
        {
            skillIcon.color = Color.white;
        }
        skillIcon.sprite = image;
        skillInfo.transform.Find("Icon").GetComponent<Image>().sprite = image;
        skillPoints.text = "Points:" + skillManager.skillpoints.ToString();
        costText.text = "COST:" + cost.ToString() + " POINTS";
        
        if(exclusiveWith != null && exclusiveWith.GetComponent<Skill>().lvl >= 1)
        {
            spendBut.gameObject.SetActive(false);
        }
        else
        {
            int cnt = 0;
            int coolCnt = 0;
            for(int i = 0; i < requiredSkills.Length; i++)
            {
                if (requiredSkills[i] != null)
                {
                    if (requiredSkills[i].lvl != 100)
                    {
                        cnt++;
                    }
                    else if(requiredSkills[i].lvl == 100)
                    {
                        coolCnt++;
                    }
                }
            }
            if(((cnt != requiredSkills.Length && coolCnt == requiredSkills.Length) || requiredSkills.Length == 0) && lvl != 100)
            {
                spendBut.gameObject.SetActive(true);
                spendBut.GetComponent<UpgradeSkill>().skill = this;
            }
            else
            {
                spendBut.gameObject.SetActive(false);
            }
        }  
    }

    public void CloseSkillProg()
    {
        isActivated = false;
        skillProgress.gameObject.SetActive(false);
        skillInfo.gameObject.SetActive(false);
        closeBut.gameObject.SetActive(false);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        TryActivateSkillInfo();
    }

    public void TryActivateSkillInfo()
    {
        if (!skillInfo.activeSelf)
        {
            isActivated = true;
            skillInfo.SetActive(true);
            skillProgressionObject.SetActive(true);
            SetupUi();
        }
        else
        {
            //isActivated = false;
            //skillInfo.SetActive(false);
            //skillProgressionObject.SetActive(false);
            //closeBut.gameObject.SetActive(false);
        }
    }

    public void SaveSkillUpg()
    {
        SaveSystem.SaveSkillUpgrade(this, this.name, this.transform.parent.name);
    }

    public void LoadSkillUpg()
    {
        SkillUpgradeData data = SaveSystem.LoadSkillUpgrade(this.name, this.transform.parent.name);

        if (data != null)
        {
            skillManager.skillpoints = data.skillpoints;
            this.isActivated = data.isActivated;
            this.lvl = data.lvl;
            this.cost = data.skillCost;

            if (this.isActivated)
            {
                TryActivateSkillInfo();
                this.skillIcon.sprite = this.image;
                this.skillInfo.transform.Find("Icon").GetComponent<Image>().sprite = this.image;
                skillInfo.SetActive(true);
                skillProgressionObject.SetActive(true);
                closeBut.gameObject.SetActive(true);
            }

            if (this.lvl > 0)
            {
                this.skillIcon.color = Color.white;
            }
            else
            {
                this.skillIcon.color = new Color32(207, 207, 207, 255);
            }

            skillProgress.UpdateSkillProgress(this.lvl);
        }
    }
}
