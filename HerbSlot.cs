using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HerbSlot : MonoBehaviour
{
    [SerializeField] private GatheringManager manager;
    [SerializeField] private Text amountText; 

    public Herb herb;
    public int amount;
    private bool isAdded = false;
    private bool isLoaded = false;

    void Update()
    {
        UpdateUI();
    }

    void UpdateUI()
    {
        if(herb != null)
        {
            amountText.text = amount.ToString();
        }
        
        if (isLoaded && herb != null && !isAdded)
        {
            string name = this.name;
            name = name.Replace("Inv", "");
            //manager.AddHerbToSlot(herb, int.Parse(name));
            isAdded = true;
        }
    }

    void ActivateAfterLoad()
    {
        transform.Find("Herb").GetComponent<InventoryHerb>().UseHerb(herb.activeTime - herb.currentActiveTime);
    }

    public void Reset()
    {
        if(herb != null)
        {
            herb.isActive = false;
            herb.currentActiveTime = 0;
        }
        herb = null;
        amount = 0;
    }

    public void SaveHerb()
    {
        if (herb != null)
        {
            SaveSystem.SaveHerb(herb, amount, this.name);
        }
    }

    public void LoadHerb()
    {
        HerbData data = SaveSystem.LoadHerb(this.name);

        if (data != null)
        {
            string name = this.name;
            name = name.Replace("Inv", "");
            manager.AddHerbToSlot(Herb.GetHerbById(int.Parse(name)), int.Parse(name));

            herb = Herb.GetHerbById(int.Parse(name));
            herb.isActive = data.isActive;
            herb.currentActiveTime = data.currentActiveTime;
            amount = data.amount;
            herb.id = data.herbId.ToString();

            if (herb.isActive)
            {
                if (herb.activeTime != herb.currentActiveTime)
                {
                    ActivateAfterLoad();
                }
            }
        }
    }
}
