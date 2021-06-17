using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        Debug.Log("겟레벨업카운트");
        return temp;
    }

    public float CalculateNowExpPercent()
    {
        Debug.Log(nowExpPoint + " " + maxExpPoint);
        Debug.Log((float)(nowExpPoint / maxExpPoint));
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
        level++;
    }

    private void OnApplicationQuit()
    {
        levelUpCount = 0;
    }
}
