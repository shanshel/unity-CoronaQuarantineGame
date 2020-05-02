using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameManager : MonoBehaviour
{
    private static InGameManager _instance;
    public static InGameManager _inst { get { return _instance; } }

    public CinemachineVirtualCamera cinemaCamera;
    public Camera mainCamera;

    public Vector3 screenMousePosition, worldMousePosition;
    private void Awake()
    {

        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
            mainCamera = Camera.main;
        }
    }


    private void Start()
    {
        SoundManager._inst.stopAllMusic();

    }

    private void Update()
    {
        screenMousePosition = Input.mousePosition;
        worldMousePosition = mainCamera.ScreenToWorldPoint(screenMousePosition);
    }

    public void setCameraFollow(Transform follow)
    {
        
        cinemaCamera.transform.position = follow.transform.position;
        cinemaCamera.Follow = follow;
        cinemaCamera.LookAt = follow;
    }

    int deadFollowIndexCamera = 0;
    public void setDeadFollowNexT()
    {
        cinemaCamera.Follow = null;
        cinemaCamera.LookAt = null;
        Transform[] trans = new Transform[4];
        int i = 0;
        foreach (var player in NetworkPlayers._inst.playerList)
        {
            if (player.Value._thisPlayerTeam == NetworkPlayers._inst._localCPlayer._thisPlayerTeam)
            {
                if (player.Value == NetworkPlayers._inst._localCPlayer) continue;
                trans[i] = player.Value.transform;
                i++;
            }
        }
        
        if (deadFollowIndexCamera > trans.Length)
        {
            deadFollowIndexCamera = 0;
        }

        if (trans[deadFollowIndexCamera] == null) return;
        cinemaCamera.transform.position = trans[deadFollowIndexCamera].position;

        cinemaCamera.Follow = trans[deadFollowIndexCamera];
        cinemaCamera.LookAt = trans[deadFollowIndexCamera];
    }
}
