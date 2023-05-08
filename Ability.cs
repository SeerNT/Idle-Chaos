using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Ability : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public AbilityManager manager;

    public GameAbility ability;
    public EnemyGameAbility enemyAbility;
    public GodAbility godAbility;
    public bool isHotbar;

    public GameObject enterObj;
    public bool isEnter = false;

    void Update()
    {
        if(godAbility == null)
        {
            if (this.transform.parent.parent.name.Contains("Slots"))
            {
                isHotbar = true;
            }
            else
            {
                isHotbar = false;
            }
        }
        

        if (isEnter && this.gameObject == enterObj && enemyAbility == null)
        {
            if (GetSlot() != null)
            {
                if (Input.GetMouseButtonDown(1) && isHotbar && !GetSlot().ability.isCooldown)
                {
                    if (manager.chosenToPick != null)
                    {
                        manager.chosenToPick.isEnter = false;
                        manager.chosenToPick.enterObj = null;


                        string name = manager.chosenToPick.name;
                        if (name.Contains("Slot"))
                        {
                            name = name.Replace("Slot", "");
                            manager.chosenToPick.transform.SetSiblingIndex(int.Parse(name) - 1);
                        }
                    }
                    manager.activeAbilityIDs[this.ability.id] = false;
                    manager.chosenToPick = null;
                    manager.chosenToReplace = null;
                    name = this.GetSlot().name;
                    this.GetSlot().ability = null;
                    this.GetSlot().isAdded = false;

                    AbilityHotbar hotbarSlot = manager.battle.transform.Find("Slots").Find(this.GetSlot().name).GetComponent<AbilityHotbar>();
                    hotbarSlot.ability = null;
                    hotbarSlot.isAdded = false;

                    name = name.Replace("Slot", "");
                    manager.hotbarSlots[int.Parse(name) - 1] = null;
                    Destroy(this.gameObject);
                    Destroy(hotbarSlot.transform.Find("Ability").gameObject);
                    
                }
            }
            else {
                if (Input.GetMouseButtonDown(0))
                {
                    if (manager.battle.GetComponent<Battle>().IsBattleOn())
                    {
                        if(ability != null)
                        {
                            GetHotbarSlot().UseAbility();
                        }
                        if(godAbility != null)
                        {
                            GetGodHotbarSlot().UseAbility();
                        }
                    }
                }
            }
        }

        if(enemyAbility != null)
        {
            enemyAbility.UseAbility();
        }

        if(ability == null && godAbility == null && enemyAbility == null)
        {
            Destroy(this.gameObject);
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

    public AbilitySlot GetSlot()
    {
        return transform.parent.GetComponent<AbilitySlot>();
    }

    public AbilityHotbar GetHotbarSlot()
    {
        return transform.parent.GetComponent<AbilityHotbar>();
    }

    public GodAbilityHotbar GetGodHotbarSlot()
    {
        return transform.parent.GetComponent<GodAbilityHotbar>();
    }
}
