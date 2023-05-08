[System.Serializable]
public class SkillData 
{
    public int abilityId;
    public float cooldown;
    public float currentTime;
    public bool isCooldown;

    public SkillData(int id, float cooldown, float curTime, bool isCooldown)
    {
        this.abilityId = id;
        this.cooldown = cooldown;
        this.isCooldown = isCooldown;
        this.currentTime = curTime;
    }
}
