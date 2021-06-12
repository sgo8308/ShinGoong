﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory instance;
    public int slotCount;

    public List<Item> items = new List<Item>();

    public delegate void OnChangeItem();
    public OnChangeItem onChangeItem;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        slotCount = 15;
    }

    public void AddItem(Item item)
    {
        if (items.Count < slotCount)
        {
            items.Add(item);
            if(onChangeItem != null)
                onChangeItem.Invoke();
        }
    }

    public void RemoveItem(int index)
    {
        items.RemoveAt(index);
        onChangeItem.Invoke();
    }

    public void ChangeItem(int slotNum, Item item)
    {
        items.RemoveAt(slotNum);
        items.Insert(slotNum, item);
    }
}