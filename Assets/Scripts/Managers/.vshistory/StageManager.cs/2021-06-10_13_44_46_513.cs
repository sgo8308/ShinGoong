
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

    float _playTime;

    Stopwatch _stopWatch;

    Player _player;
    private void Start()
    {
        stageState = StageState.UNCLEAR;

        _player = GameObject.Find("Player").GetComponent<Player>();
        _player.onPlayerDead += StopTime;

        _stopWatch = new Stopwatch();
        _stopWatch.Start();
    }

    private void Update()
    {
        Debug.Log(_stopWatch.Elapsed);

        TimeSpan ts = _stopWatch.Elapsed;
        string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}",
            ts.Hours, ts.Minutes, ts.Seconds);
    }

    void StopTime()
    {
        _stopWatch.Stop();
    }

    public string GetPlayTime()
    {

        //플레이 타임 시간 분 초로 계산해서 주기
        return "";
    }

}
