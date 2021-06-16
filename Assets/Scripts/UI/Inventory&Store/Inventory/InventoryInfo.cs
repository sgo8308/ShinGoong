﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryInfo : MonoBehaviour
{
    public List<Item> items = new List<Item>();

    public InventorySlotInfo[] slotInfos;

    public Transform slotHolder;

    public int inventorySlotCount { get; private set; }

    public int coinCount { get; private set; }

    public int arrowCount { get; private set; }

    private void Start()
    {
        slotInfos = slotHolder.GetComponentsInChildren<InventorySlotInfo>();

        inventorySlotCount = 15;

        AssignSlotsNum();

        InitializeArrowCount(45);

        coinCount = 50000;
    }

    public void AddItemInfo(Item item)
    {
        items.Add(item);
    }

    public void RemoveItemInfo(int index)
    {
        items.RemoveAt(index);
    }

    public void ChangeItemInfo(int slotNum, Item item)
    {
        items.RemoveAt(slotNum);
        items.Insert(slotNum, item);
    }

    public void RemoveAllItemInfo()
    {
        for (int i = 0; i < slotInfos.Length; i++)
        {
            slotInfos[i].RemoveItemInfo();
        }
    }

    public void AddAllItemInfo()
    {
        for (int i = 0; i < items.Count; i++)
        {
            slotInfos[i].SetItemInfo(items[i]);
        }
    }

    public bool CanAddItem()
    {
        if (items.Count >= inventorySlotCount)
            return false;

        return true;
    }

    public void AddCoinCount(int coinCount)
    {
        this.coinCount += coinCount;
    }

    public void SubtractCoinCount(int coinCount)
    {
        this.coinCount -= coinCount;
    }

    public void AddArrowCount()
    {
        arrowCount++;
    }

    public void SubtractArrowCount()
    {
        arrowCount--;
    }

    public void InitializeArrowCount(int arrowCount)
    {
        this.arrowCount = arrowCount;
    }
    public void AssignSlotsNum()
    {
        for (int i = 0; i < slotInfos.Length; i++)
        {
            slotInfos[i].AssignSlotNum(i);
        }
    }
}