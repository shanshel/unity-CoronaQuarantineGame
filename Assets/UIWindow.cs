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
    public static bool ignoreNextOnHideCompleteEvent;
    //public static WindowEnum lastTransToenum;
    public static bool isFiristInitDone;
    public bool isMainForThisScene;
    public static List<WindowEnum> mainWindows = new List<WindowEnum>();
 
    private void Start()
    {
        hideTween = transform.DOScale(0, .4f).SetAutoKill(false).SetId(window.ToString() + "_hide").OnComplete(() => { UIWindow.onHideComplete(); });
        showTween = transform.DOScale(1f, .3f).SetAutoKill(false).SetId(window.ToString() + "_show").OnComplete(() => { UIWindow.onShowComplete(); });
        idleTween = transform.DOMove(new Vector3(10f, 10f, 0f), 20f).SetId(window.ToString() + "_idle").SetAutoKill(false);
        _rectTransform = GetComponent<RectTransform>();


        UIWindow.windows.Add(this);
        if (isMainForThisScene && !mainWindows.Contains(window))
        {
            UIWindow.mainWindows.Add(window);
        }
        if (mainWindows.Contains(window))
        {
            currentWindow = this;
            targetWindow = this;
            if (!isFiristInitDone)
            {
                gameObject.SetActive(true);
            } else
            {
                gameObject.SetActive(false);
            }
        } else
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
        if (ignoreNextOnHideCompleteEvent)
        {
            return;
        }
        targetWindow.show();
        currentWindow = targetWindow;
        
    }

    public static void onShowComplete()
    {

    }

    public static void beforeTransLoadNewScene()
    {
        UIWindow.windows.Clear();
        windows = new List<UIWindow>();
    }
    public static void transWaitForNewSceneComplete()
    {
        ignoreNextOnHideCompleteEvent = false;
        backTargetWindow = null;
        targetWindow.show();
    }
    public static void transTo(WindowEnum transToenum, SceneEnum newScene = SceneEnum.None)
    {
       
        isFiristInitDone = true;
        if (newScene != SceneEnum.None)
        {
            ignoreNextOnHideCompleteEvent = true;
            currentWindow.hide();
            //lastTransToenum = transToenum;
            CustomSceneLoader._inst.loadScene(newScene);
        }
        else
        {
            foreach (var win in UIWindow.windows)
            {
                if (win.window == transToenum)
                {
                    targetWindow = win;
                }
            }
            SoundManager._inst.playSoundOnce(SoundEnum.UISwipe);
            currentWindow.hide();
        }
 
    }

   



    public static void back()
    {
  
        
        if (backTargetWindow == null || mainWindows.Contains(currentWindow.window))
        {
            transTo(WindowEnum.AnyFirstWindow, SceneEnum.PrevScene);
        }
        else
        {
            targetWindow = backTargetWindow;
            currentWindow.hide();
        }
       
    }

    public void onBackEvent()
    {
        UIManager._inst.back();
    }
}
