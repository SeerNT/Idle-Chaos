﻿[System.Serializable]
public class GrowData
{
    public bool isGrowing;
    public bool isWaiting;
    public int progress;
    public float progressValue;
    public int seedAmount;
    public int baseCost;
    public float curLimitTime;
    public float limitTime;

    public GrowData(Growing growing)
    {
        isGrowing = growing.isGrowing;
        isWaiting = growing.isWaiting;

        progress = growing.progress;

        if(growing.progressValue == 0)
        {
            progressValue = 0;
        }
        else
        {
            progressValue = growing.progressBar.value;
        }

        seedAmount = growing.seedAmount;
        baseCost = growing.baseCost;

        curLimitTime = growing.curLimitTime;
        limitTime = growing.limitTime;
    }
}
