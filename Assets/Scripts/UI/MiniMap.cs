using Cinemachine;
using UnityEngine;

public class MiniMap : MonoBehaviour
{
    Camera minimapCamera;
    Vector3 MMCPos; // 미니맵 카메라 포지션
    public bool isVerticalMoveNeeded;
    public Transform player;
    public float leftLimX; //카메라 왼쪽 제한 x 좌표 
    public float rightLimX; //카메라 오른쪽 제한 x 좌표 
    public float topLimY; //카메라 위쪽 제한 y 좌표 
    public float botLimY; //카메라 아래쪽 제한 y 좌표 

    void Start()
    {
        DontDestroyOnLoad(gameObject);
        minimapCamera = GetComponent<Camera>();
        MMCPos = minimapCamera.gameObject.transform.position;
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
                MMCPos.Set(leftLimX, topLimY, transform.position.z);
            else if (playerPos.x < leftLimX && playerPos.y < botLimY)
                MMCPos.Set(leftLimX, botLimY, transform.position.z);
            else if (playerPos.x > rightLimX && playerPos.y > topLimY)
                MMCPos.Set(rightLimX, topLimY, transform.position.z);
            else if (playerPos.x > rightLimX && playerPos.y < botLimY)
                MMCPos.Set(rightLimX, botLimY, transform.position.z);
            else if (playerPos.x < leftLimX)
                MMCPos.Set(leftLimX, playerPos.y, transform.position.z);
            else if (playerPos.x > rightLimX)
                MMCPos.Set(rightLimX, playerPos.y, transform.position.z);
            else if (playerPos.y > topLimY)
                MMCPos.Set(playerPos.x, topLimY, transform.position.z);
            else if (playerPos.y < botLimY)
                MMCPos.Set(playerPos.x, botLimY, transform.position.z);
            else
                MMCPos.Set(player.position.x, player.position.y, transform.position.z);

            minimapCamera.transform.SetPositionAndRotation(MMCPos, Quaternion.identity);
        }
        else
        {
            if (playerPos.x < leftLimX)
                MMCPos.Set(leftLimX, MMCPos.y, transform.position.z);
            else if (playerPos.x > rightLimX)
                MMCPos.Set(rightLimX, MMCPos.y, transform.position.z);
            else
                MMCPos.Set(player.position.x, MMCPos.y, transform.position.z);

            minimapCamera.transform.SetPositionAndRotation(MMCPos, Quaternion.identity);
        }
    }

    
}
