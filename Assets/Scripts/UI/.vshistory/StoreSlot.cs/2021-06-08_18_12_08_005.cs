using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class StoreSlot : MonoBehaviour , IPointerClickHandler
{
    public Item item;
    
    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("btn click");
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            Debug.Log("right btn click");

            Inventory.instance.AddItem(item);
        }

    }
}
