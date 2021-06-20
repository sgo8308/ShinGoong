using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemToolTip : MonoBehaviour
{
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

        itemToolTip.transform.Find("Skill")
                             .Find("Name&Info")
                             .Find("Name").GetComponent<TextMeshProUGUI>().text = item.skillName;

        itemToolTip.transform.Find("Skill")
                             .Find("Name&Info")
                             .Find("Info").GetComponent<TextMeshProUGUI>().text = item.skillInfo;

        if (isInStore)
        {
            itemToolTip.transform.Find("Price")
                             .Find("CoinCount")
                             .GetComponent<TextMeshProUGUI>()
                             .text = item.itemBuyingPrice.ToString();

            itemToolTip.transform.Find("HowToEquipOrBuyOrSell")
                             .Find("EquipOrBuyOrSellText")
                             .GetComponent<TextMeshProUGUI>()
                             .text = "BUY";
            return;
        }

        if (isWithStore)
        {
            itemToolTip.transform.Find("Price")
                             .Find("CoinCount")
                             .GetComponent<TextMeshProUGUI>()
                             .text = item.itemSellingPrice.ToString();

            itemToolTip.transform.Find("HowToEquipOrBuyOrSell")
                             .Find("EquipOrBuyOrSellText")
                             .GetComponent<TextMeshProUGUI>()
                             .text = "SELL";
            return;
        }

        //Without store
        itemToolTip.transform.Find("Price")
                             .Find("CoinCount")
                             .GetComponent<TextMeshProUGUI>()
                             .text = item.itemSellingPrice.ToString();

        if (isInEquippedSlot)
        {
            itemToolTip.transform.Find("HowToEquipOrBuyOrSell")
                             .Find("EquipOrBuyOrSellText")
                             .GetComponent<TextMeshProUGUI>()
                             .text = "UNEQUIP";
            return;
        }

        itemToolTip.transform.Find("HowToEquipOrBuyOrSell")
                             .Find("EquipOrBuyOrSellText")
                             .GetComponent<TextMeshProUGUI>()
                             .text = "EQUIP";
    }

    void SetItemInfo()
    {

    }

    void ShowToolTip()
    {

    }

}
