using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AbilitySlot : MonoBehaviour
{
    [SerializeField] private AbilityManager manager;

    public GameAbility ability;
    public bool isAdded = false;
    public bool isBank = false;

    public GameObject battleHotbarSlots;
    public GameObject hotbarSlots;

    [SerializeField] private Button addAbilityButton;
    [SerializeField] private Button removeAbilityButton;

    void Update()
    {
        UpdateUI();
    }

    public void PickAbility()
    {
        Transform abilitySlotT;
        // If Ability slot is chosen
        if (manager.chosenToPick != null)
        {
            // Reset Icon
            abilitySlotT = manager.chosenToPick.transform.GetChild(0);
            abilitySlotT.GetComponent<Image>().sprite = abilitySlotT.GetComponent<Ability>().ability.icon;
        }
        // Set new ability slot to chosen
        manager.chosenToPick = this;

        abilitySlotT = this.transform.GetChild(0);
        abilitySlotT.GetComponent<Image>().sprite = ability.chosenIcon;

        FindFreeInvHotbarSlotAndActivate();
    }

    void FindFreeInvHotbarSlotAndActivate()
    {
        for(int i = 0; i <  hotbarSlots.transform.childCount; i++)
        {
            AbilitySlot abilitySlot = hotbarSlots.transform.GetChild(i).GetComponent<AbilitySlot>();
            if(abilitySlot.ability == null)
            {
                abilitySlot.addAbilityButton.gameObject.SetActive(true);
                abilitySlot.removeAbilityButton.gameObject.SetActive(false);
            }
        }
    }

    void FindFreeInvHotbarSlotAndDeactivate()
    {
        for (int i = 0; i < hotbarSlots.transform.childCount; i++)
        {
            AbilitySlot abilitySlot = hotbarSlots.transform.GetChild(i).GetComponent<AbilitySlot>();
            if (abilitySlot.ability == null)
            {
                abilitySlot.addAbilityButton.gameObject.SetActive(false);
                abilitySlot.removeAbilityButton.gameObject.SetActive(false);
            }
        }
    }

    public void SetAbility()
    {
        if (manager.chosenToPick != null)
        {
            AbilitySlot chosenSlot = manager.chosenToPick;

            SetAbilityUI();

            string abilitySlotId = this.name.Replace("Slot", "");
            // If Ability is not in hotbar already
            if (manager.activeAbilityIDs[manager.chosenToPick.ability.id] != true)
            {
                // Add ability to hotbar
                this.ability = manager.chosenToPick.ability;
                manager.AddAbilityToHotbar(manager.chosenToPick.ability, int.Parse(abilitySlotId));
                manager.hotbarSlots[int.Parse(abilitySlotId) - 1] = manager.chosenToPick.ability;
                // Set Icon to hotbar
                Transform abilityFromSlot = manager.chosenToPick.transform.GetChild(0);
                abilityFromSlot.GetComponent<Image>().sprite = manager.chosenToPick.ability.icon;
                // Reset used
                Transform hotbarAbilitySlotT = hotbarSlots.transform.GetChild(int.Parse(abilitySlotId) - 1);
                hotbarAbilitySlotT.GetComponent<AbilitySlot>().addAbilityButton.gameObject.SetActive(false);
                manager.chosenToPick = null;
                FindFreeInvHotbarSlotAndDeactivate();
            }
            // Reset if ability is already in hotbar
            else
            {
                Transform abilityFromSlot = manager.chosenToPick.transform.GetChild(0);
                abilityFromSlot.GetComponent<Image>().sprite = manager.chosenToPick.ability.icon;
                manager.chosenToPick = null;
                FindFreeInvHotbarSlotAndDeactivate();
            }
        }
    }

    public void RemoveAbility()
    {
        string slotId = this.name.Replace("Slot", "");
        // Reset used
        Transform hotbarAbilitySlotT = hotbarSlots.transform.GetChild(int.Parse(slotId) - 1);
        hotbarAbilitySlotT.GetComponent<AbilitySlot>().removeAbilityButton.gameObject.SetActive(false);
        // Remove ability from battle hotbar
        Transform battleHotbarAbilitySlotT = battleHotbarSlots.transform.GetChild(int.Parse(slotId) - 1);
        battleHotbarAbilitySlotT.GetComponent<AbilityHotbar>().ability = null;
        battleHotbarAbilitySlotT.GetComponent<AbilityHotbar>().isAdded = false;
        Destroy(battleHotbarAbilitySlotT.GetChild(0).gameObject);
        battleHotbarAbilitySlotT.Find("Manacost").gameObject.SetActive(false); // Disable Manacost
        battleHotbarAbilitySlotT.Find("Time").gameObject.SetActive(false); // Disable Time
        battleHotbarAbilitySlotT.Find("Cooldown")
            .GetComponent<RectTransform>().sizeDelta = new Vector2(100, 0); // Disable Cooldown
        // Remove ability from hotbar
        Destroy(hotbarAbilitySlotT.GetChild(0).gameObject);
        manager.hotbarSlots[int.Parse(slotId) - 1] = null;
        manager.activeAbilityIDs[ability.id] = false;
        this.ability = null;
        // Update add and remove buttons
        FindFreeInvHotbarSlotAndDeactivate();
    }

    public void SetAbilityUI()
    {
        AbilitySlot chosenSlot = manager.chosenToPick;
        Transform chosenAbilityTransform = manager.chosenToPick.transform.GetChild(0);
        chosenAbilityTransform.GetComponent<Image>().sprite = chosenSlot.ability.icon;
    }

    void UpdateUI()
    {
        if (this.ability != null && !this.isAdded && isBank)
        {
            string name = this.name;
            name = name.Replace("Slot", "");
            manager.AddAbilityToSlot(this.ability, int.Parse(name));
            this.isAdded = true;
        }
        else if (this.ability != null && !this.isAdded && !isBank)
        {
            string name = this.name;
            name = name.Replace("Slot", "");
            manager.AddAbilityToHotbar(this.ability, int.Parse(name));
            this.isAdded = true;
        }

        if (!isBank && this.ability != null)
        {
            removeAbilityButton.gameObject.SetActive(!this.ability.isCooldown);
        }
    }

    public void SaveAbility()
    {
        if(ability != null)
        {
            ability.startCooldown = manager.battle.GetComponent<Battle>().startCooldown;
            SaveSystem.SaveSkill(ability, this.name);
        }
        else
        {
            SaveSystem.SaveSkill(null, this.name);
        } 
    }

    public void LoadAbility()
    {
        SkillData data = SaveSystem.LoadSkill(this.name);
        if (data != null && data.abilityId != -10)
        {
            manager.abilitiesPool.TryGetValue(data.abilityId, out this.ability);

            ability.isCooldown = data.isCooldown;
            ability.cooldown = data.cooldown;
            ability.startCooldown = data.currentTime;
        }
        else
        {
            ability = null;
        }
    }

    public void SaveHotbarAbility()
    {
        if (ability != null)
        {
            SaveSystem.SaveHotbarSkill(ability, this.name);
        }
        else
        {
            SaveSystem.SaveHotbarSkill(null, this.name);
        }
    }

    public void LoadHotbarAbility()
    {
        SkillData data = SaveSystem.LoadHotbarSkill(this.name);

        if (data != null && data.abilityId != -10)
        {
            manager.abilitiesPool.TryGetValue(data.abilityId, out this.ability);
            
            ability.isCooldown = data.isCooldown;
            ability.cooldown = data.cooldown;
            ability.startCooldown = data.currentTime;

            if (ability.isCooldown)
            {
                if (ability.startCooldown != ability.cooldown)
                {
                    string n = this.name;
                    n = n.Replace("Slot", "");
                    int i = int.Parse(n);
                    ability.cooldownObj = battleHotbarSlots.transform.GetChild(i-1).GetChild(0).gameObject;
                    ability.OnCooldown(ability.cooldown - ability.startCooldown);
                    ability.isLoaded = true;
                }
            }
        }
        else
        {
            ability = null;
        }
    }
}
