using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GodAbilityHotbar : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private AbilityManager manager;

    private Text bloodCostText;

    private Text cooldownTime;
    private Timer cooldownTimer;
    private float currentCooldown;

    public GodAbility ability;
    public bool isAdded = false;

    public GameObject enterObj;
    public bool isEnter = false;

    void Start()
    {
        cooldownTime = transform.Find("Time").GetComponent<Text>();
        bloodCostText = transform.Find("Bloodcost").GetComponent<Text>();
        cooldownTimer = new Timer(IncreaseCooldown, null, Timeout.Infinite, 1000);
    }

    void Update()
    {
        UpdateUI();
        if (ability != null)
        {
            if (ability.isLoaded)
            {
                ability.isLoaded = false;
                CooldownAfterLoad();
            }
        }

    }
    public void Reset()
    {
        cooldownTimer.Change(Timeout.Infinite, Timeout.Infinite);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        enterObj = this.gameObject;
        isEnter = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        enterObj = null;
        isEnter = false;
    }

    public void UseAbility()
    {
        if (!ability.isCooldown && ability != null)
        {
            ability.UseAbility();
            currentCooldown = ability.cooldown;
            cooldownTimer.Change(0, 100);
        }
    }

    public void IncreaseCooldown(object o)
    {
        if (ability.isCooldown)
        {
            currentCooldown -= 0.1f;
            currentCooldown = (float)Math.Round(currentCooldown, 1);
        }
        else
        {
            if (cooldownTimer != null)
            {
                currentCooldown = ability.cooldown;
                cooldownTimer.Change(Timeout.Infinite, Timeout.Infinite);
            }
        }
    }

    void CooldownAfterLoad()
    {
        currentCooldown = ability.cooldown - ability.startCooldown;
        cooldownTimer.Change(0, 100);
    }

    void UpdateUI()
    {
        if (!isAdded)
        {
            string name = this.name;
            name = name.Replace("Slot", "");
            if (manager.godSlots[int.Parse(name) - 1] != null)
            {
                ability = manager.godSlots[int.Parse(name) - 1];
                //manager.AddAbilityToBattleGodSlot(ability, int.Parse(name));
                isAdded = true;
            }
        }

        if (ability != null)
        {
            bloodCostText.gameObject.SetActive(!ability.isCooldown);
            cooldownTime.gameObject.SetActive(ability.isCooldown);

            cooldownTime.text = currentCooldown + "S";
            bloodCostText.text = ability.bloodCost.ToString();
        }
        else
        {
            cooldownTime.gameObject.SetActive(false);
            bloodCostText.gameObject.SetActive(false);
        }
    }

    public void SaveHotbarAbility()
    {
        if (ability != null)
        {
            SaveSystem.SaveGodSkill(ability, this.name);
        }
    }

    public void LoadHotbarAbility()
    {
        GodSkillData data = SaveSystem.LoadGodSkill(this.name);

        if (data != null)
        {
            manager.godAbilitiesPool.TryGetValue(data.abilityId, out this.ability);

            if(ability != null)
            {
                ability.isCooldown = data.isCooldown;
                ability.cooldown = data.cooldown;
                ability.startCooldown = data.currentTime;

                string name = this.name;
                name = name.Replace("Slot", "");
                manager.AddAbilityToBattleGodSlot(ability, int.Parse(name));
                isAdded = true;
                ability.isLoaded = true;
                if (ability.isCooldown)
                {
                    if (ability.startCooldown != ability.cooldown)
                    {
                        ability.OnCooldown(ability.cooldown - ability.startCooldown);
                    }
                }
            }
            else
            {
                cooldownTime.gameObject.SetActive(false);
                bloodCostText.gameObject.SetActive(false);
            }
        }

    }
}
