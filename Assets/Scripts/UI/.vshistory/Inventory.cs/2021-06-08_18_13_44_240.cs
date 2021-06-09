using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory instance;

    private void Awake()
    {
        instance = this;
    }

    int _slotCount = 15;

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
