﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class GameOverUI : MonoBehaviour
{
    public static GameOverUI instance;

    Player _player;
    StageManager _stageManager;
    TextMeshProUGUI _playTime;
    TextMeshProUGUI _level;

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

    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        _player.onPlayerDead += ShowGameOverUI;
        _player.onPlayerDead += SetPlayTime;

        _stageManager = GameObject.Find("StageManager").GetComponent<StageManager>();
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
}
