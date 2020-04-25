using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class PopupManager : MonoBehaviour
{
    private static PopupManager _instance;
    public static PopupManager _inst { get { return _instance; } }
    [SerializeField]
    GameObject warningBackObject, errorBackObject, popupWindow, background;
    [SerializeField]
    TextMeshProUGUI titleMeshPro, descMeshPro;
    public Tweener hideTween, showTween;


    // Start is called before the first frame update
    bool isInTransition;

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
        popupWindow.SetActive(false);
        background.SetActive(false);
        hideTween = popupWindow.transform.DOScale(0, .4f).SetAutoKill(false).SetId("popup_hide").SetEase(Ease.InOutElastic).OnComplete(() => { this.onHideDone(); }); ;
        showTween = popupWindow.transform.DOScale(1f, .3f).SetAutoKill(false).SetId("popup_show").SetEase(Ease.InOutElastic);
    }
    public void showError(string title, string desc)
    {
        warningBackObject.SetActive(false);
        errorBackObject.SetActive(true);
        titleMeshPro.text = title;
        descMeshPro.text = desc;
        show();
    }

    public void showWarning(string title, string desc)
    {
        warningBackObject.SetActive(true);
        errorBackObject.SetActive(false);
        titleMeshPro.text = title;
        descMeshPro.text = desc;
        show();
    }

    void show()
    {
        popupWindow.transform.localScale = Vector3.zero;
        popupWindow.SetActive(true);
        showTween.Restart();
        SoundManager._inst.playSoundOnce(EnumsData.SoundEnum.WarningMessage);
        background.SetActive(true);
    }
    public void onOkClicked()
    {
        hideTween.Restart();
    }

    void onHideDone()
    {
        popupWindow.SetActive(false);
        background.SetActive(false);
    }
}
