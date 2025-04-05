using NT_Utils.Conversion;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class CutsceneManager : MonoBehaviour
{
    [Header("End1")]
    [SerializeField] InputField consoleInput;
    [SerializeField] Text consoleText;
    [Header("End2")]
    [SerializeField] InputField consoleInput2;
    [SerializeField] Text consoleText2;
    [Header("End3")]
    [SerializeField] GameObject firstPart;
    [SerializeField] GameObject secondPart;
    [SerializeField] GameObject ending;
    [SerializeField] GameObject endingChoices;
    [SerializeField] private Button finalChoice1;
    [SerializeField] private Button finalChoice2;
    [SerializeField] private Image universeImage;

    [SerializeField] private Transform cutscenes;
    [SerializeField] private GameObject[] notCutsceneObjs = new GameObject[10];

    [SerializeField] private AutoSave saveSystem;

    [SerializeField] private Button choice1;
    [SerializeField] private Button choice2;
    [SerializeField] private Button continue3;

    [SerializeField] private GameObject dialogue;
    [SerializeField] private GameObject consequence1;
    [SerializeField] private GameObject consequence2;
    [SerializeField] private GameObject[] cutscene3Messages = new GameObject[14];
    [SerializeField] private GameObject cutscene3Glitch;

    [SerializeField] private Battle battle;

    [SerializeField] private Transform hotbarPlayerAbilities;

    [SerializeField] private Transform godlikePlayerAbilities;

    [SerializeField] private Transform enemyAbilities;

    private bool[] shownCutscenes = new bool[6];

    public static bool isShowing;

    public void Awake()
    {
        choice1.onClick.AddListener(delegate { Choose(1); });
        choice2.onClick.AddListener(delegate { Choose(2); });
        continue3.onClick.AddListener(delegate { Choose(3); });
        finalChoice1.onClick.AddListener(delegate { Ending3_Choose(1); });
        finalChoice2.onClick.AddListener(delegate { Ending3_Choose(2); });
    }

    public void Choose(int choice)
    {
        if (choice == 1)
        {
            dialogue.SetActive(false);
            consequence1.SetActive(true);
            StartCoroutine(AfterCutsceneEffect(choice));
        }
        else if (choice == 2)
        {
            dialogue.SetActive(false);
            consequence2.SetActive(true);
            StartCoroutine(AfterCutsceneEffect(choice));
        }
        else if (choice == 3)
        {
            StartCoroutine(AfterCutsceneEffect(choice));
        }
    }

    public IEnumerator AfterCutsceneEffect(int choice)
    {
        foreach (GameObject obj in notCutsceneObjs)
        {
            obj.SetActive(true);
        }
        notCutsceneObjs[18].SetActive(false);
        if (choice != 3 && choice != 4)
            yield return new WaitForSeconds(5);
        else
            yield return new WaitForSeconds(0.2f);

        cutscenes.GetChild(0).gameObject.SetActive(false);
        cutscenes.GetChild(1).gameObject.SetActive(false);
        cutscenes.GetChild(2).gameObject.SetActive(false);
        
        isShowing = false;
        shownCutscenes = new bool[6] { false, false, false, false, false, false };

        if (choice == 1)
        {
            Reincarnation.totalPlayTime = 0;
            Reincarnation.totalEarnMagic = 0;
            Reincarnation.totalEarnXp = 0;
            Reincarnation.totalKilledEnemies = 0;
            saveSystem.ResetByReincarnation();
            Reincarnation.totalPlayTime = 0;
            Reincarnation.totalEarnMagic = 0;
            Reincarnation.totalEarnXp = 0;
            Reincarnation.totalKilledEnemies = 0;
        }
        if (choice == 2)
        {
            battle.player.chaosFactor += 1;
            saveSystem.LoadAfterCutscene();
            for (int i = 0; i < hotbarPlayerAbilities.childCount; i++)
            {
                if(hotbarPlayerAbilities.GetChild(i).GetComponent<AbilityHotbar>().ability != null)
                {
                    hotbarPlayerAbilities.GetChild(i).GetComponent<AbilityHotbar>().ability.startCooldown = 0;
                    hotbarPlayerAbilities.GetChild(i).GetComponent<AbilityHotbar>().ability.isCooldown = false;
                    hotbarPlayerAbilities.GetChild(i).GetComponent<AbilityHotbar>().ability.cooldownObj.transform.SetAsLastSibling();
                    hotbarPlayerAbilities.GetChild(i).GetComponent<AbilityHotbar>().ability.cooldownObj.GetComponent<RectTransform>().sizeDelta = new Vector2(100, 0);
                }
            }
            for (int i = 0; i < godlikePlayerAbilities.childCount; i++)
            {
                if (godlikePlayerAbilities.GetChild(i).GetComponent<GodAbilityHotbar>().ability != null)
                {
                    godlikePlayerAbilities.GetChild(i).GetComponent<GodAbilityHotbar>().ability.startCooldown = 0;
                    godlikePlayerAbilities.GetChild(i).GetComponent<GodAbilityHotbar>().ability.isCooldown = false;
                    godlikePlayerAbilities.GetChild(i).GetComponent<GodAbilityHotbar>().ability.cooldownObj.transform.SetAsLastSibling();
                    godlikePlayerAbilities.GetChild(i).GetComponent<GodAbilityHotbar>().ability.cooldownObj.GetComponent<RectTransform>().sizeDelta = new Vector2(100, 0);
                }
            }

            for (int i = 0; i < enemyAbilities.childCount; i++)
            {
                if (enemyAbilities.GetChild(i).GetComponent<EnemyAbilitySlot>().ability != null)
                {
                    enemyAbilities.GetChild(i).GetComponent<EnemyAbilitySlot>().ability.startCooldown = 0;
                    enemyAbilities.GetChild(i).GetComponent<EnemyAbilitySlot>().ability.isCooldown = false;
                    enemyAbilities.GetChild(i).GetComponent<EnemyAbilitySlot>().ability.cooldownObj.transform.SetAsLastSibling();
                    enemyAbilities.GetChild(i).GetComponent<EnemyAbilitySlot>().ability.cooldownObj.GetComponent<RectTransform>().sizeDelta = new Vector2(100, 0);
                }
            }

        }
        if (choice == 3)
        {
            battle.player.chaosFactor += 3;

            saveSystem.LoadAfterCutscene();
            for (int i = 0; i < hotbarPlayerAbilities.childCount; i++)
            {
                if (hotbarPlayerAbilities.GetChild(i).GetComponent<AbilityHotbar>().ability != null)
                {
                    hotbarPlayerAbilities.GetChild(i).GetComponent<AbilityHotbar>().ability.startCooldown = 0;
                    hotbarPlayerAbilities.GetChild(i).GetComponent<AbilityHotbar>().ability.isCooldown = false;
                    hotbarPlayerAbilities.GetChild(i).GetComponent<AbilityHotbar>().ability.cooldownObj.transform.SetAsLastSibling();
                    hotbarPlayerAbilities.GetChild(i).GetComponent<AbilityHotbar>().ability.cooldownObj.GetComponent<RectTransform>().sizeDelta = new Vector2(100, 0);
                }
            }
            for (int i = 0; i < godlikePlayerAbilities.childCount; i++)
            {
                if (godlikePlayerAbilities.GetChild(i).GetComponent<GodAbilityHotbar>().ability != null)
                {
                    godlikePlayerAbilities.GetChild(i).GetComponent<GodAbilityHotbar>().ability.startCooldown = 0;
                    godlikePlayerAbilities.GetChild(i).GetComponent<GodAbilityHotbar>().ability.isCooldown = false;
                    godlikePlayerAbilities.GetChild(i).GetComponent<GodAbilityHotbar>().ability.cooldownObj.transform.SetAsLastSibling();
                    godlikePlayerAbilities.GetChild(i).GetComponent<GodAbilityHotbar>().ability.cooldownObj.GetComponent<RectTransform>().sizeDelta = new Vector2(100, 0);
                }
            }
            for (int i = 0; i < enemyAbilities.childCount; i++)
            {
                if (enemyAbilities.GetChild(i).GetComponent<EnemyAbilitySlot>().ability != null)
                {
                    enemyAbilities.GetChild(i).GetComponent<EnemyAbilitySlot>().ability.startCooldown = 0;
                    enemyAbilities.GetChild(i).GetComponent<EnemyAbilitySlot>().ability.isCooldown = false;
                    enemyAbilities.GetChild(i).GetComponent<EnemyAbilitySlot>().ability.cooldownObj.transform.SetAsLastSibling();
                    enemyAbilities.GetChild(i).GetComponent<EnemyAbilitySlot>().ability.cooldownObj.GetComponent<RectTransform>().sizeDelta = new Vector2(100, 0);
                }
            }
        }
        if (choice == 4)
        {
            battle.player.chaosFactor += 3;

            saveSystem.LoadAfterCutscene();
            for (int i = 0; i < hotbarPlayerAbilities.childCount; i++)
            {
                if (hotbarPlayerAbilities.GetChild(i).GetComponent<AbilityHotbar>().ability != null)
                {
                    hotbarPlayerAbilities.GetChild(i).GetComponent<AbilityHotbar>().ability.startCooldown = 0;
                    hotbarPlayerAbilities.GetChild(i).GetComponent<AbilityHotbar>().ability.isCooldown = false;
                    hotbarPlayerAbilities.GetChild(i).GetComponent<AbilityHotbar>().ability.cooldownObj.transform.SetAsLastSibling();
                    hotbarPlayerAbilities.GetChild(i).GetComponent<AbilityHotbar>().ability.cooldownObj.GetComponent<RectTransform>().sizeDelta = new Vector2(100, 0);
                }
            }
            for (int i = 0; i < godlikePlayerAbilities.childCount; i++)
            {
                if (godlikePlayerAbilities.GetChild(i).GetComponent<GodAbilityHotbar>().ability != null)
                {
                    godlikePlayerAbilities.GetChild(i).GetComponent<GodAbilityHotbar>().ability.startCooldown = 0;
                    godlikePlayerAbilities.GetChild(i).GetComponent<GodAbilityHotbar>().ability.isCooldown = false;
                    godlikePlayerAbilities.GetChild(i).GetComponent<GodAbilityHotbar>().ability.cooldownObj.transform.SetAsLastSibling();
                    godlikePlayerAbilities.GetChild(i).GetComponent<GodAbilityHotbar>().ability.cooldownObj.GetComponent<RectTransform>().sizeDelta = new Vector2(100, 0);
                }
            }
            for (int i = 0; i < enemyAbilities.childCount; i++)
            {
                if (enemyAbilities.GetChild(i).GetComponent<EnemyAbilitySlot>().ability != null)
                {
                    enemyAbilities.GetChild(i).GetComponent<EnemyAbilitySlot>().ability.startCooldown = 0;
                    enemyAbilities.GetChild(i).GetComponent<EnemyAbilitySlot>().ability.isCooldown = false;
                    enemyAbilities.GetChild(i).GetComponent<EnemyAbilitySlot>().ability.cooldownObj.transform.SetAsLastSibling();
                    enemyAbilities.GetChild(i).GetComponent<EnemyAbilitySlot>().ability.cooldownObj.GetComponent<RectTransform>().sizeDelta = new Vector2(100, 0);
                }
            }
        }
    }

    public void ShowCutscene(int id)
    {
        saveSystem.Save();
        cutscenes.gameObject.SetActive(true);
        cutscenes.GetChild(id - 1).gameObject.SetActive(true);
        shownCutscenes[id - 1] = true;
        foreach (GameObject obj in notCutsceneObjs)
        {
            obj.SetActive(false);
        }
        if(id == 3)
        {
            StartCoroutine(StartShow());
        }
        isShowing = true;
    }

    public void ShowEnding(int id)
    {
        saveSystem.Save();
        cutscenes.gameObject.SetActive(true);
        cutscenes.GetChild(id + 2).gameObject.SetActive(true);
        shownCutscenes[id + 2] = true;
        if (id == 1)
        {
            StartCoroutine(StartEnding(1));
        }
        else if (id == 2)
        {
            StartCoroutine(StartEnding(2));
        }
        else
        {
            foreach (GameObject obj in notCutsceneObjs)
            {
                obj.SetActive(false);
            }
            StartCoroutine(StartEnding(3));
        }
        isShowing = true;
    }

    public IEnumerator StartEnding(int id)
    {
        yield return new WaitForSeconds(0.35f);
        if (id == 1)
        {
            battle.enemy.hp = 0.01f;
            battle.enemyHpText.text = NumberConversion.AbbreviateNumber(battle.enemy.hp, NumberConversion.StartAbbrevation.M);
            yield return new WaitForSeconds(2.15f);
            cutscenes.GetChild(id + 2).GetChild(0).gameObject.SetActive(true);
            yield return new WaitForSeconds(0.75f);
            consoleText.text = "// USER2 RECEIVED FULL RIGHTS\n";
            yield return new WaitForSeconds(3.15f);
            consoleInput.text = "CONNECT ";
            yield return new WaitForSeconds(2.15f);
            consoleInput.text = "CONNECT SERVER";
            yield return new WaitForSeconds(1.75f);
            consoleInput.text = "CONNECT SERVER MAIN";
            yield return new WaitForSeconds(2.15f);
            consoleInput.text = "";
            consoleText.text += "// USER2 CONNECTED TO SERVER MAIN\n";
            yield return new WaitForSeconds(3.05f);
            consoleInput.text = "START";
            yield return new WaitForSeconds(2.05f);
            consoleInput.text = "START DATA_TRANSFER";
            yield return new WaitForSeconds(2.45f);
            consoleInput.text = "START DATA_TRANSFER USER2";
            yield return new WaitForSeconds(2.15f);
            consoleInput.text = "START DATA_TRANSFER USER2 MAIN";
            yield return new WaitForSeconds(1.15f);
            consoleInput.text = "";
            consoleText.text += "// DATA IS BEING TRANSFERRED 0%";
            yield return new WaitForSeconds(2.15f);
            consoleText.text = "// USER2 RECEIVED FULL RIGHTS\n// USER2 CONNECTED TO SERVER MAIN\n// DATA IS BEING TRANSFERRED 6%";
            consoleInput.text = "USER1";
            yield return new WaitForSeconds(2.65f);
            consoleText.text = "// USER2 RECEIVED FULL RIGHTS\n// USER2 CONNECTED TO SERVER MAIN\n// DATA IS BEING TRANSFERRED 15%";
            consoleInput.text = "USER1 VAR";
            yield return new WaitForSeconds(1.85f);
            consoleText.text = "// USER2 RECEIVED FULL RIGHTS\n// USER2 CONNECTED TO SERVER MAIN\n// DATA IS BEING TRANSFERRED 19%";
            consoleInput.text = "USER1 VAR isAlive";
            yield return new WaitForSeconds(3.50f);
            consoleText.text = "// USER2 RECEIVED FULL RIGHTS\n// USER2 CONNECTED TO SERVER MAIN\n// DATA IS BEING TRANSFERRED 44%";
            consoleInput.text = "USER1 VAR isAlive False";
            yield return new WaitForSeconds(2.50f);
            consoleText.text = "// USER2 RECEIVED FULL RIGHTS\n// USER2 CONNECTED TO SERVER MAIN\n// DATA IS BEING TRANSFERRED 91%\n// USER1 VAR isAlive is now False\n// USER1 GOT ENDING #1";
            consoleInput.text = "";
            SaveSystem.SaveCutscenes(shownCutscenes, true);
            Debug.Log("ura");
            yield return new WaitForSeconds(4.70f);
            Application.Quit();
        }
        else if (id == 2)
        {
            battle.enemy.hp = 0f;
            battle.enemyHpText.text = NumberConversion.AbbreviateNumber(battle.enemy.hp, NumberConversion.StartAbbrevation.M);
            yield return new WaitForSeconds(2.15f);
            cutscenes.GetChild(id + 2).GetChild(0).gameObject.SetActive(true);
            yield return new WaitForSeconds(0.75f);
            consoleText2.text = "// NEW OWNER USER3 DETECTED\n";
            yield return new WaitForSeconds(1.15f);
            consoleText2.text += "// ACCESS RIGHTS ARE TRANSFERRED TO USER3\n";
            yield return new WaitForSeconds(1.15f);
            consoleInput2.text = "CONNECT ";
            yield return new WaitForSeconds(2.15f);
            consoleInput2.text = "CONNECT SERVER";
            yield return new WaitForSeconds(1.75f);
            consoleInput2.text = "CONNECT SERVER MAIN";
            yield return new WaitForSeconds(2.15f);
            consoleInput2.text = "";
            consoleText2.text += "// USER3 CONNECTED TO SERVER MAIN\n";
            yield return new WaitForSeconds(3.05f);
            consoleInput2.text = "START";
            yield return new WaitForSeconds(2.05f);
            consoleInput2.text = "START DATA_TRANSFER";
            yield return new WaitForSeconds(2.45f);
            consoleInput2.text = "START DATA_TRANSFER MAIN";
            yield return new WaitForSeconds(2.15f);
            consoleInput2.text = "START DATA_TRANSFER MAIN USER3";
            yield return new WaitForSeconds(1.15f);
            consoleInput2.text = "";
            consoleText2.text += "// DATA IS BEING TRANSFERRED 0%";
            yield return new WaitForSeconds(2.15f);
            consoleText2.text = "// NEW OWNER USER3 DETECTED\n// ACCESS RIGHTS ARE TRANSFERRED TO USER3\n// USER3 CONNECTED TO SERVER MAIN\n// DATA IS BEING TRANSFERRED 12%";
            yield return new WaitForSeconds(2.65f);
            consoleText2.text = "// NEW OWNER USER3 DETECTED\n// ACCESS RIGHTS ARE TRANSFERRED TO USER3\n// USER3 CONNECTED TO SERVER MAIN\n// DATA IS BEING TRANSFERRED 25%";
            yield return new WaitForSeconds(1.85f);
            consoleText2.text = "// NEW OWNER USER3 DETECTED\n// ACCESS RIGHTS ARE TRANSFERRED TO USER3\n// USER3 CONNECTED TO SERVER MAIN\n// DATA IS BEING TRANSFERRED 60%";
            yield return new WaitForSeconds(3.50f);
            consoleText2.text = "// NEW OWNER USER3 DETECTED\n// ACCESS RIGHTS ARE TRANSFERRED TO USER3\n// USER3 CONNECTED TO SERVER MAIN\n// DATA IS BEING TRANSFERRED 93%";
            yield return new WaitForSeconds(2.50f);
            consoleText2.text = "// NEW OWNER USER3 DETECTED\n// ACCESS RIGHTS ARE TRANSFERRED TO USER3\n// USER3 CONNECTED TO SERVER MAIN\n// DATA IS TRANSFERRED TO USER3\n";
            yield return new WaitForSeconds(3.05f);
            consoleInput2.text = "START";
            yield return new WaitForSeconds(2.05f);
            consoleInput2.text = "START DATA_TRANSFER";
            yield return new WaitForSeconds(2.45f);
            consoleInput2.text = "START DATA_TRANSFER USER3";
            yield return new WaitForSeconds(2.15f);
            consoleInput2.text = "START DATA_TRANSFER USER3 h&)&7cZHqWppx;x@jM";
            yield return new WaitForSeconds(1.25f);
            consoleInput2.text = "";
            consoleText2.text += "// DATA IS BEING TRANSFERRED 3%\n";
            yield return new WaitForSeconds(1.25f);
            consoleText2.text += "// USER1 GOT ENDING #2\n";

            SaveSystem.SaveCutscenes(shownCutscenes, true);
            yield return new WaitForSeconds(4.70f);
            Application.Quit();
        }
        else if (id == 3)
        {
            yield return new WaitForSeconds(5.35f);
            cutscenes.GetChild(5).GetComponent<AudioSource>().Stop();
            firstPart.SetActive(false);
            secondPart.SetActive(true);
            //SaveSystem.SaveCutscenes(shownCutscenes, true);
            yield return new WaitForSeconds(4.70f);
            //Application.Quit();
        }
    }

    public IEnumerator StartShow()
    {
        cutscene3Messages[0].SetActive(true);
        yield return new WaitForSeconds(3f);
        cutscene3Messages[0].SetActive(false);
        cutscene3Messages[1].SetActive(true);
        yield return new WaitForSeconds(2.5f);
        cutscene3Messages[1].SetActive(false);
        cutscene3Messages[2].SetActive(true);
        yield return new WaitForSeconds(2.5f);
        cutscene3Messages[2].SetActive(false);
        cutscene3Messages[3].SetActive(true);
        yield return new WaitForSeconds(2.2f);
        cutscene3Messages[3].SetActive(false);
        cutscene3Messages[4].SetActive(true);
        yield return new WaitForSeconds(1.8f);
        cutscene3Messages[4].SetActive(false);
        cutscene3Messages[5].SetActive(true);
        yield return new WaitForSeconds(3.8f);
        cutscene3Messages[5].SetActive(false);
        cutscene3Messages[6].SetActive(true);
        yield return new WaitForSeconds(2.2f);
        cutscene3Messages[6].SetActive(false);
        cutscene3Messages[7].SetActive(true);
        yield return new WaitForSeconds(3.2f);
        cutscene3Messages[7].SetActive(false);
        cutscene3Messages[8].SetActive(true);
        yield return new WaitForSeconds(4.0f);
        cutscene3Messages[8].SetActive(false);
        cutscene3Messages[9].SetActive(true);
        yield return new WaitForSeconds(3.0f);
        cutscene3Messages[9].SetActive(false);
        cutscene3Messages[10].SetActive(true);
        yield return new WaitForSeconds(2.2f);
        cutscene3Messages[10].SetActive(false);
        cutscene3Messages[11].SetActive(true);
        yield return new WaitForSeconds(2.6f);
        cutscene3Messages[11].SetActive(false);
        cutscene3Messages[12].SetActive(true);
        yield return new WaitForSeconds(4.6f);
        cutscenes.GetChild(2).GetComponent<AudioSource>().Stop();
        cutscene3Glitch.SetActive(true);
        yield return new WaitForSeconds(6.0f);
        StartCoroutine(AfterCutsceneEffect(4));
    }

    public void Ending3_Choose(int id)
    {
        finalChoice1.interactable = false;
        finalChoice2.interactable = false;
        if (id == 2)
        {
            finalChoice2.GetComponent<Text>().text = "•doom entire universe";
        }
        StartCoroutine(EndUniverse());
    }

    public IEnumerator EndUniverse()
    {
        yield return new WaitForSeconds(3f);
        secondPart.GetComponent<AudioSource>().Play();
        universeImage.color = new Color32(229, 0, 0, 255);
        endingChoices.SetActive(false);
        yield return new WaitForSeconds(5f);
        ending.SetActive(true);
        SaveSystem.SaveCutscenes(shownCutscenes, true);
        yield return new WaitForSeconds(6.70f);
        Application.Quit();
    }

    public void Save()
    {
        if(!shownCutscenes[3] && !shownCutscenes[4] && !shownCutscenes[5])
            SaveSystem.SaveCutscenes(shownCutscenes);
    }

    public void Load()
    {
        CutsceneData data = SaveSystem.LoadCutscenes();
        CutsceneData data2 = SaveSystem.LoadEndings();
        if (data != null)
        {
            shownCutscenes = data.shownCutscenes;
            isShowing = data.isShowing;
            if (data2 != null)
            {
                isShowing = true;

                battle.isRegenerate = false;
                battle.isBattle = false;

                battle.LoadPlayer();
                battle.LoadEnemy();
                battle.InitTimers();
                battle.timerPlayer.Change(Timeout.Infinite, Timeout.Infinite);
                battle.timerEnemy.Change(Timeout.Infinite, Timeout.Infinite);
                battle.hpRegPlayer.Change(Timeout.Infinite, Timeout.Infinite);
                battle.hpRegEnemy.Change(Timeout.Infinite, Timeout.Infinite);
                
                battle.enemyN = 31;
                battle.enemy.hp = battle.enemy.maxHp;
                battle.enemy.mana = battle.enemy.maxMana;
                battle.SwitchEnemy(31);
                StartCoroutine(battle.ReturnAfterEnd());
                saveSystem.LoadAfterEnd();
            }
            else
            {
                if (isShowing)
                {
                    if (shownCutscenes[0])
                        ShowCutscene(1);
                    else if (shownCutscenes[1])
                        ShowCutscene(2);
                    else if (shownCutscenes[2])
                        ShowCutscene(3);
                    else if (shownCutscenes[3])
                        ShowEnding(1);
                    else if (shownCutscenes[4])
                        ShowEnding(2);
                    else if (shownCutscenes[5])
                        ShowEnding(3);
                    foreach (GameObject obj in notCutsceneObjs)
                    {
                        obj.SetActive(false);
                    }
                }
                else
                {
                    cutscenes.GetChild(0).gameObject.SetActive(false);
                    cutscenes.GetChild(1).gameObject.SetActive(false);
                    cutscenes.GetChild(2).gameObject.SetActive(false);
                }
            }
        }
        else
        {
            cutscenes.GetChild(0).gameObject.SetActive(false);
            cutscenes.GetChild(1).gameObject.SetActive(false);
            cutscenes.GetChild(2).gameObject.SetActive(false);
        }
    }
}
