using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
using Photon.Realtime;

public class PopupManager : MonoBehaviour
{
    private static PopupManager _instance;
    public static PopupManager _inst { get { return _instance; } }
    [SerializeField]
    GameObject warningBackObject, errorBackObject, popupWindow, background, agreeButton;
    [SerializeField]
    TextMeshProUGUI titleMeshPro, descMeshPro;
    [SerializeField]
    TMP_InputField _input;
    public Tweener hideTween, showTween;

    EnumsData.popupStatus poppupStatus = EnumsData.popupStatus.idle; 

    System.Action<string> currentActiveCallback;

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
        _input.gameObject.SetActive(false);
        agreeButton.SetActive(false);
        show();
    }

    public void showWarning(string title, string desc)
    {
   
        warningBackObject.SetActive(true);
        errorBackObject.SetActive(false);
        titleMeshPro.text = title;
        descMeshPro.text = desc;
        _input.gameObject.SetActive(false);
        agreeButton.SetActive(false);
        show();
    }



    public void showInputPopup(string title, string desc, System.Action<string> callback)
    {
        warningBackObject.SetActive(true);
        errorBackObject.SetActive(false);
        titleMeshPro.text = title;
        descMeshPro.text = desc;
        _input.gameObject.SetActive(true);
        agreeButton.SetActive(true);
        currentActiveCallback = callback;
        show();
    }

    void show()
    {
        StartCoroutine(showPopup());
    }

    IEnumerator showPopup()
    {
        while(poppupStatus == EnumsData.popupStatus.hiding)
        {
            yield return null;
        }
        popupWindow.transform.localScale = Vector3.zero;
        popupWindow.SetActive(true);
        showTween.Restart();
        SoundManager._inst.playSoundOnce(EnumsData.SoundEnum.WarningMessage);
        background.SetActive(true);
    }
    public void onOkClicked()
    {
        poppupStatus = EnumsData.popupStatus.hiding;
        hideTween.Restart();
       
    }

    void onHideDone()
    {
        popupWindow.SetActive(false);
        background.SetActive(false);
        poppupStatus = EnumsData.popupStatus.idle;

    }

    public void onAgreeClicked()
    {
        poppupStatus = EnumsData.popupStatus.hiding;
        currentActiveCallback(_input.text);
        hideTween.Restart();
    }


}
