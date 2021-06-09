using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour , IPointerUpHandler , IPointerEnterHandler , IPointerExitHandler
{
    public int slotNumber;
    public Item item;
    public Image itemIcon;
    public EquippedBowSlot equippedBowItemSlot;
    InventoryUI inventoryUI;

    private void Start()
    {
        inventoryUI = transform.root.GetComponent<InventoryUI>();
        item = null;
    }

    public void UpdateSlotUI()
    {
        itemIcon.sprite = item.itemImage;
        itemIcon.gameObject.SetActive(true);
    }

    public void RemoveSlot()
    {
        item = null;
        itemIcon.gameObject.SetActive(false);
    }

    //When click a item
    public void OnPointerUp(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            if (item == null)
                return;
            
            switch (item.itemType)
            {
                case ItemType.Bow:
                    Item tempItem = item;

                    if (equippedBowItemSlot.isItemSet)
                    {
                        this.item = equippedBowItemSlot.item;
                        UpdateSlotUI();

                        equippedBowItemSlot.Equip(tempItem);
                        equippedBowItemSlot.UpdateSlotUI();

                        Inventory.instance.ChangeItem(slotNumber, item);
                    }
                    else 
                    {
                        equippedBowItemSlot.Equip(tempItem);
                        equippedBowItemSlot.UpdateSlotUI();

                        Inventory.instance.RemoveItem(slotNumber);
                    }
                    break;

                case ItemType.Consumables:
                    Inventory.instance.RemoveItem(slotNumber);
                    break;

                default:
                    break;
            }
        }
    }
    //When mouse hover a item.
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (item == null)
            return;
        inventoryUI.SetToolTipInfo(item);
        inventoryUI.ShowToolTip(transform.position, false);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        inventoryUI.HideToolTip();
    }
}
