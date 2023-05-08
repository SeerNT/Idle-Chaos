[System.Serializable]
public class TalantData 
{
    public bool isBought;
    public bool isActivated;

    public int talantPoints;

    public TalantData(bool isBought, bool isActivated, int points)
    {
        this.isBought = isBought;
        this.isActivated = isActivated;

        this.talantPoints = points;
    }
}
