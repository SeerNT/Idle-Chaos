[System.Serializable]
public class PlayerData
{
    public float hp;
    public float maxHp;

    public float mana;
    public float maxMana;

    public float manaRegSpeed;
    public float hpRegSpeed;

    public float magicPower;
    public float physicPower;

    public float armor;
    public float magicArmor;
    public float pierce;
    public float luck;
    public float critDmg;
    public float attackSpeed;
    public float chaosFactor;

    public int lvl;
    public int xp;
    public int xpCap;

    public float percentHp;
    public float percentMana;
    public float percentXp;

    public float stunChance;
    public float stunDuration;

    public float pureDmg;

    public float automationSpeed;

    public float abstractARM;
    public float abstractMARM;
    public float abstractAS;
    public float abstractLCK;
    public float abstractPRC;

    public PlayerData(Player player)
    {
        this.maxHp = player.maxHp;
        this.hp = player.hp;
        this.mana = player.mana;
        this.maxMana = player.maxMana;
        this.manaRegSpeed = player.manaRegSpeed;
        this.hpRegSpeed = player.hpRegSpeed;
        this.magicPower = player.magicPower;
        this.physicPower = player.physicPower;
        this.armor = player.armor;
        this.magicArmor = player.magicArmor;
        this.pierce = player.pierce;
        this.luck = player.luck;
        this.critDmg = player.critDmg;
        this.attackSpeed = player.attackSpeed;
        this.chaosFactor = player.chaosFactor;

        this.lvl = player.lvl;
        this.xp = player.xp;
        this.xpCap = player.xpCap;

        this.percentHp = player.percentHp;
        this.percentMana = player.percentMana;
        this.percentXp = player.percentXp;

        this.stunChance = player.stunChance;
        this.stunDuration = player.stunDur;

        this.pureDmg = player.pureDmg;
        this.automationSpeed = TalantManager.automationSpeed;

        this.abstractARM = player.abstractARM;
        this.abstractMARM = player.abstractMARM;
        this.abstractAS = player.abstractAS;
        this.abstractLCK = player.abstractLCK;
        this.abstractPRC = player.abstractPRC;
    }
}
