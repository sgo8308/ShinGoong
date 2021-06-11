using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlotUI : MonoBehaviour
{
    public InventorySlotInfo inventorySlotInfo;
    public Image itemIcon;
    public InventoryEquipSlotUI equipSlot;
    public GameObject storePanel;

    private void Start()
    {
        equipSlot = transform.root.Find("InventoryPanel")
                                  .Find("EquipSlotPanel")
                                  .Find("EquipSlot")
                                  .GetComponent<InventoryEquipSlotUI>();
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
