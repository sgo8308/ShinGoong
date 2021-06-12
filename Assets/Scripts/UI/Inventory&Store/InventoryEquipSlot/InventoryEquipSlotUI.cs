using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryEquipSlotUI : MonoBehaviour
{
    public InventoryEquipSlotInfo inventoryEquipSlotInfo;
    public Image itemIcon;
    public GameObject storePanel;

    public void SetItemImage()
    {
        itemIcon.sprite = inventoryEquipSlotInfo.item.itemImage;
        itemIcon.gameObject.SetActive(true);
    }

    public void RemoveItemImage()
    {
        itemIcon.sprite = null;
        itemIcon.gameObject.SetActive(false);
    }
}
