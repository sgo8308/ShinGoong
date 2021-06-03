using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BombShot : MonoBehaviour
{

    public static bool _bombShot_State;    

    void Start()
    {


        _bombShot_State = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (!_bombShot_State)
            {
                GetComponent<RawImage>().enabled = true;
                _bombShot_State = true;
            }

            else if (_bombShot_State)
            {
                GetComponent<RawImage>().enabled = false;
                _bombShot_State = false;
            }

        }
    }
}
