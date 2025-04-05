using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAbilitySlot : MonoBehaviour
{
    [SerializeField] private AbilityManager manager;

    public EnemyGameAbility ability;
    public bool isAdded = false;

    public GameObject enterObj;
    public bool isEnter = false;

    void Update()
    {
        UpdateUI();
    }
    void UpdateUI()
    {
        if (!isAdded)
        {
            string name = this.name;
            name = name.Replace("Slot", "");
            if (this.ability != null)
            {
                //manager.AddAbilityToEnemyHotbar(ability, int.Parse(name));
                isAdded = true;
            }

        }
    }


    public void SaveAbility()
    {
        if (ability != null)
        {
            ability.startCooldown = manager.battle.GetComponent<Battle>().startCooldown;
            SaveSystem.SaveEnemySkill(ability, this.name);
        }
    }

    public void LoadAbility()
    {
        EnemySkillData data = SaveSystem.LoadEnemySkill(this.name);

        if (data != null)
        {
            manager.enemyAbilitiesPool.TryGetValue(data.abilityId, out this.ability);
            ability.isCooldown = data.isCooldown;
            ability.cooldown = data.cooldown;
            ability.startCooldown = data.currentTime;

            if (ability.isCooldown)
            {
                if (ability.startCooldown != ability.cooldown)
                {
                    ability.OnCooldown(ability.startCooldown);
                }
            }
        }
    }
}
