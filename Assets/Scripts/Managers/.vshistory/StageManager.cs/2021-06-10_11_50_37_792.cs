
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

    Stopwatch stopWatch;
    private void Start()
    {
        stageState = StageState.UNCLEAR;

        stopWatch = new Stopwatch();
        stopWatch.Start();
    }

    private void Update()
    {
        Debug.Log(" sw    :  " + stopWatch.ElapsedMilliseconds.ToString() + "ms");
    }

}
