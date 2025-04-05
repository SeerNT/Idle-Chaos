using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public enum ImmuneType
    {
        MAGIC,
        PHYSICAL
    }
    /// <summary>
    /// This property defines current health
    /// </summary>
    public float hp = 100f;
    /// <summary>
    /// This property defines max health possible
    /// </summary>
    public float maxHp = 100f;

    public float mana = 10f;
    public float maxMana = 10f;

    public float manaRegSpeed = 1000f;
    public float hpRegSpeed = 1000f;

    public float magicPower = 1f;
    public float physicPower = 1f;

    /// <summary>
    ///     <para>
    ///         This property reduces physical damage taken 
    ///     </para>
    ///     <para>
    ///         10 dmg taken and 50 armor => 5 dmg
    ///     </para>
    /// </summary>
    public float armor = 0f;
    /// <summary>
    ///     <para>
    ///         This property reduces magic damage taken 
    ///     </para>
    ///     <para>
    ///         10 dmg taken and 50 armor => 5 dmg
    ///     </para>
    /// </summary>
    public float magicArmor = 0f;

    /// <summary>
    ///     <para>
    ///         This property reduces armor damage reduction 
    ///     </para>
    ///     <para>
    ///         10 dmg dealt and 50 armor, but 0.3 pierce => 6 dmg
    ///     </para>
    ///     <para>
    ///         armor - (armor / (1/(armor - pierce)))
    ///     </para>
    /// </summary>
    public float pierce = 0f;
    public float luck = 0f;
    public float critDmg = 0.5f;
    /// <summary>
    ///     <para>
    ///         This property define how many seconds it takes to attack
    ///     </para>
    ///     <para>
    ///         100 attackSpeed => 1 attack per second
    ///     </para>
    ///     <para>
    ///         150 attackSpeed => 1 attack per 1.5 seconds
    ///     </para>
    /// </summary>
    public float attackSpeed = 100f;

    public float chaosFactor = 0f;

    public float vampirismChance = 0f;

    public int lvl = 1;
    public int xp = 0;
    public int xpCap = 100;

    public float percentHp = 1f;
    public float percentMana = 1f;
    public float percentXp = 1f;

    public float stunChance = 0f; 
    public float stunDur = 1f;

    private GameItem weapon;

    public NegativeEffect[] negativeEffects = new NegativeEffect[NegativeEffect.effectIds.Count];

    public float weakness = 0f;

    public bool isRegenerating = false;

    public float pureDmg = 0;

    public float abstractARM;
    public float abstractMARM;
    public float abstractAS;
    public float abstractLCK;
    public float abstractPRC;

    public Player(float mana, float hp, float mp, float pp, float arm, float mgarm, float prc, float lck, float cdmg, float atks, float cf) {
        this.maxMana = mana;
        this.mana = mana;
        this.maxHp = hp;
        this.hp = hp;
        //this.maxHp = 15000;
       // this.hp = 15000;
        this.magicPower = mp;
        this.physicPower = pp;
        //this.physicPower = 100000;
        this.armor = arm;
        this.magicArmor = mgarm;
        this.pierce = prc;
        this.luck = lck;
        this.critDmg = cdmg;
        this.attackSpeed = atks;
        this.chaosFactor = cf;
        this.stunChance = 0f;
        this.stunDur = 1f;
    }

    public void CheckStatLimit()
    {
        if(this.hp > this.maxHp)
        {
            this.hp = this.maxHp;
        }
        if (this.armor > 100)
        {
            this.armor = 100;
        }
        if (this.magicArmor > 100)
        {
            this.magicArmor = 100;
        }
        if(this.attackSpeed <= 0)
        {
            this.attackSpeed = 1;
        }
        if (this.luck > 1)
        {
            this.luck = 1;
        }
        if(this.pierce > 1)
        {
            this.pierce = 1;
        }
        if(this.manaRegSpeed <= 0)
        {
            this.manaRegSpeed = 200;
        }
        if (this.hpRegSpeed <= 0)
        {
            this.hpRegSpeed = 200;
        }
        if (this.manaRegSpeed > 1000)
        {
            this.manaRegSpeed = 1000;
        }
        if (this.hpRegSpeed > 1000)
        {
            this.hpRegSpeed = 1000;
        }
    }

    public void GrantImmune(ImmuneType type)
    {
        if(type == ImmuneType.MAGIC)
        {
            this.magicArmor = 100;
        }
        else if(type == ImmuneType.PHYSICAL)
        {
            this.armor = 100;
        }
    }

    public void DisableImmune(ImmuneType type, float stat)
    {
        if (type == ImmuneType.MAGIC)
        {
            this.magicArmor = stat;
        }
        else if (type == ImmuneType.PHYSICAL)
        {
            this.armor = stat;
        }
    }

    public void EquipItem(GameItem item, int lvl) {
        weapon = item;
        float lvlAffection = (lvl - 1) / 100.0f;
        if (Reincarnation.permanentsUpgrades[0] > 0 || Reincarnation.permanentsUpgrades[6] > 0)
        {
            if(item.bonusPP >= 0)
                this.physicPower = (float)((this.physicPower + (item.bonusPP + (item.bonusPP * lvlAffection)) * (Math.Pow(2, Reincarnation.permanentsUpgrades[0] / 100 + Reincarnation.permanentsUpgrades[6] / 100))));
            else
                this.physicPower = (float)((this.physicPower + (item.bonusPP - (item.bonusPP * lvlAffection)) * (Math.Pow(2, Reincarnation.permanentsUpgrades[0] / 100 + Reincarnation.permanentsUpgrades[6] / 100))));
        }
        else
        {
            if (item.bonusPP != 0)
            {
                if (item.bonusPP >= 0)
                    this.physicPower = this.physicPower + (item.bonusPP + (item.bonusPP * lvlAffection));
                else
                    this.physicPower = this.physicPower + (item.bonusPP - (item.bonusPP * lvlAffection));
            }
        }

        if (Reincarnation.permanentsUpgrades[1] > 0 || Reincarnation.permanentsUpgrades[7] > 0)
        {
            if(item.bonusHP >= 0)
                this.maxHp = (float)((this.maxHp + (item.bonusHP + (item.bonusHP * lvlAffection)) * (Math.Pow(2, Reincarnation.permanentsUpgrades[1] / 100 + Reincarnation.permanentsUpgrades[7] / 100))));
            else
                this.maxHp = (float)((this.maxHp + (item.bonusHP - (item.bonusHP * lvlAffection)) * (Math.Pow(2, Reincarnation.permanentsUpgrades[1] / 100 + Reincarnation.permanentsUpgrades[7] / 100))));
        }
        else
        {
            if (item.bonusHP >= 0)
                this.maxHp = this.maxHp + (item.bonusHP + (item.bonusHP * lvlAffection));
            else
                this.maxHp = this.maxHp + (item.bonusHP - (item.bonusHP * lvlAffection));
        }

        if (Reincarnation.permanentsUpgrades[2] > 0)
        {
            if(item.bonusMP >= 0)
                this.magicPower = (float)((this.magicPower + (item.bonusMP + (item.bonusMP * lvlAffection)) * (Math.Pow(2, Reincarnation.permanentsUpgrades[2] / 100))));
            else
                this.magicPower = (float)((this.magicPower + (item.bonusMP - (item.bonusMP * lvlAffection)) * (Math.Pow(2, Reincarnation.permanentsUpgrades[2] / 100))));
        }
        else
        {
            if (item.bonusMP >= 0)
                this.magicPower = this.magicPower + (item.bonusMP + (item.bonusMP * lvlAffection));
            else
                this.magicPower = this.magicPower + (item.bonusMP - (item.bonusMP * lvlAffection));
        }

        if (Reincarnation.permanentsUpgrades[3] > 0)
        {
            if(item.bonusArm >= 0)
            {
                if(this.abstractARM == 0)
                    this.abstractARM = (float)((this.armor + (item.bonusArm + (item.bonusArm * lvlAffection)) * (Math.Pow(1.8f, Reincarnation.permanentsUpgrades[3] / 100))));
                else
                    this.abstractARM = (float)((this.abstractARM + (item.bonusArm + (item.bonusArm * lvlAffection)) * (Math.Pow(1.8f, Reincarnation.permanentsUpgrades[3] / 100))));
                this.armor = (float)((this.armor + (item.bonusArm + (item.bonusArm * lvlAffection)) * (Math.Pow(1.8f, Reincarnation.permanentsUpgrades[3] / 100))));
            }
            else
            {
                if(this.abstractARM == 0)
                    this.abstractARM = (float)((this.armor + (item.bonusArm - (item.bonusArm * lvlAffection)) * (Math.Pow(1.8f, Reincarnation.permanentsUpgrades[3] / 100))));
                else
                    this.abstractARM = (float)((this.abstractARM + (item.bonusArm - (item.bonusArm * lvlAffection)) * (Math.Pow(1.8f, Reincarnation.permanentsUpgrades[3] / 100))));
                this.armor = (float)((this.armor + (item.bonusArm - (item.bonusArm * lvlAffection)) * (Math.Pow(1.8f, Reincarnation.permanentsUpgrades[3] / 100))));
            }
        }
        else
        {
            if (item.bonusArm >= 0)
            {
                if(this.abstractARM == 0)
                    this.abstractARM = this.armor + (item.bonusArm + (item.bonusArm * lvlAffection));
                else
                    this.abstractARM = this.abstractARM + (item.bonusArm + (item.bonusArm * lvlAffection));
                this.armor = this.armor + (item.bonusArm + (item.bonusArm * lvlAffection));
            }
            else
            {
                if(this.abstractARM == 0)
                    this.abstractARM = this.armor + (item.bonusArm - (item.bonusArm * lvlAffection));
                else
                    this.abstractARM = this.abstractARM + (item.bonusArm - (item.bonusArm * lvlAffection));
                this.armor = this.armor + (item.bonusArm - (item.bonusArm * lvlAffection));
            }    
        }

        if (Reincarnation.permanentsUpgrades[4] > 0)
        {
            if(item.bonusPrc >= 0)
            {
                if(this.abstractPRC == 0)
                    this.abstractPRC = (float)((this.pierce + (item.bonusPrc + (item.bonusPrc * lvlAffection)) * (Math.Pow(1.5f, Reincarnation.permanentsUpgrades[4] / 100))));
                else
                    this.abstractPRC = (float)((this.abstractPRC + (item.bonusPrc + (item.bonusPrc * lvlAffection)) * (Math.Pow(1.5f, Reincarnation.permanentsUpgrades[4] / 100))));
                this.pierce = (float)((this.pierce + (item.bonusPrc + (item.bonusPrc * lvlAffection)) * (Math.Pow(1.5f, Reincarnation.permanentsUpgrades[4] / 100))));
            }
            else
            {
                if(this.abstractPRC == 0)
                    this.abstractPRC = (float)((this.pierce + (item.bonusPrc - (item.bonusPrc * lvlAffection)) * (Math.Pow(1.5f, Reincarnation.permanentsUpgrades[4] / 100))));
                else
                    this.abstractPRC = (float)((this.abstractPRC + (item.bonusPrc - (item.bonusPrc * lvlAffection)) * (Math.Pow(1.5f, Reincarnation.permanentsUpgrades[4] / 100))));
                this.pierce = (float)((this.pierce + (item.bonusPrc - (item.bonusPrc * lvlAffection)) * (Math.Pow(1.5f, Reincarnation.permanentsUpgrades[4] / 100))));
            } 
        }
        else
        {
            if (item.bonusPrc >= 0)
            {
                if(this.abstractPRC == 0)
                    this.abstractPRC = this.pierce + (item.bonusPrc + (item.bonusPrc * lvlAffection));
                else
                    this.abstractPRC = this.abstractPRC + (item.bonusPrc + (item.bonusPrc * lvlAffection));
                this.pierce = this.pierce + (item.bonusPrc + (item.bonusPrc * lvlAffection));
            }
            else
            {
                if (this.abstractPRC == 0)
                    this.abstractPRC = this.pierce + (item.bonusPrc -(item.bonusPrc * lvlAffection));
                else
                    this.abstractPRC = this.abstractPRC + (item.bonusPrc - (item.bonusPrc * lvlAffection));
                this.pierce = this.pierce + (item.bonusPrc - (item.bonusPrc * lvlAffection));
            } 
        }

        if (Reincarnation.permanentsUpgrades[5] > 0)
        {
            if(item.bonusMgcArm >= 0)
            {
                if(this.abstractMARM == 0)
                    this.abstractMARM = (float)Math.Round(this.magicArmor + (item.bonusMgcArm + (item.bonusMgcArm * lvlAffection)) * (Math.Pow(1.6f, Reincarnation.permanentsUpgrades[5] / 100)), 2);
                else
                    this.abstractMARM = (float)Math.Round(this.abstractMARM + (item.bonusMgcArm + (item.bonusMgcArm * lvlAffection)) * (Math.Pow(1.6f, Reincarnation.permanentsUpgrades[5] / 100)), 2);
                this.magicArmor = (float)Math.Round(this.magicArmor + (item.bonusMgcArm + (item.bonusMgcArm * lvlAffection)) * (Math.Pow(1.6f, Reincarnation.permanentsUpgrades[5] / 100)), 2);
            }
            else
            {
                if(this.abstractMARM == 0)
                    this.abstractMARM = (float)Math.Round(this.magicArmor + (item.bonusMgcArm - (item.bonusMgcArm * lvlAffection)) * (Math.Pow(1.6f, Reincarnation.permanentsUpgrades[5] / 100)), 2);
                else
                    this.abstractMARM = (float)Math.Round(this.abstractMARM + (item.bonusMgcArm - (item.bonusMgcArm * lvlAffection)) * (Math.Pow(1.6f, Reincarnation.permanentsUpgrades[5] / 100)), 2);
                this.magicArmor = (float)Math.Round(this.magicArmor + (item.bonusMgcArm - (item.bonusMgcArm * lvlAffection)) * (Math.Pow(1.6f, Reincarnation.permanentsUpgrades[5] / 100)), 2);
            } 
        }
        else
        {
            if (item.bonusMgcArm >= 0)
            {
                if(this.abstractMARM == 0)
                    this.abstractMARM = (float)Math.Round(this.magicArmor + (item.bonusMgcArm + (item.bonusPrc * lvlAffection)), 2);
                else
                    this.abstractMARM = (float)Math.Round(this.abstractMARM + (item.bonusMgcArm + (item.bonusPrc * lvlAffection)), 2);
                this.magicArmor = (float)Math.Round(this.magicArmor + (item.bonusMgcArm + (item.bonusPrc * lvlAffection)), 2);
            }
            else
            {
                if(this.abstractMARM == 0)
                    this.abstractMARM = (float)Math.Round(this.magicArmor + (item.bonusMgcArm - (item.bonusPrc * lvlAffection)), 2); 
                else
                    this.abstractMARM = (float)Math.Round(this.abstractMARM + (item.bonusMgcArm - (item.bonusPrc * lvlAffection)), 2);
                this.magicArmor = (float)Math.Round(this.magicArmor + (item.bonusMgcArm - (item.bonusPrc * lvlAffection)), 2); ;
            }    
        }

        if(item.bonusLuck >= 0)
        {
            if(this.abstractLCK == 0)
                this.abstractLCK = (float)Math.Round(this.luck + (item.bonusLuck + (item.bonusLuck * lvlAffection)), 2);
            else
                this.abstractLCK = (float)Math.Round(this.abstractLCK + (item.bonusLuck + (item.bonusLuck * lvlAffection)), 2);
            this.luck = (float)Math.Round(this.luck + (item.bonusLuck + (item.bonusLuck * lvlAffection)), 2);
        }
        else
        {
            if(this.abstractLCK == 0)
                this.abstractLCK = (float)Math.Round(this.luck + (item.bonusLuck + (item.bonusLuck * lvlAffection)), 2);
            else
                this.abstractLCK = (float)Math.Round(this.abstractLCK + (item.bonusLuck + (item.bonusLuck * lvlAffection)), 2);
            this.luck = (float)Math.Round(this.luck + (item.bonusLuck - (item.bonusLuck * lvlAffection)), 2);
        }

        if(item.bonusCrit >= 0)
            this.critDmg = this.critDmg + (item.bonusCrit + (item.bonusCrit * lvlAffection));
        else
            this.critDmg = this.critDmg + (item.bonusCrit - (item.bonusCrit * lvlAffection));

        if(item.bonusAS >= 0)
        {
            if(this.abstractAS == 0)
                this.abstractAS = this.attackSpeed - (item.bonusAS + (item.bonusAS * lvlAffection));
            else
                this.abstractAS = this.abstractAS - (item.bonusAS + (item.bonusAS * lvlAffection));
            this.attackSpeed = this.attackSpeed - (item.bonusAS + (item.bonusAS * lvlAffection));
        }
        else
        {
            if (this.abstractAS == 0)
                this.abstractAS = this.attackSpeed - (item.bonusAS - (item.bonusAS * lvlAffection));
            else
                this.abstractAS = this.abstractAS - (item.bonusAS - (item.bonusAS * lvlAffection));

            if((100 + this.abstractAS) <= 0)
            {
                this.attackSpeed = 1;
            }
            else
                this.attackSpeed = this.attackSpeed - (item.bonusAS - (item.bonusAS * lvlAffection));
        }
 
        if(item.bonusManaReg >= 0)
            this.manaRegSpeed = this.manaRegSpeed - (item.bonusManaReg + (item.bonusManaReg * lvlAffection));
        else
            this.manaRegSpeed = this.manaRegSpeed - (item.bonusManaReg - (item.bonusManaReg * lvlAffection));

        if(item.bonusHpReg >= 0)
            this.hpRegSpeed = this.hpRegSpeed - (item.bonusHpReg + (item.bonusHpReg * lvlAffection));
        else
            this.hpRegSpeed = this.hpRegSpeed - (item.bonusHpReg - (item.bonusHpReg * lvlAffection));
        CheckStatLimit();
    }

    public void UnequipItem(GameItem item, int lvl)
    {
        weapon = item;
        float lvlAffection = (lvl - 1) / 100.0f;
        if (Reincarnation.permanentsUpgrades[0] > 0 || Reincarnation.permanentsUpgrades[6] > 0)
        {
            if(item.bonusPP != 0)
            {
                if (item.bonusPP >= 0)
                    this.physicPower = (float)((this.physicPower - (item.bonusPP + (item.bonusPP * lvlAffection)) * (Math.Pow(2, Reincarnation.permanentsUpgrades[0] / 100 + Reincarnation.permanentsUpgrades[6] / 100))));
                else
                    this.physicPower = (float)((this.physicPower - (item.bonusPP - (item.bonusPP * lvlAffection)) * (Math.Pow(2, Reincarnation.permanentsUpgrades[0] / 100 + Reincarnation.permanentsUpgrades[6] / 100))));
            }
        }
        else
        {
            if (item.bonusPP >= 0)
                this.physicPower = this.physicPower - (item.bonusPP + (item.bonusPP * lvlAffection));
            else
                this.physicPower = this.physicPower - (item.bonusPP - (item.bonusPP * lvlAffection));
        }

        if (Reincarnation.permanentsUpgrades[1] > 0 || Reincarnation.permanentsUpgrades[7] > 0)
        {
            if (item.bonusHP >= 0)
                this.maxHp = (float)((this.maxHp - (item.bonusHP + (item.bonusHP * lvlAffection)) * (Math.Pow(2, Reincarnation.permanentsUpgrades[1] / 100 + Reincarnation.permanentsUpgrades[7] / 100))));
            else
                this.maxHp = (float)((this.maxHp - (item.bonusHP - (item.bonusHP * lvlAffection)) * (Math.Pow(2, Reincarnation.permanentsUpgrades[1] / 100 + Reincarnation.permanentsUpgrades[7] / 100))));
        }
        else
        {
            if (item.bonusHP >= 0)
                this.maxHp = this.maxHp - (item.bonusHP + (item.bonusHP * lvlAffection));
            else
                this.maxHp = this.maxHp - (item.bonusHP - (item.bonusHP * lvlAffection));
        }

        if (Reincarnation.permanentsUpgrades[2] > 0)
        {
            if (item.bonusMP >= 0)
                this.magicPower = (float)((this.magicPower - (item.bonusMP + (item.bonusMP * lvlAffection)) * (Math.Pow(2, Reincarnation.permanentsUpgrades[2] / 100))));
            else
                this.magicPower = (float)((this.magicPower - (item.bonusMP - (item.bonusMP * lvlAffection)) * (Math.Pow(2, Reincarnation.permanentsUpgrades[2] / 100))));
        }
        else
        {
            if (item.bonusMP >= 0)
                this.magicPower = this.magicPower - (item.bonusMP + (item.bonusMP * lvlAffection));
            else
                this.magicPower = this.magicPower - (item.bonusMP - (item.bonusMP * lvlAffection));
        }

        if (Reincarnation.permanentsUpgrades[3] > 0)
        {
            if (item.bonusArm >= 0)
            {
                if(this.abstractARM >= 100)
                {
                    double mod = ((item.bonusArm + (item.bonusArm * lvlAffection)) * Math.Pow(1.8f, Reincarnation.permanentsUpgrades[3] / 100));
                    this.armor = (float)Math.Round(Math.Abs(mod - this.abstractARM), 3);
                }
                else {
                    this.armor = (float)Math.Round(((this.armor - (item.bonusArm + (item.bonusArm * lvlAffection)) * (Math.Pow(1.8f, Reincarnation.permanentsUpgrades[3] / 100)))), 3);
                }
                this.abstractARM = (float)((this.abstractARM - (item.bonusArm + (item.bonusArm * lvlAffection)) * (Math.Pow(1.8f, Reincarnation.permanentsUpgrades[3] / 100))));
            }
            else
            {
                this.armor = (float)((this.armor - (item.bonusArm - (item.bonusArm * lvlAffection)) * (Math.Pow(1.8f, Reincarnation.permanentsUpgrades[3] / 100))));
                this.abstractARM = (float)((this.abstractARM - (item.bonusArm - (item.bonusArm * lvlAffection)) * (Math.Pow(1.8f, Reincarnation.permanentsUpgrades[3] / 100))));
            }
        }
        else
        {
            if (item.bonusArm >= 0)
            {
                if (this.abstractARM >= 100)
                {
                    double mod = ((item.bonusArm + (item.bonusArm * lvlAffection)));
                    this.armor = (float)Math.Round(Math.Abs(mod - this.abstractARM), 3);
                }
                else
                {
                    this.armor = (float)Math.Round(((this.armor - (item.bonusArm + (item.bonusArm * lvlAffection)))), 3);
                }
                this.abstractARM = (float)((this.abstractARM - (item.bonusArm + (item.bonusArm * lvlAffection))));
            }
            else
            {
                this.armor = (float)((this.armor - (item.bonusArm - (item.bonusArm * lvlAffection))));
                this.abstractARM = (float)((this.abstractARM - (item.bonusArm - (item.bonusArm * lvlAffection))));
            }
        }

        if (Reincarnation.permanentsUpgrades[4] > 0)
        {
            if (item.bonusPrc >= 0)
            {
                if (this.abstractPRC >= 1)
                {
                    double mod = ((item.bonusPrc + (item.bonusPrc * lvlAffection)) * Math.Pow(1.5f, Reincarnation.permanentsUpgrades[4] / 100));
                    this.pierce = (float)Math.Round(Math.Abs(mod - this.abstractPRC), 3);
                }
                else {
                    this.pierce = (float)Math.Round(((this.pierce - (item.bonusPrc + (item.bonusPrc * lvlAffection)) * (Math.Pow(1.5f, Reincarnation.permanentsUpgrades[4] / 100)))), 3);
                }
                this.abstractPRC = (float)((this.abstractPRC - (item.bonusPrc + (item.bonusPrc * lvlAffection)) * (Math.Pow(1.5f, Reincarnation.permanentsUpgrades[4] / 100))));
            }
            else
            {
                this.pierce = (float)((this.pierce - (item.bonusPrc - (item.bonusPrc * lvlAffection)) * (Math.Pow(1.5f, Reincarnation.permanentsUpgrades[4] / 100))));
                this.abstractPRC = (float)((this.abstractPRC - (item.bonusPrc - (item.bonusPrc * lvlAffection)) * (Math.Pow(1.5f, Reincarnation.permanentsUpgrades[4] / 100))));

            }
        }
        else
        {
            if (item.bonusPrc >= 0)
            {
                if (this.abstractPRC >= 1)
                {
                    double mod = ((item.bonusPrc + (item.bonusPrc * lvlAffection)));
                    this.pierce = (float)Math.Round(Math.Abs(mod - this.abstractPRC), 3);
                }
                else
                {
                    this.pierce = (float)Math.Round(((this.pierce - (item.bonusPrc + (item.bonusPrc * lvlAffection)))), 3);
                }
                this.abstractPRC = (float)((this.abstractPRC - (item.bonusPrc + (item.bonusPrc * lvlAffection))));
            }
            else
            {
                this.pierce = (float)((this.pierce - (item.bonusPrc - (item.bonusPrc * lvlAffection))));
                this.abstractPRC = (float)((this.abstractPRC - (item.bonusPrc - (item.bonusPrc * lvlAffection))));
            }
        }

        if (Reincarnation.permanentsUpgrades[5] > 0)
        {
            if (item.bonusMgcArm >= 0)
            {
                if (this.abstractMARM >= 100)
                {
                    double mod = ((item.bonusMgcArm + (item.bonusMgcArm * lvlAffection)) * Math.Pow(1.6f, Reincarnation.permanentsUpgrades[5] / 100));
                    this.magicArmor = (float)Math.Round(Math.Abs(mod - this.abstractMARM), 3);
                }
                else
                {
                    this.magicArmor = (float)Math.Round(((this.magicArmor - (item.bonusMgcArm + (item.bonusMgcArm * lvlAffection)) * (Math.Pow(1.6f, Reincarnation.permanentsUpgrades[5] / 100)))), 3);
                }
                this.abstractMARM = (float)((this.abstractMARM - (item.bonusMgcArm + (item.bonusMgcArm * lvlAffection)) * (Math.Pow(1.6f, Reincarnation.permanentsUpgrades[5] / 100))));
            }
            else
            {
                this.magicArmor = (float)((this.magicArmor - (item.bonusMgcArm - (item.bonusMgcArm * lvlAffection)) * (Math.Pow(1.6f, Reincarnation.permanentsUpgrades[5] / 100))));
                this.abstractMARM = (float)((this.abstractMARM - (item.bonusMgcArm - (item.bonusMgcArm * lvlAffection)) * (Math.Pow(1.6f, Reincarnation.permanentsUpgrades[5] / 100))));

            }
        }
        else
        {
            if (item.bonusMgcArm >= 0)
            {
                if (this.abstractMARM >= 100)
                {
                    double mod = ((item.bonusMgcArm + (item.bonusMgcArm * lvlAffection)));
                    this.magicArmor = (float)Math.Round(Math.Abs(mod - this.abstractMARM), 3);
                }
                else
                {
                    this.magicArmor = (float)Math.Round(((this.magicArmor - (item.bonusMgcArm + (item.bonusMgcArm * lvlAffection)))), 3);
                }
                this.abstractMARM = (float)((this.abstractMARM - (item.bonusMgcArm + (item.bonusMgcArm * lvlAffection))));
            }
            else
            {
                this.magicArmor = (float)((this.magicArmor - (item.bonusMgcArm - (item.bonusMgcArm * lvlAffection))));
                this.abstractMARM = (float)((this.abstractMARM - (item.bonusMgcArm - (item.bonusMgcArm * lvlAffection))));
            }
        }

        if (item.bonusLuck >= 0)
        {
            this.luck = (float)Math.Round(Math.Abs((item.bonusLuck + (item.bonusLuck * lvlAffection)) - this.abstractLCK ), 2);
            this.abstractLCK = (float)(this.abstractLCK - (item.bonusLuck + (item.bonusLuck * lvlAffection)));
        }
        else
        {
            this.luck = (float)Math.Round(Math.Abs((item.bonusLuck - (item.bonusLuck * lvlAffection)) - this.abstractLCK), 2);
            this.abstractLCK = (float)(this.abstractLCK - (item.bonusLuck - (item.bonusLuck * lvlAffection)));
        }
        if (item.bonusCrit >= 0)
            this.critDmg = this.critDmg - (item.bonusCrit + (item.bonusCrit * lvlAffection));
        else
            this.critDmg = this.critDmg - (item.bonusCrit - (item.bonusCrit * lvlAffection));
        if (item.bonusAS >= 0)
        {
            this.attackSpeed = (item.bonusAS + (item.bonusAS * lvlAffection)) + this.abstractAS;
            this.abstractAS = this.abstractAS + (item.bonusAS + (item.bonusAS * lvlAffection));
        }
        else
        {
            this.attackSpeed = (item.bonusAS - (item.bonusAS * lvlAffection)) + this.abstractAS;
            this.abstractAS = this.abstractAS + (item.bonusAS - (item.bonusAS * lvlAffection));
        }

        if (item.bonusManaReg >= 0)
            this.manaRegSpeed = this.manaRegSpeed + (item.bonusManaReg + (item.bonusManaReg * lvlAffection));
        else
            this.manaRegSpeed = this.manaRegSpeed + (item.bonusManaReg - (item.bonusManaReg * lvlAffection));
        if (item.bonusHpReg >= 0)
            this.hpRegSpeed = this.hpRegSpeed + (item.bonusHpReg + (item.bonusHpReg * lvlAffection));
        else
            this.hpRegSpeed = this.hpRegSpeed + (item.bonusHpReg - (item.bonusHpReg * lvlAffection));
        CheckStatLimit();
    }
    
    public void UnequipItemPrepared(GameItem item, int lvl)
    {
        weapon = item;
        float lvlAffection = (lvl - 1) / 100.0f;
        if (Reincarnation.permanentsUpgrades[0] > 0 || Reincarnation.permanentsUpgrades[6] > 0)
        {
            if(item.bonusPP != 0)
            {
                if (item.bonusPP >= 0)
                    this.physicPower = (float)((this.physicPower - (item.bonusPP + (item.bonusPP * lvlAffection)) * (Math.Pow(2, (Reincarnation.permanentsUpgrades[0] - 100) / 100 + (Reincarnation.permanentsUpgrades[6] - 100) / 100))));
                else
                    this.physicPower = (float)((this.physicPower - (item.bonusPP - (item.bonusPP * lvlAffection)) * (Math.Pow(2, (Reincarnation.permanentsUpgrades[0] - 100) / 100 + (Reincarnation.permanentsUpgrades[6] - 100) / 100))));
            }
        }
        else
        {
            if (item.bonusPP >= 0)
                this.physicPower = this.physicPower - (item.bonusPP + (item.bonusPP * lvlAffection));
            else
                this.physicPower = this.physicPower - (item.bonusPP - (item.bonusPP * lvlAffection));
        }

        if (Reincarnation.permanentsUpgrades[1] > 0 || Reincarnation.permanentsUpgrades[7] > 0)
        {
            if (item.bonusHP >= 0)
                this.maxHp = (float)((this.maxHp - (item.bonusHP + (item.bonusHP * lvlAffection)) * (Math.Pow(2, (Reincarnation.permanentsUpgrades[1]-100) / 100 + (Reincarnation.permanentsUpgrades[7]-100) / 100))));
            else
                this.maxHp = (float)((this.maxHp - (item.bonusHP - (item.bonusHP * lvlAffection)) * (Math.Pow(2, (Reincarnation.permanentsUpgrades[1]-100) / 100 + (Reincarnation.permanentsUpgrades[7] - 100) / 100))));
        }
        else
        {
            if (item.bonusHP >= 0)
                this.maxHp = this.maxHp - (item.bonusHP + (item.bonusHP * lvlAffection));
            else
                this.maxHp = this.maxHp - (item.bonusHP - (item.bonusHP * lvlAffection));
        }

        if (Reincarnation.permanentsUpgrades[2] > 0)
        {
            if (item.bonusMP >= 0)
                this.magicPower = (float)((this.magicPower - (item.bonusMP + (item.bonusMP * lvlAffection)) * (Math.Pow(2, (Reincarnation.permanentsUpgrades[2]-100) / 100))));
            else
                this.magicPower = (float)((this.magicPower - (item.bonusMP - (item.bonusMP * lvlAffection)) * (Math.Pow(2, (Reincarnation.permanentsUpgrades[2] - 100) / 100))));
        }
        else
        {
            if (item.bonusMP >= 0)
                this.magicPower = this.magicPower - (item.bonusMP + (item.bonusMP * lvlAffection));
            else
                this.magicPower = this.magicPower - (item.bonusMP - (item.bonusMP * lvlAffection));
        }

        if (Reincarnation.permanentsUpgrades[3] > 0)
        {
            if (item.bonusArm >= 0)
            {
                this.armor = (float)Math.Abs(((item.bonusArm + (item.bonusArm * lvlAffection)) * Math.Pow(1.8f, (Reincarnation.permanentsUpgrades[3]-80) / 100) - this.abstractARM));
                this.abstractARM = (float)((this.abstractARM - (item.bonusArm + (item.bonusArm * lvlAffection)) * (Math.Pow(1.8f, (Reincarnation.permanentsUpgrades[3] - 80) / 100))));
            }
            else
            {
                this.armor = (float)(((item.bonusArm - (item.bonusArm * lvlAffection)) * Math.Pow(1.8f, (Reincarnation.permanentsUpgrades[3] - 80) / 100) - this.abstractARM));
                this.abstractARM = (float)((this.abstractARM - (item.bonusArm - (item.bonusArm * lvlAffection)) * (Math.Pow(1.8f, (Reincarnation.permanentsUpgrades[3] - 80) / 100))));
            }
        }
        else
        {
            if (item.bonusArm >= 0)
            {
                this.armor = Math.Abs((item.bonusArm + (item.bonusArm * lvlAffection)) - this.abstractARM);
                this.abstractARM = this.abstractARM - (item.bonusArm + (item.bonusArm * lvlAffection));
            }
            else
            {
                this.armor = Math.Abs((item.bonusArm - (item.bonusArm * lvlAffection)) - this.abstractARM);
                this.abstractARM = this.abstractARM - (item.bonusArm - (item.bonusArm * lvlAffection));
            }
        }

        if (Reincarnation.permanentsUpgrades[4] > 0)
        {
            if (item.bonusPrc >= 0)
            {
                this.pierce = (float)Math.Round(Math.Abs(((item.bonusPrc + (item.bonusPrc * lvlAffection)) * Math.Pow(1.5f, (Reincarnation.permanentsUpgrades[4]-50) / 100) - this.abstractPRC)), 2);
                this.abstractPRC = (float)(this.abstractPRC - (item.bonusPrc + (item.bonusPrc * lvlAffection)) * Math.Pow(1.5f, (Reincarnation.permanentsUpgrades[4] - 50) / 100));
            }
            else
            {
                this.pierce = (float)Math.Round(Math.Abs(((item.bonusPrc - (item.bonusPrc * lvlAffection)) * Math.Pow(1.5f, (Reincarnation.permanentsUpgrades[4] - 50) / 100) - this.abstractPRC)), 2);
                this.abstractPRC = (float)(this.abstractPRC - (item.bonusPrc - (item.bonusPrc * lvlAffection)) * Math.Pow(1.5f, (Reincarnation.permanentsUpgrades[4] - 50) / 100));
            }
        }
        else
        {
            if (item.bonusPrc >= 0)
            {
                this.pierce = (float)Math.Round(Math.Abs((item.bonusPrc + (item.bonusPrc * lvlAffection)) - this.abstractPRC), 2);
                this.abstractPRC = this.abstractPRC - (item.bonusPrc + (item.bonusPrc * lvlAffection));
            }
            else
            {
                this.pierce = (float)Math.Round(Math.Abs((item.bonusPrc - (item.bonusPrc * lvlAffection)) - this.abstractPRC), 2);
                this.abstractPRC = this.abstractPRC - (item.bonusPrc - (item.bonusPrc * lvlAffection));
            }
        }

        if (Reincarnation.permanentsUpgrades[5] > 0)
        {
            if (item.bonusMgcArm >= 0)
            {
                this.magicArmor = (float)Math.Round(Math.Abs((item.bonusMgcArm + (item.bonusMgcArm * lvlAffection)) * Math.Pow(1.6f, (Reincarnation.permanentsUpgrades[5]-60) / 100) - this.abstractMARM), 2);
                this.abstractMARM = (float)(this.abstractMARM - (item.bonusMgcArm + (item.bonusMgcArm * lvlAffection)) * Math.Pow(1.6f, (Reincarnation.permanentsUpgrades[5] - 60) / 100));
            }
            else
            {
                this.magicArmor = (float)Math.Round(Math.Abs((item.bonusMgcArm - (item.bonusMgcArm * lvlAffection)) * Math.Pow(1.6f, (Reincarnation.permanentsUpgrades[5] - 60) / 100) - this.abstractMARM), 2);
                this.abstractMARM = (float)(this.abstractMARM - (item.bonusMgcArm - (item.bonusMgcArm * lvlAffection)) * Math.Pow(1.6f, (Reincarnation.permanentsUpgrades[5] - 60) / 100));
            }
        }
        else
        {
            if (item.bonusMgcArm >= 0)
            {
                this.magicArmor = (float)Math.Round(Math.Abs((item.bonusMgcArm + (item.bonusPrc * lvlAffection)) - this.abstractMARM), 2);
                this.abstractMARM = (float)(this.abstractMARM - (item.bonusMgcArm + (item.bonusPrc * lvlAffection)));
            }
            else
            {
                this.magicArmor = (float)Math.Round(Math.Abs((item.bonusMgcArm - (item.bonusPrc * lvlAffection)) - this.abstractMARM), 2);
                this.abstractMARM = (float)(this.abstractMARM - (item.bonusMgcArm - (item.bonusPrc * lvlAffection)));
            }
        }

        if (item.bonusLuck >= 0)
        {
            this.luck = (float)Math.Round(Math.Abs((item.bonusLuck + (item.bonusLuck * lvlAffection)) - this.abstractLCK), 2);
            this.abstractLCK = (float)(this.abstractLCK - (item.bonusLuck + (item.bonusLuck * lvlAffection)));
        }
        else
        {
            this.luck = (float)Math.Round(Math.Abs((item.bonusLuck - (item.bonusLuck * lvlAffection)) - this.abstractLCK), 2);
            this.abstractLCK = (float)(this.abstractLCK - (item.bonusLuck - (item.bonusLuck * lvlAffection)));
        }
        if (item.bonusCrit >= 0)
            this.critDmg = this.critDmg - (item.bonusCrit + (item.bonusCrit * lvlAffection));
        else
            this.critDmg = this.critDmg - (item.bonusCrit - (item.bonusCrit * lvlAffection));
        if (item.bonusAS >= 0)
        {
            this.attackSpeed = (item.bonusAS + (item.bonusAS * lvlAffection)) + this.abstractAS;
            this.abstractAS = this.abstractAS + (item.bonusAS + (item.bonusAS * lvlAffection));
        }
        else
        {
            this.attackSpeed = (item.bonusAS - (item.bonusAS * lvlAffection)) + this.abstractAS;
            this.abstractAS = this.abstractAS + (item.bonusAS - (item.bonusAS * lvlAffection));
        }

        if (item.bonusManaReg >= 0)
            this.manaRegSpeed = this.manaRegSpeed + (item.bonusManaReg + (item.bonusManaReg * lvlAffection));
        else
            this.manaRegSpeed = this.manaRegSpeed + (item.bonusManaReg - (item.bonusManaReg * lvlAffection));
        if (item.bonusHpReg >= 0)
            this.hpRegSpeed = this.hpRegSpeed + (item.bonusHpReg + (item.bonusHpReg * lvlAffection));
        else
            this.hpRegSpeed = this.hpRegSpeed + (item.bonusHpReg - (item.bonusHpReg * lvlAffection));
        CheckStatLimit();
    }

    public void UseHerb(Herb item)
    {
        item.isActive = true;

        float lvlAffection = 0;
        if (Reincarnation.permanentsUpgrades[0] > 0 || Reincarnation.permanentsUpgrades[6] > 0)
        {
            if (item.bonusPP >= 0)
                this.physicPower = (float)((this.physicPower + (item.bonusPP + (item.bonusPP * lvlAffection)) * (Math.Pow(2, Reincarnation.permanentsUpgrades[0] / 100 + Reincarnation.permanentsUpgrades[6] / 100))));
            else
                this.physicPower = (float)((this.physicPower + (item.bonusPP - (item.bonusPP * lvlAffection)) * (Math.Pow(2, Reincarnation.permanentsUpgrades[0] / 100 + Reincarnation.permanentsUpgrades[6] / 100))));
        }
        else
        {
            if (item.bonusPP != 0)
            {
                if (item.bonusPP >= 0)
                    this.physicPower = this.physicPower + (item.bonusPP + (item.bonusPP * lvlAffection));
                else
                    this.physicPower = this.physicPower + (item.bonusPP - (item.bonusPP * lvlAffection));
            }
        }

        if (Reincarnation.permanentsUpgrades[1] > 0 || Reincarnation.permanentsUpgrades[7] > 0)
        {
            if (item.bonusHP >= 0)
                this.maxHp = (float)((this.maxHp + (item.bonusHP + (item.bonusHP * lvlAffection)) * (Math.Pow(2, Reincarnation.permanentsUpgrades[1] / 100 + Reincarnation.permanentsUpgrades[7] / 100))));
            else
                this.maxHp = (float)((this.maxHp + (item.bonusHP - (item.bonusHP * lvlAffection)) * (Math.Pow(2, Reincarnation.permanentsUpgrades[1] / 100 + Reincarnation.permanentsUpgrades[7] / 100))));
        }
        else
        {
            if (item.bonusHP >= 0)
                this.maxHp = this.maxHp + (item.bonusHP + (item.bonusHP * lvlAffection));
            else
                this.maxHp = this.maxHp + (item.bonusHP - (item.bonusHP * lvlAffection));
        }

        if (Reincarnation.permanentsUpgrades[2] > 0)
        {
            if (item.bonusMP >= 0)
                this.magicPower = (float)((this.magicPower + (item.bonusMP + (item.bonusMP * lvlAffection)) * (Math.Pow(2, Reincarnation.permanentsUpgrades[2] / 100))));
            else
                this.magicPower = (float)((this.magicPower + (item.bonusMP - (item.bonusMP * lvlAffection)) * (Math.Pow(2, Reincarnation.permanentsUpgrades[2] / 100))));
        }
        else
        {
            if (item.bonusMP >= 0)
                this.magicPower = this.magicPower + (item.bonusMP + (item.bonusMP * lvlAffection));
            else
                this.magicPower = this.magicPower + (item.bonusMP - (item.bonusMP * lvlAffection));
        }

        if (Reincarnation.permanentsUpgrades[3] > 0)
        {
            if (item.bonusArm >= 0)
            {
                if (this.abstractARM == 0)
                    this.abstractARM = (float)((this.armor + (item.bonusArm + (item.bonusArm * lvlAffection)) * (Math.Pow(1.8f, Reincarnation.permanentsUpgrades[3] / 100))));
                else
                    this.abstractARM = (float)((this.abstractARM + (item.bonusArm + (item.bonusArm * lvlAffection)) * (Math.Pow(1.8f, Reincarnation.permanentsUpgrades[3] / 100))));
                this.armor = (float)((this.armor + (item.bonusArm + (item.bonusArm * lvlAffection)) * (Math.Pow(1.8f, Reincarnation.permanentsUpgrades[3] / 100))));
            }
            else
            {
                if (this.abstractARM == 0)
                    this.abstractARM = (float)((this.armor + (item.bonusArm - (item.bonusArm * lvlAffection)) * (Math.Pow(1.8f, Reincarnation.permanentsUpgrades[3] / 100))));
                else
                    this.abstractARM = (float)((this.abstractARM + (item.bonusArm - (item.bonusArm * lvlAffection)) * (Math.Pow(1.8f, Reincarnation.permanentsUpgrades[3] / 100))));
                this.armor = (float)((this.armor + (item.bonusArm - (item.bonusArm * lvlAffection)) * (Math.Pow(1.8f, Reincarnation.permanentsUpgrades[3] / 100))));
            }
        }
        else
        {
            if (item.bonusArm >= 0)
            {
                if (this.abstractARM == 0)
                    this.abstractARM = this.armor + (item.bonusArm + (item.bonusArm * lvlAffection));
                else
                    this.abstractARM = this.abstractARM + (item.bonusArm + (item.bonusArm * lvlAffection));
                this.armor = this.armor + (item.bonusArm + (item.bonusArm * lvlAffection));
            }
            else
            {
                if (this.abstractARM == 0)
                    this.abstractARM = this.armor + (item.bonusArm - (item.bonusArm * lvlAffection));
                else
                    this.abstractARM = this.abstractARM + (item.bonusArm - (item.bonusArm * lvlAffection));
                this.armor = this.armor + (item.bonusArm - (item.bonusArm * lvlAffection));
            }
        }

        if (Reincarnation.permanentsUpgrades[4] > 0)
        {
            if (item.bonusPrc >= 0)
            {
                if (this.abstractPRC == 0)
                    this.abstractPRC = (float)((this.pierce + (item.bonusPrc + (item.bonusPrc * lvlAffection)) * (Math.Pow(1.5f, Reincarnation.permanentsUpgrades[4] / 100))));
                else
                    this.abstractPRC = (float)((this.abstractPRC + (item.bonusPrc + (item.bonusPrc * lvlAffection)) * (Math.Pow(1.5f, Reincarnation.permanentsUpgrades[4] / 100))));
                this.pierce = (float)((this.pierce + (item.bonusPrc + (item.bonusPrc * lvlAffection)) * (Math.Pow(1.5f, Reincarnation.permanentsUpgrades[4] / 100))));
            }
            else
            {
                if (this.abstractPRC == 0)
                    this.abstractPRC = (float)((this.pierce + (item.bonusPrc - (item.bonusPrc * lvlAffection)) * (Math.Pow(1.5f, Reincarnation.permanentsUpgrades[4] / 100))));
                else
                    this.abstractPRC = (float)((this.abstractPRC + (item.bonusPrc - (item.bonusPrc * lvlAffection)) * (Math.Pow(1.5f, Reincarnation.permanentsUpgrades[4] / 100))));
                this.pierce = (float)((this.pierce + (item.bonusPrc - (item.bonusPrc * lvlAffection)) * (Math.Pow(1.5f, Reincarnation.permanentsUpgrades[4] / 100))));
            }
        }
        else
        {
            if (item.bonusPrc >= 0)
            {
                if (this.abstractPRC == 0)
                    this.abstractPRC = this.pierce + (item.bonusPrc + (item.bonusPrc * lvlAffection));
                else
                    this.abstractPRC = this.abstractPRC + (item.bonusPrc + (item.bonusPrc * lvlAffection));
                this.pierce = this.pierce + (item.bonusPrc + (item.bonusPrc * lvlAffection));
            }
            else
            {
                if (this.abstractPRC == 0)
                    this.abstractPRC = this.pierce + (item.bonusPrc - (item.bonusPrc * lvlAffection));
                else
                    this.abstractPRC = this.abstractPRC + (item.bonusPrc - (item.bonusPrc * lvlAffection));
                this.pierce = this.pierce + (item.bonusPrc - (item.bonusPrc * lvlAffection));
            }
        }

        if (Reincarnation.permanentsUpgrades[5] > 0)
        {
            if (item.bonusMgcArm >= 0)
            {
                if (this.abstractMARM == 0)
                    this.abstractMARM = (float)Math.Round(this.magicArmor + (item.bonusMgcArm + (item.bonusMgcArm * lvlAffection)) * (Math.Pow(1.6f, Reincarnation.permanentsUpgrades[5] / 100)), 2);
                else
                    this.abstractMARM = (float)Math.Round(this.abstractMARM + (item.bonusMgcArm + (item.bonusMgcArm * lvlAffection)) * (Math.Pow(1.6f, Reincarnation.permanentsUpgrades[5] / 100)), 2);
                this.magicArmor = (float)Math.Round(this.magicArmor + (item.bonusMgcArm + (item.bonusMgcArm * lvlAffection)) * (Math.Pow(1.6f, Reincarnation.permanentsUpgrades[5] / 100)), 2);
            }
            else
            {
                if (this.abstractMARM == 0)
                    this.abstractMARM = (float)Math.Round(this.magicArmor + (item.bonusMgcArm - (item.bonusMgcArm * lvlAffection)) * (Math.Pow(1.6f, Reincarnation.permanentsUpgrades[5] / 100)), 2);
                else
                    this.abstractMARM = (float)Math.Round(this.abstractMARM + (item.bonusMgcArm - (item.bonusMgcArm * lvlAffection)) * (Math.Pow(1.6f, Reincarnation.permanentsUpgrades[5] / 100)), 2);
                this.magicArmor = (float)Math.Round(this.magicArmor + (item.bonusMgcArm - (item.bonusMgcArm * lvlAffection)) * (Math.Pow(1.6f, Reincarnation.permanentsUpgrades[5] / 100)), 2);
            }
        }
        else
        {
            if (item.bonusMgcArm >= 0)
            {
                if (this.abstractMARM == 0)
                    this.abstractMARM = (float)Math.Round(this.magicArmor + (item.bonusMgcArm + (item.bonusPrc * lvlAffection)), 2);
                else
                    this.abstractMARM = (float)Math.Round(this.abstractMARM + (item.bonusMgcArm + (item.bonusPrc * lvlAffection)), 2);
                this.magicArmor = (float)Math.Round(this.magicArmor + (item.bonusMgcArm + (item.bonusPrc * lvlAffection)), 2);
            }
            else
            {
                if (this.abstractMARM == 0)
                    this.abstractMARM = (float)Math.Round(this.magicArmor + (item.bonusMgcArm - (item.bonusPrc * lvlAffection)), 2);
                else
                    this.abstractMARM = (float)Math.Round(this.abstractMARM + (item.bonusMgcArm - (item.bonusPrc * lvlAffection)), 2);
                this.magicArmor = (float)Math.Round(this.magicArmor + (item.bonusMgcArm - (item.bonusPrc * lvlAffection)), 2); ;
            }
        }

        if (item.bonusLuck >= 0)
        {
            if (this.abstractLCK == 0)
                this.abstractLCK = (float)Math.Round(this.luck + (item.bonusLuck + (item.bonusLuck * lvlAffection)), 2);
            else
                this.abstractLCK = (float)Math.Round(this.abstractLCK + (item.bonusLuck + (item.bonusLuck * lvlAffection)), 2);
            this.luck = (float)Math.Round(this.luck + (item.bonusLuck + (item.bonusLuck * lvlAffection)), 2);
        }
        else
        {
            if (this.abstractLCK == 0)
                this.abstractLCK = (float)Math.Round(this.luck + (item.bonusLuck + (item.bonusLuck * lvlAffection)), 2);
            else
                this.abstractLCK = (float)Math.Round(this.abstractLCK + (item.bonusLuck + (item.bonusLuck * lvlAffection)), 2);
            this.luck = (float)Math.Round(this.luck + (item.bonusLuck - (item.bonusLuck * lvlAffection)), 2);
        }

        if (item.bonusCrit >= 0)
            this.critDmg = this.critDmg + (item.bonusCrit + (item.bonusCrit * lvlAffection));
        else
            this.critDmg = this.critDmg + (item.bonusCrit - (item.bonusCrit * lvlAffection));

        if (item.bonusAS >= 0)
        {
            if (this.abstractAS == 0)
                this.abstractAS = this.attackSpeed - (item.bonusAS + (item.bonusAS * lvlAffection));
            else
                this.abstractAS = this.abstractAS - (item.bonusAS + (item.bonusAS * lvlAffection));
            this.attackSpeed = this.attackSpeed - (item.bonusAS + (item.bonusAS * lvlAffection));
        }
        else
        {
            if (this.abstractAS == 0)
                this.abstractAS = this.attackSpeed - (item.bonusAS - (item.bonusAS * lvlAffection));
            else
                this.abstractAS = this.abstractAS - (item.bonusAS - (item.bonusAS * lvlAffection));

            if ((100 + this.abstractAS) <= 0)
            {
                this.attackSpeed = 1;
            }
            else
                this.attackSpeed = this.attackSpeed - (item.bonusAS - (item.bonusAS * lvlAffection));
        }

        if (item.bonusManaReg >= 0)
            this.manaRegSpeed = this.manaRegSpeed - (item.bonusManaReg + (item.bonusManaReg * lvlAffection));
        else
            this.manaRegSpeed = this.manaRegSpeed - (item.bonusManaReg - (item.bonusManaReg * lvlAffection));

        if (item.bonusHpReg >= 0)
            this.hpRegSpeed = this.hpRegSpeed - (item.bonusHpReg + (item.bonusHpReg * lvlAffection));
        else
            this.hpRegSpeed = this.hpRegSpeed - (item.bonusHpReg - (item.bonusHpReg * lvlAffection));

        GameManager.gold += item.bonusGold;

        CheckStatLimit();
    }

    public void DisableHerbPrepared(Herb item)
    {
        item.isActive = false;

        if (Reincarnation.permanentsUpgrades[0] > 0 || Reincarnation.permanentsUpgrades[6] > 0)
        {
            this.physicPower = (float)((this.physicPower - item.bonusPP * (Math.Pow(2, (Reincarnation.permanentsUpgrades[0] - 100) / 100 + (Reincarnation.permanentsUpgrades[6] - 100) / 100))));
        }
        else
        {
            this.physicPower = this.physicPower - item.bonusPP;
        }

        if (Reincarnation.permanentsUpgrades[1] > 0 || Reincarnation.permanentsUpgrades[7] > 0)
        {
            this.maxHp = (float)((this.maxHp - item.bonusHP * (Math.Pow(2, (Reincarnation.permanentsUpgrades[1] - 100) / 100 + (Reincarnation.permanentsUpgrades[7] - 100) / 100))));
        }
        else
        {
            this.maxHp = this.maxHp - item.bonusHP;
        }

        if (Reincarnation.permanentsUpgrades[2] > 0)
        {
            this.magicPower = (float)((this.magicPower - item.bonusMP * (Math.Pow(2, (Reincarnation.permanentsUpgrades[2] - 100) / 100))));
        }
        else
        {
            this.magicPower = this.magicPower - item.bonusMP;
        }

        if (Reincarnation.permanentsUpgrades[3] > 0)
        {
            this.armor = (float)Math.Abs((item.bonusArm * Math.Pow(1.8f, (Reincarnation.permanentsUpgrades[3] - 80) / 100) - this.abstractARM));
            this.abstractARM = (float)((this.abstractARM - item.bonusArm * (Math.Pow(1.8f, (Reincarnation.permanentsUpgrades[3] - 80) / 100))));
        }
        else
        {
            this.armor = Math.Abs(item.bonusArm - this.abstractARM);
            this.abstractARM = this.abstractARM - item.bonusArm ;
        }

        if (Reincarnation.permanentsUpgrades[4] > 0)
        {
            this.pierce = (float)Math.Round(Math.Abs(item.bonusPrc * Math.Pow(1.5f, (Reincarnation.permanentsUpgrades[4] - 50) / 100) - this.abstractPRC), 2);
            this.abstractPRC = (float)(this.abstractPRC - item.bonusPrc * Math.Pow(1.5f, (Reincarnation.permanentsUpgrades[4] - 50) / 100));
        }
        else
        {
            this.pierce = (float)Math.Round(Math.Abs(item.bonusPrc - this.abstractPRC), 2);
            this.abstractPRC = this.abstractPRC - item.bonusPrc;
        }

        if (Reincarnation.permanentsUpgrades[5] > 0)
        {
            this.magicArmor = (float)Math.Round(Math.Abs(item.bonusMgcArm * Math.Pow(1.6f, (Reincarnation.permanentsUpgrades[5] - 60) / 100) - this.abstractMARM), 2);
            this.abstractMARM = (float)(this.abstractMARM - item.bonusMgcArm * Math.Pow(1.6f, (Reincarnation.permanentsUpgrades[5] - 60) / 100));
        }
        else
        {
            this.magicArmor = (float)Math.Round(Math.Abs(item.bonusMgcArm - this.abstractMARM), 2);
            this.abstractMARM = (float)(this.abstractMARM - item.bonusMgcArm);
        }

        this.luck = (float)Math.Round(Math.Abs(item.bonusLuck - this.abstractLCK), 2);
        this.abstractLCK = (float)(this.abstractLCK - item.bonusLuck);

        this.critDmg = this.critDmg - item.bonusCrit;
        this.attackSpeed = item.bonusAS + this.abstractAS;
        this.abstractAS = this.abstractAS + item.bonusAS;
        this.manaRegSpeed = this.manaRegSpeed + item.bonusManaReg;
        this.hpRegSpeed = this.hpRegSpeed + item.bonusHpReg;
        CheckStatLimit();
    }

    public void DisableHerb(Herb item)
    {
        Debug.Log("yep");
        item.isActive = false;

        float lvlAffection = 0f;
        if (Reincarnation.permanentsUpgrades[0] > 0 || Reincarnation.permanentsUpgrades[6] > 0)
        {
            if(item.bonusPP != 0)
            {
                if (item.bonusPP >= 0)
                    this.physicPower = (float)((this.physicPower - (item.bonusPP + (item.bonusPP * lvlAffection)) * (Math.Pow(2, Reincarnation.permanentsUpgrades[0] / 100 + Reincarnation.permanentsUpgrades[6] / 100))));
                else
                    this.physicPower = (float)((this.physicPower - (item.bonusPP - (item.bonusPP * lvlAffection)) * (Math.Pow(2, Reincarnation.permanentsUpgrades[0] / 100 + Reincarnation.permanentsUpgrades[6] / 100))));
            }
        }
        else
        {
            if (item.bonusPP >= 0)
                this.physicPower = this.physicPower - (item.bonusPP + (item.bonusPP * lvlAffection));
            else
                this.physicPower = this.physicPower - (item.bonusPP - (item.bonusPP * lvlAffection));
        }

        if (Reincarnation.permanentsUpgrades[1] > 0 || Reincarnation.permanentsUpgrades[7] > 0)
        {
            if (item.bonusHP >= 0)
                this.maxHp = (float)((this.maxHp - (item.bonusHP + (item.bonusHP * lvlAffection)) * (Math.Pow(2, Reincarnation.permanentsUpgrades[1] / 100 + Reincarnation.permanentsUpgrades[7] / 100))));
            else
                this.maxHp = (float)((this.maxHp - (item.bonusHP - (item.bonusHP * lvlAffection)) * (Math.Pow(2, Reincarnation.permanentsUpgrades[1] / 100 + Reincarnation.permanentsUpgrades[7] / 100))));
        }
        else
        {
            if (item.bonusHP >= 0)
                this.maxHp = this.maxHp - (item.bonusHP + (item.bonusHP * lvlAffection));
            else
                this.maxHp = this.maxHp - (item.bonusHP - (item.bonusHP * lvlAffection));
        }

        if (Reincarnation.permanentsUpgrades[2] > 0)
        {
            if (item.bonusMP >= 0)
                this.magicPower = (float)((this.magicPower - (item.bonusMP + (item.bonusMP * lvlAffection)) * (Math.Pow(2, Reincarnation.permanentsUpgrades[2] / 100))));
            else
                this.magicPower = (float)((this.magicPower - (item.bonusMP - (item.bonusMP * lvlAffection)) * (Math.Pow(2, Reincarnation.permanentsUpgrades[2] / 100))));
        }
        else
        {
            if (item.bonusMP >= 0)
                this.magicPower = this.magicPower - (item.bonusMP + (item.bonusMP * lvlAffection));
            else
                this.magicPower = this.magicPower - (item.bonusMP - (item.bonusMP * lvlAffection));
        }

        if (Reincarnation.permanentsUpgrades[3] > 0)
        {
            if (item.bonusArm >= 0)
            {
                if(this.abstractARM >= 100)
                {
                    double mod = ((item.bonusArm + (item.bonusArm * lvlAffection)) * Math.Pow(1.8f, Reincarnation.permanentsUpgrades[3] / 100));
                    this.armor = (float)Math.Round(Math.Abs(mod - this.abstractARM), 3);
                }
                else {
                    this.armor = (float)Math.Round(((this.armor - (item.bonusArm + (item.bonusArm * lvlAffection)) * (Math.Pow(1.8f, Reincarnation.permanentsUpgrades[3] / 100)))), 3);
                }
                this.abstractARM = (float)((this.abstractARM - (item.bonusArm + (item.bonusArm * lvlAffection)) * (Math.Pow(1.8f, Reincarnation.permanentsUpgrades[3] / 100))));
            }
            else
            {
                this.armor = (float)((this.armor - (item.bonusArm - (item.bonusArm * lvlAffection)) * (Math.Pow(1.8f, Reincarnation.permanentsUpgrades[3] / 100))));
                this.abstractARM = (float)((this.abstractARM - (item.bonusArm - (item.bonusArm * lvlAffection)) * (Math.Pow(1.8f, Reincarnation.permanentsUpgrades[3] / 100))));
            }
        }
        else
        {
            if (item.bonusArm >= 0)
            {
                if (this.abstractARM >= 100)
                {
                    double mod = ((item.bonusArm + (item.bonusArm * lvlAffection)));
                    this.armor = (float)Math.Round(Math.Abs(mod - this.abstractARM), 3);
                }
                else
                {
                    this.armor = (float)Math.Round(((this.armor - (item.bonusArm + (item.bonusArm * lvlAffection)))), 3);
                }
                this.abstractARM = (float)((this.abstractARM - (item.bonusArm + (item.bonusArm * lvlAffection))));
            }
            else
            {
                this.armor = (float)((this.armor - (item.bonusArm - (item.bonusArm * lvlAffection))));
                this.abstractARM = (float)((this.abstractARM - (item.bonusArm - (item.bonusArm * lvlAffection))));
            }
        }

        if (Reincarnation.permanentsUpgrades[4] > 0)
        {
            if (item.bonusPrc >= 0)
            {
                if (this.abstractPRC >= 1)
                {
                    double mod = ((item.bonusPrc + (item.bonusPrc * lvlAffection)) * Math.Pow(1.5f, Reincarnation.permanentsUpgrades[4] / 100));
                    this.pierce = (float)Math.Round(Math.Abs(mod - this.abstractPRC), 3);
                }
                else {
                    this.pierce = (float)Math.Round(((this.pierce - (item.bonusPrc + (item.bonusPrc * lvlAffection)) * (Math.Pow(1.5f, Reincarnation.permanentsUpgrades[4] / 100)))), 3);
                }
                this.abstractPRC = (float)((this.abstractPRC - (item.bonusPrc + (item.bonusPrc * lvlAffection)) * (Math.Pow(1.5f, Reincarnation.permanentsUpgrades[4] / 100))));
            }
            else
            {
                this.pierce = (float)((this.pierce - (item.bonusPrc - (item.bonusPrc * lvlAffection)) * (Math.Pow(1.5f, Reincarnation.permanentsUpgrades[4] / 100))));
                this.abstractPRC = (float)((this.abstractPRC - (item.bonusPrc - (item.bonusPrc * lvlAffection)) * (Math.Pow(1.5f, Reincarnation.permanentsUpgrades[4] / 100))));

            }
        }
        else
        {
            if (item.bonusPrc >= 0)
            {
                if (this.abstractPRC >= 1)
                {
                    double mod = ((item.bonusPrc + (item.bonusPrc * lvlAffection)));
                    this.pierce = (float)Math.Round(Math.Abs(mod - this.abstractPRC), 3);
                }
                else
                {
                    this.pierce = (float)Math.Round(((this.pierce - (item.bonusPrc + (item.bonusPrc * lvlAffection)))), 3);
                }
                this.abstractPRC = (float)((this.abstractPRC - (item.bonusPrc + (item.bonusPrc * lvlAffection))));
            }
            else
            {
                this.pierce = (float)((this.pierce - (item.bonusPrc - (item.bonusPrc * lvlAffection))));
                this.abstractPRC = (float)((this.abstractPRC - (item.bonusPrc - (item.bonusPrc * lvlAffection))));
            }
        }

        if (Reincarnation.permanentsUpgrades[5] > 0)
        {
            if (item.bonusMgcArm >= 0)
            {
                if (this.abstractMARM >= 100)
                {
                    double mod = ((item.bonusMgcArm + (item.bonusMgcArm * lvlAffection)) * Math.Pow(1.6f, Reincarnation.permanentsUpgrades[5] / 100));
                    this.magicArmor = (float)Math.Round(Math.Abs(mod - this.abstractMARM), 3);
                }
                else
                {
                    this.magicArmor = (float)Math.Round(((this.magicArmor - (item.bonusMgcArm + (item.bonusMgcArm * lvlAffection)) * (Math.Pow(1.6f, Reincarnation.permanentsUpgrades[5] / 100)))), 3);
                }
                this.abstractMARM = (float)((this.abstractMARM - (item.bonusMgcArm + (item.bonusMgcArm * lvlAffection)) * (Math.Pow(1.6f, Reincarnation.permanentsUpgrades[5] / 100))));
            }
            else
            {
                this.magicArmor = (float)((this.magicArmor - (item.bonusMgcArm - (item.bonusMgcArm * lvlAffection)) * (Math.Pow(1.6f, Reincarnation.permanentsUpgrades[5] / 100))));
                this.abstractMARM = (float)((this.abstractMARM - (item.bonusMgcArm - (item.bonusMgcArm * lvlAffection)) * (Math.Pow(1.6f, Reincarnation.permanentsUpgrades[5] / 100))));

            }
        }
        else
        {
            if (item.bonusMgcArm >= 0)
            {
                if (this.abstractMARM >= 100)
                {
                    double mod = ((item.bonusMgcArm + (item.bonusMgcArm * lvlAffection)));
                    this.magicArmor = (float)Math.Round(Math.Abs(mod - this.abstractMARM), 3);
                }
                else
                {
                    this.magicArmor = (float)Math.Round(((this.magicArmor - (item.bonusMgcArm + (item.bonusMgcArm * lvlAffection)))), 3);
                }
                this.abstractMARM = (float)((this.abstractMARM - (item.bonusMgcArm + (item.bonusMgcArm * lvlAffection))));
            }
            else
            {
                this.magicArmor = (float)((this.magicArmor - (item.bonusMgcArm - (item.bonusMgcArm * lvlAffection))));
                this.abstractMARM = (float)((this.abstractMARM - (item.bonusMgcArm - (item.bonusMgcArm * lvlAffection))));
            }
        }

        if (item.bonusLuck >= 0)
        {
            this.luck = (float)Math.Round(Math.Abs((item.bonusLuck + (item.bonusLuck * lvlAffection)) - this.abstractLCK ), 2);
            this.abstractLCK = (float)(this.abstractLCK - (item.bonusLuck + (item.bonusLuck * lvlAffection)));
        }
        else
        {
            this.luck = (float)Math.Round(Math.Abs((item.bonusLuck - (item.bonusLuck * lvlAffection)) - this.abstractLCK), 2);
            this.abstractLCK = (float)(this.abstractLCK - (item.bonusLuck - (item.bonusLuck * lvlAffection)));
        }
        if (item.bonusCrit >= 0)
            this.critDmg = this.critDmg - (item.bonusCrit + (item.bonusCrit * lvlAffection));
        else
            this.critDmg = this.critDmg - (item.bonusCrit - (item.bonusCrit * lvlAffection));
        if (item.bonusAS >= 0)
        {
            this.attackSpeed = (item.bonusAS + (item.bonusAS * lvlAffection)) + this.abstractAS;
            this.abstractAS = this.abstractAS + (item.bonusAS + (item.bonusAS * lvlAffection));
        }
        else
        {
            this.attackSpeed = (item.bonusAS - (item.bonusAS * lvlAffection)) + this.abstractAS;
            this.abstractAS = this.abstractAS + (item.bonusAS - (item.bonusAS * lvlAffection));
        }

        if (item.bonusManaReg >= 0)
            this.manaRegSpeed = this.manaRegSpeed + (item.bonusManaReg + (item.bonusManaReg * lvlAffection));
        else
            this.manaRegSpeed = this.manaRegSpeed + (item.bonusManaReg - (item.bonusManaReg * lvlAffection));
        if (item.bonusHpReg >= 0)
            this.hpRegSpeed = this.hpRegSpeed + (item.bonusHpReg + (item.bonusHpReg * lvlAffection));
        else
            this.hpRegSpeed = this.hpRegSpeed + (item.bonusHpReg - (item.bonusHpReg * lvlAffection));
        CheckStatLimit();
    }

    public bool Attack(Enemy enemy) {
        bool isStun = false;
        float powerPure = physicPower * pureDmg;
        float damage = 0;
        if((pierce*100) < enemy.armor)
        {
            damage = (physicPower - ((physicPower - powerPure) * ((enemy.armor / 100) - ((enemy.armor / 100) / (1 / ((enemy.armor / 100) - pierce))))));
        }
        else
        {
            damage = physicPower;
        }
        
        System.Random generator = new System.Random(DateTime.Now.Millisecond);
        damage = damage - (damage * weakness);
        if (generator.Next(100) < stunChance)
        {
            isStun = true;
        }

        if(generator.Next(100) < vampirismChance)
        {
            hp = hp + (damage * 0.1f);
            CheckStatLimit();
        }

        if (generator.Next(1000) <= luck * 1000) {
            damage = damage + (damage * critDmg);
        }
        enemy.hp = (float)Math.Round(enemy.hp - damage, 0);
        enemy.percentHp = 1 - (enemy.maxHp - enemy.hp) / enemy.maxHp;

        return isStun;
    }

    public void RegenerateHp()
    {
        if(Mathf.Floor((float)(0.1 * physicPower)) != 0)
        {
            hp = hp + Mathf.Floor((float)(0.1 * physicPower));
        }
        else
        {
            hp = hp + 1;
        }
        
        
        percentHp = 1 - (maxHp - hp) / maxHp;
    }

    public void RegenerateMana()
    {
        if(mana >= maxMana)
        {
            mana = maxMana;
        }
        else
        {
            mana = mana + magicPower;
            if (mana >= maxMana)
            {
                mana = maxMana;
            }
        }

        percentMana = 1 - ((maxMana - mana) / maxMana);
    }

    public void RegenerateMana(double amount)
    {
        if(amount != 0)
        {
            if (mana >= maxMana)
            {
                mana = maxMana;
            }
            else
            {
                mana = mana + magicPower;
                if (mana >= maxMana)
                {
                    mana = maxMana;
                }
            }

            percentMana = 1 - (maxMana - mana) / maxMana;
            amount--;
            RegenerateMana(amount);
        }
    }

    public void IncreaseStat(StatUpgrade.Stats stat, float increaseValue)
    {
        if (stat == StatUpgrade.Stats.PP)
        {
            if(Reincarnation.permanentsUpgrades[0] > 0 || Reincarnation.permanentsUpgrades[6] > 0)
            {
                this.physicPower += (float)(increaseValue * ((100 + Reincarnation.permanentsUpgrades[0] + Reincarnation.permanentsUpgrades[6]) / 100));
            }
            else
            {
                this.physicPower += increaseValue;
            }
        }
        else if (stat == StatUpgrade.Stats.MP)
        {
            if (Reincarnation.permanentsUpgrades[2] > 0)
            {
                this.magicPower += (float)(increaseValue * ((100 + Reincarnation.permanentsUpgrades[2]) / 100));
            }
            else
                this.magicPower += increaseValue;
        }
        else if (stat == StatUpgrade.Stats.ARM)
        {
            if(Reincarnation.permanentsUpgrades[3] > 0)
            {
                this.armor += (float)(increaseValue * ((100 + Reincarnation.permanentsUpgrades[3]) / 100));
                this.abstractARM += (float)(increaseValue * ((100 + Reincarnation.permanentsUpgrades[3]) / 100));
            }
            else
            {
                this.armor += (increaseValue);
                this.abstractARM += (increaseValue);
            }
            CheckStatLimit();
        }
        else if (stat == StatUpgrade.Stats.MRM)
        {
            if (Reincarnation.permanentsUpgrades[5] > 0)
            {
                this.magicArmor += (float)(increaseValue * ((100 + Reincarnation.permanentsUpgrades[5]) / 100));
                this.abstractMARM += (float)(increaseValue * ((100 + Reincarnation.permanentsUpgrades[5]) / 100));
            }
            else
            {
                this.magicArmor += (increaseValue);
                this.abstractMARM += (increaseValue);
            }
            CheckStatLimit();
        }
        else if (stat == StatUpgrade.Stats.PRC)
        {
            if (increaseValue >= 1)
            {
                if(Reincarnation.permanentsUpgrades[4] > 0)
                {
                    this.pierce += (float)(increaseValue * ((100 + Reincarnation.permanentsUpgrades[4]) / 100) / 100);
                    this.abstractPRC += (float)(increaseValue * ((100 + Reincarnation.permanentsUpgrades[4]) / 100) / 100);
                }
                else
                {
                    this.pierce += increaseValue / 100;
                    this.abstractPRC += increaseValue / 100;
                }  
            }
            else
            {
                if (Reincarnation.permanentsUpgrades[4] > 0)
                {
                    this.pierce += (float)(increaseValue * ((100 + Reincarnation.permanentsUpgrades[4]) / 100));
                    this.abstractPRC += (float)(increaseValue * ((100 + Reincarnation.permanentsUpgrades[4]) / 100));
                }
                else
                {
                    this.pierce += increaseValue;
                    this.abstractPRC += increaseValue;
                } 
            }
        }
        else if (stat == StatUpgrade.Stats.CRT)
        {
            this.critDmg += increaseValue;
        }
        else if (stat == StatUpgrade.Stats.HP)
        {
            if(Reincarnation.permanentsUpgrades[1] > 0 || Reincarnation.permanentsUpgrades[6] > 0)
            {
                this.maxHp += (float)(increaseValue * ((100 + Reincarnation.permanentsUpgrades[1] + Reincarnation.permanentsUpgrades[7]) / 100));
            }else
                this.maxHp += increaseValue;
        }
        else if (stat == StatUpgrade.Stats.AS)
        {
            this.attackSpeed -= increaseValue;
            this.abstractAS -= increaseValue;
            CheckStatLimit();
        }
        else if (stat == StatUpgrade.Stats.PD)
        {
            this.pureDmg += (increaseValue / 100);
        }
        else if (stat == StatUpgrade.Stats.LCK)
        {
            if (increaseValue >= 1)
            {
                this.luck += increaseValue / 100;
                this.abstractLCK += increaseValue / 100;
            }
            else
            {
                this.luck += increaseValue;
                this.abstractLCK += increaseValue;
            }
            CheckStatLimit();
        }
        CheckStatLimit();
    }

    public Tuple<StatUpgrade.Stats, int> GetRandomStat()
    {
        System.Random generator = new System.Random(DateTime.Now.Millisecond);
        StatUpgrade.Stats stat = StatUpgrade.Stats.PP;
        if (generator.Next(7) == 0)
        {
            stat = StatUpgrade.Stats.PP;
        }
        else if (generator.Next(7) == 1)
        {
            stat = StatUpgrade.Stats.MP;
        }
        else if (generator.Next(7) == 2)
        {
            stat = StatUpgrade.Stats.ARM;
        }
        else if (generator.Next(7) == 3)
        {
            stat = StatUpgrade.Stats.MRM;
        }
        else if (generator.Next(7) == 4)
        {
            stat = StatUpgrade.Stats.CRT;
        }
        else if (generator.Next(7) == 5)
        {
            stat = StatUpgrade.Stats.PRC;
        }
        else if (generator.Next(7) == 6)
        {
            stat = StatUpgrade.Stats.HP;
        }
        int value = generator.Next(1, (int)Math.Pow(2, lvl/(lvl/2)));
        return Tuple.Create(stat, value);
    }
}
