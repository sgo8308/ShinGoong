using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TentuPlay.Api;

public class PlayerInfo : MonoBehaviour
{
    public static PlayerInfo instance;

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

    public float maxSpeed;
    public float jumpPower;
    public int level;

    public int levelUpCount = 0;
    public float maxExpPoint = 20.0f;
    public float nowExpPoint = 0;

    public int GetLevelUpCount()
    {
        int temp = levelUpCount;
        levelUpCount = 0;
        return temp;
    }

    public float CalculateNowExpPercent()
    {
        return (float)(nowExpPoint / maxExpPoint);
    }

    public void AddExpPoint(Monster monster)
    {
        nowExpPoint += monster.expPoint;

        if (nowExpPoint >= maxExpPoint)
        {
            while (true)
            {
                if (nowExpPoint >= maxExpPoint)
                {
                    LevelUp();
                    levelUpCount++;
                    nowExpPoint -= maxExpPoint;
                    maxExpPoint += 30;
                    continue;
                }
                break;
            }
        }
    }

    private void LevelUp()
    {
        int new_level = level + 1;

        string player_uuid = "TentuPlayer"; // player_uuid can be anything that uniquely identifies each of your game user.
        string character_uuid = TentuPlayKeyword._DUMMY_CHARACTER_ID_;

        Debug.Log("레벨업 들어옴");
        new TPStashEvent().LevelUpCharacter(
            player_uuid: player_uuid, // unique identifier of player
            character_uuid: character_uuid,
            level_from: level,  // leveled up from
            level_to: new_level  // leveled up to 
            );

        this.level = new_level;
    }

    private void OnApplicationQuit()
    {
        levelUpCount = 0;
    }
}
