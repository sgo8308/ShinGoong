﻿
using UnityEngine;
using System.Diagnostics;
using Debug = UnityEngine.Debug;

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

    Player _player
    private void Start()
    {
        stageState = StageState.UNCLEAR;

        _stopWatch = new Stopwatch();
        _stopWatch.Start();
    }
    

}