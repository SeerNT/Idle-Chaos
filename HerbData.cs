[System.Serializable]
public class HerbData
{
    public int herbId;
    public float currentActiveTime;
    public bool isActive;
    public int amount;

    public HerbData(int id, float time, bool isActive, int amount)
    {
        this.herbId = id;
        this.currentActiveTime = time;
        this.isActive = isActive;
        this.amount = amount;
    }
}
