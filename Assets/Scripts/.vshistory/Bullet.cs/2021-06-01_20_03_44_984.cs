﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    Vector3 _direction;
    bool _isDirectionChecked;
    public float speed;
    void Start()
    {
        
    }

    //void Update()
    //{
    //    if (!_isDirectionChecked)
    //    {
    //        GameObject.Find("Mon")
    //        _isDirectionChecked = true;
    //    }
    //    transform.Translate(direction * speed * Time.deltaTime);
    //}
}