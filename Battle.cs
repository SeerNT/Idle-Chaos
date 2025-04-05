using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using NT_Utils.UI;
using NT_Utils.Conversion;
using System.IO;

public class Battle : MonoBehaviour
{
    public Slider playerHp;
    public Slider enemyHp;
    private Text playerHpText;
    public Text enemyHpText;

    public Image enemyIcon;
    public Image playerIcon;
    public Text enemyName;
    public Text enemyNum;

    public Slider playerMana;
    public Slider enemyMana;
    private Text playerManaText;
    private Text enemyManaText;
    [Header("Battle Information")]
    public Button startBattle;

    public int enemyN = 1;

    public Player player;
    public Enemy enemy;

    public bool isBattle = false;
    public bool isRegenerate = false;

    public List<string> names = new List<string>();
    public List<Sprite> images = new List<Sprite>();
    public List<Sprite> bossInteractionsSprites = new List<Sprite>();
    public List<Sprite> enemyInteractionsSprites = new List<Sprite>();
    public GameObject bossTreat;
    [Header("Timers")]
    public Timer timerPlayer;
    public Timer timerEnemy;

    public Timer hpRegPlayer;
    public Timer hpRegEnemy;

    public Timer manaRegPlayer;
    public Timer manaRegEnemy;
    [Header("Item Drop")]
    public GameObject dropScreen;

    public GameItem itemDrop;
    public Text titleDrop;
    public GameObject iconDrop;
    public Text rarityDrop;
    public Text itemNameDrop;
    public Text itemDescDrop;
    private Button acceptDropBut;

    private bool isEnemySwitched = false;
    [Header("Items")]
    [SerializeField] public Inventory inventory;

    public GameItem[] commonItems = new GameItem[100];
    public GameItem[] uncommonItems = new GameItem[100];
    public GameItem[] rareItems = new GameItem[100];
    public GameItem[] epicItems = new GameItem[100];
    public GameItem[] legendaryItems = new GameItem[100];
    public GameItem[] mythicItems = new GameItem[100];
    public GameItem[] shopItems = new GameItem[100];
    public GameItem[] chaoticItems = new GameItem[100];

    [Header("Stat Texts")]
    public Text ppText;
    public Text mpText;
    public Text armText;
    public Text magArmText;
    public Text prcText;
    public Text lckText;
    public Text critText;
    public Text asText;
    public Text mxhpText;
    public Text chaosFactor;
    [Header("Other")]
    public BattleLogger logger;

    public SkillManager skillManager;
    public TalantManager talantManager;
    public Color32 enemyIconColor = new Color32(255, 255, 255, 255);
    public Color32 playerIconColor = new Color32(255, 255, 255, 255);

    public int[] xpRewards = new int[0];
    public AbilityManager abilityManager;
    public GameAbility[] abilities;

    public bool playerStunned = false;
    public bool playerWeakned = false;

    public bool isSetup = false;
    private bool battleReturn = false;

    public Transform bossInteractions;
    public Transform enemyInteractions;

    public float startCooldown;

    public Timer cooldownTimer;

    public EnemyGameAbility[] bossAbilities;

    public EnemyGameAbility[] enemyAbilities;

    public GameObject effectsObject;

    private double lastStartBattleTime;

    private double prevPlayerHP = -1;
    private double prevEnemyHP = -1;

    [SerializeField] private Sprite[] boss5_Stages = new Sprite[6];
    [SerializeField] private Sprite[] boss3_Stages = new Sprite[3];
    [SerializeField] private string[] boss3_Names = new string[3];
    [SerializeField] private Sprite[] bossInteractionSprites = new Sprite[2];
    [SerializeField] private CutsceneManager cutsceneManager;
    public GameObject finalBossAdder;
    [SerializeField] private Animator finalBossAnimator;
    public GameObject enemyObject;

    [SerializeField] private GameObject seaButObj;
    [SerializeField] private GameObject seaObj;
    [SerializeField] private ItemSetManager itemSetManager;
    [SerializeField] private SkinsManager skinsManager;

    public bool isEnemyStun = false;
    public bool isPlayerStun = false;

    private bool isSwitchingDeath = false;

    void Start() {
        isSetup = false;
        titleDrop = dropScreen.transform.Find("Title").gameObject.GetComponent<Text>();
        rarityDrop = dropScreen.transform.Find("Rarity").gameObject.GetComponent<Text>();
        iconDrop = dropScreen.transform.Find("ItemIcon").gameObject;
        itemNameDrop = dropScreen.transform.Find("Name").gameObject.GetComponent<Text>();
        itemDescDrop = dropScreen.transform.Find("Description").gameObject.GetComponent<Text>();
        acceptDropBut = dropScreen.transform.Find("Button").gameObject.GetComponent<Button>();
        acceptDropBut.onClick.AddListener(CloseDropScreen);

        playerHpText = this.transform.GetChild(1).GetChild(1).GetChild(2).GetComponent<Text>();
        enemyHpText = this.transform.GetChild(2).GetChild(1).GetChild(2).GetComponent<Text>();

        playerManaText = this.transform.GetChild(1).GetChild(0).GetChild(2).GetComponent<Text>();
        enemyManaText = this.transform.GetChild(2).GetChild(0).GetChild(2).GetComponent<Text>();

        startBattle.onClick.AddListener(StartBattle);

        isEnemySwitched = false;

        string path = Application.persistentDataPath + "/player.save";
        if (!File.Exists(path))
        {
            float playerMana = (float)(10 + (GameManager.magic * 0.1));
            player = new Player(playerMana, 100f, 1f, 1f, 2f, 0.1f, 0f, 0.01f, 0.5f, 100f, 0f);
        }
        if (!SaveSystem.isEnemyLoaded)
        {
            SwitchEnemy(enemyN);
        }

        startBattle.transform.GetChild(0).GetComponent<Text>().color = Color.green;
        hpRegPlayer = new Timer(InitPlayerHpRegenerate, null, Timeout.Infinite, (int)((player.hpRegSpeed / 200) * 100));
        hpRegEnemy = new Timer(InitEnemyHpRegenerate, null, Timeout.Infinite, (int)((enemy.hpRegSpeed / 200) * 100));
        manaRegPlayer = new Timer(InitPlayerManaRegenerate, null, 0, (int)((player.manaRegSpeed / 100) * 1000));
        manaRegEnemy = new Timer(InitEnemyManaRegenerate, null, 0, (int)((enemy.manaRegSpeed / 100) * 1000));

        timerPlayer = new Timer(InitPlayerBattling, null, Timeout.Infinite, (int)(player.attackSpeed / 100) * 1000);
        timerEnemy = new Timer(InitEnemyBattling, null, Timeout.Infinite, (int)(enemy.attackSpeed / 100) * 1000);
        hpRegPlayer.Change(Timeout.Infinite, Timeout.Infinite);
        hpRegEnemy.Change(Timeout.Infinite, Timeout.Infinite);
        timerPlayer.Change(Timeout.Infinite, Timeout.Infinite);
        timerEnemy.Change(Timeout.Infinite, Timeout.Infinite);
        manaRegPlayer.Change(Timeout.Infinite, Timeout.Infinite);
        manaRegEnemy.Change(Timeout.Infinite, Timeout.Infinite);

        UpdateUI();

        logger = GameObject.Find("Log").GetComponent<BattleLogger>();
        
        isSetup = true;
    }

