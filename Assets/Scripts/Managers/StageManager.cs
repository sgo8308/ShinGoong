
using UnityEngine;
using System.Diagnostics;
using Debug = UnityEngine.Debug;
using System;
using UnityEngine.SceneManagement;

public enum StageState
{
    CLEAR,
    UNCLEAR
}

public class StageManager : MonoBehaviour
{
    public static StageManager instance;

    public StageState stageState;

    int _monsterCount;

    Stopwatch _stopWatch;

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

    private void Start()
    {
        UnityEngine.SceneManagement.SceneManager.sceneLoaded += StartStopWatch;
        UnityEngine.SceneManagement.SceneManager.sceneLoaded += InitializeStageState;
        UnityEngine.SceneManagement.SceneManager.sceneLoaded += RegisterOnPlayerDead;
        UnityEngine.SceneManagement.SceneManager.sceneLoaded += ResetStopWatch;

        _stopWatch = new Stopwatch();
    }

    void StartStopWatch(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "ShelterScene")
            return;

        if (_stopWatch.IsRunning)
            return;

        _stopWatch.Start();
    }

    void RegisterOnPlayerDead(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "ShelterScene")
            return;

        Player _player = GameObject.Find("Player").GetComponent<Player>();
        _player.onPlayerDead += StopStopWatch;
    }

    void StopStopWatch()
    {
        _stopWatch.Stop();
    }

    void ResetStopWatch(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "ShelterScene")
            _stopWatch.Reset();
    }

    public string GetPlayTime()
    {
        TimeSpan ts = _stopWatch.Elapsed;
        string playTime = String.Format("{0:00}:{1:00}:{2:00}",
            ts.Hours, ts.Minutes, ts.Seconds);

        return playTime;
    }

    public void InitializeStageState(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "ShelterScene")
            stageState = StageState.CLEAR;
        else
            stageState = StageState.UNCLEAR;
    }

    public void UpdateStageState()
    {
        //몬스터 다 잡으면 클리어로 변경
    }


    #region Shelter
   
    public GameObject inventoryPanel;
    public GameObject storePanel;
    
    void InitializeShelter()
    {
        InitializeStore();

    }

    public void InitializeStore()
    {
        GameObject.Find("StoreNpc").GetComponent<Store>().inventoryPanel = inventoryPanel;
        GameObject.Find("StoreNpc").GetComponent<Store>().storePanel = storePanel;
    }
    
     

    #endregion
}
