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
        InventoryInfo.instance.onChangeItem += RedrawSlotsUI;

        UpdateCoinUI();
    }

    void RedrawSlotsUI()
    {
        for (int i = 0; i < slotUIs.Length; i++)
        {
            InventoryInfo.instance.slotInfos[i].RemoveItem();
            slotUIs[i].RemoveItemImage();
        }

        for (int i = 0; i < InventoryInfo.instance.items.Count; i++)
        {
            InventoryInfo.instance.slotInfos[i].SetItem(InventoryInfo.instance.items[i]);
            slotUIs[i].SetItemImage();
        }
    }

    public void UpdateCoinUI()
    {
        transform.Find("InventoryPanel")
                 .Find("MoneyPanel")
                 .Find("CoinCount")
                 .GetComponent<TextMeshProUGUI>()
                 .text = InventoryInfo.instance.coinCount.ToString();
    }
}
