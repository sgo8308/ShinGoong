using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemToolTipUI : MonoBehaviour
{
    TextMeshProUGUI _levelLimit;
    TextMeshProUGUI _itemName;
    Image _itemImage;
    TextMeshProUGUI _itemAbility;
    TextMeshProUGUI _itemInfo;
    Image _skillImage;
    TextMeshProUGUI _skillName;
    TextMeshProUGUI _skillInfo;
    TextMeshProUGUI _price;
    TextMeshProUGUI _rightClickInfo;

    private void Start()
    {
        _levelLimit = transform.Find("LevelLimit")
                               .Find("LevelLimitText")
                               .GetComponent<TextMeshProUGUI>();

        _itemName = transform.Find("Name")
                             .GetComponent<TextMeshProUGUI>();

        _itemImage = transform.Find("ItemImage&Ability")
                              .Find("ItemImage")
                              .GetComponent<Image>();

        _itemAbility = transform.Find("ItemImage&Ability")
                                .Find("Ability")
                                .GetComponent<TextMeshProUGUI>();

        _itemInfo = transform.Find("ItemInfo")
                             .GetComponent<TextMeshProUGUI>();

        _skillImage = transform.Find("Skill")
                               .Find("SkillImage")
                               .GetComponent<Image>();

        _skillName = transform.Find("Skill")
                              .Find("Name&Info")
                              .Find("Name").GetComponent<TextMeshProUGUI>();

        _skillInfo = transform.Find("Skill")
                              .Find("Name&Info")
                              .Find("Info").GetComponent<TextMeshProUGUI>();

        _price = transform.Find("Price")
                          .Find("CoinCount")
                          .GetComponent<TextMeshProUGUI>();

        _rightClickInfo = transform.Find("RightClick")
                                   .Find("RightClickInfo")
                                   .GetComponent<TextMeshProUGUI>();

        gameObject.SetActive(false);
    }
    
    public void SetInventoryItemToolTipInfo(SlotType slotType, Item item, bool isStoreActive)
    {
        switch (slotType)
        {
            case SlotType.Inventory:
                SetInventoryItemInfo(item, isStoreActive);
                break;

            case SlotType.Equipped:
                SetEquippedItemInfo(item, isStoreActive);
                break;

            default:
                break;
        }
    }

    public void SetStoreItemInfo(Item item)
    {
        SetImmutableInfo(item);

        _price.text = item.priceInStore.ToString();
        _rightClickInfo.text = "BUY";
    }

    private void SetInventoryItemInfo(Item item, bool isStoreActive)
    {
        SetImmutableInfo(item);

        _price.text = item.priceInInventory.ToString();

        if (isStoreActive)
            _rightClickInfo.text = "SELL";
        else
            _rightClickInfo.text = "EQUIP";
    }

    private void SetEquippedItemInfo(Item item, bool isStoreActive)
    {
        SetImmutableInfo(item);

        _price.text = item.priceInInventory.ToString();

        if (isStoreActive)
            _rightClickInfo.text = "SELL";
        else
            _rightClickInfo.text = "UNEQUIP";
    }

    private void SetImmutableInfo(Item item)
    {
        if (item.hasLevelLimit)
        {
            _levelLimit.transform.parent.gameObject.SetActive(true);

            _levelLimit.text = item.levelLimit.ToString();
        }
        else
        {
            _levelLimit.transform.parent.gameObject.SetActive(false);
        }

        _itemName.text = item.itemName;
        _itemImage.sprite = item.itemImage;
        _itemAbility.text = item.itemAbilityInfo;
        _itemInfo.text = item.itemInfo;

        if (item.hasSkill)
        {
            _skillImage.transform.parent.gameObject.SetActive(true);

            _skillImage.sprite = item.skillImage;
            _skillName.text = item.skillName;
            _skillInfo.text = item.skillInfo;
        }
        else
        {
            _skillImage.transform.parent.gameObject.SetActive(false);
        }
    }
}
