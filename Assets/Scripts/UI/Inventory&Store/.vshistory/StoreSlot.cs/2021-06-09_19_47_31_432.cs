using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class StoreSlot : MonoBehaviour , IPointerClickHandler
{
    public Item item;

    private void Start()
    {
        Initialize();
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

    void Initialize()
    {
        this.transform.Find("Item")
              .Find("ItemImage")
              .GetComponent<Image>().sprite = item.itemImage;

        this.transform.Find("Item")
              .Find("Name&Price")
              .Find("Name")
              .GetComponent<TEXMMES>().sprite = item.itemImage;

    }
}
