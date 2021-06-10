﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameOverUI : MonoBehaviour
{
    Player _player;
    StageManager _stageManager;
    TextMeshProUGUI _playTime;
    TextMeshProUGUI _level;

    private void Awake()
    {
        var obj = FindObjectsOfType<GameOverUI>();

        if (obj.Length == 1)
        {
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        UnityEngine.SceneManagement.SceneManager.sceneLoaded += RegisterOnPlayerDead;
        UnityEngine.SceneManagement.SceneManager.sceneLoaded += setUIinActive;

        _stageManager = StageManager.instance;

        _playTime = transform.Find("GameOverPanel")
                             .Find("PlayTime")
                             .Find("PlayTimeText")
                             .GetComponent<TextMeshProUGUI>();

        _level = transform.Find("GameOverPanel")
                          .Find("Level")
                          .Find("LevelText")
                          .GetComponent<TextMeshProUGUI>();
    }

    void RegisterOnPlayerDead(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "ShelterScene")
            return;

        _player = GameObject.Find("Player").GetComponent<Player>();
        _player.onPlayerDead += ShowGameOverUI;
        _player.onPlayerDead += SetPlayTime;
    }

    void SetPlayTime()
    {
        _playTime.text = _stageManager.GetPlayTime();
    }

    void ShowGameOverUI()
    {
        transform.Find("GameOverPanel").gameObject.SetActive(true);
    }

    void UpdateExperienceUI()
    {

    }

    void UpdateLevelUI()
    { 
    
    }

    void setUIinActive(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "ShelterScene")
            this.gameObject.SetActive(false);
    }
}
