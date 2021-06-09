using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    int _slotCount;

    public List<Item> items = new List<Item>();

    public bool AddItem(Item item)
    {
        if (items.Count < _slotCount)
        {
            items.Add(item);
        }
    }
}
