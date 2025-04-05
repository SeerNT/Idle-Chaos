[System.Serializable]
public class SeaBattleData
{
    public double xpDrop;
    public double remainingTime;
    public double timeSpent;
    public int enemyNum;

    public SeaBattleData(double xpDrop, double remainingTime, double timeSpent, int enemyNum)
    {
        this.xpDrop = xpDrop;
        this.remainingTime = remainingTime;
        this.timeSpent = timeSpent;
        this.enemyNum = enemyNum;
    }
}
