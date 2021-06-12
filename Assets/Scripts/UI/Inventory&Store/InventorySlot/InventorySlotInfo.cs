using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlotInfo : MonoBehaviour
{
    public Item item { get; private set; }

    public int slotNum { get; private set; }

    public bool isItemSet { get; private set; }

    private void Start()
    {
        isItemSet = false;
    }

    public void AssignSlotNum(int slotNum)
    {
        this.slotNum = slotNum;
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
