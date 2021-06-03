using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMap : MonoBehaviour
{
    Camera _minimapCamera;
    Vector3 _MMCPos; // 미니맵 카메라 포지션
    public bool isVerticalMoveNeeded;
    public Transform player;
    public float leftLimX; //카메라 왼쪽 제한 x 좌표 
    public float rightLimX; //카메라 오른쪽 제한 x 좌표 
    public float topLimY; //카메라 위쪽 제한 y 좌표 
    public float botLimY; //카메라 아래쪽 제한 y 좌표 

    void Start()
    {
        _minimapCamera = GetComponent<Camera>();
        _MMCPos = _minimapCamera.gameObject.transform.position;
    }

    void Update()
    {
        FollowPlayer();
    }

    private void FollowPlayer()
    {
        Vector3 playerPos = player.position;

        if (isVerticalMoveNeeded)
        {
            if (playerPos.x < leftLimX && playerPos.y > topLimY)
                _MMCPos.Set(leftLimX, topLimY, transform.position.z);
            else if (playerPos.x < leftLimX && playerPos.y < botLimY)
                _MMCPos.Set(leftLimX, botLimY, transform.position.z);
            else if (playerPos.x > rightLimX && playerPos.y > topLimY)
                _MMCPos.Set(rightLimX, topLimY, transform.position.z);
            else if (playerPos.x > rightLimX && playerPos.y < botLimY)
                _MMCPos.Set(rightLimX, botLimY, transform.position.z);
            else if (playerPos.x < leftLimX)
                _MMCPos.Set(leftLimX, playerPos.y, transform.position.z);
            else if (playerPos.x > rightLimX)
                _MMCPos.Set(rightLimX, playerPos.y, transform.position.z);
            else if (playerPos.y > topLimY)
                _MMCPos.Set(playerPos.x, topLimY, transform.position.z);
            else if (playerPos.y < botLimY)
                _MMCPos.Set(playerPos.x, botLimY, transform.position.z);
            else
                _MMCPos.Set(player.position.x, player.position.y, transform.position.z);

            _minimapCamera.transform.SetPositionAndRotation(_MMCPos, Quaternion.identity);
        }
        else
        {
            if (playerPos.x < leftLimX)
            {
                _MMCPos.Set(leftLimX, _MMCPos.y, transform.position.z);
            }
            else if (playerPos.x > rightLimX)
            {
                _MMCPos.Set(rightLimX, _MMCPos.y, transform.position.z);
            }
            else
            {
                _MMCPos.Set(player.position.x, _MMCPos.y, transform.position.z);
            }
            _minimapCamera.transform.SetPositionAndRotation(_MMCPos, Quaternion.identity);
        }
    }
}
