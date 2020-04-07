using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PostProccessEffects : MonoBehaviour
{
  
    Volume postProccessVolume;


    Vignette _vignette;
    float vignetteIntensityTarget, vignetteSmothnessTarget, _vignetteTransSpeed;
    private void Awake()
    {
        postProccessVolume = GetComponent<Volume>();
        postProccessVolume.profile.TryGet(typeof(Vignette), out _vignette);

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

    private void Update()
    {
        //TestTrans

        testEffects();
        if (!Mathf.Equals(_vignette.intensity.value, vignetteIntensityTarget))
        {
            _vignette.intensity.value = Mathf.MoveTowards(_vignette.intensity.value, vignetteIntensityTarget, Time.deltaTime * _vignetteTransSpeed);
        }
        if (!Mathf.Equals(_vignette.smoothness.value, vignetteSmothnessTarget))
        {
            _vignette.smoothness.value = Mathf.MoveTowards(_vignette.smoothness.value, vignetteSmothnessTarget, Time.deltaTime * _vignetteTransSpeed);
        }
    }

    void testEffects()
    {
        if (Time.time > 5f)
        {
            setVignetteSlowly(1f, 1f, .5f);
        }
    }
}
