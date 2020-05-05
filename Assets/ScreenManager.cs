using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using DG.Tweening;
using Cinemachine;

public class ScreenManager : MonoBehaviour
{
    private static ScreenManager _instance;
    public static ScreenManager _inst { get { return _instance; } }

    //Camera 
 

    Volume postProccessVolume;


    Vignette _vignette;
    float vignetteIntensityTarget, vignetteSmothnessTarget, _vignetteTransSpeed;

    public Camera mainCamera;
    public CinemachineVirtualCamera cinemaCamera;
    private CinemachineBasicMultiChannelPerlin virtualCameraNoise;


    float __m_FrequencyGain, __shakeTime;
    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
            mainCamera = Camera.main;
            postProccessVolume = GetComponent<Volume>();
            postProccessVolume.profile.TryGet(typeof(Vignette), out _vignette);
        }
    }

    private void Start()
    {
        virtualCameraNoise = cinemaCamera.GetCinemachineComponent<Cinemachine.CinemachineBasicMultiChannelPerlin>();
        __m_FrequencyGain = virtualCameraNoise.m_FrequencyGain;
    }

    public void quickShake()
    {
        __shakeTime = .5f;
    }
    public void setVignette(float intensity, float smoothness)
    {
        _vignette.intensity.value = intensity;
        _vignette.smoothness.value = smoothness;
    }

    public void setVignetteSlowly(float intensity, float smoothness, float transSpeed)
    {
        vignetteIntensityTarget = intensity;
        vignetteSmothnessTarget = smoothness;
        _vignetteTransSpeed = transSpeed;

    }
    public Vector3 screenMousePosition, worldMousePosition;
    private void Update()
    {
        //Mouse follow 
        screenMousePosition = Input.mousePosition;
        worldMousePosition = mainCamera.ScreenToWorldPoint(screenMousePosition);


        //Camera Shake 
        cameraShakeUpdate();

        if (!Mathf.Equals(_vignette.intensity.value, vignetteIntensityTarget))
        {
            _vignette.intensity.value = Mathf.MoveTowards(_vignette.intensity.value, vignetteIntensityTarget, Time.deltaTime * _vignetteTransSpeed);
        }
        if (!Mathf.Equals(_vignette.smoothness.value, vignetteSmothnessTarget))
        {
            _vignette.smoothness.value = Mathf.MoveTowards(_vignette.smoothness.value, vignetteSmothnessTarget, Time.deltaTime * _vignetteTransSpeed);
        }
    }


    void cameraShakeUpdate()
    {
        if (virtualCameraNoise != null)
        {
            // If Camera Shake effect is still playing
            if (__shakeTime > 0)
            {
                Debug.Log("shakkkekkeeke");
                // Set Cinemachine Camera Noise parameters
                virtualCameraNoise.m_FrequencyGain = 7f;
                // Update Shake Timer
                __shakeTime -= Time.deltaTime;
            }
            else
            {
                // If Camera Shake effect is over, reset variables
                virtualCameraNoise.m_FrequencyGain = __m_FrequencyGain;
            }
        }
    }
    void testEffects()
    {
        if (Time.time > 5f)
        {
            setVignetteSlowly(1f, 1f, .5f);
        }
    }

    //Camera Follow 
    public void setCameraFollow(Transform follow)
    {

        cinemaCamera.transform.position = follow.transform.position;
        cinemaCamera.Follow = follow;
        cinemaCamera.LookAt = follow;
    }

    int deadFollowIndexCamera = 0;
    public void setDeadFollowNexT()
    {
        cinemaCamera.Follow = null;
        cinemaCamera.LookAt = null;
        Transform[] trans = new Transform[4];
        int i = 0;
        foreach (var player in NetworkPlayers._inst.playerList)
        {
            if (player.Value._thisPlayerTeam == NetworkPlayers._inst._localCPlayer._thisPlayerTeam)
            {
                if (player.Value == NetworkPlayers._inst._localCPlayer) continue;
                trans[i] = player.Value.transform;
                i++;
            }
        }

        if (deadFollowIndexCamera > trans.Length)
        {
            deadFollowIndexCamera = 0;
        }

        if (trans[deadFollowIndexCamera] == null) return;
        cinemaCamera.transform.position = trans[deadFollowIndexCamera].position;

        cinemaCamera.Follow = trans[deadFollowIndexCamera];
        cinemaCamera.LookAt = trans[deadFollowIndexCamera];
    }
}
