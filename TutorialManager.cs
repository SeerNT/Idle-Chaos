using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    public Transit menu;
    public GameObject[] dialogues = new GameObject[10];

    [SerializeField] private Battle battle;

    private int currentDialogue = 0;
    public GameObject itemStats;

    void Start()
    {
        string path = Application.persistentDataPath + "/tutorial.save";
        if (!File.Exists(path))
        {
            StartTutorial();
        }
    }

    public void StartTutorial()
    {
        for(int i = 0; i < transform.childCount-1; i++)
        {
            dialogues[i] = transform.GetChild(i).gameObject;
        }
        
        dialogues[currentDialogue].SetActive(true);
        menu.MainChange(2);
    }

    public void NextTutorial()
    {
        dialogues[currentDialogue].SetActive(false);
        currentDialogue++;
        if (currentDialogue == 33)
        {
            currentDialogue = -1;
            Save();
            this.gameObject.SetActive(false);
        }
        else
        {
            dialogues[currentDialogue].SetActive(true);
            if (currentDialogue == 6)
            {
                menu.MainChange(3);
            }
            if (currentDialogue == 17)
            {
                menu.MainChange(0);
                if (Application.platform == RuntimePlatform.Android)
                {
                    dialogues[currentDialogue].transform.GetChild(0).GetChild(0).gameObject.SetActive(false);
                    dialogues[currentDialogue].transform.GetChild(0).GetChild(1).gameObject.SetActive(true);
                }
                else
                {
                    dialogues[currentDialogue].transform.GetChild(0).GetChild(0).gameObject.SetActive(true);
                    dialogues[currentDialogue].transform.GetChild(0).GetChild(1).gameObject.SetActive(false);
                }
            }
            if (currentDialogue == 18)
            {
                menu.MainChange(1);
            }
            if (currentDialogue == 19)
            {
                if (Application.platform == RuntimePlatform.Android)
                {
                    dialogues[currentDialogue].transform.GetChild(0).GetChild(0).gameObject.SetActive(false);
                    dialogues[currentDialogue].transform.GetChild(0).GetChild(1).gameObject.SetActive(true);
                }
                else
                {
                    dialogues[currentDialogue].transform.GetChild(0).GetChild(0).gameObject.SetActive(true);
                    dialogues[currentDialogue].transform.GetChild(0).GetChild(1).gameObject.SetActive(false);
                }
            }
            if (currentDialogue == 20)
            {
                if (Application.platform == RuntimePlatform.Android)
                {
                    dialogues[currentDialogue].transform.GetChild(0).GetChild(0).gameObject.SetActive(false);
                    dialogues[currentDialogue].transform.GetChild(0).GetChild(1).gameObject.SetActive(true);
                }
                else
                {
                    dialogues[currentDialogue].transform.GetChild(0).GetChild(0).gameObject.SetActive(true);
                    dialogues[currentDialogue].transform.GetChild(0).GetChild(1).gameObject.SetActive(false);
                }
            }
            if (currentDialogue == 21)
            {
                if (Application.platform == RuntimePlatform.Android)
                {
                    dialogues[currentDialogue].transform.GetChild(0).GetChild(0).gameObject.SetActive(false);
                    dialogues[currentDialogue].transform.GetChild(0).GetChild(1).gameObject.SetActive(true);
                }
                else
                {
                    dialogues[currentDialogue].transform.GetChild(0).GetChild(0).gameObject.SetActive(true);
                    dialogues[currentDialogue].transform.GetChild(0).GetChild(1).gameObject.SetActive(false);
                }
            }
            if (currentDialogue == 22)
            {
                if (Application.platform == RuntimePlatform.Android)
                {
                    dialogues[currentDialogue].transform.GetChild(0).GetChild(0).gameObject.SetActive(false);
                    dialogues[currentDialogue].transform.GetChild(0).GetChild(1).gameObject.SetActive(true);
                }
                else
                {
                    dialogues[currentDialogue].transform.GetChild(0).GetChild(0).gameObject.SetActive(true);
                    dialogues[currentDialogue].transform.GetChild(0).GetChild(1).gameObject.SetActive(false);
                }
            }
            if (currentDialogue == 23)
            {
                menu.MainChange(3);
            }
            if (currentDialogue == 24)
            {
                menu.MainChange(4);
            }
            if (currentDialogue == 26)
            {
                menu.MainChange(3);
                itemStats.SetActive(false);
            }
            if (currentDialogue == 30)
            {
                itemStats.SetActive(true);
                GameItem itemDrop = GameItem.GetItemById(1);
                if (itemDrop != null)
                {
                    string[] arrNames = battle.names.ToArray();
                    battle.titleDrop.text = "I(cool dude) DROPPED ITEM";
                    battle.rarityDrop.text = itemDrop.rarity.ToString();
                    battle.itemNameDrop.text = itemDrop.displayName;
                    battle.itemDescDrop.text = itemDrop.description;
                    battle.iconDrop.GetComponent<Image>().sprite = itemDrop.icon;
                    battle.dropScreen.SetActive(true);
                    battle.inventory.AddItem(itemDrop);
                }
            }
        }
    }

    public void Skip()
    {
        currentDialogue = -1;
        Save();
    }

    public void Save()
    {
        SaveSystem.SaveTutorial(currentDialogue);
    }

    public void Load()
    {
        TutorialData data = SaveSystem.LoadTutorial();

        if (data != null)
        {
            if(data.currentDialogue != -1)
            {
                for (int i = 0; i < transform.childCount - 1; i++)
                {
                    dialogues[i] = transform.GetChild(i).gameObject;
                }

                if (data.currentDialogue != 0)
                {
                    currentDialogue = data.currentDialogue - 1;
                    NextTutorial();
                }
                else
                {
                    dialogues[currentDialogue].SetActive(true);
                }
            }
            else
            {
                this.gameObject.SetActive(false);
            }
        }
    }
}
