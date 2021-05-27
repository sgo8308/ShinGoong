using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCtrl : MonoBehaviour
{

    public Camera m_Camera;
    public Transform m_Player;
    public float camPosXLimitLeft;
    public float camPosXLimitRight;
    public float camPosYLimitTop;
    public float camPosYLimitBot;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        FollowPlayer();
    }

    private void FollowPlayer()
    {
        Vector3 position = m_Player.position;
        //카메라 최대 X좌표 제한두기
        if (m_Player.position.x < camPosXLimitLeft)
        {
            position.Set(camPosXLimitLeft, m_Player.position.y, transform.position.z);
        }
        else if (camPosXLimitLeft <= m_Player.position.x && m_Player.position.x <= camPosXLimitRight)
        {
            position.Set(m_Player.position.x, m_Player.position.y, transform.position.z);
        }
        else if (m_Player.position.x > camPosXLimitRight)
        {
            position.Set(camPosXLimitRight, m_Player.position.y, transform.position.z);
        }
        //카메라 최대 Y좌표 제한두기


        //왼쪽 제한, 아래 제한
        //왼쪽 제한, 위에 제한
        //왼쪽만 제한
        //오른쪽 제한, 아래 제한
        //오른쪽 제한, 위에 제한
        //오른쪽만 제한
        if (m_Player.position.y < camPosYLimitBot)
        {
            position.Set(camPosXLimitLeft, m_Player.position.y, transform.position.z);
        }
        else if (camPosXLimitLeft <= m_Player.position.x && m_Player.position.x <= camPosXLimitRight)
        {
            position.Set(m_Player.position.x, m_Player.position.y, transform.position.z);
        }
        else if (m_Player.position.x > camPosXLimitRight)
        {
            position.Set(camPosXLimitRight, m_Player.position.y, transform.position.z);
        }

        m_Camera.transform.SetPositionAndRotation(position, Quaternion.identity);
    }
}
