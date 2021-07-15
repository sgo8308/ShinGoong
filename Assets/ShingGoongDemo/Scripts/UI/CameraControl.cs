using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraControl : MonoBehaviour
{
    Camera mainCam;
    Vector3 mainCamPos; // 미니맵 카메라 포지션
    public Transform player;
    public float leftLimX; //카메라 왼쪽 제한 x 좌표 
    public float rightLimX; //카메라 오른쪽 제한 x 좌표 
    public float topLimY; //카메라 위쪽 제한 y 좌표 
    public float botLimY; //카메라 아래쪽 제한 y 좌표 

    void Start()
    {
        UnityEngine.SceneManagement.SceneManager.sceneLoaded += GetCameraLimit;
        GetCameraLimit();
        mainCam = Camera.main;
        mainCamPos = mainCam.transform.position;
    }

    void LateUpdate()
    {
        FollowPlayer();
    }

    private void FollowPlayer()
    {
        Vector3 playerPos = player.position;

        if (playerPos.x < leftLimX && playerPos.y > topLimY) 
            mainCamPos.Set(leftLimX, topLimY, transform.position.z);
        else if (playerPos.x < leftLimX && playerPos.y < botLimY)
            mainCamPos.Set(leftLimX, botLimY, transform.position.z);
        else if (playerPos.x > rightLimX && playerPos.y > topLimY)
            mainCamPos.Set(rightLimX, topLimY, transform.position.z);
        else if (playerPos.x > rightLimX && playerPos.y < botLimY)
            mainCamPos.Set(rightLimX, botLimY, transform.position.z);
        else if (playerPos.x < leftLimX)
            mainCamPos.Set(leftLimX, playerPos.y, transform.position.z);
        else if (playerPos.x > rightLimX)
            mainCamPos.Set(rightLimX, playerPos.y, transform.position.z);
        else if (playerPos.y > topLimY)
            mainCamPos.Set(playerPos.x, topLimY, transform.position.z);
        else if (playerPos.y < botLimY)
            mainCamPos.Set(playerPos.x, botLimY, transform.position.z);
        else
            mainCamPos.Set(player.position.x, player.position.y, transform.position.z);

        mainCam.transform.SetPositionAndRotation(mainCamPos, Quaternion.identity);
    }

    void GetCameraLimit(Scene scene, LoadSceneMode mode)
    {
        CameraLimit cl = GameObject.Find("CameraLimit").GetComponent<CameraLimit>();
        leftLimX = cl.leftLimitXAxis;
        rightLimX = cl.rightLimitXAxis; 
        topLimY = cl.topLimitYAxis; 
        botLimY = cl.bottomLimitYAxis;  
    }

    void GetCameraLimit()
    {
        CameraLimit cl = GameObject.Find("CameraLimit").GetComponent<CameraLimit>();
        leftLimX = cl.leftLimitXAxis;
        rightLimX = cl.rightLimitXAxis;
        topLimY = cl.topLimitYAxis;
        botLimY = cl.bottomLimitYAxis;
    }
}
