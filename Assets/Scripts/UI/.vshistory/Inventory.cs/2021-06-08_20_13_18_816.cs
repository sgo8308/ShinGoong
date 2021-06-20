using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory instance;
    int _slotCount;
    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        _slotCount = 15;
    }

    public List<Item> items = new List<Item>();

    public delegate void OnChangeItem();
    public OnChangeItem onChangeItem;

    public void AddItem(Item item)
    {
        Debug.Log("add item if state before");

        if (items.Count < _slotCount)
        {
            Debug.Log("add item if state enter");
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
