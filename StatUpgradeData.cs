[System.Serializable]
public class StatUpgradeData 
{
    public bool isUpgrading;
    public bool isLocked;
    public bool readyForUpdate;

    public int gainLvl;
    public int timeLvl;
    public int costLvl;
    public int upgradeLvl;

    public int locks;
    public int locksLimit;

    public float progressValue;

    public StatUpgradeData(StatUpgrade statObject)
    {
        this.progressValue = statObject.progressValue;

        this.isUpgrading = statObject.isUpgrading;
        this.isLocked = statObject.isLocked;
        this.readyForUpdate = statObject.readyForUpdate;

        this.gainLvl = statObject.gainLvl;
        this.timeLvl = statObject.timeLvl;
        this.costLvl = statObject.costLvl;
        this.upgradeLvl = statObject.upgradeLvl;

        this.locks = StatUpgrade.locks;
        this.locksLimit = StatUpgrade.locksLimit;
    }
}
