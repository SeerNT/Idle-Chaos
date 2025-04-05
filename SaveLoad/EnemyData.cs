[System.Serializable]
public class EnemyData
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

    public EnemyData(Enemy enemy)
    {
        this.maxHp = enemy.maxHp;
        this.hp = enemy.hp;
        this.mana = enemy.mana;
        this.maxMana = enemy.maxMana;
        this.manaRegSpeed = enemy.manaRegSpeed;
        this.hpRegSpeed = enemy.hpRegSpeed;
        this.magicPower = enemy.magicPower;
        this.physicPower = enemy.physicPower;
        this.armor = enemy.armor;
        this.magicArmor = enemy.magicArmor;
        this.pierce = enemy.pierce;
        this.luck = enemy.luck;
        this.critDmg = enemy.critDmg;
        this.attackSpeed = enemy.attackSpeed;

        this.lvl = enemy.lvl;

        this.percentHp = enemy.percentHp;
        this.percentMana = enemy.percentMana;

        this.stunChance = enemy.stunChance;
        this.stunDuration = enemy.stunDur;
    }
}
