using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeSkill : MonoBehaviour
{
    public Skill skill;
    public SkillManager skillManager;
    
    public void Start()
    {
        this.GetComponent<Button>().onClick.AddListener(Upgrade);
    }
    public void Upgrade()
    {
        if(skillManager.skillpoints >= skill.cost && skill.lvl < skill.lvlCap)
        {
            skillManager.skillpoints = skillManager.skillpoints - skill.cost;
            skillManager.OnPointsChange();
            skill.lvl++;
            /*
            if (skill.lvl % 25 == 0 && skill.lvl != 99)
            {
                skill.cost = (int)Mathf.Ceil((float)(skill.cost + (skill.cost * 0.2)));
            }
            else if (skill.lvl == 99)
            {
                skill.cost = (int)Mathf.Ceil((float)(skill.cost + (skill.cost * 1.5)));
            }*/
            skill.skillProgress.TryPassSkillProgress(skill.lvl);
            skill.SetupUi();
        }
    }

}
