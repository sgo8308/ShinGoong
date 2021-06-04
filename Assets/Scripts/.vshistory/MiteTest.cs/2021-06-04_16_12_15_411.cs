using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiteTest : Monster
{
    protected override void Awake()
    {
        base.Awake();
        Debug.Log("나는 아들이야 1!");
    }
}
