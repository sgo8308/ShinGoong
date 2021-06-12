﻿using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class StoreSlot : MonoBehaviour , IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public Item item;
    public InventoryUI inventoryUi;
    private void Start()
    {
        Initialize();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        //if (MainUI.instance.coinCount < item.price)
        //    return;

        if (eventData.button == PointerEventData.InputButton.Right)
        {
            MainUI.instance.coinCount -= item.priceInInventory;
            MainUI.instance.UpdateCoinUI();
            inventoryUi.UpdateCoinUI();

            Inventory.instance.AddItem(item);
        }
    }

    void Initialize()
    {
        this.transform.Find("ItemImage")
                      .GetComponent<Image>().sprite = item.itemImage;

        this.transform.Find("Name&Price")
                      .Find("Name")
                      .GetComponent<TextMeshProUGUI>().text = item.itemName;
        
        this.transform.Find("Name&Price")
                      .Find("Price")
                      .Find("PriceText")
                      .GetComponent<TextMeshProUGUI>().text = item.priceInInventory.ToString();
    }

    public ItemToolTip toolTip;
    public GameObject storePanel;
    public void OnPointerEnter(PointerEventData eventData)
    {

        toolTip.SetStoreItemInfo(item);

        if (transform.position.y > storePanel.transform.position.y)
        {
            toolTip.ShowToolTip(transform.position, SlotType.StoreTopSide);
        }
        else
        {
            toolTip.ShowToolTip(transform.position, SlotType.StoreBottomSide);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        toolTip.HideToolTip();
    }
}