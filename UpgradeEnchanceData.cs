[System.Serializable]
public class UpgradeEnchanceData
{
    public int lvl;
    public bool isActivated;

    public UpgradeEnchanceData(int lvl, bool isActivated)
    {
        this.lvl = lvl;
        this.isActivated = isActivated;
    }
}
