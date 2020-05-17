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
    public TextMeshProUGUI TimerText, infectedText, quarantinedText;
    public GameObject stickerCanvas;
    public string[] listOfStickers = new string[5];
    Tweener uiHealthIncreaseTween, uiHealthDecreaseTween;
    TextMeshProUGUI killText, deathText, botInteractText;
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

    public void updateInfectionSlider(int infectionCount, int quarantinedCount)
    {
    
        uiInflictedSlider.maxValue = 50;
     
        uiInflictedSlider.value = 25 + infectionCount - quarantinedCount;
        infectedText.text = infectionCount + " Infected";
        quarantinedText.text = quarantinedCount + " Quarantined";
    }

    public void setLocalPlayerStats(int killsOnPlayers, int killsOnBot, int death)
    {
        killText.text = killsOnPlayers.ToString();
        deathText.text = death.ToString();
        botInteractText.text = killsOnBot.ToString();
    }

    public void openStickerPanel()
    {
        stickerCanvas.SetActive(true);
    }

    public void onStickerSlotClicked(int index)
    {
        NetworkPlayers._inst._localCPlayer.sendSticker(listOfStickers[index]);
        stickerCanvas.SetActive(false);
    }
}
