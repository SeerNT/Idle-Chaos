[System.Serializable]
public class TechniqueData
{
    public int lvl;
    public int gainLvl;
    public int timeLvl;
    public bool isTraining;
    public bool isCooldown;
    public float progressValue;
    public float currentTime;

    public TechniqueData(Technique tech)
    {
        gainLvl = tech.gainLvl;
        timeLvl = tech.timeLvl;

        lvl = tech.lvl;

        isTraining = tech.isTraining;
        isCooldown = tech.isCooldown;

        progressValue = tech.progressValue;
        currentTime = tech.currentTime;
    }
}
