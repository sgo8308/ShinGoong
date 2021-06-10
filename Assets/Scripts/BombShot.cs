using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BombShot : MonoBehaviour
{

    public static bool bombShot_State;    

    void Start()
    {


        bombShot_State = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (!bombShot_State)
            {
                GetComponent<RawImage>().enabled = true;
                bombShot_State = true;
            }

            else if (bombShot_State)
            {
                GetComponent<RawImage>().enabled = false;
                bombShot_State = false;
            }

        }
    }
}
