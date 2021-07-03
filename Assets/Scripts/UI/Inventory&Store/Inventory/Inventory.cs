using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory instance;

    private InventoryInfo info;
    private InventoryUI ui;

    public InventoryEquipSlot inventoryEquipSlot;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(transform.root.gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(transform.root.gameObject);

        info = GetComponent<InventoryInfo>();
        ui = GetComponent<InventoryUI>();
    }

    public int GetSlotCount()
    {
        return info.inventorySlotCount;
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

    public int GetItemCount()
    {
        return info.items.Count;
    }

    public bool CanAddItem()
    {
        return info.CanAddItem();
    }

    public void UpdateCoin()
    {
        ui.UpdateCoinUI();
    }

    public void AddCoin(int coinCount)
    {
        info.AddCoinCount(coinCount);
        ui.UpdateCoinUI();
        info.AddCoinCountPerStage(coinCount);
    }

    public void SubtractCoin(int coinCount)
    {
        info.SubtractCoinCount(coinCount);
        ui.UpdateCoinUI();
    }

    public int GetCoinCount()
    {
        return info.coinCount;
    }

    public void AddArrow()
    {
        info.AddArrowCount();
    }

    public void UseArrow()
    {
        info.SubtractArrowCount();
    }

    public int GetArrowCount()
    {
        return info.arrowCount;
    }

    public void InitializeArrowCount(int arrowCount)
    {
        info.InitializeArrowCount(arrowCount);
    }

    public int GetCoinCountPerStage()
    {
        return info.coinCountPerStage;
    }

    public bool isItemEquipped()
    {
        return inventoryEquipSlot.IsItemSet();
    }

    public Item GetEquippedItem()
    {
        return inventoryEquipSlot.GetItem();
    }
}
