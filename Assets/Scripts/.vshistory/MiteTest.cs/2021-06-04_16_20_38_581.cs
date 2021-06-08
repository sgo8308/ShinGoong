using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiteTest : Monster
{
    protected override void Awake()
    {
        base.Awake();

    }

    protected override void Initialize()
    {
        _speed = 1;
        _hp = 100;
        _defensivePower = 0;

        Invoke("Think", 5);
    }
}
