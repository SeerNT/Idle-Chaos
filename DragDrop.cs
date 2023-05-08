using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using NT_Utils.Conversion;

public class DragDrop : MonoBehaviour,IBeginDragHandler, IEndDragHandler, IDragHandler
{
    public Inventory inventory;
    public Canvas canvas;
    public GameObject itemStatsObject;

    private RectTransform rectTrans;
    private CanvasGroup canvasGroup;
    private Vector3 mousePos;
    private static bool isHoveringItem;
    private bool canDrag;
    private static bool isDragging = false;
    private static GameItem draggingItem;
    private static GameObject draggingObj;
    private static bool isStatsShown = false;

    private static GameObject currentHoverSlot = null;

    private void Awake()
    {
        canDrag = true;
        inventory = GameObject.Find("Inventory").GetComponent<Inventory>();
        itemStatsObject = GameObject.Find("ItemStats");
        rectTrans = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
    }

    void Update() {
        
        if(this.name == "CONTROLDRAGDROP")
        {
            IsPointerOverItem();
        }
        else
        {
            canDrag = !this.transform.parent.parent.name.Contains("BuyAvailable");
            if (isHoveringItem)
            {
                if (!isStatsShown)
                {
                    if (currentHoverSlot != null)
                    {
                        if (isDragging)
                        {
                            if(draggingObj.transform.parent.name != currentHoverSlot.name)
                            {
                                CreateStatsWindow();
                            }
                        }
                        else
                        {
                            CreateStatsWindow();
                        }
                    } 
                }
                else if (isStatsShown)
                {
                    if (currentHoverSlot.GetComponent<InventorySlot>() != null)
                    {
                        // Change Stats Window Height depending on bonuses amount
                        float backgroundOffsetBot = 90 - (20 * currentHoverSlot.transform.GetChild(0).GetComponent<Item>().item.givenBonuses.Count);
                        float backgroundOffsetTop = 90 - (10 * currentHoverSlot.transform.GetChild(0).GetComponent<Item>().item.givenBonuses.Count);

                        float backgroundHeight = currentHoverSlot.transform.GetChild(0).GetComponent<Item>().item.givenBonuses.Count * 27.9f;
                        RectTransform background = itemStatsObject.transform.Find("Background").gameObject.GetComponent<RectTransform>();
                        background.offsetMin = new Vector2(background.offsetMin.x, backgroundOffsetBot);
                        background.offsetMax = new Vector2(background.offsetMax.x, backgroundOffsetTop);
                        background.sizeDelta = new Vector2(background.sizeDelta.x, backgroundHeight);

                        // Change Stats Window Pos depending on screen edge
                        mousePos = Input.mousePosition / canvas.GetComponent<RectTransform>().localScale.x;
                        if (mousePos.x + 235 >= canvas.GetComponent<RectTransform>().rect.width)
                        {
                            if (mousePos.y + 258 > canvas.GetComponent<RectTransform>().rect.height)
                            {
                                itemStatsObject.transform.position = new Vector3(mousePos.x + 320.2f, mousePos.y - (-50 + 20 * currentHoverSlot.transform.GetChild(0).GetComponent<Item>().item.givenBonuses.Count), mousePos.z);
                            }
                            else
                            {
                                itemStatsObject.transform.position = new Vector3(mousePos.x + 320.2f, mousePos.y - 50 + 20 * currentHoverSlot.transform.GetChild(0).GetComponent<Item>().item.givenBonuses.Count, mousePos.z);
                            }
                        }
                        else
                        {
                            if (mousePos.y + 258 > canvas.GetComponent<RectTransform>().rect.height)
                            {
                                itemStatsObject.transform.position = new Vector3(mousePos.x + 558, mousePos.y - (-50 + 20 * currentHoverSlot.transform.GetChild(0).GetComponent<Item>().item.givenBonuses.Count), mousePos.z);
                            }
                            else
                            {
                                itemStatsObject.transform.position = new Vector3(mousePos.x + 558, mousePos.y - 50 + 20 * currentHoverSlot.transform.GetChild(0).GetComponent<Item>().item.givenBonuses.Count, mousePos.z);
                            }
                        }
                    }
                    else if (currentHoverSlot.GetComponent<BuySlot>() != null || currentHoverSlot.GetComponent<SellSlot>() != null)
                    {
                        // Change Stats Window Height depending on bonuses amount
                        float backgroundOffsetBot = 90 - (20 * currentHoverSlot.transform.GetChild(2).GetComponent<Item>().item.givenBonuses.Count);
                        float backgroundOffsetTop = 90 - (10 * currentHoverSlot.transform.GetChild(2).GetComponent<Item>().item.givenBonuses.Count);

                        float backgroundHeight = currentHoverSlot.transform.GetChild(2).GetComponent<Item>().item.givenBonuses.Count * 27.9f;
                        RectTransform background = itemStatsObject.transform.Find("Background").gameObject.GetComponent<RectTransform>();
                        background.offsetMin = new Vector2(background.offsetMin.x, backgroundOffsetBot);
                        background.offsetMax = new Vector2(background.offsetMax.x, backgroundOffsetTop);
                        background.sizeDelta = new Vector2(background.sizeDelta.x, backgroundHeight);

                        // Change Stats Window Pos depending on screen edge
                        mousePos = Input.mousePosition / canvas.GetComponent<RectTransform>().localScale.x;
                        if (mousePos.x + 235 >= canvas.GetComponent<RectTransform>().rect.width)
                        {
                            if (mousePos.y + 258 > canvas.GetComponent<RectTransform>().rect.height)
                            {
                                itemStatsObject.transform.position = new Vector3(mousePos.x + 320.2f, mousePos.y - (-50 + 20 * currentHoverSlot.transform.GetChild(2).GetComponent<Item>().item.givenBonuses.Count), mousePos.z);
                            }
                            else
                            {
                                itemStatsObject.transform.position = new Vector3(mousePos.x + 320.2f, mousePos.y - 50 + 20 * currentHoverSlot.transform.GetChild(2).GetComponent<Item>().item.givenBonuses.Count, mousePos.z);
                            }
                        }
                        else
                        {
                            if (mousePos.y + 258 > canvas.GetComponent<RectTransform>().rect.height)
                            {
                                itemStatsObject.transform.position = new Vector3(mousePos.x + 558, mousePos.y - (-50 + 20 * currentHoverSlot.transform.GetChild(2).GetComponent<Item>().item.givenBonuses.Count), mousePos.z);
                            }
                            else
                            {
                                itemStatsObject.transform.position = new Vector3(mousePos.x + 558, mousePos.y - 50 + 20 * currentHoverSlot.transform.GetChild(2).GetComponent<Item>().item.givenBonuses.Count, mousePos.z);
                            }
                        }
                    }
                }
            }
        }
    }

