using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
/// <summary>
/// 플레이어가 몬스터를 공격할 때 화면을 흔들어주는 스크립트.
/// </summary>
public class CameraShake : MonoBehaviour
{
    public float ShakeAmount;
    float ShakeTime;
    public float time;
    Vector3 initialPosition;

    public void VibrateForTime(float time)
    {
        ShakeAmount = time;
    }


    private void Start()
    {
        initialPosition = Camera.main.transform.position;
    }

    private void Update()
    {
        if (ShakeTime > 0)
        {
            Camera.main.transform.position = Random.insideUnitSphere * ShakeAmount + initialPosition;
            ShakeTime -= Time.deltaTime;
        }
        else
        {
            ShakeTime = 0.0f;
            Camera.main.transform.position = initialPosition;
        }
    }

    public void StartShake() 
    {
        ShakeTime = time;
    }
}
