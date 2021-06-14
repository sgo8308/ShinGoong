using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraCtrl : MonoBehaviour
{
    public PlayerMove playerMove;
    public int maxCamDistance;
    public int defaultCamDistance;

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
}