    void CreateStatsWindow()
    {
        ResetBonusTexts();
        isStatsShown = true;
        // Item Lvl Text
        int lvl = 0;
        if (currentHoverSlot.transform.parent.name.Contains("SellAvailable"))
        {
            itemStatsObject.transform.GetChild(2).GetComponent<Text>().text = "Lvl." + currentHoverSlot.transform.GetComponent<SellSlot>().lvl.ToString();
            lvl = currentHoverSlot.transform.GetComponent<SellSlot>().lvl;
        }
        else if (!currentHoverSlot.transform.parent.name.Contains("BuyAvailable"))
        {
            itemStatsObject.transform.GetChild(2).GetComponent<Text>().text = "Lvl." + currentHoverSlot.GetComponent<InventorySlot>().lvl.ToString();
            lvl = currentHoverSlot.GetComponent<InventorySlot>().lvl;
        }
        else
        {
            itemStatsObject.transform.GetChild(2).GetComponent<Text>().text = "Lvl.1";
            lvl = 1;
        }
        // Item Type Text
        if (currentHoverSlot.GetComponent<InventorySlot>() != null)
        {
            itemStatsObject.transform.GetChild(3).GetComponent<Text>().text = "Type: " + currentHoverSlot.transform.GetChild(0).GetComponent<Item>().item.type.ToString();
        }
        else if(currentHoverSlot.GetComponent<BuySlot>() != null || currentHoverSlot.GetComponent<SellSlot>() != null)
        {
            itemStatsObject.transform.GetChild(3).GetComponent<Text>().text = "Type: " + currentHoverSlot.transform.GetChild(2).GetComponent<Item>().item.type.ToString();
        }
        itemStatsObject.transform.GetChild(3).gameObject.SetActive(true);


        // Create Bonuses Texts
        int i = 2;

        float lvlAffection = (lvl - 1) / 100.0f;
        if (currentHoverSlot.GetComponent<InventorySlot>() != null)
        {
            // Item First Init
            if (currentHoverSlot.GetComponent<InventorySlot>().item.givenBonuses.Count == 0)
            {
                currentHoverSlot.GetComponent<InventorySlot>().item.InitItem();
            }
            foreach (KeyValuePair<string, float> bonus in currentHoverSlot.transform.GetChild(0).GetComponent<Item>().item.givenBonuses)
            {
                i++;
                if(bonus.Value >= 0)
                {
                    itemStatsObject.transform.GetChild(i + 1).GetComponent<Text>().text = bonus.Key + ": " + NumberConversion.AbbreviateNumber(bonus.Value + (bonus.Value * lvlAffection));
                }
                else
                {
                    itemStatsObject.transform.GetChild(i + 1).GetComponent<Text>().text = bonus.Key + ": " + NumberConversion.AbbreviateNumber(bonus.Value - (bonus.Value * lvlAffection));
                }
                
                itemStatsObject.transform.GetChild(i + 1).gameObject.SetActive(true);

            }
        }
        else if(currentHoverSlot.GetComponent<BuySlot>() != null || currentHoverSlot.GetComponent<SellSlot>() != null)
        {
            if (currentHoverSlot.GetComponent<BuySlot>() != null)
            {
                // Item First Init
                if (currentHoverSlot.GetComponent<BuySlot>().item.givenBonuses.Count == 0)
                {
                    currentHoverSlot.GetComponent<BuySlot>().item.InitItem();
                }
            }
            else
            {
                // Item First Init
                if (currentHoverSlot.GetComponent<SellSlot>().item.givenBonuses.Count == 0)
                {
                    currentHoverSlot.GetComponent<SellSlot>().item.InitItem();
                }
            }
            foreach (KeyValuePair<string, float> bonus in currentHoverSlot.transform.GetChild(2).GetComponent<Item>().item.givenBonuses)
            {
                i++;
                if (bonus.Value >= 0)
                {
                    itemStatsObject.transform.GetChild(i + 1).GetComponent<Text>().text = bonus.Key + ": " + NumberConversion.AbbreviateNumber(bonus.Value + (bonus.Value * lvlAffection));
                }
                else
                {
                    itemStatsObject.transform.GetChild(i + 1).GetComponent<Text>().text = bonus.Key + ": " + NumberConversion.AbbreviateNumber(bonus.Value - (bonus.Value * lvlAffection));
                }
                
                itemStatsObject.transform.GetChild(i + 1).gameObject.SetActive(true);

            }
        }

        if (currentHoverSlot.GetComponent<InventorySlot>() != null)
        {
            // Change Stats Window Height depending on bonuses amount
            float backgroundOffsetBot = 90 - (20 * currentHoverSlot.transform.GetChild(0).GetComponent<Item>().item.givenBonuses.Count);
            float backgroundOffsetTop = 90 - (10 * currentHoverSlot.transform.GetChild(0).GetComponent<Item>().item.givenBonuses.Count);

            float backgroundHeight = currentHoverSlot.transform.GetChild(0).GetComponent<Item>().item.givenBonuses.Count * 27.9f;
            RectTransform background = itemStatsObject.transform.Find("Background").gameObject.GetComponent<RectTransform>();
            background.offsetMin = new Vector2(background.offsetMin.x, backgroundOffsetBot);
            background.offsetMax = new Vector2(background.offsetMax.x, backgroundOffsetTop);
            background.sizeDelta = new Vector2(background.sizeDelta.x, backgroundHeight);

            // Change Stats Window Pos depending on screen edge
            mousePos = Input.mousePosition / canvas.GetComponent<RectTransform>().localScale.x;
            if (mousePos.x + 235 >= canvas.GetComponent<RectTransform>().rect.width)
            {
                if (mousePos.y + 258 > canvas.GetComponent<RectTransform>().rect.height)
                {
                    itemStatsObject.transform.position = new Vector3(mousePos.x + 320.2f, mousePos.y - (-50 + 20 * currentHoverSlot.transform.GetChild(0).GetComponent<Item>().item.givenBonuses.Count), mousePos.z);
                }
                else
                {
                    itemStatsObject.transform.position = new Vector3(mousePos.x + 320.2f, mousePos.y - 50 + 20 * currentHoverSlot.transform.GetChild(0).GetComponent<Item>().item.givenBonuses.Count, mousePos.z);
                }
            }
            else
            {
                if (mousePos.y + 258 > canvas.GetComponent<RectTransform>().rect.height)
                {
                    itemStatsObject.transform.position = new Vector3(mousePos.x + 558, mousePos.y - (-50 + 20 * currentHoverSlot.transform.GetChild(0).GetComponent<Item>().item.givenBonuses.Count), mousePos.z);
                }
                else
                {
                    itemStatsObject.transform.position = new Vector3(mousePos.x + 558, mousePos.y - 50 + 20 * currentHoverSlot.transform.GetChild(0).GetComponent<Item>().item.givenBonuses.Count, mousePos.z);
                }
            }
        }
        else if(currentHoverSlot.GetComponent<BuySlot>() != null || currentHoverSlot.GetComponent<SellSlot>() != null)
        {
            // Change Stats Window Height depending on bonuses amount
            float backgroundOffsetBot = 90 - (20 * currentHoverSlot.transform.GetChild(2).GetComponent<Item>().item.givenBonuses.Count);
            float backgroundOffsetTop = 90 - (10 * currentHoverSlot.transform.GetChild(2).GetComponent<Item>().item.givenBonuses.Count);

            float backgroundHeight = currentHoverSlot.transform.GetChild(2).GetComponent<Item>().item.givenBonuses.Count * 27.9f;
            RectTransform background = itemStatsObject.transform.Find("Background").gameObject.GetComponent<RectTransform>();
            background.offsetMin = new Vector2(background.offsetMin.x, backgroundOffsetBot);
            background.offsetMax = new Vector2(background.offsetMax.x, backgroundOffsetTop);
            background.sizeDelta = new Vector2(background.sizeDelta.x, backgroundHeight);

            // Change Stats Window Pos depending on screen edge
            mousePos = Input.mousePosition / canvas.GetComponent<RectTransform>().localScale.x;
            if (mousePos.x + 235 >= canvas.GetComponent<RectTransform>().rect.width)
            {
                if (mousePos.y + 258 > canvas.GetComponent<RectTransform>().rect.height)
                {
                    itemStatsObject.transform.position = new Vector3(mousePos.x + 320.2f, mousePos.y - (-50 + 20 * currentHoverSlot.transform.GetChild(2).GetComponent<Item>().item.givenBonuses.Count), mousePos.z);
                }
                else
                {
                    itemStatsObject.transform.position = new Vector3(mousePos.x + 320.2f, mousePos.y - 50 + 20 * currentHoverSlot.transform.GetChild(2).GetComponent<Item>().item.givenBonuses.Count, mousePos.z);
                }
            }
            else
            {
                if (mousePos.y + 258 > canvas.GetComponent<RectTransform>().rect.height)
                {
                    itemStatsObject.transform.position = new Vector3(mousePos.x + 558, mousePos.y - (-50 + 20 * currentHoverSlot.transform.GetChild(2).GetComponent<Item>().item.givenBonuses.Count), mousePos.z);
                }
                else
                {
                    itemStatsObject.transform.position = new Vector3(mousePos.x + 558, mousePos.y - 50 + 20 * currentHoverSlot.transform.GetChild(2).GetComponent<Item>().item.givenBonuses.Count, mousePos.z);
                }
            }
        }
           
    }

