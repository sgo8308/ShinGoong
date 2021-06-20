using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverUI : MonoBehaviour
{
    Player _player;
    StageManager _stageManager;



    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        _player.onPlayerDead += ShowGameOverUI;

        _stageManager = GameObject.Find("StageManager").GetComponent<StageManager>();
    }

    void Update()
    {
        
    }

    void SetPlayTime()
    {
        
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
