﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class SharedGameManager : MonoBehaviour
{
    private static SharedGameManager _instance;
    public static SharedGameManager _inst { get { return _instance; } }


    private void Awake()
    {

        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            DontDestroyOnLoad(this.gameObject);
            _instance = this;
            DOTween.defaultAutoPlay = AutoPlay.None;
            Application.targetFrameRate = 60;
            Cursor.visible = false;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        SoundManager._inst.playMusic(EnumsData.SoundEnum.MenuMusic);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
