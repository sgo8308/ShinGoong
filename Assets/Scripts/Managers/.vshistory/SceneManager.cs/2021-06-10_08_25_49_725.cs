﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum SceneType
{
    SHELTER,
    STAGE
}

public class SceneManager : MonoBehaviour
{
    public static SceneManager instance;
    public SceneType sceneType;

    public const int STAGE1_ARROW_COUNT = 50;
    public const int SHELTER_ARROW_COUNT = 45;

    public GameObject inventoryPanel;
    public GameObject storePanel;

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

    public void GoTo(string sceneName)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
        InitializeArrowCount(sceneName);

    }

    void InitializeArrowCount(string sceneName)
    {
        switch (sceneName)
        {
            case "ShelterScene":
                MainUI.instance.InitializeArrowCount(SHELTER_ARROW_COUNT);
                sceneType = SceneType.SHELTER;
                break;

            case "Stage1Scene":
                MainUI.instance.InitializeArrowCount(STAGE1_ARROW_COUNT);
                sceneType = SceneType.STAGE;
                break;

            default:
                MainUI.instance.InitializeArrowCount(45);
                break;
        }
    }

    void InitializeStore()
    {
        GameObject.Find("StoreNpc").GetComponent<Store>().inventoryPanel = inventoryPanel;
        GameObject.Find("StoreNpc").GetComponent<Store>().storePanel = storePanel;
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
