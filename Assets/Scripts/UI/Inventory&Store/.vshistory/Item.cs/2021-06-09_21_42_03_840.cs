using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    Bow,
    Consumables,
    Etc
}

[System.Serializable]
public class Item
{
    public ItemType itemType;
    public string itemName;
    public Sprite itemImage;
    public string itemInfo;
    public string itemAbilityInfo;
    public bool hasSkill;
    public string skillName;
    public string skillInfo;
    public Sprite skillImage;
    public int itemSellingPrice;
    public int itemBuyingPrice;


    public bool Equip()
    {
        return false;
    }

    public bool Use()
    {
        return false;
    }
}
