using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryEquipSlot : MonoBehaviour, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
{
    public InventoryEquipSlotInfo info;
    public InventoryEquipSlotUI ui;
    public GameObject storePanel;

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

    public Player player;
    public void OnPointerUp(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Right)
            return;

        player.UnEquip(this);
    }

    public ItemToolTipOpener itemToolTipOpener;
    public ItemToolTipInfoSetter itemToolTipInfoSetter;

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!info.isItemSet)
            return;

        if (storePanel.activeSelf)
        {
            itemToolTipInfoSetter.
                SetInventoryItemToolTipInfo(SlotType.Equipped, info.item, storePanel.activeSelf);
        }
        else
        {
            itemToolTipInfoSetter.
                SetInventoryItemToolTipInfo(SlotType.Equipped, info.item, storePanel.activeSelf);
        }

        itemToolTipOpener.OpenToolTip(transform.position, SlotType.Equipped);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        itemToolTipOpener.CloseToolTip();
    }
}
