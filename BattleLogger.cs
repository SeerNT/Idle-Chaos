using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BattleLogger : MonoBehaviour
{
    [SerializeField] private Text log;
    public enum Target
    {
        Player,
        Enemy
    }

    [SerializeField] private string[] enemyDmgReplics;
    [SerializeField] private string[] enemyCrtReplics;
    [SerializeField] private string[] playerDmgReplics;
    [SerializeField] private string[] playerCrtReplics;
    [SerializeField] private string[] playerStunReplics;
    [SerializeField] private string[] enemyStunReplics;
    [SerializeField] private string[] playerSkillsBlockReplics;
    [SerializeField] private string[] enemySkillsBlockReplics;
    [SerializeField] private string[] playerWeakenReplics;
    [SerializeField] private string[] enemyWeakenReplics;

    public void OnReceiveDamage(Target target, bool isCrit, string enemyName)
    {
        string RandomSample = "";

        if (target == Target.Player)
        {
            System.Random rand = new System.Random();
            if (!isCrit)
            {
                RandomSample = enemyDmgReplics[rand.Next(0, enemyDmgReplics.Length)];
            }
            else
            {
                RandomSample = enemyCrtReplics[rand.Next(0, enemyCrtReplics.Length)];
            }
        }
        else if (target == Target.Enemy)
        {
            System.Random rand = new System.Random();
            if (!isCrit)
            {
                RandomSample = playerDmgReplics[rand.Next(0, playerDmgReplics.Length)];
            }
            else
            {
                RandomSample = playerCrtReplics[rand.Next(0, playerCrtReplics.Length)];
            }
        }

        RandomSample = RandomSample.Replace("{enemy}", enemyName);
        log.text += RandomSample + "!\n";
        if (Regex.Matches(log.text, "\n").Count > 14)
        {
            log.text = "";
        }
    }

    public void OnDeath(Target target, string enemyName)
    {
        string sample = "";

        if (target == Target.Player)
        {
            sample = "{enemy} killed player";
        }
        else if (target == Target.Enemy)
        {
            sample = "Player killed {enemy}";
        }

        sample = sample.Replace("{enemy}", enemyName);
        
        log.text += sample + "!\n";
        if (Regex.Matches(log.text, "\n").Count > 14)
        {
            log.text = "";
        }
    }

    public void OnStun(Target target, string enemyName)
    {
        string RandomSample = "";

        if (target == Target.Player)
        {
            System.Random rand = new System.Random();
            RandomSample = enemyStunReplics[rand.Next(0, enemyStunReplics.Length)];
        }
        else if (target == Target.Enemy)
        {
            System.Random rand = new System.Random();
            RandomSample = playerStunReplics[rand.Next(0, playerStunReplics.Length)];
        }

        RandomSample = RandomSample.Replace("{enemy}", enemyName);
        
        log.text += RandomSample + "!\n";
        if (Regex.Matches(log.text, "\n").Count > 14)
        {
            log.text = "";
        }
    }

    public void OnSkillsBlock(Target target, string enemyName)
    {
        string RandomSample = "";

        if (target == Target.Player)
        {
            System.Random rand = new System.Random();
            RandomSample = enemySkillsBlockReplics[rand.Next(0, enemySkillsBlockReplics.Length)];
        }
        else if (target == Target.Enemy)
        {
            System.Random rand = new System.Random();
            RandomSample = playerSkillsBlockReplics[rand.Next(0, playerSkillsBlockReplics.Length)];
        }

        RandomSample = RandomSample.Replace("{enemy}", enemyName);
        
        log.text += RandomSample + "!\n";
        if (Regex.Matches(log.text, "\n").Count > 14)
        {
            log.text = "";
        }
    }

    public void OnWeaken(Target target, string enemyName)
    {
        string RandomSample = "";

        if (target == Target.Player)
        {
            System.Random rand = new System.Random();
            RandomSample = enemyWeakenReplics[rand.Next(0, enemyWeakenReplics.Length)];
        }
        else if (target == Target.Enemy)
        {
            System.Random rand = new System.Random();
            RandomSample = playerWeakenReplics[rand.Next(0, playerWeakenReplics.Length)];
        }

        RandomSample = RandomSample.Replace("{enemy}", enemyName);

        log.text += RandomSample + "!\n";
        if (Regex.Matches(log.text, "\n").Count > 14)
        {
            log.text = "";
        }
    }

    public void OnUseAbility(string[] phrases)
    {
        string RandomSample = "";

        System.Random rand = new System.Random();
        RandomSample = phrases[rand.Next(0, phrases.Length)];

        log.text += RandomSample + "!\n";
        if (Regex.Matches(log.text, "\n").Count > 14)
        {
            log.text = "";
        }
    }
}
