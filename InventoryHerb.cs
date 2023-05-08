﻿using NT_Utils.Conversion;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryHerb : MonoBehaviour
{
    private GatheringManager manager;
    private bool isHoveringOver = false;
    private Battle battle;

    public Herb herb;

    private CancellationTokenSource tokenSource;

    private byte redActive = 255;

    private static GameObject currentHoverSlot = null;

    public void Awake()
    {
        manager = GameObject.Find("Gathering").GetComponent<GatheringManager>();
        battle = GameObject.Find("Battle").GetComponent<Battle>();
    }

    public void Update()
    {
        this.IsPointerOverItem();

        if (isHoveringOver)
        {
            if (Input.GetMouseButtonDown(0))
            {
                currentHoverSlot.GetComponent<InventoryHerb>().UseHerb();
            }
        }
    }

    public void IsPointerOverItem()
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);

        foreach (RaycastResult rc in results)
        {

            if (rc.gameObject.name == "Herb")
            {
                isHoveringOver = true;
                currentHoverSlot = rc.gameObject;
                Transform statsObj = manager.herbStatsObject.transform.GetChild(1);
                statsObj.gameObject.SetActive(true);
                manager.herbStatsObject.transform.GetChild(2).gameObject.SetActive(false);

                statsObj.GetChild(0).GetComponent<Text>().text = rc.gameObject.GetComponent<InventoryHerb>().herb.displayName;
                statsObj.GetChild(2).GetComponent<Text>().text = rc.gameObject.GetComponent<InventoryHerb>().herb.description;
                statsObj.GetChild(3).GetComponent<Text>().text = "Quality:\n" + rc.gameObject.GetComponent<InventoryHerb>().herb.quality.ToString();
                statsObj.GetChild(4).GetComponent<Text>().text = "Active Time:" + rc.gameObject.GetComponent<InventoryHerb>().herb.activeTime.ToString();
                if (rc.gameObject.GetComponent<InventoryHerb>().herb.givenBonuses.Count == 0)
                {
                    rc.gameObject.GetComponent<InventoryHerb>().herb.Init();
                }
                int i = 3;
                foreach (KeyValuePair<string, float> bonus in rc.gameObject.GetComponent<InventoryHerb>().herb.givenBonuses)
                {
                    i++;
                    statsObj.GetChild(i + 1).GetComponent<Text>().text = bonus.Key + ": " + NumberConversion.AbbreviateNumber(bonus.Value);
                    statsObj.GetChild(i + 1).gameObject.SetActive(true);

                }

                break;
            }
            if (rc.gameObject.transform.parent.name == "Canvas")
            {
                ExitHerb();
            }
        }
    }

    public void ExitHerb()
    {
        Transform statsObj = manager.herbStatsObject.transform.GetChild(1);
        statsObj.gameObject.SetActive(false);
        manager.herbStatsObject.transform.GetChild(2).gameObject.SetActive(true);
        isHoveringOver = false;
    }

    public void Reset()
    {
        if(tokenSource != null)
            tokenSource.Cancel();
        CancelInvoke();
    }

    public async Task TaskDelayWithCancel(CancellationToken token)
    {
        await Task.Delay((int)(currentHoverSlot.GetComponent<InventoryHerb>().herb.activeTime * 1000), token).ContinueWith(t => battle.player.DisableHerb(currentHoverSlot.GetComponent<InventoryHerb>().herb));
    }

    public async Task TaskDelayWithCancel(CancellationToken token, float time)
    {
        await Task.Delay((int)(time * 1000), token).ContinueWith(t => battle.player.DisableHerb(herb));
    }

    public void IncreaseTime()
    {
        currentHoverSlot.GetComponent<InventoryHerb>().herb.currentActiveTime++;
    }

    public void UseHerb()
    {
        if(manager.GetHerbSlot(currentHoverSlot.GetComponent<InventoryHerb>()).amount > 0)
        {
            manager.GetHerbSlot(currentHoverSlot.GetComponent<InventoryHerb>()).amount--;
            if (tokenSource != null)
            {
                tokenSource.Cancel();
                CancelInvoke();
            }
            battle.player.UseHerb(currentHoverSlot.GetComponent<InventoryHerb>().herb);

            redActive = 255;
            InvokeRepeating("LowerActive", 0, currentHoverSlot.GetComponent<InventoryHerb>().herb.activeTime / 255);
            InvokeRepeating("IncreaseTime", 0, 1);
            tokenSource = new CancellationTokenSource();
            TaskDelayWithCancel(tokenSource.Token);
        }
    }

    public void UseHerb(float time)
    {
        if(currentHoverSlot != null)
        {
            if (manager.GetHerbSlot(currentHoverSlot.GetComponent<InventoryHerb>()).amount > 0)
            {
                manager.GetHerbSlot(currentHoverSlot.GetComponent<InventoryHerb>()).amount--;
                if (tokenSource != null)
                {
                    tokenSource.Cancel();
                    CancelInvoke();
                }
                //battle.player.UseHerb(herb);
                currentHoverSlot.GetComponent<InventoryHerb>().herb.isActive = true;
                redActive = (byte)(255 - (currentHoverSlot.GetComponent<InventoryHerb>().herb.activeTime * (currentHoverSlot.GetComponent<InventoryHerb>().herb.activeTime - time)));
                this.InvokeRepeating("LowerActive", 0, time / 255);

                tokenSource = new CancellationTokenSource();
                TaskDelayWithCancel(tokenSource.Token, time);
            }
        }
        else
        {
            if (manager.GetHerbSlot(this).amount > 0)
            {
                manager.GetHerbSlot(this).amount--;
                if (tokenSource != null)
                {
                    tokenSource.Cancel();
                    CancelInvoke();
                }
                //battle.player.UseHerb(herb);
                this.herb.isActive = true;
                redActive = (byte)(255 - (this.herb.activeTime * (this.herb.activeTime - time)));
                this.InvokeRepeating("LowerActive", 0, time / 255);

                tokenSource = new CancellationTokenSource();
                TaskDelayWithCancel(tokenSource.Token, time);
            }
        }
        
    }

    public void LowerActive()
    {
        redActive--;
        this.GetComponent<Image>().color = new Color32(redActive, 255, 255, 255);
        if (redActive <= 0)
        {
            redActive = 255;
            this.GetComponent<Image>().color = new Color32(redActive, 255, 255, 255);
            CancelInvoke();
        }
    }

}
