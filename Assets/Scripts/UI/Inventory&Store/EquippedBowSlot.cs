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
    public GameObject storePanel;

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
    
    
    public void Equip(Item item)
    {
        this.item = item;
        isItemSet = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Right)
            return;

        if (Inventory.instance.items.Count < Inventory.instance.slotCount)
        {
            Inventory.instance.AddItem(item);
            RemoveSlot();
            isItemSet = false;
        }
    }

    public ItemToolTip toolTip;
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!isItemSet)
            return;

        if (storePanel.activeSelf)
        {
            toolTip.SetEquippedItemInfo(item, storePanel.activeSelf);
        }
        else
        {
            toolTip.SetEquippedItemInfo(item, storePanel.activeSelf);
        }

        toolTip.ShowToolTip(transform.position, SlotType.Equipped);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        toolTip.HideToolTip();
    }
}
