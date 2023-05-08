[System.Serializable]
public class ItemData
{
    public int itemId;
    public int lvl;

    public ItemData(int id, int lvl)
    {
        this.itemId = id;
        this.lvl = lvl;
    }
}
