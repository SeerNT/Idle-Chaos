using NT_Utils.Conversion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemSetManager : MonoBehaviour
{
    public static ItemSet completedSet;

    [SerializeField] private Transform itemSetObject;
    [SerializeField] private Transform slots;
    [SerializeField] private Battle battle;

    private bool hasShown = false;

    private void Start()
    {
        CheckCompletedSet();
    }

    private void Update()
    {
        if(completedSet != null)
        {
            if(completedSet.completionStatus == completedSet.itemsIncluded.Length)
            {
                if (!hasShown)
                    UpdateUIBonuses();
            }
        }
        else
        {
            itemSetObject.transform.GetChild(1).GetComponent<Text>().text = "No Set :C";
            itemSetObject.transform.GetChild(2).gameObject.SetActive(false);
            itemSetObject.transform.GetChild(3).gameObject.SetActive(false);
            itemSetObject.transform.GetChild(4).gameObject.SetActive(false);
            itemSetObject.transform.GetChild(5).gameObject.SetActive(false);
            hasShown = false;
        }
    }

    public void ReturnBonuses(ItemSet completedSet)
    {
        if(completedSet != null)
        {
            battle.player.IncreaseStat(StatUpgrade.Stats.HP, -completedSet.bonusHP);
            battle.player.IncreaseStat(StatUpgrade.Stats.ARM, -completedSet.bonusArm);
            battle.player.IncreaseStat(StatUpgrade.Stats.MRM, -completedSet.bonusMgcArm);
            battle.player.IncreaseStat(StatUpgrade.Stats.PP, -completedSet.bonusPP);
            battle.player.IncreaseStat(StatUpgrade.Stats.MP, -completedSet.bonusMP);
            battle.player.IncreaseStat(StatUpgrade.Stats.CRT, -completedSet.bonusCrit);
            battle.player.IncreaseStat(StatUpgrade.Stats.PRC, -completedSet.bonusPrc);
            battle.player.IncreaseStat(StatUpgrade.Stats.LCK, -completedSet.bonusLuck);
            battle.player.IncreaseStat(StatUpgrade.Stats.AS, -completedSet.bonusAS);
            battle.player.hpRegSpeed -= -completedSet.bonusHpReg;
            battle.player.manaRegSpeed -= -completedSet.bonusManaReg;
            battle.player.CheckStatLimit();
        }        
    }

    public void GiveBonuses()
    {
        if(completedSet != null)
        {
            battle.player.IncreaseStat(StatUpgrade.Stats.HP, completedSet.bonusHP);
            battle.player.IncreaseStat(StatUpgrade.Stats.ARM, completedSet.bonusArm);
            battle.player.IncreaseStat(StatUpgrade.Stats.MRM, completedSet.bonusMgcArm);
            battle.player.IncreaseStat(StatUpgrade.Stats.PP, completedSet.bonusPP);
            battle.player.IncreaseStat(StatUpgrade.Stats.MP, completedSet.bonusMP);
            battle.player.IncreaseStat(StatUpgrade.Stats.CRT, completedSet.bonusCrit);
            battle.player.IncreaseStat(StatUpgrade.Stats.PRC, completedSet.bonusPrc);
            battle.player.IncreaseStat(StatUpgrade.Stats.LCK, completedSet.bonusLuck);
            battle.player.IncreaseStat(StatUpgrade.Stats.AS, completedSet.bonusAS);
            battle.player.hpRegSpeed -= completedSet.bonusHpReg;
            battle.player.manaRegSpeed -= completedSet.bonusManaReg;
            battle.player.CheckStatLimit();
        }
    }

    public void CheckCompletedSet()
    {
        Dictionary<string, int> setsCompletion = new Dictionary<string, int>();
        for(int i = 0; i < slots.childCount; i++)
        {
            if(slots.GetChild(i).GetComponent<InventorySlot>().item != null)
            {
                GameItem item = slots.GetChild(i).GetComponent<InventorySlot>().item;
                if (item.itemSet != null)
                {
                    if (setsCompletion.ContainsKey(item.itemSet.id.ToString()))
                    {
                        setsCompletion[item.itemSet.id]++;
                    }
                    else
                    {
                        setsCompletion.Add(item.itemSet.id.ToString(), 1);
                    }
                }
            }
        }

        foreach(KeyValuePair<string, int> set in setsCompletion)
        {
            GetItemSetById(set.Key).completionStatus = set.Value;
            if (set.Value == GetItemSetById(set.Key).itemsIncluded.Length)
            {
                completedSet = GetItemSetById(set.Key);
            }
        }
    }

    public bool HasHolySet()
    {
        return GetItemSetById("8") == completedSet;
    }

    public ItemSet GetItemSetById(string id)
    {
        ItemSet set = Resources.Load<ItemSet>("Creatable/ItemSets/ItemSet" + id);
        return set;
    }

    private void UpdateUIBonuses()
    {
        if(completedSet.givenBonuses.Count == 0)
            completedSet.InitSet();

        hasShown = true;
        itemSetObject.transform.GetChild(1).GetComponent<Text>().text = completedSet.itemSetName;

        int i = 0;
        foreach (KeyValuePair<string, float> bonus in completedSet.givenBonuses)
        {
            i++;
            if (bonus.Value >= 0)
            {
                itemSetObject.transform.GetChild(i + 1).GetComponent<Text>().text = bonus.Key + ": " + NumberConversion.AbbreviateNumber(bonus.Value);
            }
            else
            {
                itemSetObject.transform.GetChild(i + 1).GetComponent<Text>().text = bonus.Key + ": " + NumberConversion.AbbreviateNumber(bonus.Value);
            }
            itemSetObject.transform.GetChild(i + 1).gameObject.SetActive(true);
        }
    }
}
