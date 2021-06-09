using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    int _slotCount;

    public List<Item> items = new List<Item>();

    public delegate void OnChangeItem();
    public OnChangeItem onChangeItem;

    public void AddItem(Item item)
    {
        if (items.Count < _slotCount)
        {
            items.Add(item);
            if(onChangeItem != null)
                onChangeItem.Invoke();
        }
    }
}
