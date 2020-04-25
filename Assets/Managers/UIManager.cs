using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;
using static EnumsData;

public class UIManager : MonoBehaviour
{
    private static UIManager _instance;
    public static UIManager _inst { get { return _instance; } }

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

    public void back()
    {
        SoundManager._inst.playSoundOnce(SoundEnum.UIBack);
        UIWindow.back();
    }



}