    public void InitTimers()
    {
        hpRegPlayer = new Timer(InitPlayerHpRegenerate, null, Timeout.Infinite, (int)((player.hpRegSpeed / 200) * 100));
        hpRegEnemy = new Timer(InitEnemyHpRegenerate, null, Timeout.Infinite, (int)((enemy.hpRegSpeed / 200) * 100));
        manaRegPlayer = new Timer(InitPlayerManaRegenerate, null, 0, (int)((player.manaRegSpeed / 100) * 1000));
        manaRegEnemy = new Timer(InitEnemyManaRegenerate, null, 0, (int)((enemy.manaRegSpeed / 100) * 1000));

        timerPlayer = new Timer(InitPlayerBattling, null, Timeout.Infinite, (int)(player.attackSpeed / 100) * 1000);
        timerEnemy = new Timer(InitEnemyBattling, null, Timeout.Infinite, (int)(enemy.attackSpeed / 100) * 1000);
    }

    public bool IsBattleOn()
    {
        return isBattle;
    }

    public IEnumerator ReturnAfterEnd()
    {
        yield return new WaitForSeconds(0.2f);
        CutsceneManager.isShowing = false;
    }

    void UpdateUI() {
        if (isSetup && !CutsceneManager.isShowing)
        {
            playerHpText.text = NumberConversion.AbbreviateNumber(player.hp, NumberConversion.StartAbbrevation.M);
            enemyHpText.text = NumberConversion.AbbreviateNumber(enemy.hp, NumberConversion.StartAbbrevation.M);
            playerManaText.text = NumberConversion.AbbreviateNumber(player.mana, NumberConversion.StartAbbrevation.M);
            enemyManaText.text = NumberConversion.AbbreviateNumber(enemy.mana, NumberConversion.StartAbbrevation.M);

            if(player.chaosFactor >= 1)
            {
                chaosFactor.text = "Chaos Factor:" + NumberConversion.AbbreviateNumber(player.chaosFactor, NumberConversion.StartAbbrevation.M);
                seaObj.SetActive(true);
                seaButObj.SetActive(true);
            }
            else
            {
                chaosFactor.text = "???:" + NumberConversion.AbbreviateNumber(player.chaosFactor, NumberConversion.StartAbbrevation.M);
                seaObj.SetActive(false);
                seaButObj.SetActive(false);
            }

            if(player.mana != player.maxMana && !player.isRegenerating)
            {
                manaRegPlayer.Change((int)((player.manaRegSpeed / 100) * 1000), (int)((player.manaRegSpeed / 100) * 1000));
                player.isRegenerating = true;
            }
            else if(player.mana >= player.maxMana)
            {
                player.isRegenerating = false;
                manaRegPlayer.Change(Timeout.Infinite, Timeout.Infinite);
            }

            if (enemy.mana != enemy.maxMana && !enemy.isRegenerating)
            {
                manaRegEnemy.Change((int)((enemy.manaRegSpeed / 100) * 1000), (int)((enemy.manaRegSpeed / 100) * 1000));
            }
            else if (enemy.mana >= enemy.maxMana)
            {
                enemy.isRegenerating = false;
                manaRegEnemy.Change(Timeout.Infinite, Timeout.Infinite);
            }

            playerHp.value = player.percentHp;
            enemyHp.value = enemy.percentHp;
            playerMana.value = player.percentMana;
            enemyMana.value = enemy.percentMana;
            enemyNum.text = "#" + enemyN.ToString();
            enemyName.text = names[enemyN - 1];
            enemyIcon.sprite = images[enemyN - 1];
            enemyIcon.color = enemyIconColor;
            playerIcon.color = playerIconColor;
            if (isBattle == false)
            {
                SwitchEnemy(enemyN);
                startBattle.transform.GetChild(0).GetComponent<Text>().text = "Start";
                startBattle.transform.GetChild(0).GetComponent<Text>().color = Color.green;
            }
            else
            {
                startBattle.transform.GetChild(0).GetComponent<Text>().text = "Stop";
                startBattle.transform.GetChild(0).GetComponent<Text>().color = Color.red;
            }
        }
    }

