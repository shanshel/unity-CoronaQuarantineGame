using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class HeadHealthBar : MonoBehaviour
{
    public Slider slider;
    public Image fill;
    public Gradient gradient;

    public void setHealth(float currentHealth, float maxHealth)
    {
        slider.maxValue = maxHealth;
        slider.value = currentHealth;

        fill.color = gradient.Evaluate(slider.normalizedValue);
    }

    private void Update()
    {
        fill.color = gradient.Evaluate(slider.normalizedValue);
    }

    public void Shake()
    {
        transform.DOShakePosition(.3f, 2f).SetAutoKill(true).Play();
    }

}
