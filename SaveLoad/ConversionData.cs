[System.Serializable]
public class ConversionData
{
    public bool isUnlocked;
    public bool isSale;

    public ConversionData(bool isUnlocked, bool isSale)
    {
        this.isUnlocked = isUnlocked;
        this.isSale = isSale;
    }
}