    public void SwitchEnemy(int id)
    {
        if (enemyN == 1 && !isEnemySwitched)
        {
            enemy = new Enemy(10f, 0f, 250f, 0f, 2f, 0f, 10f, 0f, 0.01f, 0.5f, 100f, 0f);
            isEnemySwitched = true;
            PrepareBattle(enemyN);
            UpdateUI();
        }
        if (enemyN == 2 && !isEnemySwitched)
        {
            enemy = new Enemy(15f, 0f, 540f, 0f, 4f, 6f, 10f, 0.2f, 0.04f, 0.5f, 50f, 0f);
            isEnemySwitched = true;
            PrepareBattle(enemyN);
            UpdateUI();
        }
        if (enemyN == 3 && !isEnemySwitched)
        {
            enemy = new Enemy(15f, 0f, 3940f, 0f, 26f, 8f, 10f, 0.3f, 0.04f, 0.5f, 35f, 0f);
            isEnemySwitched = true;
            PrepareBattle(enemyN);
            UpdateUI();
        }
        if (enemyN == 4 && !isEnemySwitched)
        {
            skinsManager.AddSkin(1, names[enemyN - 1]);
            enemy = new Enemy(30f, 0f, 47280f, 0f, 300f, 10f, 10f, 0.3f, 0.34f, 0.5f, 105f, 0f);
            isEnemySwitched = true;
            PrepareBattle(enemyN);
            UpdateUI();
        }
        if (enemyN == 5 && !isEnemySwitched)
        {
            enemy = new Enemy(75f, 0f, 162920f, 0f, 1600F, 12f, 10f, 0.3f, 0.34f, 0.7f, 125f, 0f);
            isEnemySwitched = true;
            PrepareBattle(enemyN);
            UpdateUI();
        }
        if (enemyN == 6 && !isEnemySwitched)
        {
            enemy = new Enemy(1, 0f, 150f, 200000f, 0f, 3000F, 12f, 10f, 0.3f, 0.34f, 0.7f, 160f, 0f);
            isEnemySwitched = true;
            PrepareBossBattle(enemy.bossId);
            UpdateUI();
        }
        if (enemyN == 7 && !isEnemySwitched)
        {
            skinsManager.AddSkin(2, names[enemyN - 1]);
            enemy = new Enemy(120f, 15f, 680000f, 0f, 12900F, 85f, 40f, 0.4f, 0.44f, 0.7f, 150f, 0f);
            enemy.stunDur = 2f;
            isEnemySwitched = true;
            PrepareBattle(enemyN);
            UpdateUI();
        }
        if (enemyN == 8 && !isEnemySwitched)
        {
            enemy = new Enemy(1f, 45f, 111111f, 0f, 50F, 80f, 40f, 0.4f, 0.34f, 0.7f, 20f, 0f);
            enemy.stunDur = 0.4f;
            isEnemySwitched = true;
            PrepareBattle(enemyN);
            UpdateUI();
        }
        if (enemyN == 9 && !isEnemySwitched)
        {
            enemy = new Enemy(999f, 0f, 999999f, 0f, 1050F, 65f, 40f, 0.7f, 0.24f, 1.7f, 80f, 0f);
            isEnemySwitched = true;
            PrepareBattle(enemyN);
            UpdateUI();
        }
        if (enemyN == 10 && !isEnemySwitched)
        {
            skinsManager.AddSkin(3, names[enemyN - 1]);
            enemy = new Enemy(500f, 0f, 2500000f, 0f, 20050F, 40f, 40f, 0.15f, 0.34f, 1.0f, 110f, 0f);
            isEnemySwitched = true;
            PrepareBattle(enemyN);
            UpdateUI();
        }
        if (enemyN == 11 && !isEnemySwitched)
        {
            enemy = new Enemy(120f, 0f, 6250000f, 0f, 47050F, 40f, 40f, 0.25f, 0.24f, 1.0f, 100f, 0f);
            isEnemySwitched = true;
            PrepareBattle(enemyN);
            UpdateUI();
        }
        if (enemyN == 12 && !isEnemySwitched)
        {
            enemy = new Enemy(2, 0f, 1000f, 15000000f, 0f, 50000F, 50f, 50f, 0.35f, 0.04f, 5.0f, 100f, 0f);
            isEnemySwitched = true;
            PrepareBossBattle(enemy.bossId);
            UpdateUI();
        }
        if (enemyN == 13 && !isEnemySwitched)
        {
            skinsManager.AddSkin(4, names[enemyN - 1]);
            enemy = new Enemy(10f, 0f, 9500000f, 0f, 500000F, 40f, 50f, 0.25f, 0.24f, 1.0f, 90f, 0f);
            isEnemySwitched = true;
            PrepareBattle(enemyN);
            UpdateUI();
        }
        if (enemyN == 14 && !isEnemySwitched)
        {
            enemy = new Enemy(10f, 0f, 15000000f, 0f, 1500000F, 10f, 20f, 0.8f, 0.7f, 2.0f, 120f, 0f);
            isEnemySwitched = true;
            PrepareBattle(enemyN);
            UpdateUI();
        }
        if (enemyN == 15 && !isEnemySwitched)
        {
            enemy = new Enemy(10f, 0f, 45000000f, 0f, 1000000F, 10f, 80f, 0.1f, 0.4f, 1.2f, 80f, 0f);
            isEnemySwitched = true;
            PrepareBattle(enemyN);
            UpdateUI();
        }
        if (enemyN == 16 && !isEnemySwitched)
        {
            skinsManager.AddSkin(5, names[enemyN - 1]);
            enemy = new Enemy(150f, 0f, 60000000f, 1500000f, 1000000F, 25f, 60f, 0.1f, 0.4f, 1.2f, 110f, 0f);
            isEnemySwitched = true;
            PrepareBattle(enemyN);
            UpdateUI();
        }
        if (enemyN == 17 && !isEnemySwitched)
        {
            enemy = new Enemy(10f, 0f, 91500000f, 0f, 500000F, 35f, 35f, 0.1f, 0.1f, 1.0f, 50f, 0f);
            isEnemySwitched = true;
            PrepareBattle(enemyN);
            UpdateUI();
        }
        if (enemyN == 18 && !isEnemySwitched)
        {
            enemy = new Enemy(3, 15f, 5000f, 250000000f, 0f, 2500000F, 50f, 50f, 0.35f, 0.04f, 2.0f, 200f, 0f);
            isEnemySwitched = true;
            PrepareBossBattle(enemy.bossId);
            UpdateUI();
        }
        if (enemyN == 19 && !isEnemySwitched)
        {
            skinsManager.AddSkin(6, names[enemyN - 1]);
            enemy = new Enemy(250f, 0f, 800000000f, 0f, 1000000F, 35f, 35f, 0.5f, 0.4f, 1.0f, 80f, 0f);
            isEnemySwitched = true;
            PrepareBattle(enemyN);
            UpdateUI();
        }
        if (enemyN == 20 && !isEnemySwitched)
        {
            enemy = new Enemy(200f, 0f, 6400000000f, 0f, 2400000F, 35f, 55f, 0.5f, 0.7f, 5.0f, 100f, 0f);
            isEnemySwitched = true;
            PrepareBattle(enemyN);
            UpdateUI();
        }
        if (enemyN == 21 && !isEnemySwitched)
        {
            enemy = new Enemy(150f, 0f, 25600000000f, 0f, 5000000F, 35f, 55f, 0.6f, 0.6f, 2.0f, 90f, 0f);
            isEnemySwitched = true;
            PrepareBattle(enemyN);
            UpdateUI();
        }
        if (enemyN == 22 && !isEnemySwitched)
        {
            enemy = new Enemy(25f, 0f, 102400000000f, 0f, 500000F, 85f, 55f, 0.4f, 0.1f, 1.0f, 1f, 0f);
            isEnemySwitched = true;
            PrepareBattle(enemyN);
            UpdateUI();
        }
        if (enemyN == 23 && !isEnemySwitched)
        {
            skinsManager.AddSkin(7, names[enemyN - 1]);
            enemy = new Enemy(10f, 0f, 409600000000f, 0f, 2000000F, 95f, 55f, 0.3f, 0.1f, 1.0f, 5f, 0f);
            isEnemySwitched = true;
            PrepareBattle(enemyN);
            UpdateUI();
        }
        if (enemyN == 24 && !isEnemySwitched)
        {
            enemy = new Enemy(4, 25f, 10000000f, 10000000000000f, 10000000F, 10000000F, 50f, 50f, 0.35f, 0.24f, 2.0f, 80f, 0f);
            isEnemySwitched = true;
            PrepareBossBattle(enemy.bossId);
            UpdateUI();
        }
        
        if (enemyN == 25 && !isEnemySwitched)
        {
            enemy = new Enemy(1200f, 10f, 780000000000000f, 0f, 18000000F, 85f, 55f, 0.3f, 0.1f, 1.0f, 55f, 0f);
            enemy.hpRegSpeed = 500;
            isEnemySwitched = true;
            PrepareBattle(enemyN);
            UpdateUI();
        }
        if (enemyN == 26 && !isEnemySwitched)
        {
            skinsManager.AddSkin(8, names[enemyN - 1]);
            enemy = new Enemy(2500f, 0f, 9600000000000000f, 0f, 512000000F, 45f, 75f, 0.3f, 0.2f, 1.5f, 75f, 0f);
            enemy.hpRegSpeed = 450;
            isEnemySwitched = true;
            PrepareBattle(enemyN);
            UpdateUI();
        }
        if (enemyN == 27 && !isEnemySwitched)
        {
            enemy = new Enemy(4100f, 0f, 12000000000000000f, 0f, 812000000F, 45f, 45f, 0.4f, 0.1f, 1.0f, 25f, 0f);
            enemy.hpRegSpeed = 400;
            isEnemySwitched = true;
            PrepareBattle(enemyN);
            UpdateUI();
        }
        if (enemyN == 28 && !isEnemySwitched)
        {
            skinsManager.AddSkin(9, names[enemyN - 1]);
            enemy = new Enemy(5000f, 0f, 5650000000000000000f, 0f, 9000000000F, 75f, 45f, 0.4f, 0.4f, 4.0f, 145f, 0f);
            enemy.hpRegSpeed = 200;
            isEnemySwitched = true;
            PrepareBattle(enemyN);
            UpdateUI();
        }
        if (enemyN == 29 && !isEnemySwitched)
        {
            enemy = new Enemy(8500f, 35f, 9990000000000000000f, 0f, 100000000000F, 35f, 45f, 0.6f, 0.4f, 1.0f, 155f, 0f);
            enemy.hpRegSpeed = 200;
            isEnemySwitched = true;
            PrepareBattle(enemyN);
            UpdateUI();
        }
        if (enemyN == 30 && !isEnemySwitched)
        {
            skinsManager.AddSkin(10, names[enemyN - 1]);
            enemy = new Enemy(5, 95, 10000f, 1000000000000000000000f, 1000000000000F, 5000000000000F, 95f, 95f, 0.9f, 0.8f, 1.2f, 50f, 0f);
            enemy.hpRegSpeed = 200;
            enemy.holyRegen = true;
            isEnemySwitched = true;
            PrepareBossBattle(enemy.bossId);
            UpdateUI();
        }
        if (enemyN == 31 && !isEnemySwitched)
        {
            skinsManager.AddSkin(11, names[enemyN - 1]);
            enemy = new Enemy(6, 95, 100000f, 1000000000000000000000000f, 1000000000000000F, 5000000000000000F, 95f, 95f, 0.9f, 0.9f, 1.5f, 30f, 0f);
            enemy.hpRegSpeed = 200;
            enemy.holyRegen = true;
            isEnemySwitched = true;
            PrepareBossBattle(enemy.bossId);
            UpdateUI();
        }
    }

