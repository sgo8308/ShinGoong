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
    public string itemName;
    public Sprite itemImage;

}
