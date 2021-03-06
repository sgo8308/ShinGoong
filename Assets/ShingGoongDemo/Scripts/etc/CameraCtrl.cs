using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// 카메랄를 누를 때 줌 아웃 또는 줌 인 해주는 스크립트.
/// </summary>
public class CameraCtrl : MonoBehaviour
{
    public PlayerMove playerMove;
    public int maxCamDistance;
    public int defaultCamDistance;

    private void Start()
    {
        UnityEngine.SceneManagement.SceneManager.sceneLoaded += RevertCameraView;
    }

    void Update()
    {
        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "ShelterScene")
            return;

        if (playerMove.canMove)
            ZoomOut();   
    }

    private void ZoomOut()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {            
            GetComponent<CinemachineVirtualCamera>().m_Lens.OrthographicSize = maxCamDistance;
        }
        else
        {           
            GetComponent<CinemachineVirtualCamera>().m_Lens.OrthographicSize = defaultCamDistance;
        }
    }

    private void RevertCameraView(Scene scene, LoadSceneMode mode)
    {
        GetComponent<CinemachineVirtualCamera>().m_Lens.OrthographicSize = defaultCamDistance;
    }
}
