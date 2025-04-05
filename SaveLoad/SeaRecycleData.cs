[System.Serializable]

public class SeaRecycleData
{
    public int common;
    public int uncommon;
    public int rare;
    public int epic;
    public int legendary;
    public int mythic;

    public SeaRecycleData(int common, int uncommon, int rare, int epic, int legendary, int mythic)
    {
        this.common = common;
        this.uncommon = uncommon;
        this.rare = rare;
        this.epic = epic;
        this.legendary = legendary;
        this.mythic = mythic;
    }
}