    public void PrepareBattle(int enemyN)
    {
        bossTreat.SetActive(false);
        if (enemyN == 10)
        {
            abilityManager.AddAbilityToEnemyHotbar(enemyAbilities[0], 1);
        }
        if (enemyN == 16)
        {
            abilityManager.AddAbilityToEnemyHotbar(enemyAbilities[1], 1);
        }
        if (enemyN == 23)
        {
            abilityManager.AddAbilityToEnemyHotbar(enemyAbilities[2], 1);
        }
    }

    public void PrepareBossBattle(int id)
    {
        bossTreat.SetActive(true);
        if (id == 1)
        {
            abilityManager.AddAbilityToEnemyHotbar(bossAbilities[id - 1], 1);
            bossInteractions.GetChild(0).GetComponent<Image>().sprite = bossInteractionSprites[0];
        }
        if (id == 2)
        {
            abilityManager.AddAbilityToEnemyHotbar(bossAbilities[id - 1], 1);
            abilityManager.AddAbilityToEnemyHotbar(bossAbilities[id], 2);
        }
        if (id == 3)
        {
            abilityManager.AddAbilityToEnemyHotbar(bossAbilities[3], 1);
            abilityManager.AddAbilityToEnemyHotbar(bossAbilities[4], 2);
            bossInteractions.GetChild(0).GetComponent<Image>().sprite = bossInteractionSprites[1];
        }
        if (id == 4)
        {
            abilityManager.AddAbilityToEnemyHotbar(bossAbilities[5], 1);
            abilityManager.AddAbilityToEnemyHotbar(bossAbilities[6], 2);
        }
        if (id == 5)
        {
            abilityManager.AddAbilityToEnemyHotbar(bossAbilities[7], 1);
            abilityManager.AddAbilityToEnemyHotbar(bossAbilities[8], 2);
        }
        if (id == 6)
        {
            finalBossAdder.SetActive(true);
            enemyObject.GetComponent<Animator>().enabled = true;
            bossTreat.GetComponent<Text>().text = "FINAL BOSS";
        }
    }

    void UpdateStats() {
        ppText.text = "Physic Power:" + NumberConversion.AbbreviateNumber(player.physicPower);
        mpText.text = "Magic Power:" + NumberConversion.AbbreviateNumber(player.magicPower);
        armText.text = "Armor:" + NumberConversion.AbbreviateNumber(player.armor);
        magArmText.text = "Magic Armor:" + NumberConversion.AbbreviateNumber(player.magicArmor);

        prcText.text = "Pierce:" + player.pierce;
        lckText.text = "Luck:" + player.luck; 
        critText.text = "Crit Damage:" + NumberConversion.AbbreviateNumber(100 + (player.critDmg * 100)) + "%";
        asText.text = "Attack Speed:" + player.attackSpeed;
        mxhpText.text = "Max Hp:" + NumberConversion.AbbreviateNumber(player.maxHp);

        if (player.chaosFactor >= 1)
        {
            chaosFactor.text = "Chaos Factor:" + NumberConversion.AbbreviateNumber(player.chaosFactor, NumberConversion.StartAbbrevation.M);
        }
        else
        {
            chaosFactor.text = "???:" + player.chaosFactor;
        }

        if (player.armor >= 100)
        {
            armText.color = Color.red;
        }
        else
        {
            armText.color = Color.white;
        }
        if (player.magicArmor >= 100)
        {
            magArmText.color = Color.red;
        }
        else
        {
            magArmText.color = Color.white;
        }
        if (player.attackSpeed <= 1)
        {
            asText.color = Color.red;
        }
        else
        {
            asText.color = Color.white;
        }
        if (player.luck >= 1)
        {
            lckText.color = Color.red;
        }
        else
        {
            lckText.color = Color.white;
        }
        if (player.pierce >= 1)
        {
            prcText.color = Color.red;
        }
        else
        {
            prcText.color = Color.white;
        }
    }

