[System.Serializable]
public class SearchData
{
    public bool isGrowing;
    public bool isWaiting;
    public int lvl;
    public float progressValue;
    public int seedChance;
    public int giveAmount;
    public float curLimitTime;
    public float limitTime;

    public SearchData(Searching searching)
    {
        isGrowing = searching.isGrowing;
        isWaiting = searching.isWaiting;

        lvl = searching.lvl;

        if(searching.progressValue == 0)
        {
            progressValue = 0;
        }
        else
        {
            progressValue = searching.progressBar.value;
        }
        
        seedChance = searching.seedChance;
        giveAmount = searching.giveAmount;

        curLimitTime = searching.curLimitTime;
        limitTime = searching.limitTime;
    }
}
