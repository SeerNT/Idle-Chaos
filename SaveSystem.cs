using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class SaveSystem
{
    public static bool isBattleLoaded;
    public static bool isPlayerLoaded;
    public static bool isEnemyLoaded;

    public static bool statUpgLoaded;
    public static bool enchanceUpgLoaded;

    public static bool skillUpgLoaded;

    public static bool trainLoaded;
    public static bool techLoaded;

    public static bool isDebug;

    public static void ResetCurrency()
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/currency.save";
        FileStream stream = new FileStream(path, FileMode.Create);

        CurrencyData data = new CurrencyData(0, 0, 0, 0, GameManager.qi, GameManager.meta);

        formatter.Serialize(stream, data);
        stream.Close();
        if (isDebug)
            Debug.Log("SUCCESSFULLY RESETED DATA CURRENCY");
    }

    public static void SaveCurrency()
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/currency.save";
        FileStream stream = new FileStream(path, FileMode.Create);

        CurrencyData data = new CurrencyData(GameManager.magic, GameManager.xp, GameManager.gold, GameManager.blood, GameManager.qi, GameManager.meta);

        formatter.Serialize(stream, data);
        stream.Close();
        if(isDebug)
            Debug.Log("SUCCESSFULLY SAVED DATA CURRENCY");
    }

    public static CurrencyData LoadCurrency()
    {
        string path = Application.persistentDataPath + "/currency.save";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            CurrencyData data = formatter.Deserialize(stream) as CurrencyData;
            stream.Close();
            if (isDebug)
                Debug.Log("SUCCESSFULLY LOADED DATA CURRENCY");
            return data;
        }
        else
        {
            if (isDebug)
                Debug.LogError("Save file not found in " + path);
            return null;
        }
    }

    public static void SaveCutscenes(bool[] shown)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/cutscenes.save";
        FileStream stream = new FileStream(path, FileMode.Create);

        CutsceneData data = new CutsceneData(shown, CutsceneManager.isShowing);

        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static CutsceneData LoadCutscenes()
    {
        string path = Application.persistentDataPath + "/cutscenes.save";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            CutsceneData data = formatter.Deserialize(stream) as CutsceneData;
            stream.Close();
            return data;
        }
        else
        {
            if (isDebug)
                Debug.LogError("Save file not found in " + path);
            return null;
        }
    }

    public static void SaveDailyReward(int day, int value, bool isCollected, string name, double time)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/daily" + name + ".save";
        FileStream stream = new FileStream(path, FileMode.Create);

        DailyRewardData data = new DailyRewardData(day, value, isCollected, DailyManager.dayRow, time);

        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static DailyRewardData LoadDailyReward(string name)
    {
        string path = Application.persistentDataPath + "/daily" + name + ".save";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            DailyRewardData data = formatter.Deserialize(stream) as DailyRewardData;
            stream.Close();
            return data;
        }
        else
        {
            if (isDebug)
                Debug.LogError("Save file not found in " + path);
            return null;
        }
    }

    public static void SaveReincarnation()
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/reincarnation.save";
        FileStream stream = new FileStream(path, FileMode.Create);

        ReincarnationData data = new ReincarnationData(Reincarnation.totalPlayTime, Reincarnation.totalEarnMagic, Reincarnation.totalEarnXp, Reincarnation.totalKilledEnemies, Reincarnation.permanentsUpgrades);

        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static ReincarnationData LoadReincarnation()
    {
        string path = Application.persistentDataPath + "/reincarnation.save";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            ReincarnationData data = formatter.Deserialize(stream) as ReincarnationData;
            stream.Close();
            return data;
        }
        else
        {
            if (isDebug)
                Debug.LogError("Save file not found in " + path);
            return null;
        }
    }

    public static void ResetShop()
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/shop.save";
        FileStream stream = new FileStream(path, FileMode.Create);

        ShopData data = new ShopData(0);

        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static void SaveShop(double time)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/shop.save";
        FileStream stream = new FileStream(path, FileMode.Create);

        ShopData data = new ShopData(time);

        formatter.Serialize(stream, data);
        stream.Close();
        if (isDebug)
            Debug.Log("SUCCESSFULLY SAVED DATA SHOP");
    }

    public static ShopData LoadShop()
    {
        string path = Application.persistentDataPath + "/shop.save";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            ShopData data = formatter.Deserialize(stream) as ShopData;
            stream.Close();

            if (isDebug)
                Debug.Log("SUCCESSFULLY LOADED DATA SHOP");
            return data;
        }
        else
        {
            if (isDebug)
                Debug.LogError("Save file not found in " + path);
            return null;
        }
    }

    public static void ResetBattle()
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/battle.save";
        FileStream stream = new FileStream(path, FileMode.Create);

        BattleData data = new BattleData(1, false, false);

        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static void SaveBattle(int enemyN, bool isBattle, bool isReg)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/battle.save";
        FileStream stream = new FileStream(path, FileMode.Create);

        BattleData data = new BattleData(enemyN, isBattle, isReg);

        formatter.Serialize(stream, data);
        stream.Close();
        if (isDebug)
            Debug.Log("SUCCESSFULLY SAVED DATA BATTLE");
    }

    public static BattleData LoadBattle()
    {
        string path = Application.persistentDataPath + "/battle.save";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            BattleData data = formatter.Deserialize(stream) as BattleData;
            stream.Close();
            if (isDebug)
                Debug.Log("SUCCESSFULLY LOADED DATA BATTLE");
            isBattleLoaded = true;
            return data;
        }
        else
        {
            if (isDebug)
                Debug.LogError("Save file not found in " + path);
            isBattleLoaded = false;
            return null;
        }
    }

    public static void ResetPlayer()
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/player.save";
        FileStream stream = new FileStream(path, FileMode.Create);

        Player player = new Player(10, 100f, 1f, 1f, 2f, 0.1f, 0f, 0.01f, 0.5f, 100f, 0f);

        PlayerData data = new PlayerData(player);

        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static void SavePlayer(Player player)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/player.save";
        FileStream stream = new FileStream(path, FileMode.Create);

        PlayerData data = new PlayerData(player);

        formatter.Serialize(stream, data);
        stream.Close();
        if (isDebug)
            Debug.Log("SUCCESSFULLY SAVED DATA PLAYER");
    }

    public static PlayerData LoadPlayer()
    {
        string path = Application.persistentDataPath + "/player.save";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            PlayerData data = formatter.Deserialize(stream) as PlayerData;
            stream.Close();

            if (isDebug)
                Debug.Log("SUCCESSFULLY LOADED DATA PLAYER");
            isPlayerLoaded = true;
            return data;
        }
        else
        {
            isPlayerLoaded = false;
            if (isDebug)
                Debug.LogError("Save file not found in " + path);
            return null;
        }
    }

    public static void ResetEnemy()
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/enemy.save";
        FileStream stream = new FileStream(path, FileMode.Create);

        Enemy enemy = new Enemy(10f, 0f, 250f, 0f, 2f, 0f, 0.1f, 0f, 0.01f, 0.5f, 100f, 0f);

        EnemyData data = new EnemyData(enemy);

        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static void SaveEnemy(Enemy enemy)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/enemy.save";
        FileStream stream = new FileStream(path, FileMode.Create);

        EnemyData data = new EnemyData(enemy);

        formatter.Serialize(stream, data);
        stream.Close();
        if (isDebug)
            Debug.Log("SUCCESSFULLY SAVED DATA ENEMY");
    }

    public static EnemyData LoadEnemy()
    {
        string path = Application.persistentDataPath + "/enemy.save";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            EnemyData data = formatter.Deserialize(stream) as EnemyData;
            stream.Close();

            if (isDebug)
                Debug.Log("SUCCESSFULLY LOADED DATA ENEMY");
            isEnemyLoaded = true;
            return data;
        }
        else
        {
            isEnemyLoaded = false;
            if (isDebug)
                Debug.LogError("Save file not found in " + path);
            return null;
        }
    }

    public static void ResetEnemyNegativeEffect(NegativeEffect effect)
    {
        string path = Application.persistentDataPath + "/enemyNegativeEffect" + effect.effectId + ".save";
        if (File.Exists(path))
        {
            File.Delete(path);
        }
    }

    public static void SaveEnemyNegativeEffect(NegativeEffect effect)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/enemyNegativeEffect" + effect.effectId +".save";
        FileStream stream = new FileStream(path, FileMode.Create);

        NegativeEffectData data = new NegativeEffectData(effect.effectTime, effect.effectCurrentDuration, effect.isEffected, effect.effectId);

        formatter.Serialize(stream, data);
        stream.Close();
        if (isDebug)
            Debug.Log("SUCCESSFULLY SAVED DATA NEGATIVE EFFECT");
    }

    public static NegativeEffectData LoadEnemyNegativeEffect(int effectId)
    {
        string path = Application.persistentDataPath + "/enemyNegativeEffect" + effectId + ".save";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            NegativeEffectData data = formatter.Deserialize(stream) as NegativeEffectData;
            stream.Close();

            if (isDebug)
                Debug.Log("SUCCESSFULLY LOADED DATA NEGATIVE EFFECT");
            return data;
        }
        else
        {
            if (isDebug)
                Debug.LogError("Save file not found in " + path);
            return null;
        }
    }

    public static void ResetPlayerNegativeEffect(NegativeEffect effect)
    {
        string path = Application.persistentDataPath + "/playerNegativeEffect" + effect.effectId + ".save";
        if (File.Exists(path))
        {
            File.Delete(path);
        }
    }

    public static void SavePlayerNegativeEffect(NegativeEffect effect)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/playerNegativeEffect" + effect.effectId + ".save";
        FileStream stream = new FileStream(path, FileMode.Create);

        NegativeEffectData data = new NegativeEffectData(effect.effectTime, effect.effectCurrentDuration, effect.isEffected, effect.effectId);

        formatter.Serialize(stream, data);
        stream.Close();
        if (isDebug)
            Debug.Log("SUCCESSFULLY SAVED DATA NEGATIVE EFFECT");
    }

    public static NegativeEffectData LoadPlayerNegativeEffect(int effectId)
    {
        string path = Application.persistentDataPath + "/playerNegativeEffect" + effectId + ".save";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            NegativeEffectData data = formatter.Deserialize(stream) as NegativeEffectData;
            stream.Close();

            if (isDebug)
                Debug.Log("SUCCESSFULLY LOADED DATA NEGATIVE EFFECT");
            return data;
        }
        else
        {
            if (isDebug)
                Debug.LogError("Save file not found in " + path);
            return null;
        }
    }

    public static void ResetStatUpgrade(StatUpgrade statObject)
    {
        statObject.ResetStatUpgrade();
    }

    public static void SaveStatUpgrade(StatUpgrade statObject, string name)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/" + "UPG_" + name + ".save";
        FileStream stream = new FileStream(path, FileMode.Create);

        StatUpgradeData data = new StatUpgradeData(statObject);

        formatter.Serialize(stream, data);
        stream.Close();
        if (isDebug)
            Debug.Log("SUCCESSFULLY SAVED DATA " + name + "_UPG");
    }

    public static StatUpgradeData LoadStatUpgrade(string name)
    {
        string path = Application.persistentDataPath + "/" + "UPG_" + name + ".save";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            StatUpgradeData data = formatter.Deserialize(stream) as StatUpgradeData;
            stream.Close();

            if (isDebug)
                Debug.Log("SUCCESSFULLY LOADED DATA " + name + "_UPG");
            statUpgLoaded = true;
            return data;
        }
        else
        {
            statUpgLoaded = false;
            if (isDebug)
                Debug.LogError("Save file not found in " + path);
            return null;
        }
    }

    public static void ResetUpgradeEnchance(string name)
    {
        string path = Application.persistentDataPath + "/" + "ENC_" + name + ".save";

        if (File.Exists(path))
        {
            File.Delete(path);
        }
    }

    public static void SaveUpgradeEnchance(int lvl, bool isActivated, string name)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/" + "ENC_" + name + ".save";
        FileStream stream = new FileStream(path, FileMode.Create);

        UpgradeEnchanceData data = new UpgradeEnchanceData(lvl, isActivated);

        formatter.Serialize(stream, data);
        stream.Close();
        if (isDebug)
            Debug.Log("SUCCESSFULLY SAVED DATA " + name + "_ENC");
    }

    public static UpgradeEnchanceData LoadUpgradeEnchance(string name)
    {
        string path = Application.persistentDataPath + "/" + "ENC_" + name + ".save";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            UpgradeEnchanceData data = formatter.Deserialize(stream) as UpgradeEnchanceData;
            stream.Close();

            if (isDebug)
                Debug.Log("SUCCESSFULLY LOADED DATA " + name + "_ENC");
            enchanceUpgLoaded = true;
            return data;
        }
        else
        {
            enchanceUpgLoaded = false;
            if (isDebug)
                Debug.LogError("Save file not found in " + path);
            return null;
        }
    }

    public static void ResetSkillUpgrade(Skill skill, string name, string branchName)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/" + name + branchName + ".save";
        FileStream stream = new FileStream(path, FileMode.Create);

        SkillManager manager = GameObject.Find("Skills").GetComponent<SkillManager>();

        Skill skillReset = new Skill();

        skillReset.isActivated = false;
        skillReset.lvl = 0;
        skillReset.cost = skill.cost;

        manager.skillpoints = 0;

        SkillUpgradeData data = new SkillUpgradeData(skillReset, manager.skillpoints);

        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static void SaveSkillUpgrade(Skill skill, string name, string branchName)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/" + name + branchName + ".save";
        FileStream stream = new FileStream(path, FileMode.Create);

        SkillManager manager = GameObject.Find("Skills").GetComponent<SkillManager>();

        SkillUpgradeData data = new SkillUpgradeData(skill, manager.skillpoints);

        formatter.Serialize(stream, data);
        stream.Close();
        if (isDebug)
            Debug.Log("SUCCESSFULLY SAVED DATA " + name);
    }

    public static SkillUpgradeData LoadSkillUpgrade(string name, string branchName)
    {
        string path = Application.persistentDataPath + "/" + name + branchName + ".save";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            SkillUpgradeData data = formatter.Deserialize(stream) as SkillUpgradeData;
            stream.Close();

            if (isDebug)
                Debug.Log("SUCCESSFULLY LOADED DATA " + name);
            skillUpgLoaded = true;
            return data;
        }
        else
        {
            skillUpgLoaded = false;
            if (isDebug)
                Debug.LogError("Save file not found in " + path);
            return null;
        }
    }

    public static void ResetWindow()
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/" + "window.save";
        FileStream stream = new FileStream(path, FileMode.Create);

        SkillManager manager = GameObject.Find("Skills").GetComponent<SkillManager>();

        bool[] isShown = new bool[9] {false, false, false, false, false, false, false, true, false};

        WindowData data = new WindowData(isShown);

        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static void SaveWindow(bool[] isShown)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/" + "window.save";
        FileStream stream = new FileStream(path, FileMode.Create);

        SkillManager manager = GameObject.Find("Skills").GetComponent<SkillManager>();

        WindowData data = new WindowData(isShown);

        formatter.Serialize(stream, data);
        stream.Close();
        if (isDebug)
            Debug.Log("SUCCESSFULLY SAVED DATA WINDOW");
    }

    public static WindowData LoadWindow()
    {
        string path = Application.persistentDataPath + "/" + "window.save";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            WindowData data = formatter.Deserialize(stream) as WindowData;
            stream.Close();

            if (isDebug)
                Debug.Log("SUCCESSFULLY LOADED DATA WINDOW");
            return data;
        }
        else
        {
            if (isDebug)
                Debug.LogError("Save file not found in " + path);
            return null;
        }
    }

    public static void ResetHerb(string name)
    {
        string path = Application.persistentDataPath + "/" + "herb" + name + ".save";
        if (File.Exists(path))
        {
            File.Delete(path);
        }
    }

    public static void SaveHerb(Herb herb, int amount, string name)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/" + "herb" + name + ".save";
        Battle battle = GameObject.Find("Battle").GetComponent<Battle>();
        HerbData data = new HerbData(int.Parse(herb.id), herb.currentActiveTime, herb.isActive, amount);
        FileStream stream = new FileStream(path, FileMode.Create);
        formatter.Serialize(stream, data);
        stream.Close();
        if (isDebug)
            Debug.Log("SUCCESSFULLY SAVED DATA HERB");
    }

    public static HerbData LoadHerb(string name)
    {
        string path = Application.persistentDataPath + "/" + "herb" + name + ".save";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            HerbData data = formatter.Deserialize(stream) as HerbData;
            stream.Close();

            if (isDebug)
                Debug.Log("SUCCESSFULLY LOADED DATA HERB");
            return data;
        }
        else
        {
            if (isDebug)
                Debug.LogError("Save file not found in " + path);
            return null;
        }
    }

    public static void ResetGodSkill(string name)
    {
        string path = Application.persistentDataPath + "/" + "godAbility_" + name + ".save";
        if (File.Exists(path))
        {
            File.Delete(path);
        }
    }

    public static void SaveGodSkill(GodAbility skillAbility, string name)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/" + "godAbility_" + name + ".save";
        Battle battle = GameObject.Find("Battle").GetComponent<Battle>();
        GodSkillData data = new GodSkillData(skillAbility.id, skillAbility.cooldown, battle.startCooldown, skillAbility.isCooldown);
        FileStream stream = new FileStream(path, FileMode.Create);
        formatter.Serialize(stream, data);
        stream.Close();
        if (isDebug)
            Debug.Log("SUCCESSFULLY SAVED DATA GOD SKILL");
    }

    public static GodSkillData LoadGodSkill(string name)
    {
        string path = Application.persistentDataPath + "/" + "godAbility_" + name + ".save";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            GodSkillData data = formatter.Deserialize(stream) as GodSkillData;
            stream.Close();

            if (isDebug)
                Debug.Log("SUCCESSFULLY LOADED DATA GOD SKILL");
            return data;
        }
        else
        {
            if (isDebug)
                Debug.LogError("Save file not found in " + path);
            return null;
        }
    }

    public static void ResetSkill(string name)
    {
        string path = Application.persistentDataPath + "/" + "ability_" + name + ".save";
        if (File.Exists(path))
        {
            File.Delete(path);
        }
    }

    public static void SaveSkill(GameAbility skillAbility, string name)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/" + "ability_" + name + ".save";
        Battle battle = GameObject.Find("Battle").GetComponent<Battle>();
        SkillData data = new SkillData(skillAbility.id, skillAbility.cooldown, battle.startCooldown, skillAbility.isCooldown);
        FileStream stream = new FileStream(path, FileMode.Create);
        formatter.Serialize(stream, data);
        stream.Close();
        if (isDebug)
            Debug.Log("SUCCESSFULLY SAVED DATA SKILL");
    }

    public static SkillData LoadSkill(string name)
    {
        string path = Application.persistentDataPath + "/" + "ability_" + name + ".save";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            SkillData data = formatter.Deserialize(stream) as SkillData;
            stream.Close();

            if (isDebug)
                Debug.Log("SUCCESSFULLY LOADED DATA SKILL");
            return data;
        }
        else
        {
            if (isDebug)
                Debug.LogError("Save file not found in " + path);
            return null;
        }
    }

    public static void ResetEnemySkill(string name)
    {
        string path = Application.persistentDataPath + "/" + "enemyAbility_" + name + ".save";
        if (File.Exists(path))
        {
            File.Delete(path);
        }
    }

    public static void SaveEnemySkill(EnemyGameAbility skillAbility, string name)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/" + "enemyAbility_" + name + ".save";
        Battle battle = GameObject.Find("Battle").GetComponent<Battle>();
        EnemySkillData data = new EnemySkillData(skillAbility.id, skillAbility.cooldown, battle.startCooldown, skillAbility.isCooldown);
        FileStream stream = new FileStream(path, FileMode.Create);
        formatter.Serialize(stream, data);
        stream.Close();
        if (isDebug)
            Debug.Log("SUCCESSFULLY SAVED DATA ENEMYSKILL");
    }

    public static EnemySkillData LoadEnemySkill(string name)
    {
        string path = Application.persistentDataPath + "/" + "enemyAbility_" + name + ".save";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            EnemySkillData data = formatter.Deserialize(stream) as EnemySkillData;
            stream.Close();

            if (isDebug)
                Debug.Log("SUCCESSFULLY LOADED DATA ENEMY SKILL");
            return data;
        }
        else
        {
            if (isDebug)
                Debug.LogError("Save file not found in " + path);
            return null;
        }
    }

    public static void ResetHotbarSkill(string name)
    {
        string path = Application.persistentDataPath + "/" + "ability_hotbar_" + name + ".save";
        if (File.Exists(path))
        {
            File.Delete(path);
        }
    }

    public static void SaveHotbarSkill(GameAbility skillAbility, string name)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/" + "ability_hotbar_" + name + ".save";
        Battle battle = GameObject.Find("Battle").GetComponent<Battle>();
        SkillData data = new SkillData(skillAbility.id, skillAbility.cooldown, battle.startCooldown, skillAbility.isCooldown);
        FileStream stream = new FileStream(path, FileMode.Create);
        formatter.Serialize(stream, data);
        stream.Close();
        if (isDebug)
            Debug.Log("SUCCESSFULLY SAVED DATA HOTBAR SKILL");
    }

    public static SkillData LoadHotbarSkill(string name)
    {
        string path = Application.persistentDataPath + "/" + "ability_hotbar_" + name + ".save";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            SkillData data = formatter.Deserialize(stream) as SkillData;
            stream.Close();

            if (isDebug)
                Debug.Log("SUCCESSFULLY LOADED DATA HOTBAR SKILL");
            return data;
        }
        else
        {
            if (isDebug)
                Debug.LogError("Save file not found in " + path);
            return null;
        }
    }

    public static void ResetItem(string name)
    {
        string path = Application.persistentDataPath + "/" + "item_" + name + ".save";
        if (File.Exists(path))
        {
            File.Delete(path);
        }
    }

    public static void SaveItem(GameItem item, string name, int lvl)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/" + "item_" + name + ".save";
        ItemData data = null;
        if (item != null)
        {
            data = new ItemData(int.Parse(item.id), lvl);
        }
        else
        {
            data = new ItemData(-1, -1);
        }
        
        FileStream stream = new FileStream(path, FileMode.Create);
        formatter.Serialize(stream, data);
        stream.Close();
        if (isDebug)
            Debug.Log("SUCCESSFULLY SAVED DATA ITEM");
    }

    public static ItemData LoadItem(string name)
    {
        string path = Application.persistentDataPath + "/" + "item_" + name + ".save";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            ItemData data = formatter.Deserialize(stream) as ItemData;
            stream.Close();

            if (isDebug)
                Debug.Log("SUCCESSFULLY LOADED DATA ITEM");
            return data;
        }
        else
        {
            if (isDebug)
                Debug.LogError("Save file not found in " + path);
            return null;
        }
    }

    public static void ResetGrowing(Growing obj, string name)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/" + "growing_" + name + ".save";
        FileStream stream = new FileStream(path, FileMode.Create);

        Growing growReset = new Growing();

        growReset.loaded = false;

        growReset.progressValue = 0;
        growReset.progress = 0;

        growReset.isGrowing = false;
        growReset.isWaiting = false;

        growReset.seedAmount = 0;
        growReset.baseCost = obj.baseCost;
        growReset.curLimitTime = 0;
        growReset.limitTime = obj.limitTime;

        GrowData data = new GrowData(growReset);

        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static void SaveGrowing(Growing obj, string name)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/" + "growing_" + name + ".save";
        FileStream stream = new FileStream(path, FileMode.Create);

        GrowData data = new GrowData(obj);

        formatter.Serialize(stream, data);
        stream.Close();
        if (isDebug)
            Debug.Log("SUCCESSFULLY SAVED DATA " + name + "_growing");
    }

    public static GrowData LoadGrowing(string name)
    {
        string path = Application.persistentDataPath + "/" + "growing_" + name + ".save";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            GrowData data = formatter.Deserialize(stream) as GrowData;
            stream.Close();

            if (isDebug)
                Debug.Log("SUCCESSFULLY LOADED DATA " + name + "growing_");
            trainLoaded = true;
            return data;
        }
        else
        {
            trainLoaded = false;
            if (isDebug)
                Debug.LogError("Save file not found in " + path);
            return null;
        }
    }

    public static void ResetSearching(Searching obj, string name)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/" + "searching_" + name + ".save";
        FileStream stream = new FileStream(path, FileMode.Create);

        Searching searchReset = new Searching();

        searchReset.loaded = false;

        searchReset.progressValue = 0;
        searchReset.lvl = 1;

        searchReset.isGrowing = false;
        searchReset.isWaiting = false;

        searchReset.seedChance = obj.seedChance;
        searchReset.giveAmount = obj.giveAmount;
        searchReset.curLimitTime = 0;
        searchReset.limitTime = obj.limitTime;

        SearchData data = new SearchData(searchReset);

        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static void SaveSearching(Searching obj, string name)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/" + "searching_" + name + ".save";
        FileStream stream = new FileStream(path, FileMode.Create);

        SearchData data = new SearchData(obj);

        formatter.Serialize(stream, data);
        stream.Close();
        if (isDebug)
            Debug.Log("SUCCESSFULLY SAVED DATA " + name + "_searching");
    }

    public static SearchData LoadSearching(string name)
    {
        string path = Application.persistentDataPath + "/" + "searching_" + name + ".save";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            SearchData data = formatter.Deserialize(stream) as SearchData;
            stream.Close();

            if (isDebug)
                Debug.Log("SUCCESSFULLY LOADED DATA " + name + "_searching");
            trainLoaded = true;
            return data;
        }
        else
        {
            trainLoaded = false;
            if (isDebug)
                Debug.LogError("Save file not found in " + path);
            return null;
        }
    }

    public static void ResetTraining(Train obj, string name)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/" + "TRN_" + name + ".save";
        FileStream stream = new FileStream(path, FileMode.Create);

        Train trainReset = new Train();

        trainReset.loaded = false;

        trainReset.progressValue = 0;

        trainReset.isTraining = false;
        trainReset.isWaiting = false;
        
        if(name != "Train1")
            trainReset.lvl = 0;
        else
            trainReset.lvl = 1;

        trainReset.bought = 0;
        trainReset.cost = obj.baseCost;
        trainReset.baseCost = obj.baseCost;
        trainReset.curLimitTime = obj.baseLimitTime;
        trainReset.limitTime = obj.baseLimitTime;
        trainReset.isAuto = false;
        trainReset.automationOn = false;

        TrainData data = new TrainData(trainReset);

        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static void SaveTraining(Train obj, string name)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/" + "TRN_" + name + ".save";
        FileStream stream = new FileStream(path, FileMode.Create);

        TrainData data = new TrainData(obj);

        formatter.Serialize(stream, data);
        stream.Close();
        if (isDebug)
            Debug.Log("SUCCESSFULLY SAVED DATA " + name + "_TRN");
    }

    public static TrainData LoadTraining(string name)
    {
        string path = Application.persistentDataPath + "/" + "TRN_" + name + ".save";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            TrainData data = formatter.Deserialize(stream) as TrainData;
            stream.Close();

            if (isDebug)
                Debug.Log("SUCCESSFULLY LOADED DATA " + name + "_TRN");
            trainLoaded = true;
            return data;
        }
        else
        {
            trainLoaded = false;
            if (isDebug)
                Debug.LogError("Save file not found in " + path);
            return null;
        }
    }

    public static void SavePermanentUpgrade(int amount, int bought, double cost, int baseCost, string name)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/" + "permanentUpg_" + name + ".save";
        FileStream stream = new FileStream(path, FileMode.Create);

        PermanentUpgradeData data = new PermanentUpgradeData(amount, bought, cost, baseCost);

        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static PermanentUpgradeData LoadPermanentUpgrade(string name)
    {
        string path = Application.persistentDataPath + "/" + "permanentUpg_" + name + ".save";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            PermanentUpgradeData data = formatter.Deserialize(stream) as PermanentUpgradeData;
            stream.Close();
            return data;
        }
        else
        {
            return null;
        }
    }

    public static void ResetTechnique(Technique obj, string name)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/" + "TCH_" + name + ".save";
        FileStream stream = new FileStream(path, FileMode.Create);

        Technique techReset = new Technique();

        techReset.progressValue = 0;

        techReset.isTraining = false;
        techReset.isCooldown = false;

        if(name == "Technique1")
            techReset.lvl = 1;
        else
            techReset.lvl = 0;

        techReset.gainLvl = 0;
        techReset.timeLvl = 0;
        techReset.currentTime = obj.cooldowns[techReset.timeLvl];

        TechniqueData data = new TechniqueData(techReset);

        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static void SaveTechnique(Technique obj, string name)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/" + "TCH_" + name + ".save";
        FileStream stream = new FileStream(path, FileMode.Create);

        TechniqueData data = new TechniqueData(obj);

        formatter.Serialize(stream, data);
        stream.Close();
        if (isDebug)
            Debug.Log("SUCCESSFULLY SAVED DATA " + name + "_TCH");
    }

    public static TechniqueData LoadTechnique(string name)
    {
        string path = Application.persistentDataPath + "/" + "TCH_" + name + ".save";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            TechniqueData data = formatter.Deserialize(stream) as TechniqueData;
            stream.Close();

            if (isDebug)
                Debug.Log("SUCCESSFULLY LOADED DATA " + name + "_TCH");
            techLoaded = true;
            return data;
        }
        else
        {
            techLoaded = false;
            if (isDebug)
                Debug.LogError("Save file not found in " + path);
            return null;
        }
    }

    public static void ResetOffline()
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/" + "offline.save";
        FileStream stream = new FileStream(path, FileMode.Create);

        OfflineData data = new OfflineData(0);

        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static void SaveOffline(float total)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/" + "offline.save";
        FileStream stream = new FileStream(path, FileMode.Create);

        OfflineData data = new OfflineData(total);

        formatter.Serialize(stream, data);
        stream.Close();
        if (isDebug)
            Debug.Log("SUCCESSFULLY SAVED OFFLINE DATA");
    }

    public static OfflineData LoadOffline()
    {
        string path = Application.persistentDataPath + "/" + "offline.save";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            OfflineData data = formatter.Deserialize(stream) as OfflineData;
            stream.Close();

            if (isDebug)
                Debug.Log("SUCCESSFULLY LOADED OFFLINE DATA");
            return data;
        }
        else
        {
            if (isDebug)
                Debug.LogError("Save file not found in " + path);
            return null;
        }
    }

    public static void ResetTalant(string name)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/" + "talant" + name + ".save";
        FileStream stream = new FileStream(path, FileMode.Create);

        TalantData data = new TalantData(false, false, 0);

        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static void SaveTalant(bool isBought, bool isActivated, int points, string name)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/" + "talant" + name + ".save";
        FileStream stream = new FileStream(path, FileMode.Create);

        TalantData data = new TalantData(isBought, isActivated, points);

        formatter.Serialize(stream, data);
        stream.Close();
        if (isDebug)
            Debug.Log("SUCCESSFULLY SAVED TALANT " + name + " DATA");
    }

    public static TalantData LoadTalant(string name)
    {
        string path = Application.persistentDataPath + "/" + "talant" + name + ".save";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            TalantData data = formatter.Deserialize(stream) as TalantData;
            stream.Close();

            if (isDebug)
                Debug.Log("SUCCESSFULLY SAVED TALANT " + name + " DATA");
            return data;
        }
        else
        {
            if (isDebug)
                Debug.LogError("Save file not found in " + path);
            return null;
        }
    }

    public static void ResetTalantMap()
    {
        string path = Application.persistentDataPath + "/" + "talantMap.save";
        if (File.Exists(path))
        {
            File.Delete(path);
        }
    }

    public static void SaveTalantMap(Transform trans)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/" + "talantMap.save";
        FileStream stream = new FileStream(path, FileMode.Create);

        TalantMapData data = new TalantMapData(trans.localPosition.x, trans.localPosition.y, trans.localScale.x);

        formatter.Serialize(stream, data);
        stream.Close();
        if (isDebug)
            Debug.Log("SUCCESSFULLY SAVED TALANT MAP DATA");
    }

    public static TalantMapData LoadTalantMap()
    {
        string path = Application.persistentDataPath + "/" + "talantMap.save";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            TalantMapData data = formatter.Deserialize(stream) as TalantMapData;
            stream.Close();

            if (isDebug)
                Debug.Log("SUCCESSFULLY LOADED TALANT MAP DATA");
            return data;
        }
        else
        {
            if (isDebug)
                Debug.LogError("Save file not found in " + path);
            return null;
        }
    }

    public static void ResetTechUpgrade(string nm)
    {
        string path = Application.persistentDataPath + "/" + nm + ".save";
        if (File.Exists(path))
        {
            File.Delete(path);
        }
    }

    public static void SaveTechUpgrade(TechniqueUpgrade tech, string nm)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/" + nm + ".save";
        FileStream stream = new FileStream(path, FileMode.Create);

        UpgradeTechData data = new UpgradeTechData(tech);

        formatter.Serialize(stream, data);
        stream.Close();
        if (isDebug)
            Debug.Log("SUCCESSFULLY SAVED TECH UPG DATA");
    }

    public static UpgradeTechData LoadUpgradeTech(string nm)
    {
        string path = Application.persistentDataPath + "/" + nm + ".save";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            UpgradeTechData data = formatter.Deserialize(stream) as UpgradeTechData;
            stream.Close();

            if (isDebug)
                Debug.Log("SUCCESSFULLY LOADED TECH UPH DATA");
            return data;
        }
        else
        {
            if (isDebug)
                Debug.LogError("Save file not found in " + path);
            return null;
        }
    }
}
