using UnityEngine;
using UnityEngine.SceneManagement;
using TentuPlay.Api;

/// <summary>
/// Player에 붙어 있는 player가 하는 대부분의 행동을 담당하는 스크립트.
/// </summary>
public class Player : MonoBehaviour
{
    private PlayerMove playerMove;
    private PlayerAttack playerAttack;
    private PlayerSkill playerSkill;
    private PlayerInfo playerInfo;
    private Animator animator;
    private GameObject bulletExplosion; 
    private GameObject electricityExplosion; 

    public delegate void OnPlayerDead();
    public OnPlayerDead onPlayerDead;
    public static bool isDead;

    private string player_uuid = "TentuPlayer"; // player_uuid can be anything that uniquely identifies each of your game user.
    private string character_uuid = TentuPlayKeyword._DUMMY_CHARACTER_ID_;
    private string[] character_uuids = new string[] { TentuPlayKeyword._DUMMY_CHARACTER_ID_ };

    void Start()
    {
        playerMove = GetComponent<PlayerMove>();
        playerAttack = GetComponent<PlayerAttack>();
        playerSkill = GetComponent<PlayerSkill>();
        playerInfo = GetComponent<PlayerInfo>();
        animator = GetComponent<Animator>();
        bulletExplosion = transform.Find("BulletExplosion").gameObject;
        electricityExplosion = transform.Find("ElectricityExplosion").gameObject;

        Cursor.visible = true;

        UnityEngine.SceneManagement.SceneManager.sceneLoaded += Revive;
    }

    #region 텐투플레이 메소드가 있는 부분
    public void AcquireCoin(int amount)
    {
        Inventory.instance.AddCoin(amount);

        MainUI.instance.UpdateCoinUI();

        Inventory.instance.UpdateCoin();

        SoundManager.instance.PlayNonPlayerSound(NonPlayerSounds.ACQUIRE_COIN);

        new TPStashEvent().GetCurrency(
            player_uuid: player_uuid,
            character_uuid: character_uuid,
            currency_slug: "Coin",
            currency_quantity: amount,
            currency_total_quantity: Inventory.instance.GetCoinCount(),
            from_entity: entity.PlayStage,
            from_category_slug: StageManager.instance.GetStageCategory(),
            from_slug: StageManager.instance.GetStageName()
            );
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

        new TPStashEvent().UseCurrency(
            player_uuid: player_uuid, // unique identifier of player
            character_uuid: character_uuid,
            currency_slug: "coin", // unique identifier of gotten item
            currency_quantity: storeSlot.item.priceInStore, // quantity of gotten item
            currency_total_quantity: Inventory.instance.GetCoinCount(),
            where_to_entity: entity.GetEquipment,  // i got this item from what category of game elements?
            where_to_category_slug: "shop",
            where_to_slug: "shelter shop"  // i got this item from exactly what game element? unique identifier of source of the item
            );

        new TPStashEvent().GetEquipment(
            player_uuid: player_uuid, // unique identifier of player
            character_uuid: character_uuid,
            item_slug: storeSlot.item.itemName, // unique identifier of gotten item
            item_quantity: 1.0F, // quantity of gotten item
            from_entity: entity.ShopPurchase,  // i got this item from what category of game elements?
            from_category_slug: StageManager.instance.GetStageCategory(),
            from_slug: StageManager.instance.GetStageName()  // i got this item from exactly what game element? unique identifier of source of the item
            );
    }

