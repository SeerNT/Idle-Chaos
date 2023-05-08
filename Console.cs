using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Console : MonoBehaviour
{
    public Text commandField;
    public InputField inputField;

    public Battle battle;

    public bool isOpen = false;

    private string[] prevCommands = new string[10] {"", "", "", "", "", "", "", "", "", ""};

    private int commandNum = 0;
    private int rollCommandNum = 0;

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Return))
        {
            SubmitCommand(inputField.text);
            ClearInput();
        }

        if (Input.GetKeyUp(KeyCode.F1))
        {
            if (!isOpen)
            {
                OpenConsole();
            }
            else
            {
                CloseConsole();
            }
        }

        if (Input.GetKeyUp(KeyCode.UpArrow))
        {
            string[] commandsRev = prevCommands;
            Array.Reverse(commandsRev);
            commandsRev = commandsRev.Where(x => !string.IsNullOrEmpty(x)).ToArray();

            rollCommandNum++;
            if(rollCommandNum <= commandsRev.Length)
            {
                inputField.text = commandsRev[rollCommandNum - 1];
            }
        }
    }
    
    void CloseConsole()
    {
        ClearInput();

        this.transform.localPosition = new Vector3(-10000, 0, 0);

        isOpen = false;

        commandField.gameObject.SetActive(false);
        inputField.gameObject.SetActive(false);
    }

    void OpenConsole()
    {
        inputField.gameObject.SetActive(true);
        commandField.gameObject.SetActive(true);

        this.transform.localPosition = new Vector3(0, 0, 0);

        isOpen = true;

        inputField.Select();
        inputField.ActivateInputField();
    }

    void SubmitCommand(string command)
    {
        string commandOutput = "";
        commandNum++;
        if (commandNum >= 9)
        {
            commandNum = 9;
        }
        prevCommands[commandNum-1] = command;

        command = command.ToLower();

        if (command.Contains("add"))
        {
            if (command.Contains("magic"))
            {
                string commandValue = PrepareCommand(command);

                GameManager.magic += double.Parse(commandValue);
                Reincarnation.totalEarnMagic += double.Parse(commandValue);

                commandOutput = "ADDED " + commandValue + " MAGIC";
            }
            else if (command.Contains("xp"))
            {
                string commandValue = PrepareCommand(command);

                GameManager.xp += double.Parse(commandValue);
                Reincarnation.totalEarnXp += double.Parse(commandValue);

                commandOutput = "ADDED " + commandValue + " XP";
            }
            else if (command.Contains("blood"))
            {
                string commandValue = PrepareCommand(command);

                GameManager.blood += double.Parse(commandValue);

                commandOutput = "ADDED " + commandValue + " BLOOD";
            }
            else if (command.Contains("qi"))
            {
                string commandValue = PrepareCommand(command);

                GameManager.qi += double.Parse(commandValue);

                commandOutput = "ADDED " + commandValue + " QI";
            }
            else if (command.Contains("meta"))
            {
                string commandValue = PrepareCommand(command);

                GameManager.meta += double.Parse(commandValue);

                commandOutput = "ADDED " + commandValue + " META";
            }

            GameManager.UpdateText();
        }
        else if (command.Contains("give"))
        {
            if (command.Contains("randomitem"))
            {
                GameItem itemDrop = null;
                if (command.Contains("common"))
                {
                    itemDrop = GameItem.GetRandomRarityItem(GameItem.Rarity.Common, battle);
                    commandOutput = "DROPPED COMMON ITEM";
                }
                else if (command.Contains("uncommon"))
                {
                    itemDrop = GameItem.GetRandomRarityItem(GameItem.Rarity.Uncommon, battle);
                    commandOutput = "DROPPED UNCOMMON ITEM";
                }
                else if (command.Contains("rare"))
                {
                    itemDrop = GameItem.GetRandomRarityItem(GameItem.Rarity.Rare, battle);
                    commandOutput = "DROPPED RARE ITEM";
                }
                else if (command.Contains("epic"))
                {
                    itemDrop = GameItem.GetRandomRarityItem(GameItem.Rarity.Epic, battle);
                    commandOutput = "DROPPED EPIC ITEM";
                }
                else if (command.Contains("legendary"))
                {
                    itemDrop = GameItem.GetRandomRarityItem(GameItem.Rarity.Legendary, battle);
                    commandOutput = "DROPPED LEGENDARY ITEM";
                }
                else if (command.Contains("mythic"))
                {
                    itemDrop = GameItem.GetRandomRarityItem(GameItem.Rarity.Mythic, battle);
                    commandOutput = "DROPPED MYTHIC ITEM";
                }
                else
                {
                    itemDrop = GameItem.GetRandomRarityItem(GameItem.Rarity.NONE, battle);
                    commandOutput = "DROPPED RANDOM ITEM";
                }

                if (itemDrop != null)
                {
                    string[] arrNames = battle.names.ToArray();
                    battle.titleDrop.text = "CONSOLE" + " DROPPED ITEM";
                    battle.rarityDrop.text = itemDrop.rarity.ToString();
                    battle.itemNameDrop.text = itemDrop.displayName;
                    battle.itemDescDrop.text = itemDrop.description;
                    battle.iconDrop.GetComponent<Image>().sprite = itemDrop.icon;
                    battle.dropScreen.SetActive(true);
                    battle.inventory.AddItem(itemDrop);
                }
            }
            else if (command.Contains("item"))
            {
                string commandValue = PrepareCommand(command);

                int itemId = int.Parse(commandValue);
                GameItem itemDrop = GameItem.GetItemById(itemId);
                if (itemDrop != null)
                {
                    string[] arrNames = battle.names.ToArray();
                    battle.titleDrop.text = "CONSOLE" + " DROPPED ITEM";
                    battle.rarityDrop.text = itemDrop.rarity.ToString();
                    battle.itemNameDrop.text = itemDrop.displayName;
                    battle.itemDescDrop.text = itemDrop.description;
                    battle.iconDrop.GetComponent<Image>().sprite = itemDrop.icon;
                    battle.dropScreen.SetActive(true);
                    battle.inventory.AddItem(itemDrop);
                }
                commandOutput = "DROPPED ITEM" + commandValue;
            }
        }

        DateTime date = DateTime.Now;

        if (commandOutput != "")
        {
            commandField.text += date + " " + commandOutput + "\n";
        }
        else
        {
            commandField.text += date + " ERROR IN COMMAND" + "\n";
        }
    }

    void ClearInput()
    {
        inputField.text = "";
        inputField.Select();
        inputField.ActivateInputField();
    }

    private static string PrepareCommand(string input)
    {
        return new string(input.Where(c => char.IsDigit(c)).ToArray());
    }
}
