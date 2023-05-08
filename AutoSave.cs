﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class AutoSave : MonoBehaviour
{
    [Header("Attachments")]
    public Battle battle;
    public Reroll shop;
    public Transform upgrades;
    public Transform skills;
    public Transform availableSlots;
    public Transform hotbarSlots;
    public Transform inventorySlots;
    public Transform buySlots;
    public Transform sellSlots;
    public Transform playerSlots;
    public Transform trainings;
    public Transform techniques;
    public Transform offline;
    public Transform talants;
    public Transform talantMap;
    public Transform enemySlots;
    public Transform godSlots;
    public Transform herbSlots;
    public Transform growings;
    public Transform searchings;
    public Transit transit;
    public Reincarnation reincarnation;
    public Transform permanentUpgrades;
    public Transform permanentUpgrades2;
    public Transform dailyRewards;

    [Header("For Reset")]
    [SerializeField] private Transform bloodCurrency;
    [SerializeField] private Text loggerText;
    [SerializeField] private Transform hotbarsSkillSlots;

    [Header("Other")]
    public AbilityManager manager;

    public Text autoSaveUI_Text;

    public GatheringManager gatheringManager;

    public Inventory inventory;

    public CutsceneManager cutsceneManager;

    public int sec = 0;

    public static bool Loaded = false;
    public static bool Reseted = false;

    void Awake()
    {
        Load();
        InvokeRepeating("Save", 30f, 30f);
        InvokeRepeating("ChangeUI_Text", 0f, 1f);
    }

    void ChangeUI_Text()
    {
        sec++;
        autoSaveUI_Text.text = "AUTOSAVE\n" + sec +"s/30s";
        Reincarnation.totalPlayTime++;
    }

    public void SaveWithout()
    {
        sec = 0;

        if (!CutsceneManager.isShowing)
        {
            GameManager.SaveCurrency();
            battle.SaveBattle();
            battle.SavePlayer();
            battle.SaveEnemy();
            reincarnation.Save();

            shop.SaveShop();

            for (int i = 2; i < 8; i++)
            {
                upgrades.GetChild(i).GetComponent<StatUpgrade>().SaveStatUpgrade();
            }
            for (int i = 9; i < 15; i++)
            {
                upgrades.GetChild(i).GetComponent<UpgradeEnchancer>().SaveUpgradeEnchance();
            }
            for (int i = 1; i <= 3; i++)
            {
                skills.GetChild(i).GetChild(0).GetComponent<Skill>().SaveSkillUpg();
            }
            transit.SaveWindow();
            for (int i = 0; i < availableSlots.childCount; i++)
            {
                availableSlots.GetChild(i).GetComponent<AbilitySlot>().SaveAbility();
            }
            for (int i = 0; i < hotbarSlots.childCount; i++)
            {
                hotbarSlots.GetChild(i).GetComponent<AbilitySlot>().SaveHotbarAbility();
            }
            for (int i = 0; i < godSlots.childCount; i++)
            {
                godSlots.GetChild(i).GetComponent<GodAbilityHotbar>().SaveHotbarAbility();
            }
            for (int i = 0; i < enemySlots.childCount; i++)
            {
                enemySlots.GetChild(i).GetComponent<EnemyAbilitySlot>().SaveAbility();
            }
            for (int i = 0; i < herbSlots.childCount; i++)
            {
                herbSlots.GetChild(i).GetComponent<HerbSlot>().SaveHerb();
            }
            for (int i = 0; i < inventorySlots.childCount; i++)
            {
                inventorySlots.GetChild(i).GetComponent<InventorySlot>().SaveItem();
            }
            for (int i = 0; i < buySlots.childCount; i++)
            {
                buySlots.GetChild(i).GetComponent<BuySlot>().SaveItem();
            }
            for (int i = 0; i < sellSlots.childCount; i++)
            {
                sellSlots.GetChild(i).GetComponent<SellSlot>().SaveItem();
            }
            for (int i = 0; i < playerSlots.childCount; i++)
            {
                playerSlots.GetChild(i).GetComponent<InventorySlot>().SaveItem();
            }
            for (int i = 1; i < growings.childCount; i++)
            {
                growings.GetChild(i).GetComponent<Growing>().SaveGrowing();
            }
            for (int i = 1; i < searchings.childCount; i++)
            {
                searchings.GetChild(i).GetComponent<Searching>().SaveSearching();
            }
            for (int i = 1; i < permanentUpgrades.childCount; i++)
            {
                permanentUpgrades.GetChild(i).GetComponent<PermanentUpgrade>().Save();
            }
            for (int i = 1; i < permanentUpgrades2.childCount; i++)
            {
                permanentUpgrades2.GetChild(i).GetComponent<PermanentUpgrade>().Save();
            }
            for (int i = 0; i < trainings.childCount; i++)
            {
                trainings.GetChild(i).GetComponent<Train>().SaveTraining();
            }
            for (int i = 0; i < techniques.childCount; i++)
            {
                techniques.GetChild(i).GetComponent<Technique>().SaveTech();
            }
            for (int i = 0; i < techniques.childCount; i++)
            {
                techniques.GetChild(i).Find("Upgrade").GetChild(1).GetComponent<TechniqueUpgrade>().SaveUpgrade();
            }
            for (int i = 0; i < dailyRewards.childCount; i++)
            {
                dailyRewards.GetChild(i).GetComponent<DailyRewardSlot>().Save();
            }
            for (int i = 1; i < talants.childCount; i++)
            {
                talants.GetChild(i).GetComponent<Talant>().SaveTalant();
            }

            for (int i = 0; i < NegativeEffect.effectIds.Count; i++)
            {
                if (battle.player.negativeEffects[i] != null)
                    SaveSystem.SavePlayerNegativeEffect(battle.player.negativeEffects[i]);
                if (battle.enemy.negativeEffects[i] != null)
                    SaveSystem.SaveEnemyNegativeEffect(battle.enemy.negativeEffects[i]);
            }


            talantMap.GetComponent<TalantMove>().SaveMap();

            offline.GetComponent<OfflineProgress>().SaveOffline();
        }

        cutsceneManager.Save();
    }

    public void Save()
    {
        sec = 0;
        CancelInvoke();
        autoSaveUI_Text.text = "AUTOSAVE\n" + "Saving...";

        if (!CutsceneManager.isShowing)
        {
            GameManager.SaveCurrency();
            battle.SaveBattle();
            battle.SavePlayer();
            battle.SaveEnemy();
            reincarnation.Save();

            shop.SaveShop();

            for (int i = 2; i <= 8; i++)
            {
                upgrades.GetChild(i).GetComponent<StatUpgrade>().SaveStatUpgrade();
            }
            for (int i = 9; i < 15; i++)
            {
                upgrades.GetChild(i).GetComponent<UpgradeEnchancer>().SaveUpgradeEnchance();
            }
            for (int i = 1; i <= 3; i++)
            {
                skills.GetChild(i).GetChild(0).GetComponent<Skill>().SaveSkillUpg();
            }
            transit.SaveWindow();
            for (int i = 0; i < availableSlots.childCount; i++)
            {
                availableSlots.GetChild(i).GetComponent<AbilitySlot>().SaveAbility();
            }
            for (int i = 0; i < hotbarSlots.childCount; i++)
            {
                hotbarSlots.GetChild(i).GetComponent<AbilitySlot>().SaveHotbarAbility();
            }
            for (int i = 0; i < godSlots.childCount; i++)
            {
                godSlots.GetChild(i).GetComponent<GodAbilityHotbar>().SaveHotbarAbility();
            }
            for (int i = 0; i < enemySlots.childCount; i++)
            {
                enemySlots.GetChild(i).GetComponent<EnemyAbilitySlot>().SaveAbility();
            }
            for (int i = 0; i < herbSlots.childCount; i++)
            {
                herbSlots.GetChild(i).GetComponent<HerbSlot>().SaveHerb();
            }
            for (int i = 0; i < inventorySlots.childCount; i++)
            {
                inventorySlots.GetChild(i).GetComponent<InventorySlot>().SaveItem();
            }
            for (int i = 0; i < buySlots.childCount; i++)
            {
                buySlots.GetChild(i).GetComponent<BuySlot>().SaveItem();
            }
            for (int i = 0; i < sellSlots.childCount; i++)
            {
                sellSlots.GetChild(i).GetComponent<SellSlot>().SaveItem();
            }
            for (int i = 0; i < playerSlots.childCount; i++)
            {
                playerSlots.GetChild(i).GetComponent<InventorySlot>().SaveItem();
            }
            for (int i = 1; i < growings.childCount; i++)
            {
                growings.GetChild(i).GetComponent<Growing>().SaveGrowing();
            }
            for (int i = 1; i < searchings.childCount; i++)
            {
                searchings.GetChild(i).GetComponent<Searching>().SaveSearching();
            }
            for (int i = 1; i < permanentUpgrades.childCount; i++)
            {
                permanentUpgrades.GetChild(i).GetComponent<PermanentUpgrade>().Save();
            }
            for (int i = 1; i < permanentUpgrades2.childCount; i++)
            {
                permanentUpgrades2.GetChild(i).GetComponent<PermanentUpgrade>().Save();
            }
            for (int i = 0; i < trainings.childCount; i++)
            {
                trainings.GetChild(i).GetComponent<Train>().SaveTraining();
            }
            for (int i = 0; i < techniques.childCount; i++)
            {
                techniques.GetChild(i).GetComponent<Technique>().SaveTech();
            }
            for (int i = 0; i < techniques.childCount; i++)
            {
                techniques.GetChild(i).Find("Upgrade").GetChild(1).GetComponent<TechniqueUpgrade>().SaveUpgrade();
            }
            for (int i = 0; i < dailyRewards.childCount; i++)
            {
                dailyRewards.GetChild(i).GetComponent<DailyRewardSlot>().Save();
            }
            for (int i = 1; i < talants.childCount; i++)
            {
                talants.GetChild(i).GetComponent<Talant>().SaveTalant();
            }

            for (int i = 0; i < NegativeEffect.effectIds.Count; i++)
            {
                if (battle.player.negativeEffects[i] != null)
                    SaveSystem.SavePlayerNegativeEffect(battle.player.negativeEffects[i]);
                if (battle.enemy.negativeEffects[i] != null)
                    SaveSystem.SaveEnemyNegativeEffect(battle.enemy.negativeEffects[i]);
            }


            talantMap.GetComponent<TalantMove>().SaveMap();

            offline.GetComponent<OfflineProgress>().SaveOffline();
        }

        cutsceneManager.Save();
        autoSaveUI_Text.text = "AUTOSAVE\n" + "Saved!";
        InvokeRepeating("Save", 32f, 30f);
        InvokeRepeating("ChangeUI_Text", 2f, 1f);
    }

    void Load()
    {
        cutsceneManager.Load();

        if (!CutsceneManager.isShowing)
        {
            GameManager.LoadCurrency();
            battle.LoadBattle();
            battle.LoadPlayer();
            battle.LoadEnemy();

            reincarnation.Load();

            shop.LoadShop();

            for (int i = 2; i <= 8; i++)
            {
                upgrades.GetChild(i).GetComponent<StatUpgrade>().LoadStatUpgrade();
            }
            for (int i = 9; i < 15; i++)
            {
                upgrades.GetChild(i).GetComponent<UpgradeEnchancer>().LoadUpgradeEnchance();
            }
            for (int i = 1; i <= 3; i++)
            {
                skills.GetChild(i).GetChild(0).GetComponent<Skill>().LoadSkillUpg();
            }
            transit.LoadWindow();
            for (int i = 0; i < availableSlots.childCount; i++)
            {
                availableSlots.GetChild(i).GetComponent<AbilitySlot>().LoadAbility();
            }
            for (int i = 0; i < hotbarSlots.childCount; i++)
            {
                hotbarSlots.GetChild(i).GetComponent<AbilitySlot>().LoadHotbarAbility();
            }
            for (int i = 0; i < godSlots.childCount; i++)
            {
                godSlots.GetChild(i).GetComponent<GodAbilityHotbar>().LoadHotbarAbility();
            }
            for (int i = 0; i < enemySlots.childCount; i++)
            {
                enemySlots.GetChild(i).GetComponent<EnemyAbilitySlot>().LoadAbility();
            }
            for (int i = 0; i < herbSlots.childCount; i++)
            {
                herbSlots.GetChild(i).GetComponent<HerbSlot>().LoadHerb();
            }
            for (int i = 0; i < inventorySlots.childCount; i++)
            {
                inventorySlots.GetChild(i).GetComponent<InventorySlot>().LoadItem();
            }
            for (int i = 0; i < buySlots.childCount; i++)
            {
                buySlots.GetChild(i).GetComponent<BuySlot>().LoadItem();
            }
            for (int i = 0; i < sellSlots.childCount; i++)
            {
                sellSlots.GetChild(i).GetComponent<SellSlot>().LoadItem();
            }
            for (int i = 0; i < playerSlots.childCount; i++)
            {
                playerSlots.GetChild(i).GetComponent<InventorySlot>().LoadItem();
            }
            for (int i = 1; i < growings.childCount; i++)
            {
                growings.GetChild(i).GetComponent<Growing>().LoadGrowing();
            }
            for (int i = 1; i < searchings.childCount; i++)
            {
                searchings.GetChild(i).GetComponent<Searching>().LoadSearching();
            }
            for (int i = 1; i < permanentUpgrades.childCount; i++)
            {
                permanentUpgrades.GetChild(i).GetComponent<PermanentUpgrade>().Load();
            }
            for (int i = 1; i < permanentUpgrades2.childCount; i++)
            {
                permanentUpgrades2.GetChild(i).GetComponent<PermanentUpgrade>().Load();
            }
            for (int i = 0; i < trainings.childCount; i++)
            {
                trainings.GetChild(i).GetComponent<Train>().LoadTraining();
            }
            for (int i = 0; i < techniques.childCount; i++)
            {
                techniques.GetChild(i).GetComponent<Technique>().LoadTech();
            }
            for (int i = 0; i < techniques.childCount; i++)
            {
                techniques.GetChild(i).Find("Upgrade").GetChild(1).GetComponent<TechniqueUpgrade>().LoadUpgrade();
            }
            for (int i = 0; i < dailyRewards.childCount; i++)
            {
                dailyRewards.GetChild(i).GetComponent<DailyRewardSlot>().Load();
            }
            for (int i = 1; i < talants.childCount; i++)
            {
                talants.GetChild(i).GetComponent<Talant>().LoadTalant();
            }

            talantMap.GetComponent<TalantMove>().LoadMap();
            AutoSave.Loaded = true;
        }
        
    }

    void LoadAfterReincarnation()
    {
        cutsceneManager.Load();

        if (!CutsceneManager.isShowing)
        {
            GameManager.LoadCurrency();
            battle.LoadBattle();
            battle.LoadPlayer();
            battle.LoadEnemy();

            reincarnation.Load();

            shop.LoadShop();

            for (int i = 2; i <= 8; i++)
            {
                upgrades.GetChild(i).GetComponent<StatUpgrade>().LoadStatUpgrade();
            }
            for (int i = 9; i < 15; i++)
            {
                upgrades.GetChild(i).GetComponent<UpgradeEnchancer>().LoadUpgradeEnchance();
            }
            for (int i = 1; i <= 3; i++)
            {
                skills.GetChild(i).GetChild(0).GetComponent<Skill>().LoadSkillUpg();
            }
            transit.LoadWindow();
            for (int i = 0; i < availableSlots.childCount; i++)
            {
                availableSlots.GetChild(i).GetComponent<AbilitySlot>().LoadAbility();
            }
            for (int i = 0; i < hotbarSlots.childCount; i++)
            {
                hotbarSlots.GetChild(i).GetComponent<AbilitySlot>().LoadHotbarAbility();
            }
            for (int i = 0; i < godSlots.childCount; i++)
            {
                godSlots.GetChild(i).GetComponent<GodAbilityHotbar>().LoadHotbarAbility();
            }
            for (int i = 0; i < enemySlots.childCount; i++)
            {
                enemySlots.GetChild(i).GetComponent<EnemyAbilitySlot>().LoadAbility();
            }
            for (int i = 0; i < herbSlots.childCount; i++)
            {
                herbSlots.GetChild(i).GetComponent<HerbSlot>().LoadHerb();
            }
            for (int i = 0; i < playerSlots.childCount; i++)
            {
                if(playerSlots.GetChild(i).GetComponent<InventorySlot>().item != null)
                {
                    battle.player.EquipItem(playerSlots.GetChild(i).GetComponent<InventorySlot>().item, playerSlots.GetChild(i).GetComponent<InventorySlot>().lvl);
                }
                    
            }
            for (int i = 0; i < buySlots.childCount; i++)
            {
                //buySlots.GetChild(i).GetComponent<BuySlot>().LoadItem();
            }
            for (int i = 0; i < sellSlots.childCount; i++)
            {
                //sellSlots.GetChild(i).GetComponent<SellSlot>().LoadItem();
            }
            for (int i = 0; i < playerSlots.childCount; i++)
            {
                //playerSlots.GetChild(i).GetComponent<InventorySlot>().LoadItem();
            }
            for (int i = 1; i < growings.childCount; i++)
            {
                growings.GetChild(i).GetComponent<Growing>().LoadGrowing();
            }
            for (int i = 1; i < searchings.childCount; i++)
            {
                searchings.GetChild(i).GetComponent<Searching>().LoadSearching();
            }
            for (int i = 1; i < permanentUpgrades.childCount; i++)
            {
                permanentUpgrades.GetChild(i).GetComponent<PermanentUpgrade>().Load();
            }
            for (int i = 1; i < permanentUpgrades2.childCount; i++)
            {
                permanentUpgrades2.GetChild(i).GetComponent<PermanentUpgrade>().Load();
            }
            for (int i = 0; i < trainings.childCount; i++)
            {
                trainings.GetChild(i).GetComponent<Train>().LoadTraining();
            }
            for (int i = 0; i < techniques.childCount; i++)
            {
                techniques.GetChild(i).GetComponent<Technique>().LoadTech();
            }
            for (int i = 0; i < techniques.childCount; i++)
            {
                techniques.GetChild(i).Find("Upgrade").GetChild(1).GetComponent<TechniqueUpgrade>().LoadUpgrade();
            }
            for (int i = 0; i < dailyRewards.childCount; i++)
            {
                dailyRewards.GetChild(i).GetComponent<DailyRewardSlot>().Load();
            }
            for (int i = 1; i < talants.childCount; i++)
            {
                talants.GetChild(i).GetComponent<Talant>().LoadTalant();
            }

            talantMap.GetComponent<TalantMove>().LoadMap();
            AutoSave.Loaded = true;
        }

    }

    public void ResetByReincarnation()
    {
        sec = 0;
        CancelInvoke("ChangeUI_Text");
        CancelInvoke("Save");
        autoSaveUI_Text.text = "AUTOSAVE\n" + "Saving...";
        SaveSystem.ResetCurrency(); 
        if (battle.isBattle)
        {
            battle.isBattle = false;
            battle.timerPlayer.Change(Timeout.Infinite, Timeout.Infinite);
            battle.timerEnemy.Change(Timeout.Infinite, Timeout.Infinite);
        }

        for (int i = 1; i < permanentUpgrades.childCount; i++)
        {
            permanentUpgrades.GetChild(i).GetComponent<PermanentUpgrade>().Save();
        }
        for (int i = 1; i < permanentUpgrades2.childCount; i++)
        {
            permanentUpgrades2.GetChild(i).GetComponent<PermanentUpgrade>().Save();
        }

        SaveSystem.ResetBattle(); 
        SaveSystem.ResetPlayer(); 
        SaveSystem.ResetEnemy(); 

        SaveSystem.ResetShop(); 

        for (int i = 2; i < 8; i++)
        {
            SaveSystem.ResetStatUpgrade(upgrades.GetChild(i).GetComponent<StatUpgrade>()); 
        }
        for (int i = 9; i < 15; i++)
        {
            string nm = upgrades.GetChild(i).name.Replace("_Upgrader", "");
            SaveSystem.ResetUpgradeEnchance(nm); 
        }
        UpgradeWindow.isOpen = false;
        for (int i = 1; i <= 3; i++)
        {
            SaveSystem.ResetSkillUpgrade(skills.GetChild(i).GetChild(0).GetComponent<Skill>(), skills.GetChild(i).GetChild(0).name, skills.GetChild(i).name);
        }
        SaveSystem.ResetWindow(); 
        for (int i = 0; i < availableSlots.childCount; i++)
        {
            if (availableSlots.GetChild(i).GetComponent<AbilitySlot>().ability != null)
            {
                availableSlots.GetChild(i).GetComponent<AbilitySlot>().ability.startCooldown = 0;
                availableSlots.GetChild(i).GetComponent<AbilitySlot>().ability.isCooldown = false;
                availableSlots.GetChild(i).GetComponent<AbilitySlot>().ability = null;
                SaveSystem.ResetSkill(availableSlots.GetChild(i).name);
                Destroy(availableSlots.GetChild(i).Find("Ability").gameObject);
            }
            else
            {
                SaveSystem.ResetSkill(availableSlots.GetChild(i).name);
            }
        }
        for (int i = 0; i < hotbarSlots.childCount; i++)
        {
            if (hotbarSlots.GetChild(i).GetComponent<AbilitySlot>().ability != null)
            {
                manager.slots[i] = null;
                manager.activeAbilityIDs[hotbarSlots.GetChild(i).GetComponent<AbilitySlot>().ability.id] = false;
                hotbarSlots.GetChild(i).GetComponent<AbilitySlot>().ability.startCooldown = 0;
                hotbarSlots.GetChild(i).GetComponent<AbilitySlot>().ability.isCooldown = false;
                hotbarSlots.GetChild(i).GetComponent<AbilitySlot>().ability = null;
                SaveSystem.ResetHotbarSkill(hotbarSlots.GetChild(i).name);
                Destroy(hotbarSlots.GetChild(i).Find("Ability").gameObject);
            }
            else
            {
                SaveSystem.ResetHotbarSkill(hotbarSlots.GetChild(i).name);
            }
            
        }
        for(int i = 0; i < hotbarsSkillSlots.childCount; i++)
        {
            if (hotbarsSkillSlots.GetChild(i).GetComponent<AbilityHotbar>().ability != null)
            {
                manager.hotbarSlots[i] = null;
                hotbarsSkillSlots.GetChild(i).GetComponent<AbilityHotbar>().ability.startCooldown = 0;
                hotbarsSkillSlots.GetChild(i).GetComponent<AbilityHotbar>().ability.isCooldown = false;
                hotbarsSkillSlots.GetChild(i).GetComponent<AbilityHotbar>().isAdded = false;
                hotbarsSkillSlots.GetChild(i).GetComponent<AbilityHotbar>().ability = null;
                hotbarsSkillSlots.GetChild(i).GetComponent<AbilityHotbar>().Reset();
                hotbarsSkillSlots.GetChild(i).Find("Cooldown").GetComponent<RectTransform>().sizeDelta = new Vector2(100, 0);
                battle.Reset();
                Destroy(hotbarsSkillSlots.GetChild(i).Find("Ability").gameObject);
            }
                
        }
        for (int i = 0; i < godSlots.childCount; i++)
        {
            SaveSystem.ResetGodSkill(godSlots.GetChild(i).name);
            if (godSlots.GetChild(i).GetComponent<GodAbilityHotbar>().ability != null)
            {
                manager.godSlots[i] = null;
                godSlots.GetChild(i).GetComponent<GodAbilityHotbar>().ability.startCooldown = 0;
                godSlots.GetChild(i).GetComponent<GodAbilityHotbar>().ability.isCooldown = false;
                godSlots.GetChild(i).GetComponent<GodAbilityHotbar>().isAdded = false;
                godSlots.GetChild(i).GetComponent<GodAbilityHotbar>().ability = null;
                godSlots.GetChild(i).GetComponent<GodAbilityHotbar>().Reset();
                godSlots.GetChild(i).Find("Cooldown").GetComponent<RectTransform>().sizeDelta = new Vector2(100, 0);
                battle.Reset();
                Destroy(godSlots.GetChild(i).Find("Ability").gameObject);
            }
        }
        for (int i = 0; i < enemySlots.childCount; i++)
        {
            SaveSystem.ResetEnemySkill(enemySlots.GetChild(i).name);
            if (enemySlots.GetChild(i).GetComponent<EnemyAbilitySlot>().ability != null)
            {
                enemySlots.GetChild(i).GetComponent<EnemyAbilitySlot>().ability.startCooldown = 0;
                enemySlots.GetChild(i).GetComponent<EnemyAbilitySlot>().ability.isCooldown = false;
                enemySlots.GetChild(i).GetComponent<EnemyAbilitySlot>().isAdded = false;
                enemySlots.GetChild(i).GetComponent<EnemyAbilitySlot>().ability = null;
                enemySlots.GetChild(i).Find("Cooldown").GetComponent<RectTransform>().sizeDelta = new Vector2(100, 0);
                battle.Reset();
                Destroy(enemySlots.GetChild(i).Find("Ability").gameObject);
            }
        }
        /*
        for (int i = 0; i < inventorySlots.childCount; i++)
        {
            SaveSystem.ResetItem(inventorySlots.GetChild(i).name);
            if (inventorySlots.GetChild(i).GetComponent<InventorySlot>().item != null)
            {
                inventorySlots.GetChild(i).GetComponent<InventorySlot>().item = null;
                inventorySlots.GetChild(i).GetComponent<InventorySlot>().lvl = 1;
                inventory.slots[i] = null;
                Destroy(inventorySlots.GetChild(i).Find("Item").gameObject);
            }
        }
        for (int i = 0; i < sellSlots.childCount; i++)
        {
            SaveSystem.ResetItem(sellSlots.GetChild(i).name);
            if (sellSlots.GetChild(i).GetComponent<SellSlot>().item != null)
            {
                sellSlots.GetChild(i).GetComponent<SellSlot>().item = null;
                sellSlots.GetChild(i).GetComponent<SellSlot>().lvl = 1;
                inventory.sellSlots[i] = null;
                Destroy(sellSlots.GetChild(i).Find("Item").gameObject);
            }
        }
        for (int i = 0; i < playerSlots.childCount; i++)
        {
            SaveSystem.ResetItem(playerSlots.GetChild(i).name);
            if (playerSlots.GetChild(i).GetComponent<InventorySlot>().item != null)
            {
                playerSlots.GetChild(i).GetComponent<InventorySlot>().item = null;
                playerSlots.GetChild(i).GetComponent<InventorySlot>().lvl = 1;
                inventory.playerSlots[i] = null;
                Destroy(playerSlots.GetChild(i).Find("Item").gameObject);
            }
        }
        */
        for (int i = 0; i < herbSlots.childCount; i++)
        {
            SaveSystem.ResetHerb(herbSlots.GetChild(i).name);
            
            if (herbSlots.GetChild(i).GetComponent<HerbSlot>().herb != null)
            {
                herbSlots.GetChild(i).Find("Herb").GetComponent<InventoryHerb>().Reset();
                Destroy(herbSlots.GetChild(i).Find("Herb").gameObject);
            }
            herbSlots.GetChild(i).GetComponent<HerbSlot>().Reset();
        }
        gatheringManager.slots = new Herb[5];
        for (int i = 1; i < growings.childCount; i++)
        {
            SaveSystem.ResetGrowing(growings.GetChild(i).GetComponent<Growing>(), growings.GetChild(i).name);
            growings.GetChild(i).GetComponent<Growing>().Reset();
        }
        for (int i = 1; i < searchings.childCount; i++)
        {
            SaveSystem.ResetSearching(searchings.GetChild(i).GetComponent<Searching>(), searchings.GetChild(i).name);
            searchings.GetChild(i).GetComponent<Searching>().Reset();
        }
        for (int i = 0; i < trainings.childCount; i++)
        {
            SaveSystem.ResetTraining(trainings.GetChild(i).GetComponent<Train>(), trainings.GetChild(i).name);
            trainings.GetChild(i).GetComponent<Train>().Reset();
        }
        for (int i = 0; i < techniques.childCount; i++)
        {
            SaveSystem.ResetTechnique(techniques.GetChild(i).GetComponent<Technique>(), techniques.GetChild(i).name);
            techniques.GetChild(i).GetComponent<Technique>().Reset();
        }
        for (int i = 0; i < techniques.childCount; i++)
        {
            string nm = techniques.GetChild(i).name;
            SaveSystem.ResetTechUpgrade(nm);
        }
        
        for (int i = 1; i < talants.childCount; i++)
        {
            SaveSystem.ResetTalant(talants.GetChild(i).name);
            talants.GetChild(i).GetComponent<Talant>().Reset();
        }
        
        for (int i = 0; i < NegativeEffect.effectIds.Count; i++)
        {
            if (battle.player.negativeEffects[i] != null)
                SaveSystem.ResetPlayerNegativeEffect(battle.player.negativeEffects[i]);
            if (battle.enemy.negativeEffects[i] != null)
                SaveSystem.ResetEnemyNegativeEffect(battle.enemy.negativeEffects[i]);
        }

        SaveSystem.ResetTalantMap();

        SaveSystem.ResetOffline();

        bloodCurrency.gameObject.SetActive(true);
        loggerText.text = "";
        
        LoadAfterReincarnation();
        Save();
        AutoSave.Reseted = true;
        InvokeRepeating("Save", 30f, 30f);
        InvokeRepeating("ChangeUI_Text", 0f, 1f);
        
    }

    public void SkipTime()
    {
        Save();

        DateTime date = DateTime.UtcNow;
        float day = date.Day * 24 * 3600;
        float hour = date.Hour * 3600;
        float minute = date.Minute * 60;
        float second = date.Second;
        float totalTime = day + hour + minute + second;
        float time = totalTime;
        SaveSystem.SaveOffline(totalTime);
        offline.GetComponent<OfflineProgress>().UpdateOffline();
    }

    void Reset()
    {
        DirectoryInfo di = new DirectoryInfo(Application.persistentDataPath);

        foreach (FileInfo file in di.GetFiles())
        {
            file.Delete();
        }
    }
}
