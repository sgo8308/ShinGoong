using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StageState
{
    CLEAR,
    UNCLEAR
}

public class StageManager : MonoBehaviour
{
    public StageState stageState;

    int _monsterCount;

    float _playingTime;


    private void Start()
    {
        stageState = StageState.UNCLEAR;    
    }

}
