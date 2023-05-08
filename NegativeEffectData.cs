[System.Serializable]

public class NegativeEffectData 
{
    public float effectTime;
    public float effectCurrentDuration;
    public bool isEffected;

    public int effectId;

    public NegativeEffectData(float effectTime, float effectCurrentDuration, bool isEffected, int effectId)
    {
        this.effectTime = effectTime;
        this.effectCurrentDuration = effectCurrentDuration;
        this.isEffected = isEffected;
        this.effectId = effectId;
    }
}
