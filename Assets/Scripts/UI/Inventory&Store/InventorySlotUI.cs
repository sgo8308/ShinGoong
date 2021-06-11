using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlotUI : MonoBehaviour , IPointerUpHandler , IPointerEnterHandler , IPointerExitHandler
{
    public InventorySlotInfo inventorySlotInfo;
    public Image itemIcon;
    public EquippedBowSlot equippedBowItemSlot;
    InventoryUI _inventoryUI;
    public GameObject storePanel;

    private void Start()
    {
        equippedBowItemSlot = transform.root.Find("InventoryPanel")
                                            .Find("BowItemPanel")
                                            .Find("BowItemSlot")
                                            .GetComponent<EquippedBowSlot>();

        _inventoryUI = transform.root.GetComponent<InventoryUI>();
    }

    public void SetItemImage()
    {
        itemIcon.sprite = inventorySlotInfo.item.itemImage;
        itemIcon.gameObject.SetActive(true);
    }

    public void RemoveItemImage()
    {
        itemIcon.sprite = null;
        itemIcon.gameObject.SetActive(false);
    }

    //When click a item
    public void OnPointerUp(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            if (inventorySlotInfo.item == null)
                return;
            //With store
            if (storePanel.activeSelf)
            {
                InventoryInfo.instance.AddCoinCount(inventorySlotInfo.item.priceInInventory);
                MainUI.instance.UpdateCoinUI();
                _inventoryUI.UpdateCoinUI();
                InventoryInfo.instance.RemoveItem(inventorySlotInfo.slotNum);

                return;
            }

            //Without store
            switch (inventorySlotInfo.item.itemType)
            {
                case ItemType.Bow:
                    Item tempItem = inventorySlotInfo.item;

                    if (equippedBowItemSlot.isItemSet)
                    {
                        inventorySlotInfo.SetItem(equippedBowItemSlot.item);
                        SetItemImage();

                        equippedBowItemSlot.Equip(tempItem);
                        equippedBowItemSlot.UpdateSlotUI();

                        InventoryInfo.instance.
                            ChangeItem(inventorySlotInfo.slotNum, inventorySlotInfo.item);
                    }
                    else 
                    {
                        equippedBowItemSlot.Equip(tempItem);
                        equippedBowItemSlot.UpdateSlotUI();

                        InventoryInfo.instance.RemoveItem(inventorySlotInfo.slotNum);
                    }
                    break;

                case ItemType.Consumables:
                    InventoryInfo.instance.RemoveItem(inventorySlotInfo.slotNum);
                    break;

                default:
                    break;
            }
        }
    }

    public ItemToolTipOpener itemToolTipOpener;
    public ItemToolTipInfoSetter itemToolTipInfoSetter;
    //When mouse hover a item.
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!inventorySlotInfo.isItemSet)
            return;

        if (storePanel.activeSelf)
        {
            itemToolTipInfoSetter.
                SetInventoryItemToolTipInfo(SlotType.Inventory, inventorySlotInfo.item, storePanel.activeSelf);
        }
        else
        {
            itemToolTipInfoSetter.
                SetInventoryItemToolTipInfo(SlotType.Inventory, inventorySlotInfo.item, storePanel.activeSelf);
        }

        itemToolTipOpener.OpenToolTip(transform.position, SlotType.Inventory);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        itemToolTipOpener.CloseToolTip();
    }
}
