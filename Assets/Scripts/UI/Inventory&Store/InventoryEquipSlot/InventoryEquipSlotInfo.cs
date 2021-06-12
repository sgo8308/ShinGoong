using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryEquipSlotInfo : MonoBehaviour
{
    public Item item { get; private set; }
    public bool isItemSet { get; private set; }

    private void Start()
    {
        isItemSet = false;
    }
    public void SetItemInfo(Item item)
    {
        this.item = item;
        isItemSet = true;
    }

    public void RemoveItemInfo()
    {
        item = null;
        isItemSet = false;
    }
}
