using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class GameOverUI : MonoBehaviour
{
    Player _player;
    StageManager _stageManager;
    SceneManager _sceneManager;
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
        _sceneManager.onSceneLoad += RegisterOnPlayerDead;


        _stageManager = GameObject.Find("StageManager").GetComponent<StageManager>();
    }

    void RegisterOnPlayerDead(string sceneName)
    {
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
}
