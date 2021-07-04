using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;
using TentuPlay.Api; // import TentuPlay Api to gather/upload data
using System;
using System.Text;

/// <summary>
/// Component of Player in the scene
/// </summary>
public class TentuPlaySDKPlayerController : MonoBehaviour
{
    public float speed;
    public Text logText;
    public Text winText;
    public Text inventoryText;

    private Rigidbody2D rb2d;

    private string player_uuid = "TentuPlayer"; // player_uuid can be anything that uniquely identifies each of your game user.
    private string character_uuid = TentuPlayKeyword._DUMMY_CHARACTER_ID_; 
    private string[] character_uuids = new string[] { TentuPlayKeyword._DUMMY_CHARACTER_ID_ };

    public string thisStage = "test_STAGE_1_3";
    public string thisStageCategory = "test_MAIN_STORY";
    public string thisStageLevel = "test_1-3";
    private int player_level = 1;
    private float player_currency = 0;
    private Dictionary<string, float> player_inventory = new Dictionary<string, float>();
    private System.Diagnostics.Stopwatch timer;

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();

        // Initialize TentuPlay
        TPStashEvent myStashEvent = new TPStashEvent();

        logText.text = player_uuid + " registered the game.";

        // If you want anything to be seen on the dashboard console, Join method is neccessary.
        myStashEvent.Join(player_uuid: player_uuid); 

        // Calling UploadData after Join method is recommended.
        new TPUploadData().UploadData(false);  

        logText.text = player_uuid + " logged in";
        
        myStashEvent.LoginApp(player_uuid: player_uuid);

        logText.text = logText.text + "\r\n" + player_uuid + " started " + thisStage;
        winText.text = "";

        //also writes stage start
        myStashEvent.PlayStage(
            player_uuid: player_uuid, // unique identifier of player
            character_uuids: character_uuids,
            stage_type: stageType.PvE,
            stage_slug: thisStage, // unique identifier of played stage
            stage_category_slug: thisStageCategory, // category slug of stage (optional)
            stage_level: thisStageLevel, // level of stage (optional)
            stage_score: null, // score is null when stage starts (optional)
            stage_status: stageStatus.Start, // "Start"
            stage_playtime: null // playtime is null when stage starts (optional)
            );
        
        timer = new System.Diagnostics.Stopwatch();
        timer.Start();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        TPStashEvent myStashEvent = new TPStashEvent();

        if (other.gameObject.CompareTag("Currency"))
        {
            logText.text = logText.text + "\r\n" + player_uuid + " got currency " + other.gameObject.name;

            other.gameObject.SetActive(false);
            player_currency += 1.0F;
            SetInventoryText();
            myStashEvent.GetCurrency(
                player_uuid: player_uuid,
                character_uuid: character_uuid,
                currency_slug: other.gameObject.name,
                currency_quantity: 1.0F,
                currency_total_quantity: player_currency,
                from_entity: entity.PlayStage,
                from_category_slug: thisStageCategory,
                from_slug: thisStage
                );
        }
        else if (other.gameObject.CompareTag("Item"))
        {
            logText.text = logText.text + "\r\n" + player_uuid + " got item " + other.gameObject.name;

            other.gameObject.SetActive(false);
            player_inventory.Add(other.gameObject.name, 1.0F);
            SetInventoryText();

            if (other.gameObject.name == "test_BattleAxe")
                myStashEvent.GetEquipment(
                    player_uuid: player_uuid, // unique identifier of player
                    character_uuid: character_uuid,
                    item_slug: other.gameObject.name, // unique identifier of gotten item
                    item_quantity: 1.0F, // quantity of gotten item
                    from_entity: entity.PlayStage,  // i got this item from what category of game elements?
                    from_category_slug: thisStageCategory,
                    from_slug: thisStage  // i got this item from exactly what game element? unique identifier of source of the item
                    ); 
            else if (other.gameObject.name == "test_Potion" || other.gameObject.name == "test_LevelUpScroll")
                myStashEvent.GetConsumable(
                    player_uuid: player_uuid,
                    character_uuid: character_uuid,
                    item_slug: other.gameObject.name,
                    item_quantity: 1.0F,
                    from_entity: entity.PlayStage,
                    from_category_slug: thisStageCategory,
                    from_slug: thisStage
                    ); 

            if (other.gameObject.name == "test_LevelUpScroll")
                LevelUp();
        }

