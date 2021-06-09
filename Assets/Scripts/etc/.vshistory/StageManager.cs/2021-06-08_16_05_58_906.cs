using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    StageStage stageState;

    enum StageStage
    {
        CLEAR,
        UNCLEAR
    }

    private void Start()
    {
        stageState = StageStage.UNCLEAR;    
    }

}
