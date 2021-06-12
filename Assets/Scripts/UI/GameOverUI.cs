using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameOverUI : MonoBehaviour
{
    StageManager stageManager;
    TextMeshProUGUI playTime;
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
        UnityEngine.SceneManagement.SceneManager.sceneLoaded += HideGameOverUI;

        stageManager = StageManager.instance;

        playTime = transform.Find("GameOverPanel")
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

        Player player = GameObject.Find("Player").GetComponent<Player>();
        player.onPlayerDead += ShowGameOverUI;
        player.onPlayerDead += SetPlayTime;
    }

    void SetPlayTime()
    {
        playTime.text = stageManager.GetPlayTime();
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

    void HideGameOverUI(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "ShelterScene")
            transform.Find("GameOverPanel").gameObject.SetActive(false);
    }
}
