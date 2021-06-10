using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemToolTip : MonoBehaviour
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
    void SetImmutableInfo(Item item)
    {
        _itemName.text = item.itemName;
        _itemImage.sprite = item.itemImage;
        _itemAbility.text = item.itemAbilityInfo;
        _itemInfo.text = item.itemInfo;
        _skillImage.sprite = item.skillImage;
        _skillName.text = item.skillName;
        _skillInfo.text = item.skillInfo;
    }

    public void SetStoreItemInfo(Item item)
    {
        SetImmutableInfo(item);

        _price.text = item.priceInStore.ToString();
        _rightClickInfo.text = "BUY";
    }

    public void SetInventoryItemInfo(Item item, bool isStoreActive)
    {
        SetImmutableInfo(item);

        _price.text = item.priceInInventory.ToString();
        
        if (isStoreActive)
            _rightClickInfo.text = "SELL";
        else
            _rightClickInfo.text = "EQUIP";
    }

    public void SetEquippedItemInfo(Item item, bool isStoreActive)
    {
        SetImmutableInfo(item);

        _price.text = item.priceInInventory.ToString();

        if (isStoreActive)
            _rightClickInfo.text = "SELL";
        else
            _rightClickInfo.text = "UNEQUIP";
    }

    public void ShowToolTip(Vector3 position, SlotType slotType)
    {
        switch (slotType)
        {
            case SlotType.Inventory:
                gameObject.GetComponent<RectTransform>().pivot = new Vector2(1, 0);
                break;

            case SlotType.Equipped:
                gameObject.GetComponent<RectTransform>().pivot = new Vector2(1, 1);
                break;

            case SlotType.StoreTopSide:
                gameObject.GetComponent<RectTransform>().pivot = new Vector2(0, 1);
                break;

            case SlotType.StoreBottomSide:
                gameObject.GetComponent<RectTransform>().pivot = new Vector2(0, 0);
                break;

            default:
                break;
        }

        gameObject.SetActive(true);
        transform.position = position;
    }

    public void HideToolTip()
    {
        gameObject.SetActive(false);
    }
}
