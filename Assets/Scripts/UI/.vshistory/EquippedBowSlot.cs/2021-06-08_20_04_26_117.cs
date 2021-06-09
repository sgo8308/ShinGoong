using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EquippedBowSlot : MonoBehaviour , IPointerUpHandler
{
    public Item item;
    public Image itemIcon;
    public bool isItemSet = false;

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
    
    
    public void ChangeItem(Item item)
    {
        this.item = item;
    }

    public void OnPointerUp(PointerEventData eventData)
    {

    }
}
