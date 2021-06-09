using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    int _slotCount;

    public List<Item> items = new List<Item>();

    public delegate void OnChangeItem();
    public OnChangeItem onChangeItem;
    public bool AddItem(Item item)
    {
        if (items.Count < _slotCount)
        {
            items.Add(item);
            onChangeItem.Invoke();
            return true;
        }

        return false;
    }
}
