[System.Serializable]
public class CurrencyData
{
    public double magic;
    public double xp;
    public double gold;
    public double blood;
    public double qi;
    public double meta;

    public CurrencyData(double magic, double xp, double gold, double blood, double qi, double meta)
    {
        this.magic = magic;
        this.xp = xp;
        this.gold = gold;
        this.blood = blood;
        this.qi = qi;
        this.meta = meta;
    }
}
