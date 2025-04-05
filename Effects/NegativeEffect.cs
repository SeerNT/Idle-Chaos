using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class NegativeEffect : MonoBehaviour
{
    public float effectTime;
    public float effectCurrentDuration;
    public bool isEffected = false;

    public int effectId;
    public static Dictionary<string, int> effectIds = new Dictionary<string, int>()
    {
        {"StunPlayer", 0},
        {"StunEnemy", 1},
        {"BlockPlayerSkills", 2},
        {"BlockEnemySkills", 3},
        {"WeakenEnemy", 4},
        {"WeakenPlayer", 5}
    };

    private Battle battle;
    private bool effectOnPlayer;

    public void ApplyEffect(Player player)
    {
        battle = GameObject.Find("Battle").GetComponent<Battle>();
        this.effectOnPlayer = true;

        if (this.effectId == 0)
        {
            battle.playerIconColor = new Color32(171, 171, 171, 255);
            battle.playerStunned = true;
            battle.timerPlayer.Change(Timeout.Infinite, Timeout.Infinite);
        }
        if (this.effectId == 5)
        {
            battle.playerWeakned = true;
            battle.playerIconColor = new Color32(99, 171, 171, 255);
        }

        CancelInvoke("IncreaseEffectTime");
        InvokeRepeating("IncreaseEffectTime", 0f, 1f);

    }

    public void ApplyEffect(Enemy enemy)
    {
        battle = GameObject.Find("Battle").GetComponent<Battle>();
        this.effectOnPlayer = false;

        if (this.effectId == 1)
        {
            battle.enemyIconColor = new Color32(171, 171, 171, 255);
            battle.timerEnemy.Change(Timeout.Infinite, Timeout.Infinite);
        }
        if(this.effectId == 4)
        {
            battle.enemyIconColor = new Color32(99, 171, 171, 255);
        }

        CancelInvoke("IncreaseEffectTime");
        InvokeRepeating("IncreaseEffectTime", 0f, 1f);
    }

    public void IncreaseEffectTime()
    {
        this.effectTime += 1f;
        if (this.effectTime >= (this.effectCurrentDuration / 1000))
        {
            this.effectTime = 0f;
            CancelInvoke("IncreaseEffectTime");
            if (this.effectOnPlayer)
            {
                ReturnPlayer();
            }
            else
            {
                ReturnEnemy();
            }
        }
    }

    private void ReturnPlayer()
    {
        if(GameObject.Find("Battle") != null)
        {
            battle = GameObject.Find("Battle").GetComponent<Battle>();
            if (this.effectId == 0)
            {
                if (battle.isBattle)
                {
                    battle.timerPlayer.Change(190, (int)((battle.player.attackSpeed / 100) * 1000));
                    battle.playerStunned = false;
                }
            }

            if (this.effectId == 5)
            {
                battle.player.weakness = 0;
            }

            battle.bossInteractions.gameObject.SetActive(false);
            battle.enemyInteractions.gameObject.SetActive(false);
            battle.playerIconColor = new Color32(255, 255, 255, 255);

            this.isEffected = false;
            this.effectTime = 0f;
        }
    }

    private void ReturnEnemy()
    {
        if(GameObject.Find("Battle") != null)
        {
            battle = GameObject.Find("Battle").GetComponent<Battle>();

            if (this.effectId == 1)
            {
                if (battle.isBattle)
                {
                    battle.timerEnemy.Change(190, (int)((battle.enemy.attackSpeed / 100) * 1000));
                }
            }
            if (this.effectId == 4)
            {
                battle.enemy.weakness = 0;
            }

            battle.enemyIconColor = new Color32(255, 255, 255, 255);

            this.isEffected = false;
            this.effectTime = 0f;
        }
    }

    public void StopEffect()
    {
        CancelInvoke("IncreaseEffectTime");
        if (effectOnPlayer)
        {
            ReturnPlayer();
        }
        else
        {
            ReturnEnemy();
        }
    }
}
