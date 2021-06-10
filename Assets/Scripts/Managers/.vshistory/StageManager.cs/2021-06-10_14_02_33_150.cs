﻿
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
    public StageState stageState;

    int _monsterCount;

    string _playTime;

    Stopwatch _stopWatch;

    Player _player;
    private void Start()
    {
        stageState = StageState.UNCLEAR;

        _player = GameObject.Find("Player").GetComponent<Player>();
        _player.onPlayerDead += StopStopWatch;
        SceneManager sceneManager = SceneManager.instance;
        sceneManager.onSceneLoad += StartStopWatch;

        _stopWatch = new Stopwatch();
        _stopWatch.Start();
    }
    //씬매니저에서 새로운 씬이 로딩되면 알람 받고 만약 쉘터면 스톱워치 중단.
    //스테이지인데 스탑워치가 실행중라면 리턴
    // 스탑워치가 실행중이 아니라면 시작.

    void StartStopWatch(string sceneName)
    {

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

}
