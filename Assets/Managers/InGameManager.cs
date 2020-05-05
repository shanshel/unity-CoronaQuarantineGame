using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameManager : MonoBehaviour
{
    private static InGameManager _instance;
    public static InGameManager _inst { get { return _instance; } }

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
        SoundManager._inst.stopAllMusic();
    }


    public void setCameraFollow(Transform follow)
    {

        ScreenManager._inst.setCameraFollow(follow);
    }


    public void setDeadFollowNexT()
    {
        ScreenManager._inst.setDeadFollowNexT();
    }
}
