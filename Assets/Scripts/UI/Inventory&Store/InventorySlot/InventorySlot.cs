using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject storePanel;
    public InventoryEquipSlot inventoryEquipSlot;
    private InventorySlotInfo info;
    private InventorySlotUI ui;

    private void Start()
    {
        info = GetComponent<InventorySlotInfo>();
        ui = GetComponent<InventorySlotUI>();
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

    public int GetSlotNum()
    {
        return info.slotNum;
    }

    public Player player;

    //When click a item
    public void OnPointerUp(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            if (info.item == null)
                return;

            //With store
            if (storePanel.activeSelf)
            {
                player.Sell(this);

                return;
            }

            //Without store
            switch (info.item.itemType)
            {
                case ItemType.Bow:
                    player.Equip(this, inventoryEquipSlot);
                    break;

                case ItemType.Consumables:
                    player.Use(info);
                    break;

                default:
                    break;
            }
        }
    }

    public ItemToolTipOpener itemToolTipOpener;
    public ItemToolTipUI itemToolTipUI;
    //When mouse hover a item.
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!info.isItemSet)
            return;

        if (storePanel.activeSelf)
        {
            itemToolTipUI.
                SetInventoryItemToolTipInfo(SlotType.Inventory, info.item, storePanel.activeSelf);
        }
        else
        {
            itemToolTipUI.
                SetInventoryItemToolTipInfo(SlotType.Inventory, info.item, storePanel.activeSelf);
        }

        itemToolTipOpener.OpenToolTip(transform.position, SlotType.Inventory);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        itemToolTipOpener.CloseToolTip();
    }
}
