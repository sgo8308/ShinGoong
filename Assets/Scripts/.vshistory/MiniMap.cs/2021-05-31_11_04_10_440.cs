﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minimap : MonoBehaviour
{
    Camera minimapCamera;
    Vector3 MMCPos; // 미니맵 카메라 포지션

    void Start()
    {
        minimapCamera = GetComponent<Camera>();
    }

    void Update()
    {
        Vector3 position = m_Player.position;
        position.Set(m_Player.position.x, m_Player.position.y, transform.position.z);

        //1. 왼쪽 제한 초과 아래제한 초과
        //1. 왼쪽 제한 초과 위 제한 초과
        //2. 왼쪽 제한만 초과
        //4. 오른쪽 제한만 초과
        //4. 오른쪽 제한 초과 아래 제한 초과
        //4. 오른쪽 제한 초과 위 제한 초과
        //3. 위 제한만 초과
        //3. 아래 제한만 초과
        //3. 아무것도 초과하지 않음

        if (mmcPos)
        {

        }
    }
}
