using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Ability : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
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
        // Detect if hotbar
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
        // If HotbarAbility
        if (isHotbar)
        {
            if (isEnter && this.gameObject == enterObj && enemyAbility == null)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    if (manager.battle.GetComponent<Battle>().IsBattleOn())
                    {
                        if (ability != null && transform.parent.GetComponent<AbilityHotbar>() != null)
                        {
                            GetHotbarSlot().UseAbility();
                        }
                        if (godAbility != null && transform.parent.GetComponent<GodAbilityHotbar>() != null)
                        {
                            GetGodHotbarSlot().UseAbility();
                        }
                    }
                }
            }
        }
        // EnemyAbility Case
        if (enemyAbility != null)
        {
            enemyAbility.UseAbility();
        }
        // Delete Ability if none game ability
        if (ability == null && godAbility == null && enemyAbility == null)
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

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!isHotbar)
        {
            GetSlot().PickAbility();
        }
    }
}
