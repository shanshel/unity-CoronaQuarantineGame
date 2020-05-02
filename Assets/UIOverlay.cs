using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIOverlay : MonoBehaviour
{
    public static List<UIOverlay> overlays = new List<UIOverlay> { };
    public EnumsData.UIOverlay overlayId;
    public Tweener hideTween, showTween;
    public RectTransform _rectTransform;

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();

        hideTween = transform.DOScale(0, .4f).SetAutoKill(false).SetId(overlayId.ToString() + "_hide").OnComplete(() => { onHideComplete(); });
        showTween = transform.DOScale(1f, .3f).SetAutoKill(false).SetId(overlayId.ToString() + "_show").OnComplete(() => { onShowComplete(); });
        overlays.Add(this);
    }


    public static void show(EnumsData.UIOverlay overLayenum)
    {
        foreach (var overlay in overlays) { 
            if (overLayenum == overlay.overlayId)
            {
                overlay.transform.localScale = Vector3.zero;
                overlay._rectTransform.anchoredPosition = new Vector2(0f, 0f);
                overlay.gameObject.SetActive(true);
                overlay.showTween.Restart();
                return;
            }
        }
    }

    public static void hide(EnumsData.UIOverlay overLayenum)
    {
        foreach (var overlay in overlays)
        {
            if (overLayenum == overlay.overlayId)
            {
                overlay.hideTween.Restart();
                return;
            }
        }
    }

    void onHideComplete()
    {

    }

    void onShowComplete()
    {

    }
}
