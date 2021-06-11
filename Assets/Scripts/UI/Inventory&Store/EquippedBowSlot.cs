using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EquippedBowSlot : MonoBehaviour , IPointerUpHandler , IPointerEnterHandler, IPointerExitHandler
{
    public Item item;
    public Image itemIcon;
    public bool isItemSet = false;
    public GameObject storePanel;

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

        if (InventoryInfo.instance.items.Count < InventoryInfo.instance.inventorySlotCount)
        {
            InventoryInfo.instance.AddItem(item);
            RemoveSlot();
            isItemSet = false;
        }
    }

    public ItemToolTipOpener itemToolTipOpener;
    public ItemToolTipInfoSetter itemToolTipInfoSetter;

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!isItemSet)
            return;

        if (storePanel.activeSelf)
        {
            itemToolTipInfoSetter.
                SetInventoryItemToolTipInfo(SlotType.Equipped, item, storePanel.activeSelf);
        }
        else
        {
            itemToolTipInfoSetter.
                SetInventoryItemToolTipInfo(SlotType.Equipped, item, storePanel.activeSelf);
        }

        itemToolTipOpener.OpenToolTip(transform.position, SlotType.Equipped);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        itemToolTipOpener.CloseToolTip();
    }
}
