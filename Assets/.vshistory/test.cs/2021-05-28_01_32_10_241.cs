﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    float yAxis;
    private void Start()
    {
        yAxis = transform.position.y;
    }
    void Update()
    {
        transform.position.y = yAxis;
    }
}
