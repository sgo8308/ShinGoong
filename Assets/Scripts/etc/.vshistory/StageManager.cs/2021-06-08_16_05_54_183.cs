using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    public StageStage stageState;

    public enum StageStage
    {
        CLEAR,
        UNCLEAR
    }

    private void Start()
    {
        stageState = StageStage.UNCLEAR;    
    }

}
