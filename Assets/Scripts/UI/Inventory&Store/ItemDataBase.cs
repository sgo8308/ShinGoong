using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum ItemTag
{
    BOMB_BOW
}

public class ItemDataBase : MonoBehaviour
{
    public static ItemDataBase instance;
    
    public List<Item> items = new List<Item>();

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public string GetSkillName(ItemTag itemTag)
    {
        for (int i = 0; i < items.Count; i++)
        {
            if (items[i].itemTag == itemTag && items[i].hasSkill)
            {
                return items[i].skillName;
            }
        }

        return "NoSkill";
    }
}
