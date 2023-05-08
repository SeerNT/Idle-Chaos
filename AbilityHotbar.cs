using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AbilityHotbar : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private AbilityManager manager;

    private Text manaCostText;

    private Text cooldownTime;
    private Timer cooldownTimer;
    private float currentCooldown;

    public GameAbility ability;
    public bool isAdded = false;

    public GameObject enterObj;
    public bool isEnter = false;

    void Start()
    {
        cooldownTime = transform.Find("Time").GetComponent<Text>();
        manaCostText = transform.Find("Manacost").GetComponent<Text>();
        cooldownTimer = new Timer(IncreaseCooldown, null, Timeout.Infinite, 1000);
        if(ability != null)
        {
            //ability.cooldownObj = transform.Find("Cooldown").gameObject;
        }
    }

    void Update()
    {
        UpdateUI();
        if(ability != null)
        {
            if (ability.isLoaded)
            {
                ability.isLoaded = false;
                CooldownAfterLoad();
            }
        }
        
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
        if(ability != null)
        {
            if (!ability.isCooldown)
            {
                if (ability.battle.player.negativeEffects[2] != null)
                {
                    if (!ability.battle.player.negativeEffects[2].isEffected)
                    {
                        if (ability.battle.player.mana >= ability.manaCost && ability.battle.IsBattleOn())
                        {
                            ability.UseAbility();
                            currentCooldown = ability.cooldown;
                            cooldownTimer.Change(0, 100);
                        }
                    }
                }
                else
                {
                    if (ability.battle.player.mana >= ability.manaCost && ability.battle.IsBattleOn())
                    {
                        ability.UseAbility();
                        currentCooldown = ability.cooldown;
                        cooldownTimer.Change(0, 100);
                    }
                }
            }
        }
    }

    public void Reset()
    {
        cooldownTimer.Change(Timeout.Infinite, Timeout.Infinite);
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
            if(manager.hotbarSlots[int.Parse(name)-1] != null)
            {
                ability = manager.hotbarSlots[int.Parse(name)-1];
                manager.AddAbilityToBattleHotbar(ability, int.Parse(name));
                isAdded = true;
            }
            
        }
        if(ability != null)
        {
            manaCostText.gameObject.SetActive(!ability.isCooldown);
            cooldownTime.gameObject.SetActive(ability.isCooldown);
            
            cooldownTime.text = currentCooldown + "S";
            manaCostText.text = ability.manaCost.ToString();
        }
        else
        {
            cooldownTime.gameObject.SetActive(false);
            manaCostText.gameObject.SetActive(false);
        }
    }
}
