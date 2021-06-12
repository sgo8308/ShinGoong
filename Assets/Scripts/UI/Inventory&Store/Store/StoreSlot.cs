﻿using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class StoreSlot : MonoBehaviour , IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public Item item;

    public InventoryUI inventoryUi;

    public PlayerInfo playerInfo;

    public Player player;

    private void Start()
    {
        Initialize();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (Inventory.instance.GetCoinCount() < item.priceInStore)
            return;

        if (playerInfo.level < item.levelLimit)
            return;

        if (!Inventory.instance.CanAddItem())
            return;

        if (eventData.button == PointerEventData.InputButton.Right)
            player.Buy(this);
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
                      .GetComponent<TextMeshProUGUI>().text = item.priceInStore.ToString();
    }

    public ItemToolTipOpener itemToolTipOpener;
    public ItemToolTipUI itemToolTipUI;
    public void OnPointerEnter(PointerEventData eventData)
    {

        itemToolTipUI.SetStoreItemInfo(item);

        if (transform.position.y > this.transform.root.position.y)
        {
            itemToolTipOpener.OpenToolTip(transform.position, SlotType.StoreTopSide);
        }
        else
        {
            itemToolTipOpener.OpenToolTip(transform.position, SlotType.StoreBottomSide);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        itemToolTipOpener.CloseToolTip();
    }
}