[System.Serializable]
public class TrainData
{
    public bool isTraining;
    public bool isWaiting;
    public int lvl;
    public int bought;
    public float progressValue;
    public double cost;
    public int baseCost;
    public float curLimitTime;
    public float limitTime;

    public bool isAuto;
    public bool automationOn;

    public TrainData(Train training)
    {
        isTraining = training.isTraining;
        isWaiting = training.isWaiting;

        lvl = training.lvl;
        bought = training.bought;

        if(training.progressValue == 0)
        {
            progressValue = 0;
        }
        else
        {
            progressValue = training.progressBar.value;
        }
        
        cost = training.cost;
        baseCost = training.baseCost;

        curLimitTime = training.curLimitTime;
        limitTime = training.limitTime;

        isAuto = training.isAuto;
        automationOn = training.automationOn;
    }
}
