[System.Serializable]

public class SeaFishingData
{
    public int itemDropSetting;
    public int rarityDropSetting;
    public int autoLvlSetting;
    public int id;
    public int lvl;

    public SeaFishingData(int itemDropSetting, int rarityDropSetting, int autoLvlSetting, int id, int lvl)
    {
        this.itemDropSetting = itemDropSetting;
        this.rarityDropSetting = rarityDropSetting;
        this.autoLvlSetting = autoLvlSetting;
        this.id = id;
        this.lvl = lvl;
    }

    public SeaFishingData(int itemDropSetting, int rarityDropSetting, int autoLvlSetting)
    {
        this.itemDropSetting = itemDropSetting;
        this.rarityDropSetting = rarityDropSetting;
        this.autoLvlSetting = autoLvlSetting;
    }
}