    public void IsPointerOverItem()
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);

        foreach(RaycastResult rc in results)
        {
            if (rc.gameObject.name == "Background")
            {
                if (rc.gameObject.transform.parent.name == "Canvas")
                {
                    ExitItem();
                }
            }
            if (rc.gameObject.GetComponent<InventorySlot>() != null || rc.gameObject.GetComponent<SellSlot>() != null || rc.gameObject.GetComponent<BuySlot>() != null)
            {
                if(currentHoverSlot != null && currentHoverSlot != rc.gameObject)
                {
                    currentHoverSlot = rc.gameObject;
                    ExitItem();
                }
                else if(currentHoverSlot != null)
                {
                    if (!isDragging) {
                        currentHoverSlot = rc.gameObject;
                        if(currentHoverSlot.GetComponent<InventorySlot>() != null)
                        {
                            if (currentHoverSlot.GetComponent<InventorySlot>().item != null)
                            {
                                // Change others z pos in order to show stats window correctly
                                Transform upgrades = currentHoverSlot.transform.parent.parent.Find("Upgrades");
                                if (upgrades != null)
                                {
                                    upgrades.SetSiblingIndex(upgrades.GetSiblingIndex() - 1);
                                }
                                if (currentHoverSlot.transform.parent.name.Contains("Inventory"))
                                {
                                    currentHoverSlot.transform.parent.parent.Find("Inventory").SetAsLastSibling();
                                    currentHoverSlot.transform.parent.parent.Find("Shop").SetAsFirstSibling();
                                }
                                else if (currentHoverSlot.transform.parent.name.Contains("Shop"))
                                {
                                    currentHoverSlot.transform.parent.parent.Find("Shop").SetAsLastSibling();
                                    currentHoverSlot.transform.parent.parent.Find("Inventory").SetAsFirstSibling();
                                }

                                // Item First Init
                                if (currentHoverSlot.GetComponent<InventorySlot>().item.givenBonuses.Count == 0)
                                {
                                    currentHoverSlot.GetComponent<InventorySlot>().item.InitItem();
                                }

                                isHoveringItem = true;

                                itemStatsObject.transform.SetParent(currentHoverSlot.transform.parent.parent.parent);
                                itemStatsObject.transform.SetAsLastSibling();
                                if (!currentHoverSlot.transform.parent.name.Contains("Shop"))
                                {
                                    if (!isDragging)
                                        currentHoverSlot.transform.SetAsLastSibling();
                                }
                                itemStatsObject.transform.GetChild(1).GetComponent<Text>().text = currentHoverSlot.GetComponent<InventorySlot>().item.displayName;
                                itemStatsObject.transform.GetChild(1).GetComponent<Text>().color = GameItem.GetRarityColor(currentHoverSlot.GetComponent<InventorySlot>().item);
                            }
                        }
                        else if (currentHoverSlot.GetComponent<BuySlot>() != null)
                        {
                            if (currentHoverSlot.GetComponent<BuySlot>().item != null)
                            {
                                // Change others z pos in order to show stats window correctly
                                Transform upgrades = currentHoverSlot.transform.parent.parent.Find("Upgrades");
                                if (upgrades != null)
                                {
                                    upgrades.SetSiblingIndex(upgrades.GetSiblingIndex() - 1);
                                }
                                if (currentHoverSlot.transform.parent.name.Contains("Inventory"))
                                {
                                    currentHoverSlot.transform.parent.parent.Find("Inventory").SetAsLastSibling();
                                    currentHoverSlot.transform.parent.parent.Find("Shop").SetAsFirstSibling();
                                }
                                else if (currentHoverSlot.transform.parent.name.Contains("Shop"))
                                {
                                    currentHoverSlot.transform.parent.parent.Find("Shop").SetAsLastSibling();
                                    currentHoverSlot.transform.parent.parent.Find("Inventory").SetAsFirstSibling();
                                }
                                // Item First Init
                                if (currentHoverSlot.GetComponent<BuySlot>().item.givenBonuses.Count == 0)
                                {
                                    currentHoverSlot.GetComponent<BuySlot>().item.InitItem();
                                }
                                isHoveringItem = true;

                                itemStatsObject.transform.SetParent(currentHoverSlot.transform.parent.parent.parent);
                                itemStatsObject.transform.SetAsLastSibling();
                                if (!currentHoverSlot.transform.parent.name.Contains("Shop"))
                                {
                                    if (!isDragging)
                                        currentHoverSlot.transform.SetAsLastSibling();
                                }
                                itemStatsObject.transform.GetChild(1).GetComponent<Text>().text = currentHoverSlot.transform.GetChild(2).GetComponent<Item>().item.displayName;
                                itemStatsObject.transform.GetChild(1).GetComponent<Text>().color = GameItem.GetRarityColor(currentHoverSlot.GetComponent<BuySlot>().item);
                            }
                        }
                        else if (currentHoverSlot.GetComponent<SellSlot>() != null)
                        {
                            if (currentHoverSlot.GetComponent<SellSlot>().item != null)
                            {
                                // Change others z pos in order to show stats window correctly
                                Transform upgrades = currentHoverSlot.transform.parent.parent.Find("Upgrades");
                                if (upgrades != null)
                                {
                                    upgrades.SetSiblingIndex(upgrades.GetSiblingIndex() - 1);
                                }
                                if (currentHoverSlot.transform.parent.name.Contains("Inventory"))
                                {
                                    currentHoverSlot.transform.parent.parent.Find("Inventory").SetAsLastSibling();
                                    currentHoverSlot.transform.parent.parent.Find("Shop").SetAsFirstSibling();
                                }
                                else if (currentHoverSlot.transform.parent.name.Contains("Shop"))
                                {
                                    currentHoverSlot.transform.parent.parent.Find("Shop").SetAsLastSibling();
                                    currentHoverSlot.transform.parent.parent.Find("Inventory").SetAsFirstSibling();
                                }
                                // Item First Init
                                if (currentHoverSlot.GetComponent<SellSlot>().item.givenBonuses.Count == 0)
                                {
                                    currentHoverSlot.GetComponent<SellSlot>().item.InitItem();
                                }
                                isHoveringItem = true;

                                itemStatsObject.transform.SetParent(currentHoverSlot.transform.parent.parent.parent);
                                itemStatsObject.transform.SetAsLastSibling();
                                if (!currentHoverSlot.transform.parent.name.Contains("Shop"))
                                {
                                    if (!isDragging)
                                        currentHoverSlot.transform.SetAsLastSibling();
                                }
                                itemStatsObject.transform.GetChild(1).GetComponent<Text>().text = currentHoverSlot.transform.GetChild(2).GetComponent<Item>().item.displayName;
                                itemStatsObject.transform.GetChild(1).GetComponent<Text>().color = GameItem.GetRarityColor(currentHoverSlot.GetComponent<SellSlot>().item);
                            }
                        }
                    }
                    else
                    {
                        currentHoverSlot = rc.gameObject;

                        if (draggingObj.transform.parent.name != currentHoverSlot.name)
                        {
                            if (currentHoverSlot.GetComponent<InventorySlot>() != null)
                            {
                                if (currentHoverSlot.GetComponent<InventorySlot>().item != null)
                                {
                                    // Change others z pos in order to show stats window correctly
                                    Transform upgrades = currentHoverSlot.transform.parent.parent.Find("Upgrades");
                                    if (upgrades != null)
                                    {
                                        upgrades.SetSiblingIndex(upgrades.GetSiblingIndex() - 1);
                                    }
                                    if (currentHoverSlot.transform.parent.name.Contains("Inventory"))
                                    {
                                        currentHoverSlot.transform.parent.parent.Find("Inventory").SetAsLastSibling();
                                        currentHoverSlot.transform.parent.parent.Find("Shop").SetAsFirstSibling();
                                    }
                                    else if (currentHoverSlot.transform.parent.name.Contains("Shop"))
                                    {
                                        currentHoverSlot.transform.parent.parent.Find("Shop").SetAsLastSibling();
                                        currentHoverSlot.transform.parent.parent.Find("Inventory").SetAsFirstSibling();
                                    }

                                    // Item First Init
                                    if (currentHoverSlot.GetComponent<InventorySlot>().item.givenBonuses.Count == 0)
                                    {
                                        currentHoverSlot.GetComponent<InventorySlot>().item.InitItem();
                                    }

                                    isHoveringItem = true;

                                    itemStatsObject.transform.SetParent(currentHoverSlot.transform.parent.parent.parent);
                                    itemStatsObject.transform.SetAsLastSibling();
                                    if (!currentHoverSlot.transform.parent.name.Contains("Shop"))
                                    {
                                        if (!isDragging)
                                            currentHoverSlot.transform.SetAsLastSibling();
                                    }
                                    itemStatsObject.transform.GetChild(1).GetComponent<Text>().text = currentHoverSlot.GetComponent<InventorySlot>().item.displayName;
                                    itemStatsObject.transform.GetChild(1).GetComponent<Text>().color = GameItem.GetRarityColor(currentHoverSlot.GetComponent<InventorySlot>().item);
                                }
                            }
                            else if (currentHoverSlot.GetComponent<BuySlot>() != null)
                            {
                                if (currentHoverSlot.GetComponent<BuySlot>().item != null)
                                {
                                    // Change others z pos in order to show stats window correctly
                                    Transform upgrades = currentHoverSlot.transform.parent.parent.Find("Upgrades");
                                    if (upgrades != null)
                                    {
                                        upgrades.SetSiblingIndex(upgrades.GetSiblingIndex() - 1);
                                    }
                                    if (currentHoverSlot.transform.parent.name.Contains("Inventory"))
                                    {
                                        currentHoverSlot.transform.parent.parent.Find("Inventory").SetAsLastSibling();
                                        currentHoverSlot.transform.parent.parent.Find("Shop").SetAsFirstSibling();
                                    }
                                    else if (currentHoverSlot.transform.parent.name.Contains("Shop"))
                                    {
                                        currentHoverSlot.transform.parent.parent.Find("Shop").SetAsLastSibling();
                                        currentHoverSlot.transform.parent.parent.Find("Inventory").SetAsFirstSibling();
                                    }

                                    isHoveringItem = true;

                                    itemStatsObject.transform.SetParent(currentHoverSlot.transform.parent.parent.parent);
                                    itemStatsObject.transform.SetAsLastSibling();
                                    if (!currentHoverSlot.transform.parent.name.Contains("Shop"))
                                    {
                                        if (!isDragging)
                                            currentHoverSlot.transform.SetAsLastSibling();
                                    }
                                    itemStatsObject.transform.GetChild(1).GetComponent<Text>().text = currentHoverSlot.GetComponent<BuySlot>().item.displayName;
                                    itemStatsObject.transform.GetChild(1).GetComponent<Text>().color = GameItem.GetRarityColor(currentHoverSlot.GetComponent<BuySlot>().item);
                                }
                            }
                            else if (currentHoverSlot.GetComponent<SellSlot>() != null)
                            {
                                if (currentHoverSlot.GetComponent<SellSlot>().item != null)
                                {
                                    // Change others z pos in order to show stats window correctly
                                    Transform upgrades = currentHoverSlot.transform.parent.parent.Find("Upgrades");
                                    if (upgrades != null)
                                    {
                                        upgrades.SetSiblingIndex(upgrades.GetSiblingIndex() - 1);
                                    }
                                    if (currentHoverSlot.transform.parent.name.Contains("Inventory"))
                                    {
                                        currentHoverSlot.transform.parent.parent.Find("Inventory").SetAsLastSibling();
                                        currentHoverSlot.transform.parent.parent.Find("Shop").SetAsFirstSibling();
                                    }
                                    else if (currentHoverSlot.transform.parent.name.Contains("Shop"))
                                    {
                                        currentHoverSlot.transform.parent.parent.Find("Shop").SetAsLastSibling();
                                        currentHoverSlot.transform.parent.parent.Find("Inventory").SetAsFirstSibling();
                                    }

                                    isHoveringItem = true;

                                    itemStatsObject.transform.SetParent(currentHoverSlot.transform.parent.parent.parent);
                                    itemStatsObject.transform.SetAsLastSibling();
                                    if (!currentHoverSlot.transform.parent.name.Contains("Shop"))
                                    {
                                        if (!isDragging)
                                            currentHoverSlot.transform.SetAsLastSibling();
                                    }
                                    itemStatsObject.transform.GetChild(1).GetComponent<Text>().text = currentHoverSlot.GetComponent<SellSlot>().item.displayName;
                                    itemStatsObject.transform.GetChild(1).GetComponent<Text>().color = GameItem.GetRarityColor(currentHoverSlot.GetComponent<SellSlot>().item);
                                }
                            }
                        }
                    }
                }
                else
                {
                    currentHoverSlot = rc.gameObject;
                }
                break;
            }
        }
    }

    public void ResetBonusTexts()
    {
        if (currentHoverSlot.GetComponent<InventorySlot>() != null)
        {
            if (currentHoverSlot.GetComponent<InventorySlot>().item != null)
            {
                int i = 2;

                for (int j = 0; j < 9; j++)
                {
                    i++;
                    if (itemStatsObject != null)
                    {

                        itemStatsObject.transform.GetChild(i + 1).GetComponent<Text>().text = "";
                        itemStatsObject.transform.GetChild(i + 1).gameObject.SetActive(false);
                    }
                    else
                    {
                        break;
                    }
                }
            }
        }
        else if (currentHoverSlot.GetComponent<BuySlot>() != null)
        {
            if (currentHoverSlot.GetComponent<BuySlot>().item != null)
            {
                int i = 2;
                for (int j = 0; j < 9; j++)
                {
                    i++;
                    if (itemStatsObject != null)
                    {

                        itemStatsObject.transform.GetChild(i + 1).GetComponent<Text>().text = "";
                        itemStatsObject.transform.GetChild(i + 1).gameObject.SetActive(false);
                    }
                    else
                    {
                        break;
                    }
                }
            }
        }
        else if (currentHoverSlot.GetComponent<SellSlot>() != null)
        {
            if (currentHoverSlot.GetComponent<SellSlot>().item != null)
            {
                int i = 2;
                for (int j = 0; j < 9; j++)
                {
                    i++;
                    if (itemStatsObject != null)
                    {

                        itemStatsObject.transform.GetChild(i + 1).GetComponent<Text>().text = "";
                        itemStatsObject.transform.GetChild(i + 1).gameObject.SetActive(false);
                    }
                    else
                    {
                        break;
                    }
                }
            }
        }
    }

    public void ExitItem() 
    {
        isStatsShown = false;
        isHoveringItem = false;
        // Change others z pos in order to show stats window correctlyi
        if (currentHoverSlot != null)
        {
            Transform upgrades = currentHoverSlot.transform.parent.parent.Find("Upgrades");
            if (upgrades != null)
            {
                upgrades.SetSiblingIndex(upgrades.GetSiblingIndex() + 1);
            }
            if (currentHoverSlot.transform.parent.name.Contains("Inventory"))
            {
                currentHoverSlot.transform.parent.parent.Find("Inventory").SetAsFirstSibling();
                currentHoverSlot.transform.parent.parent.Find("Shop").SetAsLastSibling();
            }
            else if (currentHoverSlot.transform.parent.name.Contains("Shop"))
            {
                currentHoverSlot.transform.parent.parent.Find("Shop").SetAsFirstSibling();
                currentHoverSlot.transform.parent.parent.Find("Inventory").SetAsLastSibling();
            }

            ResetBonusTexts();


            // Return init slot pos
            if (currentHoverSlot.name.Contains("Inv"))
            {
                string parName = currentHoverSlot.name.Replace("Inv", "");
                currentHoverSlot.transform.SetSiblingIndex(int.Parse(parName) - 1);
            }
        }
        

        // Hide Ites Stats Window
        if (itemStatsObject != null)
            itemStatsObject.transform.localPosition = new Vector3(-10000, 0, 0);
        isStatsShown = false;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (canDrag)
        {
            isDragging = true;
            ExitItem();
            canvasGroup.alpha = .6f;
            canvasGroup.blocksRaycasts = false;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (canDrag)
        {
            isDragging = true;
            rectTrans.anchoredPosition += eventData.delta / canvas.scaleFactor;
            this.GetComponent<Image>().sprite = this.GetComponent<Item>().item.dragIcon;
            draggingItem = this.GetComponent<Item>().item;
            draggingObj = this.gameObject;
            if ((this.transform.parent.name.Contains("Inv") || this.transform.parent.name.Contains("Slot")) && (!this.transform.parent.name.Contains("Inventory") && !this.transform.parent.name.Contains("Slots")))
            {
                if (!isHoveringItem)
                {
                    this.transform.parent.SetAsLastSibling();
                }
                    
            }
            if (!this.transform.parent.parent.name.Contains("SellAvailable"))
            {
                this.transform.parent.parent.SetAsLastSibling();
            }
            else
            {
                this.transform.parent.parent.parent.SetAsLastSibling();
            }
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (canDrag)
        {
            draggingItem = null;
            draggingObj = null;
            isDragging = false;
            canvasGroup = GetComponent<CanvasGroup>();
            canvasGroup.alpha = 1f;
            canvasGroup.blocksRaycasts = true;
            this.GetComponent<Image>().sprite = this.GetComponent<Item>().item.icon;
            ExitItem();
            string name = this.transform.parent.name;
            if (name.Contains("Inv"))
            {
                name = name.Replace("Inv", "");
                this.transform.parent.SetSiblingIndex(int.Parse(name) - 1);
            }
            GameObject go = eventData.pointerCurrentRaycast.gameObject;

            bool foundParent = false;

            if(go.name != "Background")
            {
                if (!go.transform.parent.parent.name.Contains("BuyAvailable"))
                {
                    if (this.transform.parent != go.transform && go.name.Contains("Inv") && !go.name.Contains("Inventory") && go.transform.childCount == 0)
                    {
                        if (this.transform.parent.name.Contains("Slot") && !this.transform.parent.parent.name.Contains("SellAvailable"))
                        {
                            Transform hovItemParent = go.transform;
                            Transform thisParent = this.transform.parent;
                            int lvlParsHov = hovItemParent.GetComponent<InventorySlot>().lvl;
                            int lvlParsThis = thisParent.GetComponent<InventorySlot>().lvl;

                            hovItemParent.GetComponent<InventorySlot>().lvl = lvlParsThis;
                            thisParent.GetComponent<InventorySlot>().lvl = lvlParsHov;
                            GameObject.Find("Battle").GetComponent<Battle>().player.UnequipItem(this.GetComponent<Item>().item, lvlParsThis);
                            this.transform.parent.parent.SetSiblingIndex(0);
                            if (this.GetComponent<Item>().item.type == GameItem.Type.Weapon)
                                inventory.playerSlots[0] = null;
                            if (this.GetComponent<Item>().item.type == GameItem.Type.Shield)
                                inventory.playerSlots[1] = null;
                            if (this.GetComponent<Item>().item.type == GameItem.Type.Head)
                                inventory.playerSlots[2] = null;
                            if (this.GetComponent<Item>().item.type == GameItem.Type.Chest)
                                inventory.playerSlots[3] = null;
                            if (this.GetComponent<Item>().item.type == GameItem.Type.Pants)
                                inventory.playerSlots[4] = null;
                            if (this.GetComponent<Item>().item.type == GameItem.Type.Boots)
                                inventory.playerSlots[5] = null;
                            if (this.GetComponent<Item>().item.type == GameItem.Type.Accessory)
                                inventory.playerSlots[6] = null;
                        }

                        int k = 0;
                        string nameSlot = this.transform.parent.name;
                        if (nameSlot.Contains("Inv"))
                        {
                            nameSlot = nameSlot.Replace("Inv", "");
                            k = int.Parse(nameSlot);
                            inventory.slots[k - 1] = null;

                            Transform hovItemParent = go.transform;
                            Transform thisParent = this.transform.parent;
                            int lvlParsHov = hovItemParent.GetComponent<InventorySlot>().lvl;
                            int lvlParsThis = thisParent.GetComponent<InventorySlot>().lvl;

                            hovItemParent.GetComponent<InventorySlot>().lvl = lvlParsThis;
                            thisParent.GetComponent<InventorySlot>().lvl = lvlParsHov;

                        }

                        if (this.transform.parent.parent.name.Contains("SellAvailable"))
                        {
                            nameSlot = nameSlot.Replace("Slot", "");
                            k = int.Parse(nameSlot);
                            inventory.sellSlots[k - 1] = null;
                            Transform hovItemParent = go.transform;
                            Transform thisParent = this.transform.parent;
                            int lvlParsHov = hovItemParent.GetComponent<InventorySlot>().lvl;
                            int lvlParsThis = thisParent.GetComponent<SellSlot>().lvl;

                            hovItemParent.GetComponent<InventorySlot>().lvl = lvlParsThis;
                            thisParent.GetComponent<SellSlot>().lvl = lvlParsHov;
                            go.GetComponent<InventorySlot>().item = this.transform.parent.GetComponent<SellSlot>().item;
                            this.transform.parent.GetComponent<SellSlot>().item = null;
                        }
                        else
                        {
                            go.GetComponent<InventorySlot>().item = this.transform.parent.GetComponent<InventorySlot>().item;
                            this.transform.parent.GetComponent<InventorySlot>().item = null;
                        }

                        this.transform.SetParent(go.transform);
                        this.transform.localPosition = Vector3.zero;

                        foundParent = true;
                    }
                    else if (go.name.Contains("Slot") && !go.name.Contains("Slots") && go.transform.childCount == 0)
                    {
                        if (this.transform.parent.name.Contains("Inv") && !this.transform.parent.name.Contains("Inventory"))
                        {
                            string nameSlot = this.transform.parent.name;
                            nameSlot = nameSlot.Replace("Inv", "");
                            int k = 0;
                            k = int.Parse(nameSlot);
                            inventory.slots[k - 1] = null;
                        }

                        if (this.GetComponent<Item>().item.type == GameItem.Type.Weapon)
                        {
                            if (go.name.Contains("SlotWeapon") && go.transform.childCount == 0)
                            {
                                int lvlParsHov = 1;
                                int lvlParsThis = 1;
                                if (this.transform.parent.parent.name.Contains("SellAvailable"))
                                {
                                    Transform hovItemParent = go.transform;
                                    Transform thisParent = this.transform.parent;
                                    lvlParsHov = hovItemParent.GetComponent<InventorySlot>().lvl;
                                    lvlParsThis = thisParent.GetComponent<SellSlot>().lvl;
                                    hovItemParent.GetComponent<InventorySlot>().lvl = lvlParsThis;
                                    thisParent.GetComponent<SellSlot>().lvl = lvlParsHov;
                                    go.GetComponent<InventorySlot>().item = this.transform.parent.GetComponent<SellSlot>().item;
                                    this.transform.parent.GetComponent<SellSlot>().item = null;
                                }
                                else
                                {
                                    Transform hovItemParent = go.transform;
                                    Transform thisParent = this.transform.parent;
                                    lvlParsHov = hovItemParent.GetComponent<InventorySlot>().lvl;
                                    lvlParsThis = thisParent.GetComponent<InventorySlot>().lvl;
                                    hovItemParent.GetComponent<InventorySlot>().lvl = lvlParsThis;
                                    thisParent.GetComponent<InventorySlot>().lvl = lvlParsHov;

                                    go.GetComponent<InventorySlot>().item = this.transform.parent.GetComponent<InventorySlot>().item;
                                    this.transform.parent.GetComponent<InventorySlot>().item = null;
                                }
                                inventory.playerSlots[0] = this.GetComponent<Item>().item;
                                this.transform.SetParent(go.transform);
                                GameObject.Find("Battle").GetComponent<Battle>().player.EquipItem(this.GetComponent<Item>().item, lvlParsThis);
                                this.transform.parent.transform.parent.SetAsFirstSibling();
                                this.transform.localPosition = Vector3.zero;
                                foundParent = true;
                            }
                        }
                        if (this.GetComponent<Item>().item.type == GameItem.Type.Shield)
                        {
                            if (go.name.Contains("SlotShield") && go.transform.childCount == 0)
                            {
                                int lvlParsHov = 1;
                                int lvlParsThis = 1;
                                if (this.transform.parent.parent.name.Contains("SellAvailable"))
                                {
                                    Transform hovItemParent = go.transform;
                                    Transform thisParent = this.transform.parent;
                                    lvlParsHov = hovItemParent.GetComponent<InventorySlot>().lvl;
                                    lvlParsThis = thisParent.GetComponent<SellSlot>().lvl;
                                    hovItemParent.GetComponent<InventorySlot>().lvl = lvlParsThis;
                                    thisParent.GetComponent<SellSlot>().lvl = lvlParsHov;
                                    go.GetComponent<InventorySlot>().item = this.transform.parent.GetComponent<SellSlot>().item;
                                    this.transform.parent.GetComponent<SellSlot>().item = null;
                                }
                                else
                                {
                                    Transform hovItemParent = go.transform;
                                    Transform thisParent = this.transform.parent;
                                    lvlParsHov = hovItemParent.GetComponent<InventorySlot>().lvl;
                                    lvlParsThis = thisParent.GetComponent<InventorySlot>().lvl;
                                    hovItemParent.GetComponent<InventorySlot>().lvl = lvlParsThis;
                                    thisParent.GetComponent<InventorySlot>().lvl = lvlParsHov;
                                    go.GetComponent<InventorySlot>().item = this.transform.parent.GetComponent<InventorySlot>().item;
                                    this.transform.parent.GetComponent<InventorySlot>().item = null;
                                }
                                inventory.playerSlots[1] = this.GetComponent<Item>().item;
                                this.transform.SetParent(go.transform);
                                GameObject.Find("Battle").GetComponent<Battle>().player.EquipItem(this.GetComponent<Item>().item, lvlParsThis);
                                this.transform.parent.transform.parent.SetAsFirstSibling();
                                this.transform.localPosition = Vector3.zero;
                                foundParent = true;
                            }
                        }
                        if (this.GetComponent<Item>().item.type == GameItem.Type.Head)
                        {
                            if (go.name.Contains("SlotHead") && go.transform.childCount == 0)
                            {
                                int lvlParsHov = 1;
                                int lvlParsThis = 1;
                                if (this.transform.parent.parent.name.Contains("SellAvailable"))
                                {
                                    Transform hovItemParent = go.transform;
                                    Transform thisParent = this.transform.parent;
                                    lvlParsHov = hovItemParent.GetComponent<InventorySlot>().lvl;
                                    lvlParsThis = thisParent.GetComponent<SellSlot>().lvl;
                                    hovItemParent.GetComponent<InventorySlot>().lvl = lvlParsThis;
                                    thisParent.GetComponent<SellSlot>().lvl = lvlParsHov;
                                    go.GetComponent<InventorySlot>().item = this.transform.parent.GetComponent<SellSlot>().item;
                                    this.transform.parent.GetComponent<SellSlot>().item = null;
                                }
                                else
                                {
                                    Transform hovItemParent = go.transform;
                                    Transform thisParent = this.transform.parent;
                                    lvlParsHov = hovItemParent.GetComponent<InventorySlot>().lvl;
                                    lvlParsThis = thisParent.GetComponent<InventorySlot>().lvl;
                                    hovItemParent.GetComponent<InventorySlot>().lvl = lvlParsThis;
                                    thisParent.GetComponent<InventorySlot>().lvl = lvlParsHov;
                                    go.GetComponent<InventorySlot>().item = this.transform.parent.GetComponent<InventorySlot>().item;
                                    this.transform.parent.GetComponent<InventorySlot>().item = null;
                                }
                                inventory.playerSlots[2] = this.GetComponent<Item>().item;
                                this.transform.SetParent(go.transform);
                                GameObject.Find("Battle").GetComponent<Battle>().player.EquipItem(this.GetComponent<Item>().item, lvlParsThis);
                                this.transform.parent.transform.parent.SetAsFirstSibling();
                                this.transform.localPosition = Vector3.zero;
                                foundParent = true;
                            }
                        }
                        if (this.GetComponent<Item>().item.type == GameItem.Type.Chest)
                        {
                            if (go.name.Contains("SlotChest") && go.transform.childCount == 0)
                            {
                                int lvlParsHov = 1;
                                int lvlParsThis = 1;
                                if (this.transform.parent.parent.name.Contains("SellAvailable"))
                                {
                                    Transform hovItemParent = go.transform;
                                    Transform thisParent = this.transform.parent;
                                    lvlParsHov = hovItemParent.GetComponent<InventorySlot>().lvl;
                                    lvlParsThis = thisParent.GetComponent<SellSlot>().lvl;
                                    hovItemParent.GetComponent<InventorySlot>().lvl = lvlParsThis;
                                    thisParent.GetComponent<SellSlot>().lvl = lvlParsHov;
                                    go.GetComponent<InventorySlot>().item = this.transform.parent.GetComponent<SellSlot>().item;
                                    this.transform.parent.GetComponent<SellSlot>().item = null;
                                }
                                else
                                {
                                    Transform hovItemParent = go.transform;
                                    Transform thisParent = this.transform.parent;
                                    lvlParsHov = hovItemParent.GetComponent<InventorySlot>().lvl;
                                    lvlParsThis = thisParent.GetComponent<InventorySlot>().lvl;
                                    hovItemParent.GetComponent<InventorySlot>().lvl = lvlParsThis;
                                    thisParent.GetComponent<InventorySlot>().lvl = lvlParsHov;
                                    go.GetComponent<InventorySlot>().item = this.transform.parent.GetComponent<InventorySlot>().item;
                                    this.transform.parent.GetComponent<InventorySlot>().item = null;
                                }
                                inventory.playerSlots[3] = this.GetComponent<Item>().item;
                                this.transform.SetParent(go.transform);
                                GameObject.Find("Battle").GetComponent<Battle>().player.EquipItem(this.GetComponent<Item>().item, lvlParsThis);
                                this.transform.parent.transform.parent.SetAsFirstSibling();
                                this.transform.localPosition = Vector3.zero;
                                foundParent = true;
                            }
                        }
                        if (this.GetComponent<Item>().item.type == GameItem.Type.Pants)
                        {
                            if (go.name.Contains("SlotPants") && go.transform.childCount == 0)
                            {
                                int lvlParsHov = 1;
                                int lvlParsThis = 1;
                                if (this.transform.parent.parent.name.Contains("SellAvailable"))
                                {
                                    Transform hovItemParent = go.transform;
                                    Transform thisParent = this.transform.parent;
                                    lvlParsHov = hovItemParent.GetComponent<InventorySlot>().lvl;
                                    lvlParsThis = thisParent.GetComponent<SellSlot>().lvl;
                                    hovItemParent.GetComponent<InventorySlot>().lvl = lvlParsThis;
                                    thisParent.GetComponent<SellSlot>().lvl = lvlParsHov;
                                    go.GetComponent<InventorySlot>().item = this.transform.parent.GetComponent<SellSlot>().item;
                                    this.transform.parent.GetComponent<SellSlot>().item = null;
                                }
                                else
                                {
                                    Transform hovItemParent = go.transform;
                                    Transform thisParent = this.transform.parent;
                                    lvlParsHov = hovItemParent.GetComponent<InventorySlot>().lvl;
                                    lvlParsThis = thisParent.GetComponent<InventorySlot>().lvl;
                                    hovItemParent.GetComponent<InventorySlot>().lvl = lvlParsThis;
                                    thisParent.GetComponent<InventorySlot>().lvl = lvlParsHov;
                                    go.GetComponent<InventorySlot>().item = this.transform.parent.GetComponent<InventorySlot>().item;
                                    this.transform.parent.GetComponent<InventorySlot>().item = null;
                                }
                                inventory.playerSlots[4] = this.GetComponent<Item>().item;
                                this.transform.SetParent(go.transform);
                                GameObject.Find("Battle").GetComponent<Battle>().player.EquipItem(this.GetComponent<Item>().item, lvlParsThis);
                                this.transform.parent.transform.parent.SetAsFirstSibling();
                                this.transform.localPosition = Vector3.zero;
                                foundParent = true;
                            }
                        }
                        if (this.GetComponent<Item>().item.type == GameItem.Type.Boots)
                        {
                            if (go.name.Contains("SlotBoots") && go.transform.childCount == 0)
                            {
                                int lvlParsHov = 1;
                                int lvlParsThis = 1;
                                if (this.transform.parent.parent.name.Contains("SellAvailable"))
                                {
                                    Transform hovItemParent = go.transform;
                                    Transform thisParent = this.transform.parent;
                                    lvlParsHov = hovItemParent.GetComponent<InventorySlot>().lvl;
                                    lvlParsThis = thisParent.GetComponent<SellSlot>().lvl;
                                    hovItemParent.GetComponent<InventorySlot>().lvl = lvlParsThis;
                                    thisParent.GetComponent<SellSlot>().lvl = lvlParsHov;
                                    go.GetComponent<InventorySlot>().item = this.transform.parent.GetComponent<SellSlot>().item;
                                    this.transform.parent.GetComponent<SellSlot>().item = null;
                                }
                                else
                                {
                                    Transform hovItemParent = go.transform;
                                    Transform thisParent = this.transform.parent;
                                    lvlParsHov = hovItemParent.GetComponent<InventorySlot>().lvl;
                                    lvlParsThis = thisParent.GetComponent<InventorySlot>().lvl;
                                    hovItemParent.GetComponent<InventorySlot>().lvl = lvlParsThis;
                                    thisParent.GetComponent<InventorySlot>().lvl = lvlParsHov;
                                    go.GetComponent<InventorySlot>().item = this.transform.parent.GetComponent<InventorySlot>().item;
                                    this.transform.parent.GetComponent<InventorySlot>().item = null;
                                }
                                inventory.playerSlots[5] = this.GetComponent<Item>().item;
                                this.transform.SetParent(go.transform);
                                GameObject.Find("Battle").GetComponent<Battle>().player.EquipItem(this.GetComponent<Item>().item, lvlParsThis);
                                this.transform.parent.transform.parent.SetAsFirstSibling();
                                this.transform.localPosition = Vector3.zero;
                                foundParent = true;
                            }
                        }
                        if (this.GetComponent<Item>().item.type == GameItem.Type.Accessory)
                        {
                            if (go.name.Contains("SlotAcc") && go.transform.childCount == 0)
                            {
                                int lvlParsHov = 1;
                                int lvlParsThis = 1;
                                if (this.transform.parent.parent.name.Contains("SellAvailable"))
                                {
                                    Transform hovItemParent = go.transform;
                                    Transform thisParent = this.transform.parent;
                                    lvlParsHov = hovItemParent.GetComponent<InventorySlot>().lvl;
                                    lvlParsThis = thisParent.GetComponent<SellSlot>().lvl;
                                    hovItemParent.GetComponent<InventorySlot>().lvl = lvlParsThis;
                                    thisParent.GetComponent<SellSlot>().lvl = lvlParsHov;
                                    go.GetComponent<InventorySlot>().item = this.transform.parent.GetComponent<SellSlot>().item;
                                    this.transform.parent.GetComponent<SellSlot>().item = null;
                                }
                                else
                                {
                                    Transform hovItemParent = go.transform;
                                    Transform thisParent = this.transform.parent;
                                    lvlParsHov = hovItemParent.GetComponent<InventorySlot>().lvl;
                                    lvlParsThis = thisParent.GetComponent<InventorySlot>().lvl;
                                    hovItemParent.GetComponent<InventorySlot>().lvl = lvlParsThis;
                                    thisParent.GetComponent<InventorySlot>().lvl = lvlParsHov;
                                    go.GetComponent<InventorySlot>().item = this.transform.parent.GetComponent<InventorySlot>().item;
                                    this.transform.parent.GetComponent<InventorySlot>().item = null;
                                }
                                inventory.playerSlots[6] = this.GetComponent<Item>().item;
                                this.transform.SetParent(go.transform);
                                GameObject.Find("Battle").GetComponent<Battle>().player.EquipItem(this.GetComponent<Item>().item, lvlParsThis);
                                this.transform.parent.transform.parent.SetAsFirstSibling();
                                this.transform.localPosition = Vector3.zero;
                                foundParent = true;
                            }
                        }

                        if (this.transform.parent.parent.name.Contains("SellAvailable"))
                        {
                            string nameSlot = this.transform.parent.name;
                            nameSlot = nameSlot.Replace("Slot", "");
                            int k = int.Parse(nameSlot);
                            inventory.sellSlots[k - 1] = null;
                            //go.GetComponent<InventorySlot>().item = this.transform.parent.GetComponent<SellSlot>().item;
                            //this.transform.parent.GetComponent<SellSlot>().item = null;
                        }
                    }
                    else if (go.transform.parent.childCount > 0)
                    {
                        if (go.transform.parent.name.Contains("Slot") && !go.transform.parent.name.Contains("Slots"))
                        {
                            if (this.transform.parent.parent.name.Contains("Slots"))
                            {
                                if (this.GetComponent<Item>().item.type == go.GetComponent<Item>().item.type)
                                {
                                    Transform hovItem = go.transform;
                                    Transform hovItemParent = go.transform.parent;
                                    Transform thisParent = this.transform.parent;
                                    Transform thisItem = this.transform;

                                    GameItem replaceItem;
                                    GameItem prevItem;

                                    Battle battle = GameObject.Find("Battle").GetComponent<Battle>();

                                    int lvlParsHov = 1;
                                    int lvlParsThis = 1;
                                    bool canDo = false;

                                    if (this.transform.parent.parent.name.Contains("SellAvailable"))
                                    {
                                        replaceItem = hovItemParent.GetComponent<InventorySlot>().item;
                                        prevItem = thisParent.GetComponent<SellSlot>().item;
                                        if (replaceItem.type == prevItem.type)
                                        {
                                            canDo = true;
                                            lvlParsHov = hovItemParent.GetComponent<InventorySlot>().lvl;
                                            lvlParsThis = thisParent.GetComponent<SellSlot>().lvl;
                                            hovItemParent.GetComponent<InventorySlot>().item = prevItem;
                                            thisParent.GetComponent<SellSlot>().item = replaceItem;
                                            battle.player.UnequipItem(go.GetComponent<Item>().item, lvlParsHov);
                                            battle.player.EquipItem(this.GetComponent<Item>().item, lvlParsThis);
                                            hovItemParent.GetComponent<InventorySlot>().lvl = lvlParsThis;
                                            thisParent.GetComponent<SellSlot>().lvl = lvlParsHov;
                                        }   
                                    }
                                    else if (go.transform.parent.parent.name.Contains("SellAvailable"))
                                    {
                                        replaceItem = hovItemParent.GetComponent<SellSlot>().item;
                                        prevItem = thisParent.GetComponent<InventorySlot>().item;
                                        if (replaceItem.type == prevItem.type)
                                        {
                                            canDo = true;
                                            lvlParsHov = hovItemParent.GetComponent<SellSlot>().lvl;
                                            lvlParsThis = thisParent.GetComponent<InventorySlot>().lvl;
                                            hovItemParent.GetComponent<SellSlot>().item = prevItem;
                                            thisParent.GetComponent<InventorySlot>().item = replaceItem;
                                            battle.player.UnequipItem(this.GetComponent<Item>().item, lvlParsThis);
                                            battle.player.EquipItem(go.GetComponent<Item>().item, lvlParsHov);
                                            hovItemParent.GetComponent<SellSlot>().lvl = lvlParsThis;
                                            thisParent.GetComponent<InventorySlot>().lvl = lvlParsHov;
                                        }  
                                    }
                                    else
                                    {
                                        replaceItem = hovItemParent.GetComponent<InventorySlot>().item;
                                        prevItem = thisParent.GetComponent<InventorySlot>().item;
                                        if (replaceItem.type == prevItem.type)
                                        {
                                            canDo = true;
                                            lvlParsHov = hovItemParent.GetComponent<InventorySlot>().lvl;
                                            lvlParsThis = thisParent.GetComponent<InventorySlot>().lvl;
                                            hovItemParent.GetComponent<InventorySlot>().item = prevItem;
                                            thisParent.GetComponent<InventorySlot>().item = replaceItem;
                                            hovItemParent.GetComponent<InventorySlot>().lvl = lvlParsThis;
                                            thisParent.GetComponent<InventorySlot>().lvl = lvlParsHov;
                                            battle.player.UnequipItem(go.GetComponent<Item>().item, lvlParsHov);
                                            battle.player.EquipItem(this.GetComponent<Item>().item, lvlParsThis);
                                        }   
                                    }

                                    if (canDo)
                                    {
                                        string nameSlot = this.transform.parent.name;
                                        if (nameSlot.Contains("Inv"))
                                        {
                                            nameSlot = nameSlot.Replace("Inv", "");
                                            int k = int.Parse(nameSlot);
                                            inventory.slots[k - 1] = replaceItem;
                                        }
                                        else if (nameSlot.Contains("Slot") && this.transform.parent.parent.name.Contains("SellAvailable"))
                                        {
                                            nameSlot = nameSlot.Replace("Slot", "");
                                            int k = int.Parse(nameSlot);
                                            inventory.sellSlots[k - 1] = null;
                                        }

                                        hovItem.SetParent(thisParent);
                                        hovItem.localPosition = Vector3.zero;
                                        thisItem.SetParent(hovItemParent);
                                        thisItem.localPosition = Vector3.zero;
                                        if (!this.transform.parent.parent.name.Contains("SellAvailable"))
                                        {
                                            this.transform.parent.parent.SetAsFirstSibling();
                                        }
                                        foundParent = true;
                                    }
                                }
                            }
                            else if(go.transform.parent.parent.name.Contains("Slots"))
                            {
                                Transform hovItem = go.transform;
                                Transform hovItemParent = go.transform.parent;
                                Transform thisParent = this.transform.parent;
                                Transform thisItem = this.transform;

                                GameItem replaceItem;
                                GameItem prevItem;

                                Battle battle = GameObject.Find("Battle").GetComponent<Battle>();

                                int lvlParsHov = 1;
                                int lvlParsThis = 1;

                                bool canDo = false;

                                if (this.transform.parent.parent.name.Contains("SellAvailable"))
                                {
                                    replaceItem = hovItemParent.GetComponent<InventorySlot>().item;
                                    prevItem = thisParent.GetComponent<SellSlot>().item;

                                    if (replaceItem.type == prevItem.type)
                                    {
                                        canDo = true;
                                        lvlParsHov = hovItemParent.GetComponent<InventorySlot>().lvl;
                                        lvlParsThis = thisParent.GetComponent<SellSlot>().lvl;
                                        hovItemParent.GetComponent<InventorySlot>().item = prevItem;
                                        thisParent.GetComponent<SellSlot>().item = replaceItem;
                                        battle.player.UnequipItem(go.GetComponent<Item>().item, lvlParsHov);
                                        battle.player.EquipItem(this.GetComponent<Item>().item, lvlParsThis);
                                        hovItemParent.GetComponent<InventorySlot>().lvl = lvlParsThis;
                                        thisParent.GetComponent<SellSlot>().lvl = lvlParsHov;
                                    }
                                }
                                else if (go.transform.parent.parent.name.Contains("SellAvailable"))
                                {
                                    replaceItem = hovItemParent.GetComponent<SellSlot>().item;
                                    prevItem = thisParent.GetComponent<InventorySlot>().item;

                                    if (replaceItem.type == prevItem.type)
                                    {
                                        canDo = true;
                                        lvlParsHov = hovItemParent.GetComponent<SellSlot>().lvl;
                                        lvlParsThis = thisParent.GetComponent<InventorySlot>().lvl;
                                        hovItemParent.GetComponent<SellSlot>().item = prevItem;
                                        thisParent.GetComponent<InventorySlot>().item = replaceItem;
                                        battle.player.UnequipItem(this.GetComponent<Item>().item, lvlParsThis);
                                        battle.player.EquipItem(go.GetComponent<Item>().item, lvlParsHov);
                                        hovItemParent.GetComponent<SellSlot>().lvl = lvlParsThis;
                                        thisParent.GetComponent<InventorySlot>().lvl = lvlParsHov;
                                    }
                                }
                                else
                                {
                                    replaceItem = hovItemParent.GetComponent<InventorySlot>().item;
                                    prevItem = thisParent.GetComponent<InventorySlot>().item;

                                    if (replaceItem.type == prevItem.type)
                                    {
                                        canDo = true;
                                        lvlParsHov = hovItemParent.GetComponent<InventorySlot>().lvl;
                                        lvlParsThis = thisParent.GetComponent<InventorySlot>().lvl;
                                        hovItemParent.GetComponent<InventorySlot>().item = prevItem;
                                        thisParent.GetComponent<InventorySlot>().item = replaceItem;
                                        hovItemParent.GetComponent<InventorySlot>().lvl = lvlParsThis;
                                        thisParent.GetComponent<InventorySlot>().lvl = lvlParsHov;
                                        battle.player.UnequipItem(go.GetComponent<Item>().item, lvlParsHov);
                                        battle.player.EquipItem(this.GetComponent<Item>().item, lvlParsThis);
                                    }
                                }
                                if (canDo)
                                {
                                    string nameSlot = this.transform.parent.name;
                                    if (nameSlot.Contains("Inv"))
                                    {
                                        nameSlot = nameSlot.Replace("Inv", "");
                                        int k = int.Parse(nameSlot);
                                        inventory.slots[k - 1] = replaceItem;
                                    }
                                    else if (nameSlot.Contains("Slot") && this.transform.parent.parent.name.Contains("SellAvailable"))
                                    {
                                        nameSlot = nameSlot.Replace("Slot", "");
                                        int k = int.Parse(nameSlot);
                                        inventory.sellSlots[k - 1] = null;
                                    }

                                    hovItem.SetParent(thisParent);
                                    hovItem.localPosition = Vector3.zero;
                                    thisItem.SetParent(hovItemParent);
                                    thisItem.localPosition = Vector3.zero;
                                    if (!this.transform.parent.parent.name.Contains("SellAvailable"))
                                    {
                                        this.transform.parent.parent.SetAsFirstSibling();
                                    }
                                    foundParent = true;
                                }
                            }
                            else
                            {
                                if(go.transform.parent.GetComponent<SellSlot>() != null)
                                {
                                    if (this.GetComponent<Item>().item.type == go.GetComponent<Item>().item.type)
                                    {
                                        Transform hovItem = go.transform;
                                        Transform hovItemParent = go.transform.parent;
                                        Transform thisParent = this.transform.parent;
                                        Transform thisItem = this.transform;

                                        if(thisParent.GetComponent<InventorySlot>() != null)
                                        {
                                            GameItem replaceItem = hovItemParent.GetComponent<SellSlot>().item;
                                            GameItem prevItem = thisParent.GetComponent<InventorySlot>().item;
                                            if (prevItem != replaceItem)
                                            {
                                                string nameSlot1 = this.transform.parent.name;
                                                string nameSlot2 = go.transform.parent.name;

                                                int lvlParsHov = hovItemParent.GetComponent<SellSlot>().lvl;
                                                int lvlParsThis = thisParent.GetComponent<InventorySlot>().lvl;

                                                hovItemParent.GetComponent<SellSlot>().lvl = lvlParsThis;
                                                thisParent.GetComponent<InventorySlot>().lvl = lvlParsHov;

                                                hovItemParent.GetComponent<SellSlot>().item = prevItem;
                                                thisParent.GetComponent<InventorySlot>().item = replaceItem;

                                                if (nameSlot1.Contains("Inv"))
                                                {
                                                    int k = 0;
                                                    nameSlot1 = nameSlot1.Replace("Inv", "");
                                                    k = int.Parse(nameSlot1);
                                                    inventory.slots[k - 1] = replaceItem;
                                                }
                                                if (nameSlot2.Contains("Inv"))
                                                {
                                                    int k = 0;
                                                    nameSlot2 = nameSlot2.Replace("Inv", "");
                                                    k = int.Parse(nameSlot2);
                                                    inventory.slots[k - 1] = prevItem;
                                                }

                                                //this.transform.parent.GetComponent<InventorySlot>().item = null;

                                                hovItem.SetParent(thisParent);
                                                hovItem.localPosition = Vector3.zero;
                                                thisItem.SetParent(hovItemParent);
                                                thisItem.localPosition = Vector3.zero;
                                                foundParent = true;
                                            }
                                            else
                                            {
                                                if ((hovItemParent.GetComponent<SellSlot>().lvl + thisParent.GetComponent<InventorySlot>().lvl) <= 100)
                                                {
                                                    hovItemParent.GetComponent<SellSlot>().lvl += thisParent.GetComponent<InventorySlot>().lvl;
                                                    thisParent.GetComponent<InventorySlot>().item = null;

                                                    string nameSlot = this.transform.parent.name;
                                                    if (nameSlot.Contains("Inv"))
                                                    {
                                                        int k = 0;
                                                        nameSlot = nameSlot.Replace("Inv", "");
                                                        k = int.Parse(nameSlot);
                                                        inventory.slots[k - 1] = null;
                                                    }
                                                    Destroy(thisItem.gameObject);
                                                }
                                                else if ((hovItemParent.GetComponent<SellSlot>().lvl + thisParent.GetComponent<InventorySlot>().lvl) > 100 && hovItemParent.GetComponent<SellSlot>().lvl != 100 && thisParent.GetComponent<InventorySlot>().lvl != 100)
                                                {
                                                    hovItemParent.GetComponent<SellSlot>().lvl = 100;
                                                    thisParent.GetComponent<InventorySlot>().item = null;

                                                    string nameSlot = this.transform.parent.name;
                                                    if (nameSlot.Contains("Inv"))
                                                    {
                                                        int k = 0;
                                                        nameSlot = nameSlot.Replace("Inv", "");
                                                        k = int.Parse(nameSlot);
                                                        inventory.slots[k - 1] = null;
                                                    }
                                                    Destroy(thisItem.gameObject);
                                                }
                                            }
                                        }
                                        else if(thisParent.GetComponent<SellSlot>() != null)
                                        {
                                            GameItem replaceItem = hovItemParent.GetComponent<SellSlot>().item;
                                            GameItem prevItem = thisParent.GetComponent<SellSlot>().item;
                                            string nameSlot1 = this.transform.parent.name;
                                            string nameSlot2 = go.transform.parent.name;

                                            int lvlParsHov = hovItemParent.GetComponent<SellSlot>().lvl;
                                            int lvlParsThis = thisParent.GetComponent<SellSlot>().lvl;

                                            hovItemParent.GetComponent<SellSlot>().lvl = lvlParsThis;
                                            thisParent.GetComponent<SellSlot>().lvl = lvlParsHov;

                                            hovItemParent.GetComponent<SellSlot>().item = prevItem;
                                            thisParent.GetComponent<SellSlot>().item = replaceItem;

                                            if (nameSlot1.Contains("Inv"))
                                            {
                                                int k = 0;
                                                nameSlot1 = nameSlot1.Replace("Inv", "");
                                                k = int.Parse(nameSlot1);
                                                inventory.slots[k - 1] = replaceItem;
                                            }
                                            if (nameSlot2.Contains("Inv"))
                                            {
                                                int k = 0;
                                                nameSlot2 = nameSlot2.Replace("Inv", "");
                                                k = int.Parse(nameSlot2);
                                                inventory.slots[k - 1] = prevItem;
                                            }

                                            //this.transform.parent.GetComponent<InventorySlot>().item = null;

                                            hovItem.SetParent(thisParent);
                                            hovItem.localPosition = Vector3.zero;
                                            thisItem.SetParent(hovItemParent);
                                            thisItem.localPosition = Vector3.zero;
                                            foundParent = true;
                                        }
                                        
                                    }
                                    else
                                    {
                                        Transform hovItem = go.transform; // Inv2_Item
                                        Transform hovItemParent = go.transform.parent; //Inv2
                                        Transform thisParent = this.transform.parent;
                                        Transform thisItem = this.transform;

                                        GameItem replaceItem = hovItemParent.GetComponent<SellSlot>().item;
                                        GameItem prevItem = thisParent.GetComponent<InventorySlot>().item;
                                        hovItemParent.GetComponent<SellSlot>().item = prevItem;
                                        thisParent.GetComponent<InventorySlot>().item = replaceItem;

                                        string nameSlot1 = this.transform.parent.name;
                                        string nameSlot2 = go.transform.parent.name;

                                        int lvlParsHov = hovItemParent.GetComponent<SellSlot>().lvl;
                                        int lvlParsThis = thisParent.GetComponent<InventorySlot>().lvl;

                                        hovItemParent.GetComponent<SellSlot>().lvl = lvlParsThis;
                                        thisParent.GetComponent<InventorySlot>().lvl = lvlParsHov;

                                        if (nameSlot1.Contains("Inv"))
                                        {
                                            int k = 0;
                                            nameSlot1 = nameSlot1.Replace("Inv", "");
                                            k = int.Parse(nameSlot1);
                                            inventory.slots[k - 1] = replaceItem;
                                        }
                                        if (nameSlot2.Contains("Inv"))
                                        {
                                            int k = 0;
                                            nameSlot2 = nameSlot2.Replace("Inv", "");
                                            k = int.Parse(nameSlot2);
                                            inventory.slots[k - 1] = prevItem;
                                        }

                                        //this.transform.parent.GetComponent<InventorySlot>().item = null;

                                        hovItem.SetParent(thisParent);
                                        hovItem.localPosition = Vector3.zero;
                                        thisItem.SetParent(hovItemParent);
                                        thisItem.localPosition = Vector3.zero;
                                        foundParent = true;
                                    }
                                }
                            }
                        }
                        else if (go.transform.parent.name.Contains("Inv") && !go.transform.parent.name.Contains("Inventory"))
                        {
                            if(go.transform.parent.GetComponent<InventorySlot>() != null)
                            {
                                if (this.transform.parent.name.Contains("Slot") && this.transform.parent.parent.name.Contains("Slots"))
                                {
                                    if (this.GetComponent<Item>().item.type == go.GetComponent<Item>().item.type)
                                    {
                                        Transform hovItem = go.transform;
                                        Transform hovItemParent = go.transform.parent;
                                        Transform thisParent = this.transform.parent;
                                        Transform thisItem = this.transform;

                                        GameItem replaceItem = hovItemParent.GetComponent<InventorySlot>().item;
                                        GameItem prevItem = thisParent.GetComponent<InventorySlot>().item;
                                        if (prevItem != replaceItem)
                                        {
                                            hovItemParent.GetComponent<InventorySlot>().item = prevItem;
                                            thisParent.GetComponent<InventorySlot>().item = replaceItem;

                                            string nameSlot = go.transform.parent.name;
                                            if (nameSlot.Contains("Inv"))
                                            {
                                                int k = 0;
                                                nameSlot = nameSlot.Replace("Inv", "");
                                                k = int.Parse(nameSlot);
                                                inventory.slots[k - 1] = this.GetComponent<Item>().item;
                                            }
                                            int lvlParsHov = hovItemParent.GetComponent<InventorySlot>().lvl;
                                            int lvlParsThis = thisParent.GetComponent<InventorySlot>().lvl;
                                            GameObject.Find("Battle").GetComponent<Battle>().player.UnequipItem(this.GetComponent<Item>().item, lvlParsThis);
                                            GameObject.Find("Battle").GetComponent<Battle>().player.EquipItem(go.GetComponent<Item>().item, lvlParsHov);
                                            hovItemParent.GetComponent<InventorySlot>().lvl = lvlParsThis;
                                            thisParent.GetComponent<InventorySlot>().lvl = lvlParsHov;
                                            hovItem.SetParent(thisParent);
                                            hovItem.localPosition = Vector3.zero;
                                            thisItem.SetParent(hovItemParent);
                                            thisItem.localPosition = Vector3.zero;
                                            this.transform.parent.transform.parent.SetAsFirstSibling();
                                            foundParent = true;
                                        }
                                        else
                                        {
                                            if((hovItemParent.GetComponent<InventorySlot>().lvl+ thisParent.GetComponent<InventorySlot>().lvl) <= 100)
                                            {
                                                if (!this.transform.parent.parent.name.Contains("SellAvailable"))
                                                {
                                                    inventory.playerSlots[this.transform.parent.GetSiblingIndex()] = null;
                                                }
                                                hovItemParent.GetComponent<InventorySlot>().lvl += thisParent.GetComponent<InventorySlot>().lvl;
                                                int lvlParsThis = thisParent.GetComponent<InventorySlot>().lvl;
                                                GameObject.Find("Battle").GetComponent<Battle>().player.UnequipItem(this.GetComponent<Item>().item, lvlParsThis);
                                                thisParent.GetComponent<InventorySlot>().lvl = 1;
                                                thisParent.GetComponent<InventorySlot>().item = null;
                                                int lvlParsHov = hovItemParent.GetComponent<InventorySlot>().lvl;
                                                

                                                /*if (this.transform.parent.name.Contains("Inv"))
                                                {
                                                    int k = 0;
                                                    this.transform.parent.name = this.transform.parent.name.Replace("Inv", "");
                                                    k = int.Parse(this.transform.parent.name);
                                                    inventory.slots[k - 1] = null;
                                                }*///tut
                                                inventory.playerSlots[this.transform.parent.GetSiblingIndex()] = null;
                                                Destroy(thisItem.gameObject);
                                            }
                                            else if ((hovItemParent.GetComponent<InventorySlot>().lvl + thisParent.GetComponent<InventorySlot>().lvl) > 100 && hovItemParent.GetComponent<InventorySlot>().lvl != 100 && thisParent.GetComponent<InventorySlot>().lvl != 100)
                                            {
                                                if (!this.transform.parent.parent.name.Contains("SellAvailable"))
                                                {
                                                    inventory.playerSlots[this.transform.parent.GetSiblingIndex()] = null;
                                                }
                                                hovItemParent.GetComponent<InventorySlot>().lvl = 100;
                                                int lvlParsThis = thisParent.GetComponent<InventorySlot>().lvl;
                                                thisParent.GetComponent<InventorySlot>().item = null;
                                                thisParent.GetComponent<InventorySlot>().lvl = 1;
                                                GameObject.Find("Battle").GetComponent<Battle>().player.UnequipItem(this.GetComponent<Item>().item, lvlParsThis);

                                                /*if (this.transform.parent.name.Contains("Inv"))
                                                {
                                                    int k = 0;
                                                    this.transform.parent.name = this.transform.parent.name.Replace("Inv", "");
                                                    k = int.Parse(this.transform.parent.name);
                                                    inventory.slots[k - 1] = null;
                                                }*///tut
                                                inventory.playerSlots[this.transform.parent.GetSiblingIndex()] = null;
                                                Destroy(thisItem.gameObject);
                                            }

                                        }
                                    }
                                }
                                else
                                {
                                    if(this.transform.parent.GetComponent<SellSlot>() != null)
                                    {
                                        if (this.GetComponent<Item>().item.type == go.GetComponent<Item>().item.type)
                                        {
                                            Transform hovItem = go.transform;
                                            Transform hovItemParent = go.transform.parent;
                                            Transform thisParent = this.transform.parent;
                                            Transform thisItem = this.transform;

                                            GameItem replaceItem = hovItemParent.GetComponent<InventorySlot>().item;
                                            GameItem prevItem = thisParent.GetComponent<SellSlot>().item;
                                            if (prevItem != replaceItem)
                                            {
                                                string nameSlot1 = this.transform.parent.name;
                                                string nameSlot2 = go.transform.parent.name;

                                                int lvlParsHov = hovItemParent.GetComponent<InventorySlot>().lvl;
                                                int lvlParsThis = thisParent.GetComponent<SellSlot>().lvl;

                                                hovItemParent.GetComponent<InventorySlot>().lvl = lvlParsThis;
                                                thisParent.GetComponent<SellSlot>().lvl = lvlParsHov;

                                                hovItemParent.GetComponent<InventorySlot>().item = prevItem;
                                                thisParent.GetComponent<SellSlot>().item = replaceItem;

                                                if (nameSlot1.Contains("Inv"))
                                                {
                                                    int k = 0;
                                                    nameSlot1 = nameSlot1.Replace("Inv", "");
                                                    k = int.Parse(nameSlot1);
                                                    inventory.slots[k - 1] = replaceItem;
                                                }
                                                if (nameSlot2.Contains("Inv"))
                                                {
                                                    int k = 0;
                                                    nameSlot2 = nameSlot2.Replace("Inv", "");
                                                    k = int.Parse(nameSlot2);
                                                    inventory.slots[k - 1] = prevItem;
                                                }

                                                //this.transform.parent.GetComponent<InventorySlot>().item = null;

                                                hovItem.SetParent(thisParent);
                                                hovItem.localPosition = Vector3.zero;
                                                thisItem.SetParent(hovItemParent);
                                                thisItem.localPosition = Vector3.zero;
                                                foundParent = true;
                                            }
                                            else
                                            {
                                                if((hovItemParent.GetComponent<InventorySlot>().lvl + thisParent.GetComponent<SellSlot>().lvl) <= 100)
                                                {
                                                    if (!this.transform.parent.parent.name.Contains("SellAvailable"))
                                                    {
                                                        inventory.playerSlots[this.transform.parent.GetSiblingIndex()] = null;
                                                    }
                                                    hovItemParent.GetComponent<InventorySlot>().lvl += thisParent.GetComponent<SellSlot>().lvl;
                                                    thisParent.GetComponent<SellSlot>().item = null;
                                                    string nameSlot = this.transform.parent.name;
                                                    if (nameSlot.Contains("Slot"))
                                                    {
                                                        int k = 0;
                                                        nameSlot = nameSlot.Replace("Slot", "");
                                                        k = int.Parse(nameSlot);
                                                        inventory.sellSlots[k - 1] = null;
                                                    }
                                                    Destroy(thisItem.gameObject);
                                                }
                                                else if ((hovItemParent.GetComponent<InventorySlot>().lvl + thisParent.GetComponent<SellSlot>().lvl) > 100 && hovItemParent.GetComponent<InventorySlot>().lvl != 100 && thisParent.GetComponent<SellSlot>().lvl != 100)
                                                {
                                                    if (!this.transform.parent.parent.name.Contains("SellAvailable"))
                                                    {
                                                        inventory.playerSlots[this.transform.parent.GetSiblingIndex()] = null;
                                                    }
                                                    hovItemParent.GetComponent<InventorySlot>().lvl = 100;
                                                    thisParent.GetComponent<SellSlot>().item = null;
                                                    string nameSlot = this.transform.parent.name;
                                                    if (nameSlot.Contains("Slot"))
                                                    {
                                                        int k = 0;
                                                        nameSlot = nameSlot.Replace("Slot", "");
                                                        k = int.Parse(nameSlot);
                                                        inventory.sellSlots[k - 1] = null;
                                                    }
                                                    Destroy(thisItem.gameObject);
                                                }
                                            }
                                        }
                                        else
                                        {
                                            Transform hovItem = go.transform; // Inv2_Item
                                            Transform hovItemParent = go.transform.parent; //Inv2
                                            Transform thisParent = this.transform.parent;
                                            Transform thisItem = this.transform;

                                            GameItem replaceItem = hovItemParent.GetComponent<InventorySlot>().item;
                                            GameItem prevItem = thisParent.GetComponent<SellSlot>().item;
                                            hovItemParent.GetComponent<InventorySlot>().item = prevItem;
                                            thisParent.GetComponent<SellSlot>().item = replaceItem;

                                            string nameSlot1 = this.transform.parent.name;
                                            string nameSlot2 = go.transform.parent.name;

                                            int lvlParsHov = hovItemParent.GetComponent<InventorySlot>().lvl;
                                            int lvlParsThis = thisParent.GetComponent<SellSlot>().lvl;

                                            hovItemParent.GetComponent<InventorySlot>().lvl = lvlParsThis;
                                            thisParent.GetComponent<SellSlot>().lvl = lvlParsHov;

                                            if (nameSlot1.Contains("Inv"))
                                            {
                                                int k = 0;
                                                nameSlot1 = nameSlot1.Replace("Inv", "");
                                                k = int.Parse(nameSlot1);
                                                inventory.slots[k - 1] = replaceItem;
                                            }
                                            if (nameSlot2.Contains("Inv"))
                                            {
                                                int k = 0;
                                                nameSlot2 = nameSlot2.Replace("Inv", "");
                                                k = int.Parse(nameSlot2);
                                                inventory.slots[k - 1] = prevItem;
                                            }

                                            //this.transform.parent.GetComponent<InventorySlot>().item = null;

                                            hovItem.SetParent(thisParent);
                                            hovItem.localPosition = Vector3.zero;
                                            thisItem.SetParent(hovItemParent);
                                            thisItem.localPosition = Vector3.zero;
                                            foundParent = true;
                                        }
                                    }
                                    else
                                    {
                                        if (this.GetComponent<Item>().item.type == go.GetComponent<Item>().item.type)
                                        {
                                            Transform hovItem = go.transform;
                                            Transform hovItemParent = go.transform.parent;
                                            Transform thisParent = this.transform.parent;
                                            Transform thisItem = this.transform;

                                            GameItem replaceItem = hovItemParent.GetComponent<InventorySlot>().item;
                                            GameItem prevItem = thisParent.GetComponent<InventorySlot>().item;
                                            if (prevItem != replaceItem)
                                            {
                                                string nameSlot1 = this.transform.parent.name;
                                                string nameSlot2 = go.transform.parent.name;

                                                int lvlParsHov = hovItemParent.GetComponent<InventorySlot>().lvl;
                                                int lvlParsThis = thisParent.GetComponent<InventorySlot>().lvl;

                                                hovItemParent.GetComponent<InventorySlot>().lvl = lvlParsThis;
                                                thisParent.GetComponent<InventorySlot>().lvl = lvlParsHov;

                                                if (nameSlot1.Contains("Inv"))
                                                {
                                                    int k = 0;
                                                    nameSlot1 = nameSlot1.Replace("Inv", "");
                                                    k = int.Parse(nameSlot1);
                                                    inventory.slots[k - 1] = replaceItem;
                                                }
                                                if (nameSlot2.Contains("Inv"))
                                                {
                                                    int k = 0;
                                                    nameSlot2 = nameSlot2.Replace("Inv", "");
                                                    k = int.Parse(nameSlot2);
                                                    inventory.slots[k - 1] = prevItem;
                                                }

                                                //this.transform.parent.GetComponent<InventorySlot>().item = null;

                                                hovItem.SetParent(thisParent);
                                                hovItem.localPosition = Vector3.zero;
                                                thisItem.SetParent(hovItemParent);
                                                thisItem.localPosition = Vector3.zero;
                                                foundParent = true;
                                            }
                                            else
                                            {
                                                if((hovItemParent.GetComponent<InventorySlot>().lvl + thisParent.GetComponent<InventorySlot>().lvl) <= 100)
                                                {
                                                    if (!this.transform.parent.parent.name.Contains("SellAvailable"))
                                                    {
                                                        string nameSlot2 = this.transform.parent.name;
                                                        int k = 0;
                                                        nameSlot2 = nameSlot2.Replace("Inv", "");
                                                        k = int.Parse(nameSlot2);
                                                        inventory.slots[k-1] = null;
                                                    }
                                                    hovItemParent.GetComponent<InventorySlot>().lvl += thisParent.GetComponent<InventorySlot>().lvl;
                                                    thisParent.GetComponent<InventorySlot>().item = null;

                                                    string nameSlot = this.transform.parent.name;
                                                    if (nameSlot.Contains("Inv"))
                                                    {
                                                        int k = 0;
                                                        nameSlot = nameSlot.Replace("Inv", "");
                                                        k = int.Parse(nameSlot);
                                                        inventory.slots[k - 1] = null;
                                                    }
                                                    Destroy(thisItem.gameObject);
                                                }
                                                else if ((hovItemParent.GetComponent<InventorySlot>().lvl + thisParent.GetComponent<InventorySlot>().lvl) > 100 && hovItemParent.GetComponent<InventorySlot>().lvl != 100 && thisParent.GetComponent<InventorySlot>().lvl != 100)
                                                {
                                                    if (!this.transform.parent.parent.name.Contains("SellAvailable"))
                                                    {
                                                        string nameSlot2 = this.transform.parent.name;
                                                        int k = 0;
                                                        nameSlot2 = nameSlot2.Replace("Inv", "");
                                                        k = int.Parse(nameSlot2);
                                                        inventory.slots[k - 1] = null;
                                                    }
                                                    hovItemParent.GetComponent<InventorySlot>().lvl = 100;
                                                    thisParent.GetComponent<InventorySlot>().item = null;

                                                    string nameSlot = this.transform.parent.name;
                                                    if (nameSlot.Contains("Inv"))
                                                    {
                                                        int k = 0;
                                                        nameSlot = nameSlot.Replace("Inv", "");
                                                        k = int.Parse(nameSlot);
                                                        inventory.slots[k - 1] = null;
                                                    }
                                                    Destroy(thisItem.gameObject);
                                                }
                                            }
                                        }
                                        else
                                        {
                                            Transform hovItem = go.transform; // Inv2_Item
                                            Transform hovItemParent = go.transform.parent; //Inv2
                                            Transform thisParent = this.transform.parent;
                                            Transform thisItem = this.transform;

                                            GameItem replaceItem = hovItemParent.GetComponent<InventorySlot>().item;
                                            GameItem prevItem = thisParent.GetComponent<InventorySlot>().item;
                                            hovItemParent.GetComponent<InventorySlot>().item = prevItem;
                                            thisParent.GetComponent<InventorySlot>().item = replaceItem;

                                            string nameSlot1 = this.transform.parent.name;
                                            string nameSlot2 = go.transform.parent.name;

                                            int lvlParsHov = hovItemParent.GetComponent<InventorySlot>().lvl;
                                            int lvlParsThis = thisParent.GetComponent<InventorySlot>().lvl;

                                            hovItemParent.GetComponent<InventorySlot>().lvl = lvlParsThis;
                                            thisParent.GetComponent<InventorySlot>().lvl = lvlParsHov;

                                            if (nameSlot1.Contains("Inv"))
                                            {
                                                int k = 0;
                                                nameSlot1 = nameSlot1.Replace("Inv", "");
                                                k = int.Parse(nameSlot1);
                                                inventory.slots[k - 1] = replaceItem;
                                            }
                                            if (nameSlot2.Contains("Inv"))
                                            {
                                                int k = 0;
                                                nameSlot2 = nameSlot2.Replace("Inv", "");
                                                k = int.Parse(nameSlot2);
                                                inventory.slots[k - 1] = prevItem;
                                            }

                                            //this.transform.parent.GetComponent<InventorySlot>().item = null;

                                            hovItem.SetParent(thisParent);
                                            hovItem.localPosition = Vector3.zero;
                                            thisItem.SetParent(hovItemParent);
                                            thisItem.localPosition = Vector3.zero;
                                            foundParent = true;
                                        }
                                    }
                                }
                            }
                        }
                        else if (go.transform.parent.name.Contains("SellAvailable") && go.transform.parent != this.transform.parent.parent)
                        {
                            int k = 0;
                            string nameSlot = this.transform.parent.name;
                            if (nameSlot.Contains("Inv"))
                            {
                                nameSlot = nameSlot.Replace("Inv", "");
                                k = int.Parse(nameSlot);
                                inventory.slots[k - 1] = null;
                            }
                            Transform hovItem = go.transform; // Inv2_Item
                            Transform hovItemParent = go.transform.parent; //Inv2
                            Transform thisParent = this.transform.parent;
                            Transform thisItem = this.transform;

                            int lvlParsHov = hovItem.GetComponent<SellSlot>().lvl;
                            int lvlParsThis = thisParent.GetComponent<InventorySlot>().lvl;
                            if(thisParent.parent.name.Contains("Slots"))
                                GameObject.Find("Battle").GetComponent<Battle>().player.UnequipItem(thisParent.GetComponent<InventorySlot>().item, lvlParsThis);
                            hovItem.GetComponent<SellSlot>().lvl = lvlParsThis;
                            thisParent.GetComponent<InventorySlot>().lvl = lvlParsHov;
                            go.GetComponent<SellSlot>().item = this.transform.parent.GetComponent<InventorySlot>().item;
                            
                            this.transform.parent.GetComponent<InventorySlot>().item = null;
                            this.transform.SetParent(go.transform);
                            this.transform.localPosition = Vector3.zero;

                            foundParent = true;
                        }
                    }
                }
            }

            if (!foundParent)
            {
                this.transform.localPosition = Vector3.zero;
            }
            else
            {
                string nameSlot = this.transform.parent.name;
                if (nameSlot.Contains("Inv"))
                {
                    int k = 0;
                    nameSlot = nameSlot.Replace("Inv", "");
                    k = int.Parse(nameSlot);
                    inventory.slots[k - 1] = this.GetComponent<Item>().item;
                }
                
            }
        }
    }
}
