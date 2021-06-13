using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlotUI : MonoBehaviour
{
    public Image itemIcon;
    public InventoryEquipSlotUI equipSlot;
    public GameObject storePanel;
    private InventorySlotInfo inventorySlotInfo;

    private void Start()
    {
        inventorySlotInfo = GetComponent<InventorySlotInfo>();
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
}
