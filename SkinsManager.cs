using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkinsManager : MonoBehaviour
{
    [Header("Images")]
    [SerializeField] Image playerSkin;
    [SerializeField] Sprite[] skinsBank;
    [SerializeField] List<Sprite> availableSkins;
    private int currentSkinNum = 0;
    private int lastId = 0;
    [Header("Drop Screen")]
    [SerializeField] private GameObject dropSkinScreen;
    [SerializeField] private Image skinIcon;
    [SerializeField] private Text titleText;
    [SerializeField] private Text skinName;

    void Start()
    {
        transform.GetChild(1).GetComponent<Button>().onClick.AddListener(PreviousSkin);
        transform.GetChild(2).GetComponent<Button>().onClick.AddListener(NextSkin);
    }

    void PreviousSkin()
    {
        if (currentSkinNum != 0)
        {
            currentSkinNum--;
            playerSkin.sprite = availableSkins[currentSkinNum];
        }
    }

    void NextSkin()
    {
        if (currentSkinNum != availableSkins.Count-1)
        {
            currentSkinNum++;
            playerSkin.sprite = availableSkins[currentSkinNum];
        }
    }

    public void AddSkin(int id, string monsterName)
    {
        if (!availableSkins.Contains(skinsBank[id]))
        {
            availableSkins.Add(skinsBank[id]);

            dropSkinScreen.SetActive(true);
            skinIcon.sprite = skinsBank[id];
            string title = titleText.text.Replace("{MONSTER}", monsterName);
            titleText.text = title;
            skinName.text = skinsBank[id].name;

            lastId = id;
        }
    }    

    public void Save()
    {
        SaveSystem.SaveSkin(currentSkinNum, lastId);
    }

    public void Load()
    {
        SkinData data = SaveSystem.LoadSkin();

        if (data != null)
        {
            this.currentSkinNum = data.currentSkinNum;
            this.lastId = data.lastId;
            playerSkin.sprite = skinsBank[currentSkinNum];
            for(int i = 1; i <= lastId; i++)
            {
                availableSkins.Add(skinsBank[i]);
            }
        }
    }
}
