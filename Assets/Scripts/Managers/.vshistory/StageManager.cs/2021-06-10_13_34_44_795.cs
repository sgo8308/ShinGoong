
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

    Player _player;
    private void Start()
    {
        stageState = StageState.UNCLEAR;

        _player = GameObject.Find("Player").GetComponent<Player>();
        _player.onPlayerDead += StopTime;

        _stopWatch = new Stopwatch();
        _stopWatch.Start();
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
