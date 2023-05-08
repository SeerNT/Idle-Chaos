using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeWindow : MonoBehaviour
{

    public static bool isOpen = false;
    private Transform upgrader;

    void Start()
    {
        this.GetComponent<Button>().onClick.AddListener(Open);
        string name = this.transform.parent.name.Replace("_UPG", "");
        upgrader = transform.parent.parent.Find(name + "_Upgrader");
        upgrader.Find("CloseBut").GetComponent<Button>().onClick.AddListener(Close);
    }


    void Open()
    {
        if (!isOpen)
        {
            isOpen = true;
            upgrader.GetComponent<UpgradeEnchancer>().ActivateOnScreen();
        }
    }
    void Close()
    {
        isOpen = false;
        upgrader.GetComponent<UpgradeEnchancer>().DeactivateOnScreen();
    }
}
