﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMap : MonoBehaviour
{
    public Transform _player;
    Camera _minimapCamera;
    Vector3 MMCPos; // 미니맵 카메라 포지션
    public float leftLimX; //카메라 왼쪽 제한 x 좌표 
    public float rightLimX; //카메라 오른쪽 제한 x 좌표 
    public float topLimY; //카메라 위쪽 제한 y 좌표 
    public float botLimY; //카메라 아래쪽 제한 y 좌표 

    void Start()
    {
        _minimapCamera = GetComponent<Camera>();
        MMCPos = _minimapCamera.gameObject.transform.position;
    }

    void Update()
    {
        FollowPlayer();

        //1. 왼쪽 제한 초과 아래제한 초과
        //1. 왼쪽 제한 초과 위 제한 초과
        //2. 왼쪽 제한만 초과
        //4. 오른쪽 제한만 초과
        //4. 오른쪽 제한 초과 아래 제한 초과
        //4. 오른쪽 제한 초과 위 제한 초과
        //3. 위 제한만 초과
        //3. 아래 제한만 초과
        //3. 아무것도 초과하지 않음

        //if (MMCPos.x > leftLimX)
        //{
        //    MMCPos
        //}
    }

    private void FollowPlayer()
    {
        MMCPos = m_Player.position;

        if ()
        {

        }

        //Vector3 position = m_Player.position;
        //position.Set(m_Player.position.x, m_Player.position.y, transform.position.z);

        _minimapCamera.transform.SetPositionAndRotation(MMCPos, Quaternion.identity);
    }
}