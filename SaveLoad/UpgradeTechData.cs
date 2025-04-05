[System.Serializable]
public class UpgradeTechData
{
    public int level = 0;
    public bool isActivated = false;

    public UpgradeTechData(TechniqueUpgrade tech)
    {
        level = tech.level;
        isActivated = tech.isActivated;
    }
}
