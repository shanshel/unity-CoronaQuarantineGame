using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static EnumsData;

public class UIWindow : MonoBehaviour
{
    public static UIWindow currentWindow, backTargetWindow, targetWindow;
    public static List<UIWindow> windows = new List<UIWindow>();

    public WindowEnum window;
    public RectTransform _rectTransform;
    public Tweener hideTween, showTween, idleTween;
    private void Start()
    {
        hideTween = transform.DOScale(0, .4f).SetAutoKill(false).SetId(window.ToString() + "_hide").OnComplete(() => { UIWindow.onHideComplete(); });
        showTween = transform.DOScale(1f, .3f).SetAutoKill(false).SetId(window.ToString() + "_show").OnComplete(() => { UIWindow.onShowComplete(); });
        idleTween = transform.DOMove(new Vector3(10f, 10f, 0f), 20f).SetId(window.ToString() + "_idle").SetAutoKill(false);
        _rectTransform = GetComponent<RectTransform>();
        UIWindow.windows.Add(this);
        if (window == WindowEnum.Main)
        {
            currentWindow = this;
        }
        else
        {
            gameObject.SetActive(false);
        }
            

    }

    public void hide()
    {
        backTargetWindow = currentWindow;
        currentWindow.hideTween.Restart();
    
    }

    public void show()
    {
        targetWindow.transform.localScale = Vector3.zero;
        targetWindow._rectTransform.anchoredPosition = new Vector2(0f, 0f);

        targetWindow.gameObject.SetActive(true);
        targetWindow.showTween.Restart();
    }


 
    public static void onHideComplete()
    {
        targetWindow.show();
        currentWindow = targetWindow;
    }

    public static void onShowComplete()
    {

    }

    public static void transTo(WindowEnum transToenum)
    {
        foreach (var win in UIWindow.windows)
        {
    
            if (win.window == transToenum)
            {
                targetWindow = win;
            }
                
        }

        currentWindow.hide();
    }

    public static void back()
    {
        targetWindow = backTargetWindow;
        currentWindow.hide();
    }
}