    public void Equip(InventorySlot inventorySlot, InventoryEquipSlot equipSlot)
    {
        Item tempItem = inventorySlot.GetItem();

        // 장착 슬롯이 비어 있지 않다면 이 아이템으로 교체
        if (equipSlot.IsItemSet())
        {
            new TPStashEvent().EquipEquipment(
                player_uuid: player_uuid, // unique identifier of player
                character_uuid: character_uuid,
                item_slug: equipSlot.GetItem().itemName,
                equip_status: equipStatus.Unequip,
                item_level: null,
                character_level: playerInfo.level
                );

            new TPStashEvent().EquipEquipment(
                player_uuid: player_uuid, // unique identifier of player
                character_uuid: character_uuid,
                item_slug: tempItem.itemName,
                equip_status: equipStatus.Equip,
                item_level: null,
                character_level: playerInfo.level
                );

            inventorySlot.SetItem(equipSlot.GetItem());

            equipSlot.SetItem(tempItem);

            Inventory.instance.
                ChangeItem(inventorySlot.GetSlotNum(), inventorySlot.GetItem());
        }
        // 장착 슬롯이 비어 있다면 이 아이템을 장착
        else
        {
            equipSlot.SetItem(tempItem);

            Inventory.instance.RemoveItem(inventorySlot.GetSlotNum());
            new TPStashEvent().EquipEquipment(
                player_uuid: player_uuid, // unique identifier of player
                character_uuid: character_uuid,
                item_slug: tempItem.itemName,
                equip_status: equipStatus.Equip,
                item_level: null,
                character_level: playerInfo.level
                );
        }
        //아이템에 붙어있는 스킬 장착
        playerSkill.SetSkill(equipSlot.GetItem());

        new TPStashEvent().EquipSkill(
                player_uuid: player_uuid, // unique identifier of player
                character_uuid: character_uuid,
                skill_slug: equipSlot.GetItem().skillName,
                skill_category_slug: null,
                equip_status: equipStatus.Equip,
                skill_level: null,
                character_level: playerInfo.level
                );

        SoundManager.instance.PlayPlayerSound(PlayerSounds.PLAYER_EQUIP);
    }

    public void UnEquip(InventoryEquipSlot equipSlot)
    {
        //아이템 창이 꽉 차지 않았다면 장비중인 아이템 장착 해제
        if (Inventory.instance.GetItemCount() < Inventory.instance.GetSlotCount())
        {
            new TPStashEvent().EquipEquipment(
                player_uuid: player_uuid, // unique identifier of player
                character_uuid: character_uuid,
                item_slug: equipSlot.GetItem().itemName,
                equip_status: equipStatus.Unequip,
                item_level: null,
                character_level: playerInfo.level
                );

            playerSkill.UnSetSkill();

            new TPStashEvent().EquipSkill(
                player_uuid: player_uuid, // unique identifier of player
                character_uuid: character_uuid,
                skill_slug: equipSlot.GetItem().skillName,
                skill_category_slug: null,
                equip_status: equipStatus.Unequip,
                skill_level: null,
                character_level: playerInfo.level
                );

            Inventory.instance.AddItem(equipSlot.GetItem());
            equipSlot.RemoveItem();

            SoundManager.instance.PlayPlayerSound(PlayerSounds.PLAYER_UNEQUIP);
        }
    }

    public void Use(InventorySlotInfo slotInfo)
    {
        Inventory.instance.RemoveItem(slotInfo.slotNum);
    }
    #endregion 
    #endregion


    #region Revive And Die
    public void Revive(Scene scene, LoadSceneMode mode)
    {
        if (scene.name != "ShelterScene")
            return;

        gameObject.SetActive(true);
        
        animator.enabled = true;
        animator.SetBool("isHit", false);
        animator.SetBool("isJumpingFinal", false);

        isDead = false;

        playerMove.SetCanMove(true);

        playerAttack.SetCanShoot(true);
    }
    
    private const int LAYER_NUM_ELECTRICITY_FOR_PLAYER = 24;

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.layer == LAYER_NUM_ELECTRICITY_FOR_PLAYER)
            DeadByElectricity();
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Radar")
            Invoke("Dead", 0.1f);
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
        Invoke("HidePlayer", 0.7f);
        SoundManager.instance.PlayPlayerSound(PlayerSounds.PLAYER_HIT);

        animator.enabled = true;
        Invoke("StartHitAnimation", 0.1f);

        if (onPlayerDead != null)
            onPlayerDead.Invoke();

        playerMove.SetCanMove(false);
        playerAttack.SetCanShoot(false);

        Time.timeScale = 0.4f;


    }


    void DeadByElectricity()
    {
        if (isDead || StageManager.instance.stageState == StageState.CLEAR)
            return;

        isDead = true;

        electricityExplosion.SetActive(true);
        Invoke("HideElectricityExplosion", 0.5f);
        Invoke("HidePlayer", 0.5f);
        SoundManager.instance.PlayPlayerSound(PlayerSounds.PLAYER_HIT);

        animator.enabled = true;

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

    void HideElectricityExplosion()
    {
        electricityExplosion.SetActive(false);
    }

    void HidePlayer()
    {
        this.gameObject.SetActive(false);
    }

    void StartHitAnimation()
    {
        animator.SetBool("isHit", true);
    }

    #endregion

    
}

