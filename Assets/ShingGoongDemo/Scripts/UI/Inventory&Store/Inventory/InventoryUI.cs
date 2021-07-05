using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class InventoryUI : MonoBehaviour
{
    public InventorySlotUI[] slotUIs;
    public Transform slotHolder;

    void Start()
    {
        slotUIs = slotHolder.GetComponentsInChildren<InventorySlotUI>();
        UpdateCoinUI();
    }

    public void RemoveAllItemImage()
    {
        for (int i = 0; i < slotUIs.Length; i++)
        {
            slotUIs[i].RemoveItemImage();
        }
    }

    public void SetAllItemImage()
    {
        for (int i = 0; i < Inventory.instance.GetItemCount(); i++)
        {
            slotUIs[i].SetItemImage();
        }
    }

    public void UpdateCoinUI()
    {
        transform.Find("InventoryPanel")
                 .Find("MoneyPanel")
                 .Find("CoinCount")
                 .GetComponent<TextMeshProUGUI>()
                 .text = Inventory.instance.GetCoinCount().ToString();
    }
}
