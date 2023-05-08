using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillProgression : MonoBehaviour
{
    public int[] levelRequirements;
    public Bonus.Type[] bonusTypes;
    public string[] bonusValues;
    public Bonus[] bonuses;

    public Transform skillProgression;
    public Text requirementSP_Text;

    public void Start()
    {
        for(int id = 0; id < bonusTypes.Length; id++)
        {
            Bonus bonus = new Bonus(bonusTypes[id], bonusValues[id]);
            bonuses[id] = bonus;
        }
    }
    public void TryPassSkillProgress(int currentLvl)
    {
        for (int lvlReqId = 0; lvlReqId < levelRequirements.Length; lvlReqId++)
        {
            if(currentLvl == levelRequirements[lvlReqId])
            {
                Transform currentSP = skillProgression.Find("SP" + (lvlReqId + 1).ToString());
                requirementSP_Text = currentSP.GetChild(1).GetComponent<Text>();
                requirementSP_Text.color = Color.green;
                bonuses[lvlReqId].GetBonus();
                break;
            }
        }
    }
    public void UpdateSkillProgress(int currentLvl)
    {
        for (int lvlReqId = 0; lvlReqId < levelRequirements.Length; lvlReqId++)
        {
            if (currentLvl >= levelRequirements[lvlReqId])
            {
                Transform currentSP = skillProgression.Find("SP" + (lvlReqId + 1).ToString());
                requirementSP_Text = currentSP.GetChild(1).GetComponent<Text>();
                requirementSP_Text.color = Color.green;
            }
            else
            {
                Transform currentSP = skillProgression.Find("SP" + (lvlReqId + 1).ToString());
                requirementSP_Text = currentSP.GetChild(1).GetComponent<Text>();
                requirementSP_Text.color = new Color32(255, 17, 0, 255);
            }
        }
    }
}
