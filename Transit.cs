using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Transit : MonoBehaviour
{
    public Button trainBut;
    public Button techniqueBut;
    public Button battleBut;
    public Button playerBut;
    public Button skillsBut;
    public Button talantsBut;
    public Button gatheringBut;
    public Button reincarnationBut;
    public Button metaBut;
    public Button seaBut;

    [SerializeField] private GameObject[] windows = new GameObject[10];
    [SerializeField] private GameObject collectionSector;
    public bool[] isShown = new bool[10];

    void Start()
    {
        trainBut.onClick.AddListener(delegate { MainChange(0); });
        techniqueBut.onClick.AddListener(delegate { MainChange(1); });
        battleBut.onClick.AddListener(delegate { MainChange(2); });
        playerBut.onClick.AddListener(delegate { MainChange(3); });
        skillsBut.onClick.AddListener(delegate { MainChange(4); });
        talantsBut.onClick.AddListener(delegate { MainChange(5); });
        gatheringBut.onClick.AddListener(delegate { MainChange(6); });
        reincarnationBut.onClick.AddListener(delegate { MainChange(7); });
        metaBut.onClick.AddListener(delegate { MainChange(8); });
        seaBut.onClick.AddListener(delegate { MainChange(9); });
    }

    public void MainChange(int index)
    {
        for(int i = 0; i < windows.Length; i++)
        {
            if(i != index)
            {
                windows[i].transform.localPosition = new Vector3(-100000, 0, 0);
                isShown[i] = false;
            }
            else
            {
                windows[i].transform.localPosition = new Vector3(0, 0, 0);
                isShown[i] = true;
            }
        }
        collectionSector.transform.localPosition = new Vector3(-100000, 0, 0);
    }

    void ChangeWindow()
    {
        for (int i = 0; i < isShown.Length; i++)
        {
            if (isShown[i])
            {
                MainChange(i);
            }
        }
    }

    public void SaveWindow()
    {
        SaveSystem.SaveWindow(isShown);
    }

    public void LoadWindow()
    {
        WindowData data = SaveSystem.LoadWindow();

        if (data != null)
        {
            this.isShown = data.isShown;

            ChangeWindow();
        }
    }
}
