[System.Serializable]
public class BattleData
{
    public int enemyN;
    public bool isBattle;
    public bool isRegenerate;

    public BattleData(int enemyN, bool isBattle, bool isReg)
    {
        this.enemyN = enemyN;
        this.isBattle = isBattle;
        this.isRegenerate = isReg;
    }
}
