using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    /// <summary>
    /// This property defines current health
    /// </summary>
    public float hp = 10f;
    /// <summary>
    /// This property defines max health possible
    /// </summary>
    public float maxHp = 10f;

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
    public float armor = 1f;
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

    public float trollFactor = 0f;

    public int lvl = 1;

    public float percentHp = 1f;
    public float percentMana = 1f;

    public float stunDur = 0f;
    public float stunChance = 0f;
    public NegativeEffect[] negativeEffects = new NegativeEffect[NegativeEffect.effectIds.Count];

    public float weakness = 0f;

    public int bossId = 0;

    public bool isRegenerating = false;

    public Enemy(float mana, float stunChance, float hp, float mp, float pp, float arm, float mgarm, float prc, float lck, float cdmg, float atks, float tf)
    {
        this.stunChance = stunChance;
        this.stunDur = 5f;
        this.maxMana = mana;
        this.mana = mana;
        this.maxHp = hp;
        this.hp = hp;
        this.magicPower = mp;
        this.physicPower = pp;
        this.armor = arm;
        this.magicArmor = mgarm;
        this.pierce = prc;
        this.luck = lck;
        this.critDmg = cdmg;
        this.attackSpeed = atks;
        this.trollFactor = tf;
    }

    public Enemy(int bossId, float stunChance, float mana, float hp, float mp, float pp, float arm, float mgarm, float prc, float lck, float cdmg, float atks, float tf)
    {
        this.stunChance = stunChance;
        this.bossId = bossId;
        this.maxMana = mana;
        this.mana = mana;
        this.maxHp = hp;
        this.hp = hp;
        this.magicPower = mp;
        this.physicPower = pp;
        this.armor = arm;
        this.magicArmor = mgarm;
        this.pierce = prc;
        this.luck = lck;
        this.critDmg = cdmg;
        this.attackSpeed = atks;
        this.trollFactor = tf; 
    }
    
    public bool Attack(Player enemy)
    {
        bool isStun = false;

        
        System.Random generator = new System.Random(DateTime.Now.Millisecond);
        
        float damage = 0;
        if ((pierce * 100) < enemy.armor)
        {
            damage = (physicPower - (physicPower * ((enemy.armor / 100) - ((enemy.armor / 100) / (1 / ((enemy.armor / 100) - pierce))))));
        }
        else
        {
            damage = physicPower;
        }
        damage = damage - (damage * weakness);
        if (generator.Next(100) < stunChance)
        {
            isStun = true;
        }

        if (generator.Next(100000) < luck * 1000)
        {
            damage = damage + (damage * critDmg);
        }
        enemy.hp = (float)Math.Round(enemy.hp - damage, 0);
        enemy.percentHp = 1 - (enemy.maxHp - enemy.hp) / enemy.maxHp;

        return isStun;
    }

    public void RegenerateHp()
    {
        hp = hp + physicPower;
        percentHp = 1 - (maxHp - hp) / maxHp;
    }

    public void RegenerateMana()
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
    }

}
