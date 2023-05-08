[System.Serializable]
public class PermanentUpgradeData
{
    public int amount;
    public int bought;
    public double cost;
    public int baseCost;

    public PermanentUpgradeData(int amount, int bought, double cost, int baseCost)
    {
        this.amount = amount;
        this.bought = bought;
        this.cost = cost;
        this.baseCost = baseCost;
    }
}

