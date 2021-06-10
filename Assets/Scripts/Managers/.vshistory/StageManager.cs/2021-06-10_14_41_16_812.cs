
using UnityEngine;
using System.Diagnostics;
using Debug = UnityEngine.Debug;
using System;

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
        _sceneManager = SceneManager.instance;
        _sceneManager.onSceneLoad += StartStopWatch;
        _sceneManager.onSceneLoad += InitializeStageState;
        _sceneManager.onSceneLoad += ConnectPlayerDead;

        _stopWatch = new Stopwatch();
    }
    //씬매니저에서 새로운 씬이 로딩되면 알람 받고 만약 쉘터면 스톱워치 중단.
    //스테이지인데 스탑워치가 실행중라면 리턴
    // 스탑워치가 실행중이 아니라면 시작.

    void StartStopWatch(string sceneName)
    {
        if (sceneName == "ShelterScene")
            return;

        if (_stopWatch.IsRunning)
            return;

        _stopWatch.Start();
    }

    void ConnectPlayerDead(string sceneName)
    {
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

    public void InitializeStageState(string sceneName)
    {
        if (sceneName == "ShelterScene")
            stageState = StageState.CLEAR;
        else
            stageState = StageState.UNCLEAR;
    }

    public void UpdateStageState()
    {
        //몬스터 다 잡으면 클리어로 변경
    }
}
