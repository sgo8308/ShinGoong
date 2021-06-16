using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    private PlayerMove playerMove;
    private PlayerAttack playerAttack;
    private PlayerSkill playerSkill;
    private Animator animator;

    public delegate void OnPlayerDead();
    public OnPlayerDead onPlayerDead;
    private bool isDead; 

    void Start()
    {
        playerMove = GetComponent<PlayerMove>();
        playerAttack = GetComponent<PlayerAttack>();
        playerSkill = GetComponent<PlayerSkill>();
        animator = GetComponent<Animator>();
        Cursor.visible = true;

        UnityEngine.SceneManagement.SceneManager.sceneLoaded += Revive;
    }

    #region Revive And Die
    public void Revive(Scene scene, LoadSceneMode mode)
    {
        animator.SetBool("isHit", false);
        isDead = false;

        playerMove.SetCanMove(true);

        playerAttack.SetCanShoot(true);
    }
        
    //private void OnTriggerEnter2D(Collider2D col)
    //{
    //    if (col.gameObject.tag == "Radar")
    //        Invoke("Dead", 0.1f); // dead after 0.1 seconds
    //}

    //private void OnTriggerExit2D(Collider2D col)
    //{
    //    if(col.gameObject.tag == "Radar")
    //        CancelInvoke("Dead");
    //}

    //void Dead()
    //{
    //    if (isDead)
    //        return;

    //    isDead = true;
    //    animator.SetBool("isHit", true);
        
    //    if(onPlayerDead != null)
    //        onPlayerDead.Invoke();

    //    playerMove.SetCanMove(false);
    //    playerAttack.SetCanShoot(false);
    //}

    #endregion
    
    public void AcquireCoin()
    {
        Inventory.instance.AddCoin(1);

        MainUI.instance.UpdateCoinUI();

        Inventory.instance.UpdateCoin();
    }
    

    #region Item
    public void Sell(InventorySlot slot)
    {
        Inventory.instance.AddCoin(slot.GetItem().priceInInventory);
        MainUI.instance.UpdateCoinUI();
        Inventory.instance.UpdateCoin();

        Inventory.instance.RemoveItem(slot.GetSlotNum());
    }

    public void Buy(StoreSlot storeSlot)
    {
        Inventory.instance.SubtractCoin(storeSlot.item.priceInStore);
        MainUI.instance.UpdateCoinUI();
        Inventory.instance.UpdateCoin();

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

            playerSkill.UnSetSkill();
        }
    }

    public void Use(InventorySlotInfo slotInfo)
    {
        Inventory.instance.RemoveItem(slotInfo.slotNum);
    } 
    #endregion
}

