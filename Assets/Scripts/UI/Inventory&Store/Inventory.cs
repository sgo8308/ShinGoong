using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory instance;

    public InventoryInfo info;
    public InventoryUI ui;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void AddItem(Item item)
    {
        if (info.items.Count < info.inventorySlotCount)
        {
            info.AddItemInfo(item);

            info.RemoveAllItemInfo();
            ui.RemoveAllItemImage();

            info.AddAllItemInfo();
            ui.SetAllItemImage();

            info.AssignSlotsNum();
        }    
    }

    public void RemoveItem(int index)
    {
        info.RemoveItemInfo(index);

        info.RemoveAllItemInfo();
        ui.RemoveAllItemImage();

        info.AddAllItemInfo();
        ui.SetAllItemImage();

        info.AssignSlotsNum();
    }

    public void ChangeItem(int slotNumOfChangedItem, Item item)
    {
        info.ChangeItemInfo(slotNumOfChangedItem, item);
    }

    public void AddCoin(int coinCount)
    {
        info.AddCoinCount(coinCount);
        ui.UpdateCoinUI();
    }

    public void SubtractCoin(int coinCount)
    {
        info.SubtractCoinCount(coinCount);
        ui.UpdateCoinUI();
    }

    public void AddArrow()
    {
        info.AddArrowCount();
    }

    public void UseArrow()
    {
        info.SubtractArrowCount();
    }

    public void InitializeArrowCount(int arrowCount)
    {
        info.InitializeArrowCount(arrowCount);
    }
}
