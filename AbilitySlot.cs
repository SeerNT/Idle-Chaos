using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AbilitySlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private AbilityManager manager;

    public GameAbility ability;
    public bool isAdded = false;

    public GameObject enterObj;
    public bool isEnter = false;

    void Update()
    {
        UpdateUI();
        if(isEnter && this.gameObject == enterObj)
        {
            if (Input.GetMouseButtonDown(0))
            {
                PickAbility();
            }
        }
    }

    public void PickAbility()
    {
        if(manager.choosingState == AbilityManager.ChoosingState.WAITING || manager.choosingState == AbilityManager.ChoosingState.DONE)
        {
            if(ability != null)
            {
                manager.chosenToPick = this;
                this.transform.GetChild(0).GetComponent<Image>().sprite = ability.chosenIcon;
                this.transform.SetAsLastSibling();

                manager.choosingState = AbilityManager.ChoosingState.CHOOSING;
            }
        }
        else
        {
            SetAbility();
        }
        
    }

    public void SetAbility()
    {
        if (manager.chosenToReplace != null)
        {
            manager.chosenToPick = null;
            manager.chosenToReplace = null;
        }
        else
        {
            if(manager.chosenToPick != null)
            {
                manager.chosenToReplace = this;
                Transform chosenParent = manager.chosenToPick.transform.parent;
                AbilitySlot chosenSlot = manager.chosenToPick;
                GameAbility chosenAbility = chosenSlot.ability;
                Transform replaceParent = manager.chosenToReplace.transform.parent;
                AbilitySlot replaceSlot = manager.chosenToReplace;
                GameAbility replaceAbility = replaceSlot.ability;

                Transform pickAbility = chosenSlot.transform.GetChild(0);

                SetAbilityUI();

                string abilitySlotId = this.name.Replace("Slot", "");

                if (replaceAbility != null && (replaceParent.name.Contains("Slots") && chosenParent.name.Contains("Available")))
                {
                    this.ability = chosenAbility;

                    replaceSlot.isEnter = false;
                    replaceSlot.enterObj = null;

                    manager.activeAbilityIDs[chosenAbility.id] = true;
                    manager.activeAbilityIDs[replaceAbility.id] = false;

                    //manager.activeAbilityIDs[this.ability.id] = false;

                    string nameRep = manager.chosenToReplace.name;
                    nameRep = nameRep.Replace("Slot", "");

                    manager.hotbarSlots[int.Parse(nameRep) - 1] = chosenAbility;
                    AbilityHotbar hotbarSlot = manager.battle.transform.Find("Slots").Find(replaceSlot.name).GetComponent<AbilityHotbar>();
                    hotbarSlot.ability = manager.chosenToPick.ability;
                    hotbarSlot.transform.GetChild(0).GetComponent<Image>().sprite = pickAbility.GetComponent<Ability>().ability.icon;
                    hotbarSlot.transform.GetChild(0).GetComponent<Ability>().ability = pickAbility.GetComponent<Ability>().ability;

                    Transform sendAbility = replaceSlot.transform.GetChild(0);
                    sendAbility.GetComponent<Ability>().ability = pickAbility.GetComponent<Ability>().ability;
                    sendAbility.GetComponent<Image>().sprite = pickAbility.GetComponent<Ability>().ability.icon;
                    replaceSlot.transform.GetChild(0).GetComponent<Image>().sprite = pickAbility.GetComponent<Ability>().ability.icon;

                    pickAbility.GetComponent<Image>().sprite = chosenAbility.icon;
                    pickAbility = manager.chosenToReplace.transform.GetChild(0);
                    //pickAbility.GetComponent<Image>().sprite = replaceAbility.icon;

                    if (manager.activeAbilityIDs[manager.chosenToPick.ability.id] != true)
                    {
                        string name = chosenSlot.name;
                        if (name.Contains("Slot"))
                        {
                            name = name.Replace("Slot", "");

                            chosenSlot.transform.SetSiblingIndex(int.Parse(name) - 1);
                        }
                    }
                    else
                    {
                        pickAbility = manager.chosenToPick.transform.GetChild(0);
                        pickAbility.GetComponent<Image>().sprite = chosenAbility.icon;
                    }
                }
                else
                {
                    if (manager.activeAbilityIDs[manager.chosenToPick.ability.id] != true)
                    {
                        this.ability = manager.chosenToPick.ability;
                        manager.AddAbilityToHotbar(manager.chosenToPick.ability, int.Parse(abilitySlotId));
                        string name = manager.chosenToPick.name;
                        AbilityHotbar hotbarSlot = manager.battle.transform.Find("Slots").Find(name).GetComponent<AbilityHotbar>();

                        if (name.Contains("Slot"))
                        {
                            name = name.Replace("Slot", "");

                            manager.chosenToPick.transform.SetSiblingIndex(int.Parse(name) - 1);
                        }
                        Transform abilityFromSlot = manager.chosenToPick.transform.GetChild(0);
                        abilityFromSlot.GetComponent<Image>().sprite = manager.chosenToPick.ability.icon;
                        manager.chosenToPick = null;
                        manager.chosenToReplace = null;
                        manager.choosingState = AbilityManager.ChoosingState.DONE;
                    }
                    else
                    {
                        Transform abilityFromSlot = manager.chosenToPick.transform.GetChild(0);
                        abilityFromSlot.GetComponent<Image>().sprite = manager.chosenToPick.ability.icon;
                        manager.chosenToPick = null;
                        manager.chosenToReplace = null;
                        manager.choosingState = AbilityManager.ChoosingState.DONE;
                    }
                }
            }
            else
            {
                manager.choosingState = AbilityManager.ChoosingState.DONE;
            }
        }
    }

    public void SetAbilityUI()
    {
        AbilitySlot chosenSlot = manager.chosenToPick;
        Transform chosenAbilityTransform = manager.chosenToPick.transform.GetChild(0);

        manager.chosenToReplace = this;

        chosenAbilityTransform.GetComponent<Image>().sprite = chosenSlot.ability.icon;
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

    void UpdateUI()
    {
        if (this.ability != null && !this.isAdded && !this.transform.parent.name.Contains("Slots"))
        {
            string name = this.name;
            name = name.Replace("Slot", "");
            manager.AddAbilityToSlot(this.ability, int.Parse(name));
            this.isAdded = true;
        }
        else if (this.ability != null && !this.isAdded && this.transform.parent.name.Contains("Slots"))
        {
            string name = this.name;
            name = name.Replace("Slot", "");
            manager.AddAbilityToHotbar(this.ability, int.Parse(name));
            this.isAdded = true;
        }
    }

    public void SaveAbility()
    {
        if(ability != null)
        {
            ability.startCooldown = manager.battle.GetComponent<Battle>().startCooldown;
            SaveSystem.SaveSkill(ability, this.name);
        } 
    }

    public void LoadAbility()
    {
        SkillData data = SaveSystem.LoadSkill(this.name);

        if (data != null)
        {
            manager.abilitiesPool.TryGetValue(data.abilityId, out this.ability);

            ability.isCooldown = data.isCooldown;
            ability.cooldown = data.cooldown;
            ability.startCooldown = data.currentTime;

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
            ability = null;
        }
    }

    public void SaveHotbarAbility()
    {
        if (ability != null)
        {
            SaveSystem.SaveHotbarSkill(ability, this.name);
        }
    }

    public void LoadHotbarAbility()
    {
        SkillData data = SaveSystem.LoadHotbarSkill(this.name);

        if (data != null)
        {
            manager.abilitiesPool.TryGetValue(data.abilityId, out this.ability);
            
            ability.isCooldown = data.isCooldown;
            ability.cooldown = data.cooldown;
            ability.startCooldown = data.currentTime;

            if (ability.isCooldown)
            {
                if (ability.startCooldown != ability.cooldown)
                {
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
