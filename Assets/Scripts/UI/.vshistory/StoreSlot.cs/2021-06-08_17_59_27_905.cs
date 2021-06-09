using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class StoreSlot : MonoBehaviour , IPointerClickHandler
{
    public Item item;
    Inventory _inventory;
    private void Start()
    {
        _inventory = Inventory.instance;
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            AddItem(item);
        }

    }
}
