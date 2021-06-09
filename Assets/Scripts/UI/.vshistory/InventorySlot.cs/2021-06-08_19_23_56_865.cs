using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour , IPointerUpHandler
{
    public int slotNumber;
    public Item item;
    public Image itemIcon;
    public InventorySlot equippedBowItemSlot;

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

    public void OnPointerUp(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            //만약 타입이 활이면 장착
            switch (item.itemType)
            {
                case ItemType.Bow:
                    //item = 메인 활 슬롯에 아이템으로 변경
                    Item tempItem = item;

                    if (equippedBowItemSlot.item != null) 
                    {
                        item = equippedBowItemSlot.item;
                        UpdateSlotUI();
                    }
                    else
                        RemoveSlot();

                    equippedBowItemSlot.ChangeItem(tempItem);
                    equippedBowItemSlot.UpdateSlotUI();
                    UpdateSlotUI();
                    break;
                case ItemType.Consumables:
                    Inventory.instance.RemoveItem(slotNumber);
                    break;
                default:
                    break;
            }
        }
    }

    public void ChangeItem(Item item)
    {
        this.item = item;
    }
}
