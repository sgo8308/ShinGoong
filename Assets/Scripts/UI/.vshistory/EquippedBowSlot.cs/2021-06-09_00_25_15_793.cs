using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EquippedBowSlot : MonoBehaviour , IPointerUpHandler , IPointerEnterHandler, IPointerExitHandler
{
    public Item item;
    public Image itemIcon;
    public bool isItemSet = false;
    InventoryUI inventoryUI;

    private void Start()
    {
        inventoryUI = transform.root.GetComponent<InventoryUI>();
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
    
    
    public void Equip(Item item)
    {
        this.item = item;
        isItemSet = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        /*
         * 만약 인벤토리 창이 꽉 차지 않았다면
           오른쪽 키 누르면 장착해제된다
           장착해제된 아이템은 맨 마지막 슬롯에 들어가고 인벤토리 스크립트에 업데이트된다.

         */

        if (eventData.button != PointerEventData.InputButton.Right)
            return;

        if (Inventory.instance.items.Count < Inventory.instance.slotCount)
        {
            Inventory.instance.AddItem(item);
            RemoveSlot();
            isItemSet = false;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        //if (item == null)
        //    return;
        inventoryUI.ShowInventoryItemToolTip(transform.position);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        inventoryUI.HideToolTip();
    }
}
