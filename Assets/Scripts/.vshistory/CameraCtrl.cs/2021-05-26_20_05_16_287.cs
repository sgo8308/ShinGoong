using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCtrl : MonoBehaviour
{

    public Camera m_Camera;
    public Transform m_Player;
    float camPosXLimitLeft = 27.5f;
    float camPosXLimitRight = 200.0f;
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

        if (m_Player.position.x > 27.5f)
        {
            position.Set(27.5f, m_Player.position.y, transform.position.z);
        }
        else 
        position.Set(m_Player.position.x, m_Player.position.y, transform.position.z);

        m_Camera.transform.SetPositionAndRotation(position, Quaternion.identity);
    }
}
