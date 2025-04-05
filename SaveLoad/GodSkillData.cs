[System.Serializable]
public class GodSkillData
{
    public int abilityId;
    public float cooldown;
    public float currentTime;
    public bool isCooldown;

    public GodSkillData(int id, float cooldown, float curTime, bool isCooldown)
    {
        this.abilityId = id;
        this.cooldown = cooldown;
        this.isCooldown = isCooldown;
        this.currentTime = curTime;
    }
}
