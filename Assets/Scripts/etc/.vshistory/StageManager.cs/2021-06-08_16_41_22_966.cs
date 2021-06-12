﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StageState
{
    CLEAR,
    UNCLEAR
}

public class StageManager : MonoBehaviour
{
    public StageStage stageState;

    int monsterCount;
    float playingTime;


    private void Start()
    {
        stageState = StageStage.UNCLEAR;    
    }

}