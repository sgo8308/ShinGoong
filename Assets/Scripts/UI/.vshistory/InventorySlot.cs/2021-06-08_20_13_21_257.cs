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
    public EquippedBowSlot equippedBowItemSlot;

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
            if (item == null)
                return;
            /*
             만약에 활 슬롯에 아이템이 있다면
            교체하고
            없다면
            활 슬롯에 지금 누른 활 아이템을 할당하고 
            지금 누른 활 아이템 슬롯은 삭제한다.
             
             
             */
            switch (item.itemType)
            {
                case ItemType.Bow:
                    //item = 메인 활 슬롯에 아이템으로 변경
                    Item tempItem = item;
                    if (equippedBowItemSlot.isItemSet) // 교체
                    {
                        item = equippedBowItemSlot.item;
                        equippedBowItemSlot.ChangeItem(tempItem);
                        UpdateSlotUI();
                        equippedBowItemSlot.UpdateSlotUI();

                        Inventory.instance
                    }
                    else // 없어지고 보우슬롯만 생김
                    {
                        equippedBowItemSlot.ChangeItem(tempItem);
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
}
