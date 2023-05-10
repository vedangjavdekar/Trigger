using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Cinemachine.CinemachineVirtualCamera))]
public class ShakeEffect : MonoBehaviour, IGameStateHandler
{
    Cinemachine.CinemachineVirtualCamera m_VCam = null;
    Cinemachine.CinemachineBasicMultiChannelPerlin m_Noise = null;

    [SerializeField]
    float m_Delay = 0.0f;

    [SerializeField]
    float m_DefaultIntensity = 1.0f;
    [SerializeField]
    float m_DefaultShakeTime = 0.5f;

    public void OnGameStateEnd(GameState state)
    {
    }

    public void OnGameStateStart(GameState state)
    {
        if(state == GameState.GoalReached)
        {
            StartCoroutine(ShakeCamera(m_DefaultIntensity, m_DefaultShakeTime));
        }
    }

    private void Awake()
    {
        m_VCam = GetComponent<Cinemachine.CinemachineVirtualCamera>();
        m_Noise = m_VCam.GetCinemachineComponent<Cinemachine.CinemachineBasicMultiChannelPerlin>();
    }

    public void StartShake(float Intensity,float ShakeTime)
    {
        StartCoroutine(ShakeCamera(Intensity,ShakeTime));
    }

    IEnumerator ShakeCamera(float Intensity, float ShakeTime)
    {
        if (m_Delay > 0.0f)
        {
            yield return new WaitForSeconds(m_Delay);
        }

        m_Noise.m_AmplitudeGain = Intensity;
        float startTime = ShakeTime;

        while(startTime > 0.0f)
        {
            startTime -= Time.deltaTime;
            yield return null;
        }
        
        m_Noise.m_AmplitudeGain = 0;
    }
}
