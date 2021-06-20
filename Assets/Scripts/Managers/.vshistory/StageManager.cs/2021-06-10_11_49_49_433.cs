
using UnityEngine;
using System.Diagnostics;

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


    private void Start()
    {
        stageState = StageState.UNCLEAR;

        Stopwatch stopWatch = new Stopwatch();
        stopWatch.Start();
    }

    private void Update()
    {
        Debug.Log(" sw    :  " + stopWatch.ElapsedMilliseconds.ToString() + "ms");
    }

}
