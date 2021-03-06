using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
/// <summary>
/// 플레이어가 몬스터를 공격할 때 화면을 흔들어주는 스크립트.
/// </summary>
public class CameraShake : MonoBehaviour
{
    public float shakeDuration = 0.3f;
    public float shakeAmplitude = 1f;
    public float shakeFrequency = 1f;
    public float shakeElapsedTime = 0f;

    public CinemachineVirtualCamera virtualCamera;
    private CinemachineBasicMultiChannelPerlin virtualCameraNoise;

    private void Start()
    {
        if (virtualCamera != null)
            virtualCameraNoise = virtualCamera.GetCinemachineComponent<Cinemachine.CinemachineBasicMultiChannelPerlin>();
    }

    private void Update()
    {
        if (virtualCamera != null || virtualCameraNoise != null)
        {
            if (shakeElapsedTime > 0)
            {
                virtualCameraNoise.m_AmplitudeGain = shakeAmplitude;
                virtualCameraNoise.m_FrequencyGain = shakeFrequency;

                shakeElapsedTime -= Time.deltaTime;
            }
            else
            {
                virtualCameraNoise.m_AmplitudeGain = 0f;
                shakeElapsedTime = 0f;
            }
        }
    }

    public void StartShake() 
    {
        shakeElapsedTime = shakeDuration;
    }
}
