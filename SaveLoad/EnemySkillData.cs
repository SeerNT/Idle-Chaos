[System.Serializable]
public class EnemySkillData
{
    public int abilityId;
    public float cooldown;
    public float currentTime;
    public bool isCooldown;

    public EnemySkillData(int id, float cooldown, float curTime, bool isCooldown)
    {
        this.abilityId = id;
        this.cooldown = cooldown;
        this.isCooldown = isCooldown;
        this.currentTime = curTime;
    }
}
