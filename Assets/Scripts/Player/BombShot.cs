﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BombShot : MonoBehaviour
{
    public PlayerMove playerMove;
    public static bool bombShotState;    

    void Start()
    {
        bombShotState = false;
    }

    void Update()
    {
        if (!playerMove.canMove)
            return;

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (!bombShotState)
            {
                GetComponent<RawImage>().enabled = true;
                bombShotState = true;
            }

            else if (bombShotState)
            {
                GetComponent<RawImage>().enabled = false;
                bombShotState = false;
            }
        }
    }
}
