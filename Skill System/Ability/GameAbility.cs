using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "Ability/Default")]
public class GameAbility : ScriptableObject
{
    public enum Type
    {
        StunEnemy,
        IncreaseAS,
        IncreaseMP,
        MultiplyMp,
        MagicImmune,
        WeakenEnemy,
        DealMagicDmg,
        PermIncreasePP,
        PermIncreaseMP,
        PermIncreaseHP
    }

    public int id;
    public string displayName;
    public string description;
    public Sprite icon;
    public Sprite chosenIcon;
    public Type abilityType;
    public float startCooldown;
    public float manaCost;
    public float cooldown;
    public float value;
    public float activeTime;

    public string[] abilityUseReplics;

    public bool isCooldown = false;

    public Battle battle;
    public GameObject cooldownObj;

    public bool isLoaded = false;

    public void UseAbility()
    {
        if(battle.player.negativeEffects[2] != null)
        {
            if (!battle.player.negativeEffects[2].isEffected)
            {
                if (!isCooldown && battle.player.mana >= manaCost && battle.IsBattleOn())
                {
                    GameObject.Find("Log").GetComponent<BattleLogger>().OnUseAbility(abilityUseReplics);
                    battle.player.mana -= manaCost;
                    if (abilityType == Type.PermIncreaseMP)
                    {
                        battle.player.magicPower = battle.player.magicPower + (value/100 * battle.player.magicPower);
                        OnCooldown();
                    }
                    if (abilityType == Type.PermIncreasePP)
                    {
                        battle.player.physicPower = battle.player.physicPower + (value / 100 * battle.player.physicPower);
                        OnCooldown();
                    }
                    if (abilityType == Type.PermIncreaseHP)
                    {
                        battle.player.maxHp = battle.player.maxHp + (value / 100 * battle.player.maxHp);
                        OnCooldown();
                    }
                    if (abilityType == Type.StunEnemy)
                    {
                        battle.StunEnemy(value * 1000);
                        OnCooldown();
                    }
                    if (abilityType == Type.WeakenEnemy)
                    {
                        battle.WeakenEnemy(activeTime * 1000);
                        OnCooldown();
                    }
                    if (abilityType == Type.MagicImmune)
                    {
                        float preMRM = battle.player.magicArmor;
                        battle.player.GrantImmune(Player.ImmuneType.MAGIC);
                        Task.Delay((int)(activeTime * 1000)).ContinueWith(t => battle.player.DisableImmune(Player.ImmuneType.MAGIC, preMRM));
                        OnCooldown();
                    }
                    if (abilityType == Type.IncreaseAS)
                    {
                        float prevAS = battle.player.attackSpeed;
                        MakeFast();
                        Task.Delay((int)(activeTime * 1000)).ContinueWith(t => ResetFast(prevAS));
                        OnCooldown();
                    }
                    if (abilityType == Type.IncreaseMP)
                    {
                        float prevMP = battle.player.magicPower;

                        battle.player.magicPower = battle.player.magicPower + value;
                        Task.Delay((int)(activeTime * 1000)).ContinueWith(t => ResetMp(prevMP));
                        OnCooldown();
                    }
                    if (abilityType == Type.MultiplyMp)
                    {
                        float prevMP = battle.player.magicPower;

                        battle.player.magicPower = battle.player.magicPower * value;
                        Task.Delay((int)(activeTime * 1000)).ContinueWith(t => ResetMp(prevMP));
                        OnCooldown();
                    }
                    if (abilityType == Type.DealMagicDmg)
                    {
                        battle.DealMagicDmg(battle.player, battle.enemy, value * battle.player.magicPower);
                        OnCooldown();
                    }
                }
            }
        }
        else
        {
            if (!isCooldown && battle.player.mana >= manaCost && battle.IsBattleOn())
            {
                battle.player.mana -= manaCost;
                if (abilityType == Type.PermIncreaseMP)
                {
                    battle.player.magicPower = battle.player.magicPower + (value / 100 * battle.player.magicPower);
                    OnCooldown();
                }
                if (abilityType == Type.PermIncreasePP)
                {
                    battle.player.physicPower = battle.player.physicPower + (value / 100 * battle.player.physicPower);
                    OnCooldown();
                }
                if (abilityType == Type.PermIncreaseHP)
                {
                    battle.player.maxHp = battle.player.maxHp + (value / 100 * battle.player.maxHp);
                    OnCooldown();
                }
                if (abilityType == Type.StunEnemy)
                {
                    battle.StunEnemy(value * 1000);
                    OnCooldown();
                }
                if (abilityType == Type.WeakenEnemy)
                {
                    battle.WeakenEnemy(activeTime * 1000);
                    OnCooldown();
                }
                if (abilityType == Type.MagicImmune)
                {
                    float preMRM = battle.player.magicArmor;
                    battle.player.GrantImmune(Player.ImmuneType.MAGIC);
                    Task.Delay((int)(activeTime * 1000)).ContinueWith(t => battle.player.DisableImmune(Player.ImmuneType.MAGIC, preMRM));
                    OnCooldown();
                }
                if (abilityType == Type.IncreaseAS)
                {
                    float prevAS = battle.player.attackSpeed;
                    MakeFast();
                    Task.Delay((int)(activeTime * 1000)).ContinueWith(t => ResetFast(prevAS));
                    OnCooldown();
                }
                if (abilityType == Type.IncreaseMP)
                {
                    float prevMP = battle.player.magicPower;

                    battle.player.magicPower = battle.player.magicPower + value;
                    Task.Delay((int)(activeTime * 1000)).ContinueWith(t => ResetMp(prevMP));
                    OnCooldown();
                }
                if (abilityType == Type.MultiplyMp)
                {
                    float prevMP = battle.player.magicPower;

                    battle.player.magicPower = battle.player.magicPower * value;
                    Task.Delay((int)(activeTime * 1000)).ContinueWith(t => ResetMp(prevMP));
                    OnCooldown();
                }
                if (abilityType == Type.DealMagicDmg)
                {
                    battle.DealMagicDmg(battle.player, battle.enemy, value * battle.player.magicPower);
                    OnCooldown();
                }
            }
        }

        
    }

    public void MakeFast()
    {
        battle.player.attackSpeed = 1;
        battle.player.CheckStatLimit();
        battle.UpdateAS();
    }

    public void OnCooldown()
    {
        isCooldown = true;
        cooldownObj.transform.SetAsLastSibling();
        cooldownObj.GetComponent<RectTransform>().sizeDelta = new Vector2(100, 100);
        battle.ResetAbility(this);
    }

    public void OnCooldown(float time)
    {
        if(battle == null)
        {
            battle = GameObject.Find("Battle").GetComponent<Battle>();
        }
        isCooldown = true;
        if(cooldownObj == null)
        {

        }
        cooldownObj.GetComponent<RectTransform>().sizeDelta = new Vector2(100, 100);
        cooldownObj.transform.SetSiblingIndex(1);
        battle.ResetAbility(this, time);
    }

    public void ResetFast(float prevAS)
    {
        if (abilityType == Type.IncreaseAS)
        {
            battle.player.attackSpeed = prevAS;
            battle.UpdateAS();
        }
    }
    
    public void ResetMp(float prevMP)
    {
        battle.player.magicPower = prevMP;
    }
}
