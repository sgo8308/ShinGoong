﻿using System.Collections;
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
    }

    void SetItemInfo()
    {

    }

    void ShowToolTip()
    {

    }

}