    void Update() {
        if (isSetup && !CutsceneManager.isShowing)
        {
            if (isEnemyStun)
            {
                StunEnemy(player.stunDur*2000);
                isEnemyStun = false;
            }
            if (isPlayerStun)
            {
                StunPlayer(enemy.stunDur * 2000);
                isPlayerStun = false;
            }
            UpdateUI();
            UpdateStats();
            if (SaveSystem.isBattleLoaded && !battleReturn)
            {
                ReturnBattleState();
            }
            float playerMana = (float)(10 + (GameManager.magic * 0.1));
            player.maxMana = playerMana;
            player.percentMana = 1 - ((player.maxMana - player.mana) / player.maxMana);

            // Catch Player getting dmg from Enemy
            if (prevPlayerHP != -1)
            {
                if(player.hp <= player.maxHp)
                {
                    if ((player.hp - prevPlayerHP) < 0)
                    {
                        logger.OnReceiveDamage(BattleLogger.Target.Player, false, enemyName.text);
                    }
                    prevPlayerHP = player.hp;
                }
            }
            else
            {
                if(player.hp <= player.maxHp)
                    prevPlayerHP = player.hp;
            }

            // Catch Enemy getting dmg from Player
            if (prevEnemyHP != -1)
            {
                if (enemy.hp <= enemy.maxHp)
                {
                    if ((enemy.hp - prevEnemyHP) < 0)
                    {
                        logger.OnReceiveDamage(BattleLogger.Target.Enemy, false, enemyName.text);
                    }
                    prevEnemyHP = enemy.hp;
                }
            }
            else
            {
                if (enemy.hp <= enemy.maxHp)
                    prevEnemyHP = enemy.hp;
            }

            if (!isBattle && !isRegenerate && (player.hp >= player.maxHp) && (enemy.hp >= enemy.maxHp))
            {
                player.hp = player.maxHp;
                enemy.hp = enemy.maxHp;
                hpRegPlayer.Change(Timeout.Infinite, Timeout.Infinite);
                hpRegEnemy.Change(Timeout.Infinite, Timeout.Infinite);
            }

            if (!isBattle && !isRegenerate && (player.hp < player.maxHp) && (enemy.hp >= enemy.maxHp))
            {
                hpRegPlayer = new Timer(InitPlayerHpRegenerate, null, (int)((player.hpRegSpeed / 200) * 100), (int)((player.hpRegSpeed / 200) * 100));
                isRegenerate = true;
            }

            if(enemy.bossId == 3)
            {
                if(enemy.hp >= enemy.maxHp)
                {
                    enemyIcon.sprite = boss3_Stages[0];
                    enemyName.text = boss3_Names[0];
                }
                else if(enemy.hp < enemy.maxHp && enemy.hp > ((enemy.maxHp * 2) / 3))
                {
                    enemyIcon.sprite = boss3_Stages[1];
                    enemyName.text = boss3_Names[1];
                }
                else if(enemy.hp <= ((enemy.maxHp * 2) / 3) && enemy.hp > (enemy.maxHp / 3)) {
                    enemyIcon.sprite = boss3_Stages[2];
                    enemyName.text = boss3_Names[2];
                }
                else if (enemy.hp <= (enemy.maxHp / 3))
                {
                    enemyIcon.sprite = boss3_Stages[3];
                    enemyName.text = boss3_Names[3];
                }
            }

            if (enemy.bossId == 6)
            {
                enemyHp.transform.GetChild(1).GetChild(0).GetComponent<Image>().color = new Color32(229, 151, 6, 255);
            }

            if (enemy.bossId == 5)
            {
                enemyHp.transform.GetChild(1).GetChild(0).GetComponent<Image>().color = new Color32(226, 223, 40, 255);
                if (enemy.hp >= enemy.maxHp)
                {
                    enemyIcon.sprite = boss5_Stages[0];
                }
                else if (enemy.hp < enemy.maxHp && enemy.hp > ((enemy.maxHp * 5) / 6))
                {
                    enemyIcon.sprite = boss5_Stages[1];
                }
                else if (enemy.hp <= ((enemy.maxHp * 5) / 6) && enemy.hp > ((enemy.maxHp * 4) / 6))
                {
                    enemyIcon.sprite = boss5_Stages[2];
                }
                else if (enemy.hp <= ((enemy.maxHp * 4) / 6) && enemy.hp > ((enemy.maxHp * 3) / 6))
                {
                    enemyIcon.sprite = boss5_Stages[3];
                }
                else if (enemy.hp <= ((enemy.maxHp * 3) / 6) && enemy.hp > ((enemy.maxHp * 2) / 6))
                {
                    enemyIcon.sprite = boss5_Stages[4];
                }
                else if (enemy.hp <= (enemy.maxHp / 3))
                {
                    enemyIcon.sprite = boss5_Stages[5];
                }
            }

            if (enemy.hp <= 0 && !isSwitchingDeath)
            {
                if(enemy.bossId != 3 && enemy.bossId != 4 && enemy.bossId != 5 && enemy.bossId != 6)
                {
                    StartOnDeath();
                }
                else if(enemy.bossId == 4)
                {
                    if(player.chaosFactor < 4)
                        cutsceneManager.ShowCutscene(2);
                    else
                    {
                        StartOnDeath();
                    }
                }
                else if (enemy.bossId == 5)
                {
                    if (player.chaosFactor < 6)
                        cutsceneManager.ShowCutscene(3);
                    else
                    {
                        StartOnDeath();
                    }
                }
                else if (enemy.bossId == 6 && !CutsceneManager.isShowing)
                {
                    if(player.chaosFactor >= 12)
                    {
                        cutsceneManager.ShowEnding(3);
                    }
                    else if (itemSetManager.HasHolySet())
                    {
                        cutsceneManager.ShowEnding(2);
                    }
                    else
                    {
                        timerPlayer.Change(Timeout.Infinite, Timeout.Infinite);
                        timerEnemy.Change(Timeout.Infinite, Timeout.Infinite);
                        hpRegPlayer.Change(Timeout.Infinite, Timeout.Infinite);
                        hpRegEnemy.Change(Timeout.Infinite, Timeout.Infinite);
                        cutsceneManager.ShowEnding(1);
                        enemy.hp = 0.001f;
                    }
                    
                }
                else
                {
                    if(player.chaosFactor < 1)
                        cutsceneManager.ShowCutscene(1);
                    else
                    {
                        StartOnDeath();
                    }
                }
            }
            
            if (player.hp <= 0)
            {
                logger.OnDeath(BattleLogger.Target.Player, enemyName.text);
                bossInteractions.gameObject.SetActive(false);
                enemyInteractions.gameObject.SetActive(false);
                playerIconColor = new Color32(255, 255, 255, 255);
                player.hp = 0;
                if(player.negativeEffects[0] != null)
                {
                    player.negativeEffects[0].StopEffect();
                }

                StartBattle();

                ResetEnemyAbilities();
                isRegenerate = true;
            }

            if (isRegenerate && player.hp >= player.maxHp)
            {
                player.hp = player.maxHp;
                hpRegPlayer.Change(Timeout.Infinite, Timeout.Infinite);
            }
            if (enemy.hp >= enemy.maxHp && isRegenerate)
            {
                enemy.hp = enemy.maxHp;
                hpRegEnemy.Change(Timeout.Infinite, Timeout.Infinite);
            }
            if (isRegenerate && enemy.hp >= enemy.maxHp && player.hp >= player.maxHp)
            {
                enemy.hp = enemy.maxHp;
                player.hp = player.maxHp;
                hpRegPlayer.Change(Timeout.Infinite, Timeout.Infinite);
                hpRegEnemy.Change(Timeout.Infinite, Timeout.Infinite);
                isRegenerate = false;
            }

            double curTime = Time.realtimeSinceStartup;
            if (Math.Abs(curTime - lastStartBattleTime) < 1f && !isBattle)
            {
                //StartCoroutine(Utilities.DisableButtonForSecs(startBattle, 1f));
            }
        }
        
    }

    public void StartOnDeath()
    {
        if (!isSwitchingDeath)
        {
            isSwitchingDeath = true;
            timerPlayer.Change(Timeout.Infinite, Timeout.Infinite);
            timerEnemy.Change(Timeout.Infinite, Timeout.Infinite);
            StartCoroutine("OnDeath");
        }
    }

