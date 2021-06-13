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
    public ItemTag itemTag;
    public string itemName;
    public Sprite itemImage;
    public string itemInfo;
    public string itemAbilityInfo;
    public bool hasSkill;
    public string skillName;
    public string skillInfo;
    public Sprite skillImage;
    public int priceInInventory;
    public int priceInStore;
    public int levelLimit;
    public bool hasLevelLimit;
}
