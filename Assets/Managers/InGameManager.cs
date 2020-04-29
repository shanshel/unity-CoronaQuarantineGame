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
        cinemaCamera.Follow = follow;
    }
}