    public IEnumerator OnDeath() {
        yield return new WaitForSeconds(0.05f);
        enemyN++;
        SwitchEnemy(enemyN);

        hpRegPlayer.Change(0, (int)((player.hpRegSpeed / 200) * 100));
        hpRegEnemy.Change(0, (int)((enemy.hpRegSpeed / 200) * 100));
        isRegenerate = true;

        isBattle = false;
        StartCoroutine(Utilities.DisableButtonForSecs(startBattle, 1f));
        timerPlayer.Change(Timeout.Infinite, Timeout.Infinite);
        timerEnemy.Change(Timeout.Infinite, Timeout.Infinite);

        ReturnPlayer();
        RemoveEnemyAbilities();
        ResetEnemyAbilities();

        Reincarnation.totalKilledEnemies++;

        logger.OnDeath(BattleLogger.Target.Enemy, enemyName.text);

        enemyIconColor = new Color32(255, 255, 255, 255);

        itemDrop = GameItem.GetRandomItem(this);
        if (itemDrop != null)
        {
            string[] arrNames = names.ToArray();
            titleDrop.text = arrNames[enemyN - 2] + " DROPPED ITEM";
            rarityDrop.text = itemDrop.rarity.ToString();
            itemNameDrop.text = itemDrop.displayName;
            itemDescDrop.text = itemDrop.description;
            iconDrop.GetComponent<Image>().sprite = itemDrop.icon;
            dropScreen.SetActive(true);
            inventory.AddItem(itemDrop);
        }

        player.xp += xpRewards[enemyN - 2];
        GameManager.xp += xpRewards[enemyN - 2];
        Reincarnation.totalEarnXp += xpRewards[enemyN - 2];

        if (TalantManager.bloodUnlocked)
            GameManager.blood += (1 + Mathf.Floor(enemyN / 10));
        GameManager.UpdateText();
        if (player.xp >= player.xpCap)
        {
            player.xp = player.xp - player.xpCap;
            player.lvl++;
            skillManager.skillpoints += player.lvl * 3;
            talantManager.talantPoints += 2;
            talantManager.OnPointsChange();
            skillManager.OnPointsChange();
            player.xpCap = (int)(player.xpCap + (player.xpCap * 0.8));
            if (player.lvl % 5 == 0)
            {
                Tuple<StatUpgrade.Stats, int> stat = player.GetRandomStat();
                player.IncreaseStat(stat.Item1, stat.Item2);
            }
        }

        isEnemySwitched = false;
        isSwitchingDeath = false;
    }

    public void Reset()
    {
        StopAllCoroutines();
        bossTreat.SetActive(false);
        bossInteractions.gameObject.SetActive(false);
        enemyInteractions.gameObject.SetActive(false);

        foreach(NegativeEffect effect in effectsObject.GetComponents<NegativeEffect>())
        {
            Destroy(effect);
        }
        playerIconColor = new Color32(255, 255, 255, 255);
        enemyIconColor = new Color32(255, 255, 255, 255);
        hpRegPlayer.Change(Timeout.Infinite, Timeout.Infinite);
        hpRegEnemy.Change(Timeout.Infinite, Timeout.Infinite);
        if (cooldownTimer != null)
            cooldownTimer.Change(Timeout.Infinite, Timeout.Infinite);
    }

    public void RemoveEnemyAbilities()
    {
        Transform enemyAbilities = transform.Find("Enemy").Find("Slots");
        for (int i = 0; i < enemyAbilities.childCount; i++)
        {
            if (enemyAbilities.GetChild(i).GetChild(0).GetComponent<Ability>() != null)
                enemyAbilities.GetChild(i).GetChild(0).gameObject.SetActive(false);
        }
    }

    public void ResetEnemyAbilities()
    {
        Transform enemyAbilities = transform.Find("Enemy").Find("Slots");
        for (int i = 0; i < enemyAbilities.childCount; i++)
        {
            if(enemyAbilities.GetChild(i).GetChild(0).GetComponent<Ability>() != null)
                InstaResetAbility(enemyAbilities.GetChild(i).GetChild(0).GetComponent<Ability>().enemyAbility);
        }
    }

    public void ResetAbility(GameAbility ability, float time)
    {
        StartCoroutine(ResetAbilityInTime(ability, time));
    }

    public void ResetAbility(GameAbility ability)
    {
        startCooldown = 0;
        cooldownTimer = new Timer(IncreaseCooldown, null, 0, 100);
        StartCoroutine(ResetAbilityInTime(ability, ability.cooldown));
    }

    public void ResetAbility(GodAbility ability, float time)
    {
        StartCoroutine(ResetAbilityInTime(ability, time));
    }

    public void ResetAbility(GodAbility ability)
    {
        startCooldown = 0;
        cooldownTimer = new Timer(IncreaseCooldown, null, 0, 100);
        StartCoroutine(ResetAbilityInTime(ability, ability.cooldown));
    }

    public void ResetAbility(EnemyGameAbility ability)
    {
        startCooldown = 0;
        cooldownTimer = new Timer(IncreaseCooldown, null, 0, 100);
        StartCoroutine(ResetAbilityInTime(ability, ability.cooldown));
    }

    public void InstaResetAbility(EnemyGameAbility ability)
    {
        startCooldown = 0f;
        if (cooldownTimer != null)
        {
            cooldownTimer.Change(Timeout.Infinite, Timeout.Infinite);
        }
        ability.isCooldown = false;
        ability.cooldownObj.GetComponent<RectTransform>().sizeDelta = new Vector2(100, 0);
        ability.startCooldown = 0;
    }

    public void ResetAbility(EnemyGameAbility ability, float time)
    {
        StartCoroutine(ResetAbilityInTime(ability, ability.cooldown - time));
    }

    public void IncreaseCooldown(object o)
    {
        startCooldown += 0.1f;
    }

    public IEnumerator ResetAbilityInTime(GameAbility ability, float time)
    {
        yield return new WaitForSeconds(time);
        startCooldown = 0f;
        if(cooldownTimer != null)
        {
            cooldownTimer.Change(Timeout.Infinite, Timeout.Infinite);
        }
        ability.isCooldown = false;
        ability.cooldownObj.GetComponent<RectTransform>().sizeDelta = new Vector2(100, 0);
    }

    public IEnumerator ResetAbilityInTime(GodAbility ability, float time)
    {
        yield return new WaitForSeconds(time);
        startCooldown = 0f;
        if (cooldownTimer != null)
        {
            cooldownTimer.Change(Timeout.Infinite, Timeout.Infinite);
        }
        ability.isCooldown = false;
        ability.cooldownObj.GetComponent<RectTransform>().sizeDelta = new Vector2(100, 0);
    }

    public IEnumerator ResetAbilityInTime(EnemyGameAbility ability, float time)
    {
        yield return new WaitForSeconds(time);
        startCooldown = 0f;
        if (cooldownTimer != null)
        {
            cooldownTimer.Change(Timeout.Infinite, Timeout.Infinite);
        }
        ability.isCooldown = false;
        ability.cooldownObj.GetComponent<RectTransform>().sizeDelta = new Vector2(100, 0);
    }

    void CloseDropScreen() {
        dropScreen.SetActive(false);
    }

    public void UpdateAS()
    {
        if (isBattle)
        {
            if(player.negativeEffects[0] != null)
            {
                if (!player.negativeEffects[0].isEffected)
                {
                    timerPlayer.Change(0, (int)((player.attackSpeed / 100) * 1000));
                    playerStunned = false;
                }
                else
                {
                    Task.Delay((int)(player.negativeEffects[0].effectCurrentDuration - (player.negativeEffects[0].effectTime * 1000f))).ContinueWith(t => UpdateAS());
                }
            }
            else
            {
                timerPlayer.Change(0, (int)((player.attackSpeed / 100) * 1000));
                playerStunned = false;
            }
            
        }
    }

    public void StartBattle() {
        if (!isBattle)
        {
            isBattle = true;
            lastStartBattleTime = Time.realtimeSinceStartup;
            if(!playerStunned)
                timerPlayer.Change(190, (int)((player.attackSpeed / 100) * 1000));
            if(!isEnemyStun)
                timerEnemy.Change(190, (int)((enemy.attackSpeed / 100) * 1000));
            hpRegPlayer.Change(Timeout.Infinite, Timeout.Infinite);
            hpRegEnemy.Change(Timeout.Infinite, Timeout.Infinite);
            isRegenerate = false;
            
        }
        else {
            isBattle = false;
            StartCoroutine(Utilities.DisableButtonForSecs(startBattle, 1f));
            timerPlayer.Change(Timeout.Infinite, Timeout.Infinite);
            timerEnemy.Change(Timeout.Infinite, Timeout.Infinite);
            hpRegPlayer.Change(0, (int)((player.hpRegSpeed / 200) * 100));
            hpRegEnemy.Change(0, (int)((enemy.hpRegSpeed / 200) * 100));
            isRegenerate = true;
        }
    }

