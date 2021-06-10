
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

    SceneManager _sceneManager;

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

        _stopWatch = new Stopwatch();
    }
    //씬매니저에서 새로운 씬이 로딩되면 알람 받고 만약 쉘터면 스톱워치 중단.
    //스테이지인데 스탑워치가 실행중라면 리턴
    // 스탑워치가 실행중이 아니라면 시작.

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
    public void InitializeStore()
    {
        GameObject.Find("StoreNpc").GetComponent<Store>().inventoryPanel = inventoryPanel;
        GameObject.Find("StoreNpc").GetComponent<Store>().storePanel = storePanel;
    } 

    #endregion
}
