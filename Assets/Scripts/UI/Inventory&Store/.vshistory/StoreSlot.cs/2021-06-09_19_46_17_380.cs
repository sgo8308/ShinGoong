using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class StoreSlot : MonoBehaviour , IPointerClickHandler
{
    public Item item;

    private void Start()
    {
        this.transform.Find("Item")
                      .Find("ItemImage")
                      .GetComponent<Image>().sprite = item.itemImage;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (MainUI.instance.coinCount < item.price)
            return;

        if (eventData.button == PointerEventData.InputButton.Right)
        {
            MainUI.instance.coinCount -= item.price;
            Inventory.instance.AddItem(item);
        }
    }

}
