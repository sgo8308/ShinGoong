using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryEquipSlot : MonoBehaviour, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject storePanel;
    private InventoryEquipSlotInfo info;
    private InventoryEquipSlotUI ui;

    private void Start()
    {
        info = GetComponent<InventoryEquipSlotInfo>();
        ui = GetComponent<InventoryEquipSlotUI>();
    }

    public void SetItem(Item item)
    {
        info.SetItemInfo(item);
        ui.SetItemImage();
    }

    public void RemoveItem()
    {
        info.RemoveItemInfo();
        ui.RemoveItemImage();
    }

    public Item GetItem()
    {
        return info.item;
    }

    public bool IsItemSet()
    {
        return info.isItemSet;
    }

    public Player player;
    public void OnPointerUp(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Right)
            return;

        player.UnEquip(this);
    }

    public ItemToolTipOpener itemToolTipOpener;
    public ItemToolTipUI itemToolTipUI;

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!info.isItemSet)
            return;

        if (storePanel.activeSelf)
        {
            itemToolTipUI.
                SetInventoryItemToolTipInfo(SlotType.Equipped, info.item, storePanel.activeSelf);
        }
        else
        {
            itemToolTipUI.
                SetInventoryItemToolTipInfo(SlotType.Equipped, info.item, storePanel.activeSelf);
        }

        itemToolTipOpener.OpenToolTip(transform.position, SlotType.Equipped);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        itemToolTipOpener.CloseToolTip();
    }
}
