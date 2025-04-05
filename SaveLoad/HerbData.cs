[System.Serializable]
public class HerbData
{
    public int herbId;
    public float currentActiveTime;
    public bool isActive;
    public double amount;

    public HerbData(int id, float time, bool isActive, double amount)
    {
        this.herbId = id;
        this.currentActiveTime = time;
        this.isActive = isActive;
        this.amount = amount;
    }
}
