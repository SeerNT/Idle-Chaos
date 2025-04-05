using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bonus : MonoBehaviour
{
    public enum Type
    {
        None,
        IncreaseHP,
        IncreasePP,
        IncreaseMP,
        IncreaseARM,
        IncreaseMRM,
        IncreaseCRT,
        IncreasePRC,
        IncreaseLCK,
        IncreaseAS,
        IncreaseHPReg,
        PassiveStunChance,
        ActiveStun,
        ActiveIAS,
        ActiveIMP,
        ActiveMMP,
        Special,
        PassivePureDmg,
        ActiveMagicImmune,
        IncreaseAutoSpeed,
        PassiveInfection,
        UnlockSkillConvertor,
        UnlockSkillConvStage1,
        UnlockSkillConvStage2,
        UnlockSkillConvStage3,
        UnlockSkillConvStage4,
        SkillConversionReduceCost
    }
    public Type bonusType;
    public string bonusValue;
    public Player player;
    public Battle battle;

    public Dictionary<string, GameAbility> abilities = new Dictionary<string, GameAbility>();

    public Bonus(Type type, string value)
    {
        bonusType = type;
        bonusValue = value;
        battle = GameObject.Find("Battle").GetComponent<Battle>();
        abilities.Add("ActiveStun", battle.abilityManager.availableAbilities[0]);
        abilities.Add("ActiveIAS", battle.abilityManager.availableAbilities[1]);
        abilities.Add("ActiveIMP", battle.abilityManager.availableAbilities[2]);
        abilities.Add("MagicImmune", battle.abilityManager.availableAbilities[3]);
        abilities.Add("PassiveInfection", battle.abilityManager.availableAbilities[4]);
    }

    public void GetBonus()
    {
        battle = GameObject.Find("Battle").GetComponent<Battle>();
        GameObject convertor = GameObject.Find("Skills").transform.Find("SkillConvertor").gameObject;
        player = battle.player;

        if(bonusType == Type.UnlockSkillConvertor)
        {
            convertor.SetActive(true);
        }
        if(bonusType == Type.UnlockSkillConvStage1)
        {
            convertor.transform.GetChild(2).gameObject.SetActive(true);
            convertor.transform.GetChild(2).GetComponent<SkillConversion>().isUnlocked = true;
        }
        if (bonusType == Type.UnlockSkillConvStage2)
        {
            convertor.transform.GetChild(3).gameObject.SetActive(true);
            convertor.transform.GetChild(4).gameObject.SetActive(true);
            convertor.transform.GetChild(3).GetComponent<SkillConversion>().isUnlocked = true;
            convertor.transform.GetChild(4).GetComponent<SkillConversion>().isUnlocked = true;
        }
        if (bonusType == Type.UnlockSkillConvStage3)
        {
            convertor.transform.GetChild(5).gameObject.SetActive(true);
            convertor.transform.GetChild(6).gameObject.SetActive(true);
            convertor.transform.GetChild(5).GetComponent<SkillConversion>().isUnlocked = true;
            convertor.transform.GetChild(6).GetComponent<SkillConversion>().isUnlocked = true;
        }
        if (bonusType == Type.UnlockSkillConvStage4)
        {
            convertor.transform.GetChild(7).gameObject.SetActive(true);
            convertor.transform.GetChild(8).gameObject.SetActive(true);
            convertor.transform.GetChild(7).GetComponent<SkillConversion>().isUnlocked = true;
            convertor.transform.GetChild(8).GetComponent<SkillConversion>().isUnlocked = true;
        }
        if (bonusType == Type.SkillConversionReduceCost)
        {
            convertor.transform.GetChild(1).GetComponent<SkillConversion>().baseCost = 1;
            convertor.transform.GetChild(2).GetComponent<SkillConversion>().baseCost = 1;
            convertor.transform.GetChild(3).GetComponent<SkillConversion>().baseCost = 1;
            convertor.transform.GetChild(4).GetComponent<SkillConversion>().baseCost = 1;
            convertor.transform.GetChild(5).GetComponent<SkillConversion>().baseCost = 1;
            convertor.transform.GetChild(6).GetComponent<SkillConversion>().baseCost = 1;
            convertor.transform.GetChild(7).GetComponent<SkillConversion>().baseCost = 1;
            convertor.transform.GetChild(8).GetComponent<SkillConversion>().baseCost = 1;
        }

        if (bonusType == Type.ActiveStun)
        {
            GameAbility ability;
            abilities.TryGetValue("ActiveStun", out ability);
            battle.GetComponent<Battle>().abilityManager.AddAbility(ability);
        }
        if (bonusType == Type.ActiveMagicImmune)
        {
            GameAbility ability;
            abilities.TryGetValue("MagicImmune", out ability);
            battle.GetComponent<Battle>().abilityManager.AddAbility(ability);
        }
        if (bonusType == Type.PassiveInfection)
        {
            GameAbility ability;
            abilities.TryGetValue("PassiveInfection", out ability);
            battle.GetComponent<Battle>().abilityManager.AddAbility(ability);
        }
        if (bonusType == Type.ActiveIAS)
        {
            GameAbility ability;
            abilities.TryGetValue("ActiveIAS", out ability);
            battle.GetComponent<Battle>().abilityManager.AddAbility(ability);
        }
        if (bonusType == Type.ActiveIMP)
        {
            GameAbility ability;
            abilities.TryGetValue("ActiveIMP", out ability);
            battle.GetComponent<Battle>().abilityManager.AddAbility(ability);
        }

        if (bonusType == Type.IncreasePP)
        {
            if (!bonusValue.Contains("%"))
            {
                player.IncreaseStat(StatUpgrade.Stats.PP, int.Parse(bonusValue));
            }
            else
            {
                bonusValue = bonusValue.Replace("%", "");
                int bonusValueClear = int.Parse(bonusValue);
                bonusValueClear = bonusValueClear / 100;
                int bonusValueProcents = (int)(player.physicPower + Mathf.Ceil(player.physicPower * bonusValueClear));
                player.IncreaseStat(StatUpgrade.Stats.PP, bonusValueProcents);
            }
        }
        if (bonusType == Type.IncreaseMP)
        {
            if (!bonusValue.Contains("%"))
            {
                player.IncreaseStat(StatUpgrade.Stats.MP, int.Parse(bonusValue));
            }
            else
            {
                bonusValue = bonusValue.Replace("%", "");
                int bonusValueClear = int.Parse(bonusValue);
                bonusValueClear = bonusValueClear / 100;
                int bonusValueProcents = (int)(player.magicPower + Mathf.Ceil(player.magicPower * bonusValueClear));
                player.IncreaseStat(StatUpgrade.Stats.MP, bonusValueProcents);
            }
        }
        if (bonusType == Type.IncreaseHP)
        {
            if (!bonusValue.Contains("%"))
            {
                player.IncreaseStat(StatUpgrade.Stats.HP, int.Parse(bonusValue));
            }
            else
            {
                bonusValue = bonusValue.Replace("%", "");
                int bonusValueClear = int.Parse(bonusValue);
                bonusValueClear = bonusValueClear / 100;
                int bonusValueProcents = (int)(player.maxHp + Mathf.Ceil(player.maxHp * bonusValueClear));
                player.IncreaseStat(StatUpgrade.Stats.HP, bonusValueProcents);
            }
        }
        if (bonusType == Type.IncreaseLCK)
        {
            if (!bonusValue.Contains("%"))
            {
                player.IncreaseStat(StatUpgrade.Stats.LCK, int.Parse(bonusValue));
            }
            else
            {
                bonusValue = bonusValue.Replace("%", "");
                int bonusValueClear = int.Parse(bonusValue);
                bonusValueClear = bonusValueClear / 100;
                int bonusValueProcents = (int)(player.luck + Mathf.Ceil(player.luck * bonusValueClear));
                player.IncreaseStat(StatUpgrade.Stats.LCK, bonusValueProcents);
            }
        }
        if (bonusType == Type.IncreasePRC)
        {
            if (!bonusValue.Contains("%"))
            {
                player.IncreaseStat(StatUpgrade.Stats.PRC, int.Parse(bonusValue));
            }
            else
            {
                bonusValue = bonusValue.Replace("%", "");
                int bonusValueClear = int.Parse(bonusValue);
                bonusValueClear = bonusValueClear / 100;
                int bonusValueProcents = (int)(player.pierce + Mathf.Ceil(player.pierce * bonusValueClear));
                player.IncreaseStat(StatUpgrade.Stats.PRC, bonusValueProcents);
            }
        }
        if (bonusType == Type.IncreaseAS)
        {
            if (!bonusValue.Contains("%"))
            {
                player.IncreaseStat(StatUpgrade.Stats.AS, float.Parse(bonusValue));
            }
            else
            {
                bonusValue = bonusValue.Replace("%", "");
                int bonusValueClear = int.Parse(bonusValue);
                bonusValueClear = bonusValueClear / 100;
                int bonusValueProcents = (int)(player.attackSpeed + Mathf.Ceil(player.attackSpeed * bonusValueClear));
                player.IncreaseStat(StatUpgrade.Stats.AS, bonusValueProcents);
            }
        }
        if (bonusType == Type.IncreaseARM)
        {
            if (!bonusValue.Contains("%"))
            {
                player.IncreaseStat(StatUpgrade.Stats.ARM, int.Parse(bonusValue));
            }
            else
            {
                bonusValue = bonusValue.Replace("%", "");
                int bonusValueClear = int.Parse(bonusValue);
                bonusValueClear = bonusValueClear / 100;
                int bonusValueProcents = (int)(player.armor + Mathf.Ceil(player.armor * bonusValueClear));
                player.IncreaseStat(StatUpgrade.Stats.ARM, bonusValueProcents);
            }
        }
        if (bonusType == Type.IncreaseMRM)
        {
            if (!bonusValue.Contains("%"))
            {
                player.IncreaseStat(StatUpgrade.Stats.MRM, int.Parse(bonusValue));
            }
            else
            {
                bonusValue = bonusValue.Replace("%", "");
                int bonusValueClear = int.Parse(bonusValue);
                bonusValueClear = bonusValueClear / 100;
                int bonusValueProcents = (int)(player.magicArmor + Mathf.Ceil(player.magicArmor * bonusValueClear));
                player.IncreaseStat(StatUpgrade.Stats.MRM, bonusValueProcents);
            }
        }
        if (bonusType == Type.IncreaseHPReg)
        {
            if (!bonusValue.Contains("%"))
            {
                player.hpRegSpeed -= int.Parse(bonusValue);
            }
            else
            {
                bonusValue = bonusValue.Replace("%", "");
                int bonusValueClear = int.Parse(bonusValue);
                bonusValueClear = bonusValueClear / 100;
                int bonusValueProcents = (int)(player.hpRegSpeed / bonusValueClear);
                player.hpRegSpeed = bonusValueClear;
            }
        }

        if (bonusType == Type.PassivePureDmg)
        {
            bonusValue = bonusValue.Replace("%", "");
            player.IncreaseStat(StatUpgrade.Stats.PD, int.Parse(bonusValue));
        }
        if (bonusType == Type.IncreaseAutoSpeed)
        {
            if (!bonusValue.Contains("%"))
            {
                TalantManager.automationSpeed += int.Parse(bonusValue);
            }
            else
            {
                bonusValue = bonusValue.Replace("%", "");
                int bonusValueClear = int.Parse(bonusValue);
                bonusValueClear = bonusValueClear / 100;
                int bonusValueProcents = (int)(TalantManager.automationSpeed + Mathf.Ceil(TalantManager.automationSpeed * bonusValueClear));
                TalantManager.automationSpeed += bonusValueProcents;
            }
        }

        if (bonusType == Type.PassiveStunChance)
        {
            player.stunChance += int.Parse(bonusValue);
        }
    }
}
