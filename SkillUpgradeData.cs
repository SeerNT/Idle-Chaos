[System.Serializable]
public class SkillUpgradeData 
{
    public int skillpoints;
    public int lvl;
    public int skillCost;
    public bool isActivated;

    public SkillUpgradeData(Skill skill, int skillpoints)
    {
        this.skillpoints = skillpoints;

        this.isActivated = skill.isActivated;
        this.lvl = skill.lvl;
        this.skillCost = skill.cost;
    }
}
