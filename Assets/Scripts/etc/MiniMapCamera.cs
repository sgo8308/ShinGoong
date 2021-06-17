using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MiniMapCamera : MonoBehaviour
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

    void AssignCameraConfiner(Scene scene, LoadSceneMode mode)
    {
        CinemachineConfiner cameraConfiner = transform.Find("MiniMapVCam").GetComponent<CinemachineConfiner>();
        GameObject camConfiner = GameObject.Find("CamConfiner");
        cameraConfiner.m_BoundingShape2D = camConfiner.GetComponent<PolygonCollider2D>();
    }
}
