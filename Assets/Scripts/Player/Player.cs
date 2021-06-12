using UnityEngine;

public class Player : MonoBehaviour
{
    private PlayerMove playerMove;
    private PlayerAttack playerAttack;
    private PlayerSkill playerSkill;
    private Animator animator;

    public delegate void OnPlayerDead();
    public OnPlayerDead onPlayerDead;

    void Start()
    {
        playerMove = GetComponent<PlayerMove>();
        playerAttack = GetComponent<PlayerAttack>();
        playerSkill = GetComponent<PlayerSkill>();
        Cursor.visible = true;
    }

    #region Dead
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Radar")
            Invoke("Dead", 0.1f); // dead after 0.1 seconds
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if(col.gameObject.tag == "Radar")
            CancelInvoke("Dead");
    }

    void Dead()
    {
        animator.SetBool("isHit", true);
        
        if(onPlayerDead != null)
            onPlayerDead.Invoke();
        
        playerMove.SetCanMove(false);
        playerAttack.SetCanShoot(false);
    }
    #endregion
    
    public void AcquireCoin()
    {
        Inventory.instance.AddCoin(1);

        MainUI.instance.UpdateCoinUI();

        Inventory.instance.UpdateCoin();
    }

    #region Item
    public void Sell(Inventory inventory, InventorySlot slot)
    {
        Inventory.instance.AddCoin(slot.GetItem().priceInInventory);
        MainUI.instance.UpdateCoinUI();
        inventory.UpdateCoin();

        Inventory.instance.RemoveItem(slot.GetSlotNum());
    }

    public void Buy(InventoryUI inventoryUI, StoreSlot storeSlot)
    {
        Inventory.instance.SubtractCoin(storeSlot.item.priceInStore);
        MainUI.instance.UpdateCoinUI();
        inventoryUI.UpdateCoinUI();

        Inventory.instance.AddItem(storeSlot.item);
    }

    public void Equip(InventorySlot inventorySlot, InventoryEquipSlot equipSlot)
    {
        Item tempItem = inventorySlot.GetItem();

        if (equipSlot.IsItemSet())
        {
            inventorySlot.SetItem(equipSlot.GetItem());

            equipSlot.SetItem(tempItem);

            Inventory.instance.
                ChangeItem(inventorySlot.GetSlotNum(), inventorySlot.GetItem());
        }
        else
        {
            equipSlot.SetItem(tempItem);

            Inventory.instance.RemoveItem(inventorySlot.GetSlotNum());
        }

        playerSkill.SetSkill(equipSlot.GetItem());
    }

    public void UnEquip(InventoryEquipSlot equipSlot)
    {
        if (Inventory.instance.GetItemCount() < Inventory.instance.GetSlotCount())
        {
            Inventory.instance.AddItem(equipSlot.GetItem());
            equipSlot.RemoveItem();
        }
    }

    public void Use(InventorySlotInfo slotInfo)
    {
        Inventory.instance.RemoveItem(slotInfo.slotNum);
    } 
    #endregion
}

