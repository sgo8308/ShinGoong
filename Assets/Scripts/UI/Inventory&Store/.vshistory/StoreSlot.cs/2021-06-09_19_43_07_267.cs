using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class StoreSlot : MonoBehaviour , IPointerClickHandler
{
    public Item item;
    
    public void OnPointerClick(PointerEventData eventData)
    {
        if (MainUI.instance.coinCount < item.price)
            return;

        if (eventData.button == PointerEventData.InputButton.Right)
        {
            Inventory.instance.AddItem(item);
        }

    }

}
