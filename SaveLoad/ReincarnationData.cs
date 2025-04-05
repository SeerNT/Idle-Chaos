[System.Serializable]
public class ReincarnationData
{
    public double totalPlayTime;
    public double totalEarnMagic;
    public double totalEarnXp;
    public double totalKilledEnemies;
    public double[] permanentsUpgrades = new double[8];

    public ReincarnationData(double playTime, double earnMagic, double earnXp, double killed, double[] permanentsUpgrades)
    {
        this.totalPlayTime = playTime;
        this.totalEarnMagic = earnMagic;
        this.totalEarnXp = earnXp;
        this.totalKilledEnemies = killed;
        this.permanentsUpgrades = permanentsUpgrades;
    }
}
