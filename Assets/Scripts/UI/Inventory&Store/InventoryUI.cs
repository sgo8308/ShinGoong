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
        for (int i = 0; i < Inventory.instance.info.items.Count; i++)
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
                 .text = Inventory.instance.info.coinCount.ToString();
    }
}