        if (player_currency >= 10)
        {
            WinText();
        }
    }

    void LevelUp()
    {
        int new_level = player_level + 1;
        SetInventoryText();

        logText.text = logText.text + "\r\n" + player_uuid + " leveled up to level " + new_level.ToString();

        new TPStashEvent().LevelUpCharacter(
            player_uuid: player_uuid, // unique identifier of player
            character_uuid: character_uuid,
            level_from: player_level,  // leveled up from
            level_to: new_level  // leveled up to 
            );
     
        this.player_level = new_level;
    }

    void WinText()
    {
        winText.text = "You win!";
        logText.text = logText.text + "\r\n" + player_uuid + " sucessfully completed " + thisStage;

        // if player finished playing a stage / dungeon / run
        new TPStashEvent().PlayStage(
            player_uuid: player_uuid, // unique identifier of player
            character_uuids: character_uuids,
            stage_type: stageType.PvE,
            stage_category_slug: thisStageCategory, // category slug of stage
            stage_slug: thisStage, // unique identifier of played stage
            stage_status: stageStatus.Win, // "Win"
            stage_level: thisStageLevel,
            stage_score: null,
            stage_playtime: (int)timer.ElapsedMilliseconds / 1000
            );
    }

    void SetInventoryText()
    {
        inventoryText.text = "Level: " + player_level.ToString() + "\r\n";
        inventoryText.text += "Currency: " + player_currency.ToString() + "\r\n";

        if (player_inventory != null)
            inventoryText.text += "\r\n" + "Inventory" + "\r\n";

        foreach (KeyValuePair<string, float> inventory_item in player_inventory)
        {
            inventoryText.text = inventoryText.text + inventory_item.Key + ": " + ((int)inventory_item.Value).ToString() + "\r\n";
        }
    }

    void FixedUpdate()
    {
        if (Input.GetMouseButtonDown(0))
        {
            float moveHorizontal = (Input.mousePosition.x - Screen.width * 0.5F) / Screen.width * 10.0F;
            float moveVertical = (Input.mousePosition.y - Screen.height * 0.5F) / Screen.height * 10.0F;
            Vector2 movement = new Vector2(moveHorizontal, moveVertical);
            rb2d.AddForce(movement * speed);
        }
    }

    // In-app purchase. 
    // Assume purchase goldPackage using player_cash, thus increase in player_gold.
    public void BuyFromShop(GameObject other)
    {
        float price = 1000.0F;
        float purchase_quantity = 1.0F;
        float purchase_total_price = purchase_quantity * price;

        logText.text = logText.text + "\r\n" + player_uuid + " purchased in " + other.name;
        player_currency += 10.0F;

        SetInventoryText();
        TPStashEvent myStashEvent = new TPStashEvent();

        myStashEvent.InAppPurchase(
            player_uuid: player_uuid, // unique identifier of player
            character_uuid: character_uuid,
            purchasable_slug: "test_GoldPackageItem001",// unique identifier of bought item
            purchase_quantity: purchase_quantity,
            purchase_unit_price: price,
            purchase_total_price: purchase_total_price,
            purchase_currency_code: currencyCode.USD
            );
        myStashEvent.GetCurrency(
            player_uuid: player_uuid,
            character_uuid: character_uuid,
            currency_slug: "test_Gold",
            currency_quantity: 10.0F,
            currency_total_quantity: player_currency,
            from_entity: entity.InAppPurchase,
            from_category_slug: "test_MAIN_SHOP",
            from_slug: "test_GoldPackageItem001"
            );
    }
}
