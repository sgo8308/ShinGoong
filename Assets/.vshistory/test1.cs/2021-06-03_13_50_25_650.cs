﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test1 : MonoBehaviour
{
    void Start()
    {
        GameObject.Find("testChild").GetComponent<TestPa>().hello();

    }

    void Update()
    {
        
    }
}
