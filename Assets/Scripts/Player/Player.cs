﻿using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    private PlayerMove playerMove;
    private PlayerAttack playerAttack;
    private PlayerSkill playerSkill;
    private Animator animator;
    private GameObject bulletExplosion; 

    public delegate void OnPlayerDead();
    public OnPlayerDead onPlayerDead;
    public static bool isDead; 

    void Start()
    {
        playerMove = GetComponent<PlayerMove>();
        playerAttack = GetComponent<PlayerAttack>();
        playerSkill = GetComponent<PlayerSkill>();
        animator = GetComponent<Animator>();
        bulletExplosion = transform.Find("BulletExplosion").gameObject;

        Cursor.visible = true;

        UnityEngine.SceneManagement.SceneManager.sceneLoaded += Revive;
    }

    #region Revive And Die
    public void Revive(Scene scene, LoadSceneMode mode)
    {
        if (scene.name != "ShelterScene")
            return;

        animator.enabled = true;
        animator.SetBool("isHit", false);
        animator.SetBool("isJumpingFinal", false);

        isDead = false;

        playerMove.SetCanMove(true);

        playerAttack.SetCanShoot(true);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Radar")
            Invoke("Dead", 0.1f); // dead after 0.1 seconds
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.tag == "Radar")
            CancelInvoke("Dead");
    }

    void Dead()
    {
        if (isDead || StageManager.instance.stageState == StageState.CLEAR)
            return;

        isDead = true;

        bulletExplosion.SetActive(true);
        Invoke("HideBulletExplosion", 0.5f);
        SoundManager.instance.PlayPlayerSound(PlayerSounds.PLAYER_HIT);

        animator.enabled = true;
        Invoke("StartHitAnimation", 0.1f);

        if (onPlayerDead != null)
            onPlayerDead.Invoke();

        playerMove.SetCanMove(false);
        playerAttack.SetCanShoot(false);

        Time.timeScale = 0.4f;
    }

    void HideBulletExplosion()
    {
        bulletExplosion.SetActive(false);
    }

    void StartHitAnimation()
    {
        animator.SetBool("isHit", true);
    }

    #endregion

    public void AcquireCoin(int amount)
    {
        Inventory.instance.AddCoin(amount);

        MainUI.instance.UpdateCoinUI();

        Inventory.instance.UpdateCoin();

        SoundManager.instance.PlayNonPlayerSound(NonPlayerSounds.ACQUIRE_COIN);
    }
    

    #region Item
    public void Sell(InventorySlot slot)
    {
        Inventory.instance.AddCoin(slot.GetItem().priceInInventory);
        MainUI.instance.UpdateCoinUI();
        Inventory.instance.UpdateCoin();

        Inventory.instance.RemoveItem(slot.GetSlotNum());

        SoundManager.instance.PlayPlayerSound(PlayerSounds.PLAYER_SELL);
    }

    public void Buy(StoreSlot storeSlot)
    {
        Inventory.instance.SubtractCoin(storeSlot.item.priceInStore);
        MainUI.instance.UpdateCoinUI();
        Inventory.instance.UpdateCoin();

        Inventory.instance.AddItem(storeSlot.item);

        SoundManager.instance.PlayPlayerSound(PlayerSounds.PLAYER_BUY);

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
        
        SoundManager.instance.PlayPlayerSound(PlayerSounds.PLAYER_EQUIP);
    }

    public void UnEquip(InventoryEquipSlot equipSlot)
    {
        if (Inventory.instance.GetItemCount() < Inventory.instance.GetSlotCount())
        {
            Inventory.instance.AddItem(equipSlot.GetItem());
            equipSlot.RemoveItem();

            playerSkill.UnSetSkill();

            SoundManager.instance.PlayPlayerSound(PlayerSounds.PLAYER_UNEQUIP);
        }
    }

    public void Use(InventorySlotInfo slotInfo)
    {
        Inventory.instance.RemoveItem(slotInfo.slotNum);
    } 
    #endregion
}

