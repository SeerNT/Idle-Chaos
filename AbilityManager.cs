using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilityManager : MonoBehaviour
{
    public enum ChoosingState
    {
        WAITING,
        CHOOSING,
        DONE
    }

    public Transform battle;
    public Transform hotbar;

    public AbilitySlot chosenToPick;
    public AbilitySlot chosenToReplace;

    public bool[] activeAbilityIDs = new bool[100];

    public ChoosingState choosingState = ChoosingState.WAITING;

    public GameAbility[] slots = new GameAbility[60];
    public GameAbility[] hotbarSlots = new GameAbility[5];
    public GodAbility[] godSlots = new GodAbility[3];

    public GameAbility[] availableAbilities;
    public EnemyGameAbility[] availableEnemyAbilities;
    public GodAbility[] availableGodAbilities;

    public Dictionary<int, GameAbility> abilitiesPool = new Dictionary<int, GameAbility>();
    public Dictionary<int, GodAbility> godAbilitiesPool = new Dictionary<int, GodAbility>();
    public Dictionary<int, EnemyGameAbility> enemyAbilitiesPool = new Dictionary<int, EnemyGameAbility>();

    public void Awake()
    {
        for (int i = 0; i < availableAbilities.Length; i++)
        {
            abilitiesPool.Add(i+1, availableAbilities[i]);
        }
        for (int i = 0; i < availableEnemyAbilities.Length; i++)
        {
            enemyAbilitiesPool.Add(i + 1, availableEnemyAbilities[i]);
        }
        for (int i = 0; i < availableGodAbilities.Length; i++)
        {
            godAbilitiesPool.Add(i + 1, availableGodAbilities[i]);
        }
    }

    internal void AddAbilityToEnemyHotbar(EnemyGameAbility ability, int slotId)
    {
        GameObject go = AddGameObjectToBattleEnemy(slotId);
        PrepareGameObject(go, ability);
        go.GetComponent<Ability>().enemyAbility.cooldownObj = battle.Find("Enemy").Find("Slots").Find("Slot" + slotId.ToString()).Find("Cooldown").gameObject;
        EnemyAbilitySlot slot = battle.Find("Enemy").Find("Slots").Find("Slot" + slotId.ToString()).GetComponent<EnemyAbilitySlot>();
        slot.ability = ability;
    }

    public GameObject AddGameObject(int slotId, bool isHotbar)
    {
        GameObject go = new GameObject("Ability");
        
        if (!isHotbar)
        {
            Transform slot = transform.Find("Available").transform.Find("Slot" + slotId);
            go.GetComponent<Transform>().SetParent(slot);
        }
        else
        {
            Transform slot = transform.Find("Slots").transform.Find("Slot" + slotId);
            go.GetComponent<Transform>().SetParent(slot);
            go.GetComponent<Transform>().SetAsFirstSibling();
        }
        
        return go;
    }

    public GameObject AddGameObjectToBattleHotbar(int slotId)
    {
        GameObject go = new GameObject("Ability");
        Transform slot = battle.Find("Slots").transform.Find("Slot" + slotId);
        go.GetComponent<Transform>().SetParent(slot);

        return go;
    }

    public GameObject AddGameObjectToBattleEnemy(int slotId)
    {
        GameObject go = new GameObject("Ability");
        Transform slot = battle.Find("Enemy").Find("Slots").transform.Find("Slot" + slotId);
        go.GetComponent<Transform>().SetParent(slot);

        return go;
    }

    public GameObject AddGameObjectToBattleGodSlot(int slotId)
    {
        
        GameObject go = new GameObject("Ability");
        Transform slot = battle.Find("Player").Find("GodSlots").transform.Find("Slot" + slotId);
        go.GetComponent<Transform>().SetParent(slot);

        return go;
    }

    public void PrepareGameObject(GameObject go, GameAbility ability)
    {
        go.AddComponent<Ability>();
        go.GetComponent<Ability>().ability = ability;
        go.GetComponent<Ability>().manager = this;
        go.GetComponent<Ability>().ability.battle = battle.GetComponent<Battle>();
        
        go.AddComponent<Image>();
        go.GetComponent<Image>().sprite = go.GetComponent<Ability>().ability.icon;
        go.transform.localPosition = Vector3.zero;
        go.GetComponent<RectTransform>().sizeDelta = new Vector2(90, 90);
        go.GetComponent<RectTransform>().SetAsFirstSibling();
    }

    public void PrepareGameObject(GameObject go, GodAbility ability)
    {
        go.AddComponent<Ability>();
        go.GetComponent<Ability>().godAbility = ability;
        go.GetComponent<Ability>().manager = this;
        go.GetComponent<Ability>().godAbility.battle = battle.GetComponent<Battle>();

        go.AddComponent<Image>();
        go.GetComponent<Image>().sprite = go.GetComponent<Ability>().godAbility.icon;
        go.transform.localPosition = Vector3.zero;
        go.GetComponent<RectTransform>().sizeDelta = new Vector2(90, 90);
        go.GetComponent<RectTransform>().SetAsFirstSibling();
    }

    public void PrepareGameObject(GameObject go, EnemyGameAbility ability)
    {
        go.AddComponent<Ability>();
        go.GetComponent<Ability>().enemyAbility = ability;
        go.GetComponent<Ability>().manager = this;
        go.GetComponent<Ability>().enemyAbility.battle = battle.GetComponent<Battle>();

        go.AddComponent<Image>();
        go.GetComponent<Image>().sprite = go.GetComponent<Ability>().enemyAbility.icon;
        go.transform.localPosition = Vector3.zero;
        go.GetComponent<RectTransform>().sizeDelta = new Vector2(90, 90);
        go.GetComponent<RectTransform>().SetAsFirstSibling();
    }

    public bool AddAbility(GameAbility ability)
    {
        bool isSuccess = false;
        for (int i = 0; i <= slots.Length; i++)
        {
            if (slots[i] == null)
            {
                slots[i] = ability;
                AbilitySlot slot = transform.Find("Available").Find("Slot" + (i+1)).GetComponent<AbilitySlot>();
                slot.ability = ability;
                GameObject go = AddGameObject(i + 1, false);
                PrepareGameObject(go, ability);
                isSuccess = true;
                break;
            }
        }
        return isSuccess;
    }

    public bool AddAbilityToSlot(GameAbility ability, int slotId)
    {
        bool isSuccess = false;
        if (slots[slotId - 1] == null)
        {
            slots[slotId - 1] = ability;
            GameObject go = AddGameObject(slotId, false);
            PrepareGameObject(go, ability);
            AbilitySlot slot = transform.Find("Available").Find("Slot" + (slotId)).GetComponent<AbilitySlot>();
            slot.ability = ability;
            isSuccess = true;
        }
        return isSuccess;
    }

    public bool AddAbilityToHotbar(GameAbility ability, int slotId)
    {
        bool isSuccess = false;
        if (hotbarSlots[slotId - 1] == null)
        {
            hotbarSlots[slotId - 1] = ability;
            GameObject go = AddGameObject(slotId, true);
            PrepareGameObject(go, ability);
            activeAbilityIDs[ability.id] = true;
            AbilitySlot slot = transform.Find("Slots").transform.Find("Slot" + (slotId)).GetComponent<AbilitySlot>();
            slot.ability = ability;
            isSuccess = true;
        }
        return isSuccess;
    }

    public void AddAbilityToBattleHotbar(GameAbility ability, int slotId)
    {
        GameObject go = AddGameObjectToBattleHotbar(slotId);
        PrepareGameObject(go, ability);
        go.GetComponent<Ability>().ability.cooldownObj = battle.Find("Slots").Find("Slot" + slotId.ToString()).Find("Cooldown").gameObject;
        activeAbilityIDs[ability.id] = true;
    }

    public void AddAbilityToBattleGodSlots(GodAbility ability)
    {
        for (int i = 0; i <= godSlots.Length; i++)
        {
            if (godSlots[i] == null)
            {
                godSlots[i] = ability;
                GodAbilityHotbar slot = battle.Find("Player").Find("GodSlots").Find("Slot" + (i + 1).ToString()).GetComponent<GodAbilityHotbar>();
                slot.ability = ability;
                GameObject go = AddGameObjectToBattleGodSlot(i+1);
                PrepareGameObject(go, ability);
                go.GetComponent<Ability>().godAbility.cooldownObj = battle.Find("Player").Find("GodSlots").Find("Slot" + (i + 1).ToString()).Find("Cooldown").gameObject;
                activeAbilityIDs[ability.id + 1000] = true;
                break;
            }
        }
    }

    public void AddAbilityToBattleGodSlot(GodAbility ability, int slotId)
    {
        GameObject go = AddGameObjectToBattleGodSlot(slotId);
        PrepareGameObject(go, ability);
        godSlots[slotId-1] = ability;
        go.GetComponent<Ability>().godAbility.cooldownObj = battle.Find("Player").Find("GodSlots").Find("Slot" + (slotId).ToString()).Find("Cooldown").gameObject;
        activeAbilityIDs[ability.id + 1000] = true;
    }

    public static GodAbility GetGodAbilityById(int id)
    {
        GodAbility ability = Resources.Load<GodAbility>("Creatable/Abilities/Godlike/GodAbility" + id.ToString());
        return ability;
    }

    public void SaveHotbarAbility()
    {
        for (int i = 0; i < hotbarSlots.Length; i++)
        {
            GameAbility ability = hotbarSlots[i];

            if(ability != null)
            {
                SaveSystem.SaveHotbarSkill(ability, i.ToString());
            }
        }
    }

    public void LoadHotbarAbility()
    {
        for (int i = 0; i < hotbarSlots.Length; i++)
        {
            SkillData data = SaveSystem.LoadHotbarSkill(i.ToString());

            if (data != null)
            {
                for (int j = 0; j < hotbar.childCount; i++)
                {
                    abilitiesPool.TryGetValue(data.abilityId, out hotbar.GetChild(j).GetComponent<AbilityHotbar>().ability);
                    GameAbility ability = hotbar.GetChild(j).GetComponent<AbilityHotbar>().ability;
                    ability.cooldown = data.cooldown;
                    ability.startCooldown = data.currentTime;
                    if (!battle.GetComponent<Battle>().IsBattleOn())
                    {
                        if(ability.startCooldown != ability.cooldown)
                        {
                            ability.OnCooldown(ability.startCooldown);
                        }
                    }
                }
            }
        }
    }
}
