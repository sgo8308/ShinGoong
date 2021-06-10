using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class StoreSlot : MonoBehaviour , IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public Item item;
    public InventoryUI inventoryUi;
    private void Start()
    {
        Initialize();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        //if (MainUI.instance.coinCount < item.price)
        //    return;

        if (eventData.button == PointerEventData.InputButton.Right)
        {
            MainUI.instance.coinCount -= item.itemSellingPrice;
            MainUI.instance.UpdateCoinUI();
            inventoryUi.UpdateCoinUI();

            Inventory.instance.AddItem(item);
        }
    }

    void Initialize()
    {
        this.transform.Find("ItemImage")
                      .GetComponent<Image>().sprite = item.itemImage;

        this.transform.Find("Name&Price")
                      .Find("Name")
                      .GetComponent<TextMeshProUGUI>().text = item.itemName;
        
        this.transform.Find("Name&Price")
                      .Find("Price")
                      .Find("PriceText")
                      .GetComponent<TextMeshProUGUI>().text = item.itemSellingPrice.ToString();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        //if (storePanel.activeSelf)
        //{
        //    _inventoryUI.SetToolTipInfo(item, false, true);
        //}
        //else
        //{
        //    _inventoryUI.SetToolTipInfo(item, false, false);
        //}

        //_inventoryUI.ShowToolTip(transform.position, false);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        //_inventoryUI.HideToolTip();
    }
}
