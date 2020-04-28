using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class RoomController : MonoBehaviourPunCallbacks
{


    private static RoomController _instance;
    public static RoomController _inst { get { return _instance; } }

    public float timer = 20f;
    public int intTimer;
    public bool isReady = false;
    bool isMatchStarted;
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
        //Equal to On Join Room 
        PhotonNetwork.IsMessageQueueRunning = true;
        UIRoomCanvas._inst.updateRoomInfo(PhotonNetwork.CurrentRoom);
    }

    private void Update()
    {
        if (isReady && timer > 0f)
        {
            timer -= Time.deltaTime;
            var localIntTimer = Mathf.FloorToInt(timer);
            if (localIntTimer != intTimer)
            {
                UIRoomCanvas._inst.updateRoomTimer(intTimer);
                intTimer = localIntTimer;
                if (intTimer == 0)
                {
                    startGameMatch();
                }
            }
        }
            
    }
    public void onStartEvent()
    {
        if (PhotonNetwork.IsMasterClient && !isReady)
        {
            var hash = new ExitGames.Client.Photon.Hashtable();
            hash.Add("IsReady", "YES");
            PhotonNetwork.CurrentRoom.SetCustomProperties(hash);
        }
    }
  
    void updateRoomLockStatusBasedOnPlayerCount()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            if (PhotonNetwork.CurrentRoom.PlayerCount == PhotonNetwork.CurrentRoom.MaxPlayers)
            {
                PhotonNetwork.CurrentRoom.IsVisible = false;
                PhotonNetwork.CurrentRoom.IsOpen = false;
            }
            else
            {
                PhotonNetwork.CurrentRoom.IsVisible = true;
                PhotonNetwork.CurrentRoom.IsOpen = true;
            }
        }

    }


    public override void OnRoomPropertiesUpdate(ExitGames.Client.Photon.Hashtable propertiesThatChanged)
    {
        UIRoomCanvas._inst.updateRoomInfo(PhotonNetwork.CurrentRoom);
        if (!isReady)
        {
            if (propertiesThatChanged.ContainsKey("IsReady"))
            {
                var localIsReady = (string)propertiesThatChanged["IsReady"];
                if (localIsReady == "YES")
                {
                    isReady = true;
                }
            }
        }
    }
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        
        UIRoomCanvas._inst.updateRoomInfo(PhotonNetwork.CurrentRoom);
        SoundManager._inst.playSoundOnce(EnumsData.SoundEnum.PlayerJoin);
        updateRoomLockStatusBasedOnPlayerCount();
        
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        UIRoomCanvas._inst.updateRoomInfo(PhotonNetwork.CurrentRoom);
        SoundManager._inst.playSoundOnce(EnumsData.SoundEnum.PlayerLeave);
        updateRoomLockStatusBasedOnPlayerCount();
       
    }

    public override void OnLeftRoom()
    {
        PhotonNetwork.IsMessageQueueRunning = false;
        UIWindow.transTo(EnumsData.WindowEnum.AnyFirstWindow, EnumsData.SceneEnum.Lobby);
    }

 
    
    public void startGameMatch()
    {
       
        if (!isMatchStarted)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                PhotonNetwork.CurrentRoom.IsVisible = false;
                PhotonNetwork.CurrentRoom.IsOpen = false;
            }
            isMatchStarted = true;
            UIWindow.transTo(EnumsData.WindowEnum.AnyFirstWindow, EnumsData.SceneEnum.InGame);
        }
    }



}
