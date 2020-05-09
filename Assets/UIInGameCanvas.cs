using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
public class UIInGameCanvas : MonoBehaviour
{
    private static UIInGameCanvas _instance;
    public static UIInGameCanvas _inst { get { return _instance; } }

    public Slider uiHealthSlider, uiInflictedSlider;
    public TextMeshProUGUI TimerText, InflicedText;
    

    Tweener uiHealthIncreaseTween, uiHealthDecreaseTween;

    private void Awake()
    {
        
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    private void Start()
    {
        uiHealthIncreaseTween = uiHealthSlider.transform.DOShakeScale(.5f, .4f, 4, 45).SetAutoKill(false).SetId("uiHealth_Increase");
        uiHealthDecreaseTween = uiHealthSlider.transform.DOShakeScale(.5f, .4f, 9, 70).SetAutoKill(false).SetId("uiHealth_Decrease");
    }

    public void healthIncrease(int newAmount, int maxValue)
    {
        uiHealthSlider.maxValue = maxValue;
        uiHealthSlider.value = newAmount;
        uiHealthIncreaseTween.Restart();
        SoundManager._inst.playSoundOnce(EnumsData.SoundEnum.takeHealth);
        Debug.Log(uiHealthSlider.value);
    }

    public void healthDecrease(int newAmount, int maxValue)
    {
        uiHealthSlider.maxValue = maxValue;
        uiHealthSlider.value = newAmount;
        uiHealthDecreaseTween.Restart();
        Debug.Log(uiHealthSlider.value);
        SoundManager._inst.playSoundOnce(EnumsData.SoundEnum.takeDamage);
        ScreenManager._inst.quickShake();
    }

    public void healthUpdate(int newAmount, int maxValue)
    {
        uiHealthSlider.maxValue = maxValue;
        uiHealthSlider.value = newAmount;
    }

    public void setTime(float time)
    {
        int minutes = (int)time / 60;
        int seconds = (int)time - 60 * minutes;

        TimerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public void setInflected(int inflected)
    {
        uiInflictedSlider.value = inflected;
        InflicedText.text = inflected.ToString() + " Inflicted" ;
    }
}
