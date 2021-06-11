using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryInfo : MonoBehaviour
{
    public static InventoryInfo instance;

    public List<Item> items = new List<Item>();

    public InventorySlotInfo[] slotInfos;

    public Transform slotHolder;

    public int inventorySlotCount { get; private set; }

    public int coinCount { get; private set; }

    public int arrowCount { get; private set; }

    public delegate void OnChangeItem();
    public OnChangeItem onChangeItem;

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

    private void Start()
    {
        slotInfos = slotHolder.GetComponentsInChildren<InventorySlotInfo>();

        inventorySlotCount = 15;

        AssignSlotsNum();

        InitializeArrowCount(45);

        coinCount = 20000;
    }

    public void AddItem(Item item)
    {
        if (items.Count < inventorySlotCount)
        {
            items.Add(item);
            if (onChangeItem != null) 
            {
                onChangeItem.Invoke();
                AssignSlotsNum();
            }
        }
    }

    public void RemoveItem(int index)
    {
        items.RemoveAt(index);
        onChangeItem.Invoke();
        AssignSlotsNum();
    }

    public void ChangeItem(int slotNum, Item item)
    {
        items.RemoveAt(slotNum);
        items.Insert(slotNum, item);
    }

    public void AddCoinCount(int coinCount)
    {
        this.coinCount += coinCount;
    }

    public void SubtractCoinCount(int coinCount)
    {
        this.coinCount -= coinCount;
    }

    public void AddArrowCount()
    {
        arrowCount++;
    }

    public void SubtractArrowCount()
    {
        arrowCount--;
    }

    public void InitializeArrowCount(int arrowCount)
    {
        this.arrowCount = arrowCount;
    }
    private void AssignSlotsNum()
    {
        for (int i = 0; i < slotInfos.Length; i++)
        {
            slotInfos[i].AssignSlotNum(i);
        }
    }
}