    public void ReturnBattleState()
    {
        if (isBattle)
        {
            if(enemy.negativeEffects[1] != null)
            {
                if (enemy.negativeEffects[1].isEffected)
                {
                    StunEnemy(((enemy.negativeEffects[1].effectCurrentDuration / 1000) - enemy.negativeEffects[1].effectTime) * 1000);
                }
                else
                {
                    timerEnemy.Change(190, (int)((enemy.attackSpeed / 100) * 1000));
                }
            }
            else
            {
                timerEnemy.Change(190, (int)((enemy.attackSpeed / 100) * 1000));
            }
            if(enemy.negativeEffects[3] != null)
            {
                if (enemy.negativeEffects[3].isEffected)
                {
                    BlockEnemySkills(((enemy.negativeEffects[3].effectCurrentDuration / 1000) - enemy.negativeEffects[3].effectTime) * 1000);
                }
            }
            if (enemy.negativeEffects[4] != null)
            {
                if (enemy.negativeEffects[4].isEffected)
                {
                    WeakenEnemy(((enemy.negativeEffects[4].effectCurrentDuration / 1000) - enemy.negativeEffects[4].effectTime) * 1000);
                }
            }


            if (player.negativeEffects[0] != null)
            {
                if (player.negativeEffects[0].isEffected)
                {
                    StunPlayer(((player.negativeEffects[0].effectCurrentDuration / 1000) - player.negativeEffects[0].effectTime) * 1000);
                }
                else
                {
                    timerPlayer.Change(190, (int)((player.attackSpeed / 100) * 1000));
                }
            }
            else
            {
                timerPlayer.Change(190, (int)((player.attackSpeed / 100) * 1000));
            }
            if (player.negativeEffects[2] != null)
            {
                if (player.negativeEffects[2].isEffected)
                {
                    BlockPlayerSkills(((player.negativeEffects[2].effectCurrentDuration / 1000) - player.negativeEffects[2].effectTime) * 1000);
                }
            }
            if (player.negativeEffects[5] != null)
            {
                if (player.negativeEffects[5].isEffected)
                {
                    WeakenPlayer(((player.negativeEffects[5].effectCurrentDuration / 1000) - player.negativeEffects[5].effectTime) * 1000);
                }
            }


            hpRegPlayer.Change(Timeout.Infinite, Timeout.Infinite);
            hpRegEnemy.Change(Timeout.Infinite, Timeout.Infinite);

            isRegenerate = false;
        }
        else
        {
            timerPlayer.Change(Timeout.Infinite, Timeout.Infinite);
            timerEnemy.Change(Timeout.Infinite, Timeout.Infinite);
            hpRegPlayer.Change(0, (int)((player.hpRegSpeed / 200) * 100));
            hpRegEnemy.Change(0, (int)((enemy.hpRegSpeed / 200) * 100));
            isRegenerate = true;
        }
        battleReturn = true;
    }

    void InitPlayerHpRegenerate(object o)
    {
        player.RegenerateHp();
    }

    void InitEnemyHpRegenerate(object o)
    {
        enemy.RegenerateHp();
    }

    void InitPlayerManaRegenerate(object o)
    {
        
        player.RegenerateMana();
    }

    void InitEnemyManaRegenerate(object o)
    {
        enemy.RegenerateMana();
    }

    void InitPlayerBattling(object o) {
        isEnemyStun = player.Attack(enemy);
    }

    void InitEnemyBattling(object o)
    {
        isPlayerStun = enemy.Attack(player);
    }

    public void StunEnemy(float ms)
    {
        NegativeEffect stunEffect = null;
        foreach (NegativeEffect effect in effectsObject.GetComponents<NegativeEffect>())
        {
            if(effect.effectId == 1)
            {
                stunEffect = effect;
                break;
            }
        }
        logger.OnStun(BattleLogger.Target.Enemy, enemyName.text);
        if(stunEffect == null)
            stunEffect = effectsObject.AddComponent<NegativeEffect>();
        stunEffect.effectId = 1;
        enemy.negativeEffects[1] = stunEffect;
        stunEffect.isEffected = true;
        stunEffect.effectCurrentDuration = ms;
        stunEffect.effectTime = 0f;
        
        stunEffect.ApplyEffect(enemy);
    }

    public void WeakenEnemy(float ms)
    {
        NegativeEffect stunEffect = effectsObject.AddComponent<NegativeEffect>();
        logger.OnWeaken(BattleLogger.Target.Enemy, enemyName.text);
        stunEffect.effectId = 4;
        enemy.negativeEffects[4] = stunEffect;
        stunEffect.isEffected = true;
        stunEffect.effectCurrentDuration = ms;
        stunEffect.effectTime = 0f;
        stunEffect.ApplyEffect(enemy);

        enemy.weakness = 0.3f;
    }

    public void WeakenPlayer(float ms)
    {
        NegativeEffect stunEffect = effectsObject.AddComponent<NegativeEffect>();
        logger.OnWeaken(BattleLogger.Target.Player, enemyName.text);
        stunEffect.effectId = 5;
        enemy.negativeEffects[5] = stunEffect;
        stunEffect.isEffected = true;
        stunEffect.effectCurrentDuration = ms;
        stunEffect.effectTime = 0f;
        stunEffect.ApplyEffect(player);

        player.weakness = 0.3f;
    }

    public void StunPlayer(float ms)
    {
        logger.OnStun(BattleLogger.Target.Player, enemyName.text);
        NegativeEffect stunEffect = effectsObject.AddComponent<NegativeEffect>();
        stunEffect.effectId = 0;
        player.negativeEffects[0] = stunEffect;
        stunEffect.isEffected = true;
        stunEffect.effectCurrentDuration = ms;
        stunEffect.effectTime = 0f;
        stunEffect.ApplyEffect(player);
    }

    public void BlockPlayerSkills(float ms)
    {
        logger.OnSkillsBlock(BattleLogger.Target.Player, enemyName.text);
        NegativeEffect stunEffect = effectsObject.AddComponent<NegativeEffect>();
        stunEffect.effectId = 2;
        player.negativeEffects[2] = stunEffect;
        stunEffect.isEffected = true;
        stunEffect.effectCurrentDuration = ms;
        stunEffect.effectTime = 0f;
        stunEffect.ApplyEffect(player);
    }

    public void BlockEnemySkills(float ms)
    {
        logger.OnSkillsBlock(BattleLogger.Target.Enemy, enemyName.text);
        NegativeEffect stunEffect = effectsObject.AddComponent<NegativeEffect>();
        stunEffect.effectId = 3;
        player.negativeEffects[3] = stunEffect;
        stunEffect.isEffected = true;
        stunEffect.effectCurrentDuration = ms;
        stunEffect.effectTime = 0f;
        stunEffect.ApplyEffect(player);
    }

    public void DealMagicDmg(Enemy caster, Player victim, float value)
    {
        float damage = caster.magicPower + value;
        damage = damage - (damage * (victim.magicArmor / 100));
        victim.hp -= damage;
    }

