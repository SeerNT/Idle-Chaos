using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "Ability/Enemy")]
public class EnemyGameAbility : ScriptableObject
{
    public enum Type
    {
        StunPlayer,
        BlockPlayerSkills,
        DealMagicDamage,
        WeakenPlayer
    }

    public int id;
    public Sprite icon;
    public Type abilityType;
    public float manaCost;
    public float startCooldown;
    public float cooldown;
    public float value;
    public float activeTime;

    public string[] abilityUseReplics;

    public bool isCooldown = false;

    public Battle battle;
    public GameObject cooldownObj;

    public void UseAbility()
    {
        if (battle.enemy.negativeEffects[3] != null)
        {
            if (!battle.enemy.negativeEffects[3].isEffected)
            {
                if (!isCooldown && battle.enemy.mana >= manaCost && battle.IsBattleOn())
                {
                    GameObject.Find("Log").GetComponent<BattleLogger>().OnUseAbility(abilityUseReplics);
                    battle.enemy.mana -= manaCost;
                    if (abilityType == Type.StunPlayer)
                    {
                        battle.StunPlayer(activeTime * 1000);
                        battle.bossInteractions.gameObject.SetActive(true);
                        OnCooldown();
                    }
                    if (abilityType == Type.BlockPlayerSkills)
                    {
                        battle.enemyInteractions.GetChild(0).GetComponent<Image>().sprite = battle.enemyInteractionsSprites[0];
                        battle.enemyInteractions.gameObject.SetActive(true);
                        battle.BlockPlayerSkills(activeTime * 1000);
                        OnCooldown();
                    }
                    if (abilityType == Type.WeakenPlayer)
                    {
                        battle.WeakenPlayer(activeTime * 1000);
                        OnCooldown();
                    }
                    if (abilityType == Type.DealMagicDamage)
                    {
                        battle.DealMagicDmg(battle.enemy, battle.player, value);
                        OnCooldown();
                    }
                }
            }
        }
        else
        {
            if (!isCooldown && battle.enemy.mana >= manaCost && battle.IsBattleOn())
            {
                GameObject.Find("Log").GetComponent<BattleLogger>().OnUseAbility(abilityUseReplics);
                battle.enemy.mana -= manaCost;
                if (abilityType == Type.StunPlayer)
                {
                    battle.StunPlayer(activeTime * 1000);
                    battle.bossInteractions.gameObject.SetActive(true);
                    OnCooldown();
                }
                if (abilityType == Type.BlockPlayerSkills)
                {
                    battle.enemyInteractions.GetChild(0).GetComponent<Image>().sprite = battle.enemyInteractionsSprites[0];
                    battle.enemyInteractions.gameObject.SetActive(true);
                    battle.BlockPlayerSkills(activeTime * 1000);
                    OnCooldown();
                }
                if (abilityType == Type.WeakenPlayer)
                {
                    battle.WeakenPlayer(activeTime * 1000);
                    OnCooldown();
                }
                if (abilityType == Type.DealMagicDamage)
                {
                    battle.DealMagicDmg(battle.enemy, battle.player, value);
                    OnCooldown();
                }
            }
        }
    }

    public void OnCooldown()
    {
        isCooldown = true;
        cooldownObj.transform.SetAsLastSibling();
        cooldownObj.GetComponent<RectTransform>().sizeDelta = new Vector2(100, 100);
        battle.ResetAbility(this);
        UseAbility();
    }

    public void OnCooldown(float time)
    {
        if (battle == null)
        {
            battle = GameObject.Find("Battle").GetComponent<Battle>();
        }
        isCooldown = true;

        cooldownObj.GetComponent<RectTransform>().sizeDelta = new Vector2(100, 100);
        cooldownObj.transform.SetSiblingIndex(1);
        battle.ResetAbility(this, time);
    }
}
