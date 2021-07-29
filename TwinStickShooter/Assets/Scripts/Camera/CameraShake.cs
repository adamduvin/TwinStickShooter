using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraShake : MonoBehaviour
{
    public static CameraShake Instance { get; set; }

    private CinemachineVirtualCamera virtualCam;
    CinemachineBasicMultiChannelPerlin multiChannelPerlin;
    private float shakeTimer;

    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        virtualCam = GetComponent<CinemachineVirtualCamera>();
        multiChannelPerlin = virtualCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        shakeTimer = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        if(shakeTimer > 0f)
        {
            shakeTimer -= Time.deltaTime;

            if(shakeTimer <= 0f)
            {
                multiChannelPerlin.m_AmplitudeGain = 0f;
                multiChannelPerlin.m_FrequencyGain = 0f;
            }
        }
    }

    public void ShakeCamera(float amplitude, float frequency, float duration)
    {
        multiChannelPerlin.m_AmplitudeGain = amplitude;
        multiChannelPerlin.m_FrequencyGain = frequency;
        shakeTimer = duration;
    }

    public float GetCurrentShakeAmplitude()
    {
        return multiChannelPerlin.m_AmplitudeGain;
    }
}