    public void DealMagicDmg(Player caster, Enemy victim, float value)
    {
        float damage = caster.magicPower + value;
        damage = damage - (damage * (victim.magicArmor / 100));
        victim.hp -= damage;
    }

    public void DealMagicDmgProcent(Player caster, Enemy victim, float value)
    {
        float damage = caster.magicPower * (value/100);
        damage = damage - (damage * (victim.magicArmor / 100));
        victim.hp -= damage;
    }

    public void ReturnPlayer()
    {
        bossInteractions.gameObject.SetActive(false);
        enemyInteractions.gameObject.SetActive(false);
        playerIconColor = new Color32(255, 255, 255, 255);
        for(int effectId = 0; effectId < player.negativeEffects.Length; effectId++)
        {
            if(player.negativeEffects[effectId] != null)
            {
                player.negativeEffects[effectId].isEffected = false;
                player.negativeEffects[effectId].effectTime = 0f;
            }
        }
    }

    void AddItemToPool(GameItem item)
    {
        if(item.rarity == GameItem.Rarity.Common)
        {
            commonItems[commonItems.Length + 1] = item;
        }
        if (item.rarity == GameItem.Rarity.Uncommon)
        {
            uncommonItems[uncommonItems.Length + 1] = item;
        }
        if (item.rarity == GameItem.Rarity.Rare)
        {
            rareItems[rareItems.Length + 1] = item;
        }
        if (item.rarity == GameItem.Rarity.Epic)
        {
            epicItems[epicItems.Length + 1] = item;
        }
        if (item.rarity == GameItem.Rarity.Legendary)
        {
            legendaryItems[legendaryItems.Length + 1] = item;
        }
        if (item.rarity == GameItem.Rarity.Mythic)
        {
            mythicItems[mythicItems.Length + 1] = item;
        }
    }

    public void SaveBattle()
    {
        SaveSystem.SaveBattle(enemyN, isBattle, isRegenerate);
    }

    public void LoadBattle()
    {
        BattleData data = SaveSystem.LoadBattle();

        if (data != null)
        {
            enemyN = data.enemyN;
            isBattle = data.isBattle;
            isRegenerate = data.isRegenerate;
            isRegenerate = false;
        }
    }

    public void SavePlayer()
    {
        SaveSystem.SavePlayer(player);
    }

    public void LoadPlayer()
    {
        PlayerData data = SaveSystem.LoadPlayer();

        if (data != null)
        {
            float playerMana = (float)(10 + (GameManager.magic * 0.1));
            player = new Player(playerMana, 100f, 1f, 1f, 2f, 0.1f, 0f, 0.01f, 0.5f, 100f, 0f);
            player.hp = data.hp;
            player.maxHp = data.maxHp;
            player.mana = data.mana;
            player.maxMana = data.maxMana;
            player.manaRegSpeed = data.manaRegSpeed;
            player.hpRegSpeed = data.hpRegSpeed;
            player.magicPower = data.magicPower;
            player.physicPower = data.physicPower;
            player.armor = data.armor;
            player.magicArmor = data.magicArmor;
            player.pierce = data.pierce;
            player.luck = data.luck;
            player.critDmg = data.critDmg;
            player.attackSpeed = data.attackSpeed;
            player.chaosFactor = data.chaosFactor;

            player.vampirismChance = data.vampChance;

            player.lvl = data.lvl;
            player.xp = data.xp;
            player.xpCap = data.xpCap;

            player.percentHp = data.percentHp;
            player.percentMana = data.percentMana;
            player.percentXp = data.percentXp;

            player.stunChance = data.stunChance;
            player.stunDur = data.stunDuration;
            player.pureDmg = data.pureDmg;

            player.abstractARM = data.abstractARM;
            player.abstractMARM = data.abstractMARM;
            player.abstractAS = data.abstractAS;
            player.abstractLCK = data.abstractLCK;
            player.abstractPRC = data.abstractPRC;

            TalantManager.automationSpeed = data.automationSpeed;
            for (int i = 0; i < NegativeEffect.effectIds.Count; i++)
            {
                NegativeEffectData data2 = SaveSystem.LoadPlayerNegativeEffect(i);
                if (data2 != null)
                {
                    NegativeEffect stunEffect = effectsObject.AddComponent<NegativeEffect>();
                    stunEffect.effectId = data2.effectId;
                    stunEffect.isEffected = data2.isEffected;
                    stunEffect.effectCurrentDuration = data2.effectCurrentDuration;
                    stunEffect.effectTime = data2.effectTime;
                    player.negativeEffects[data2.effectId] = stunEffect;
                }
            }
        }
    }

    public void SaveEnemy()
    {
        SaveSystem.SaveEnemy(enemy);
    }

    public void LoadEnemy()
    {
        EnemyData data = SaveSystem.LoadEnemy();

        if (data != null)
        {
            enemy = new Enemy(10f, 0f, 250f, 0f, 2f, 0f, 0.1f, 0f, 0.01f, 0.5f, 100f, 0f);
            enemy.hp = data.hp;
            enemy.maxHp = data.maxHp;
            enemy.mana = data.mana;
            enemy.maxMana = data.maxMana;
            enemy.manaRegSpeed = data.manaRegSpeed;
            enemy.hpRegSpeed = data.hpRegSpeed;
            enemy.magicPower = data.magicPower;
            enemy.physicPower = data.physicPower;
            enemy.armor = data.armor;
            enemy.magicArmor = data.magicArmor;
            enemy.pierce = data.pierce;
            enemy.luck = data.luck;
            enemy.critDmg = data.critDmg;
            enemy.attackSpeed = data.attackSpeed;

            enemy.lvl = data.lvl;

            enemy.percentHp = data.percentHp;
            enemy.percentMana = data.percentMana;

            enemy.stunChance = data.stunChance;
            enemy.stunDur = data.stunDuration;
            if(enemyN == 6)
            {
                bossTreat.SetActive(true);
                enemy.bossId = 1;
            }
            if (enemyN == 12)
            {
                bossTreat.SetActive(true);
                enemy.bossId = 2;
            }
            if (enemyN == 18)
            {
                bossTreat.SetActive(true);
                enemy.bossId = 3;
            }
            if (enemyN == 24)
            {
                bossTreat.SetActive(true);
                enemy.bossId = 4;
            }
            if (enemyN == 30)
            {
                bossTreat.SetActive(true);
                enemy.bossId = 5;
            }
            if (enemyN == 31)
            {
                bossTreat.SetActive(true);
                enemy.bossId = 6;
                finalBossAdder.SetActive(true);
                enemyObject.GetComponent<Animator>().enabled = true;
                bossTreat.GetComponent<Text>().text = "FINAL BOSS";
            }
            for (int i = 0; i < NegativeEffect.effectIds.Count; i++)
            {
                NegativeEffectData data2 = SaveSystem.LoadEnemyNegativeEffect(i);
                if(data2 != null)
                {
                    NegativeEffect stunEffect = effectsObject.AddComponent<NegativeEffect>();
                    stunEffect.effectId = data2.effectId;
                    stunEffect.isEffected = data2.isEffected;
                    stunEffect.effectCurrentDuration = data2.effectCurrentDuration;
                    stunEffect.effectTime = data2.effectTime;
                    enemy.negativeEffects[data2.effectId] = stunEffect;
                }
            }
        }
    }
}
