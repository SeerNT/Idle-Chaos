[System.Serializable]
public class SkinData 
{
    public int currentSkinNum;
    public int lastId;

    public SkinData(int currentSkinNum, int lastId)
    {
        this.currentSkinNum = currentSkinNum;
        this.lastId = lastId;
    }
}
