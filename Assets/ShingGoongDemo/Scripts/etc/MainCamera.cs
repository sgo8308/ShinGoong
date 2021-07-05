using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.SceneManagement;

public class MainCamera : MonoBehaviour
{
    void Start()
    {
        var obj = FindObjectsOfType<MainCamera>();

        if (obj.Length == 1)
        {
            DontDestroyOnLoad(gameObject);
            UnityEngine.SceneManagement.SceneManager.sceneLoaded += AssignCameraConfiner;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    //맵마다 카메라가 맵 밖을 나가지 못하도록 제한하는데 쓰인 콜라이더를 카메라에 할당해주는 메소드
    void AssignCameraConfiner(Scene scene, LoadSceneMode mode)
    {
        CinemachineConfiner cameraConfiner = transform.Find("MainVCam").GetComponent<CinemachineConfiner>();
        GameObject camConfiner = GameObject.Find("CamConfiner");
        cameraConfiner.m_BoundingShape2D = camConfiner.GetComponent<PolygonCollider2D>();
    }
}
