[System.Serializable]
public class DailyRewardData
{
    public int day;
    public int value;
    public bool isCollected;
    public int dayRow;
    public double time;

    public DailyRewardData(int day, int value, bool isCollected, int dayRow, double time)
    {
        this.day = day;
        this.value = value;
        this.isCollected = isCollected;
        this.dayRow = dayRow;
        this.time = time;
    }
}
