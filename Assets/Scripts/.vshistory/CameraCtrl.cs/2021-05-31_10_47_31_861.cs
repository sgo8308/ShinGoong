using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCtrl : MonoBehaviour
{
    void Start()
    {
        
    }

    void Update()
    {
        ZoomOut();        //왼쪽 shift키를 누르고 있으면 카메라 줌 아웃이 된다.
    }

    private void ZoomOut()
    {
        if (Input.GetKey(KeyCode.LeftShift))

        {            

            GetComponent<CinemachineVirtualCamera>().m_Lens.OrthographicSize = 9;

        }

        else

        {           

            GetComponent<CinemachineVirtualCamera>().m_Lens.OrthographicSize = 6;

        }

    }
}
