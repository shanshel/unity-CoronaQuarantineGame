using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;
public class LobbyController : MonoBehaviourPunCallbacks
{
    // Start is called before the first frame update
    public bool canJoinRoom;
    public int inRoomSceneIndex;

    public string currentRoomName;
    string lastRoomName;
    bool lastIsPublic;
    int lastMaxPlayerCount;
    private static LobbyController _instance;
    public static LobbyController _inst { get { return _instance; } }


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



    public Dictionary<string, RoomInfo> cachedRoomList = new Dictionary<string, RoomInfo>(){};
    public void createRoom(string roomName, bool isPublic, int maxPlayerCount)
    {
      
        RoomOptions roomOption = new RoomOptions();
        roomOption.MaxPlayers = (byte)maxPlayerCount;
        roomOption.IsVisible = true;
        roomOption.IsOpen = true;
        roomOption.CustomRoomPropertiesForLobby = new string[] { "Password" };
        lastRoomName = roomName;
        lastIsPublic = isPublic;
        lastMaxPlayerCount = maxPlayerCount;
        currentRoomName = roomName + getRandomRoomNumber();
        PhotonNetwork.CreateRoom(currentRoomName, roomOption);
    }

    public string getRandomRoomNumber()
    {
        return Random.Range(0, 10000).ToString();
    }


    public void joinRoom(RoomInfo room)
    {
        var hash = new ExitGames.Client.Photon.Hashtable();
        PhotonNetwork.JoinRoom(room.Name);
    }

    /* PUN Callbacks */
    public override void OnConnectedToMaster()
    {
        PhotonNetwork.NickName = "Player" + Random.Range(1, 10000).ToString();
        PhotonNetwork.JoinLobby();
    }
    public override void OnJoinedLobby()
    {
        canJoinRoom = true;
        UILobbyCanvas._inst.removeActionBlocker();
 
    }
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
 
        createRoom(lastRoomName, lastIsPublic, lastMaxPlayerCount);
    }

    
    public override void OnRoomListUpdate(List<RoomInfo> comingRoomList)
    {
        UILobbyCanvas._inst.clearServerList();
        UpdateCachedRoomList(comingRoomList);
        UILobbyCanvas._inst.updateServerList(cachedRoomList);
    }

    private void UpdateCachedRoomList(List<RoomInfo> roomList)
    {
        foreach (RoomInfo info in roomList)
        {
            // Remove room from cached room list if it got closed, became invisible or was marked as removed
            if (!info.IsOpen || !info.IsVisible || info.RemovedFromList)
            {
                if (cachedRoomList.ContainsKey(info.Name))
                {
                    cachedRoomList.Remove(info.Name);
                }

                continue;
            }

            // Update cached room info
            if (cachedRoomList.ContainsKey(info.Name))
            {
                cachedRoomList[info.Name] = info;
            }
            // Add new room info to cache
            else
            {
                cachedRoomList.Add(info.Name, info);
            }
        }
    }
    public override void OnJoinedRoom()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            var customRoomProprties = new ExitGames.Client.Photon.Hashtable();
            if (!lastIsPublic)
            {
                var password = getRandomRoomNumber().ToString();
                customRoomProprties.Add("Password", password);
            }
            customRoomProprties.Add("isReady", "NO");
            Room _localRoom = PhotonNetwork.CurrentRoom;
            _localRoom.SetCustomProperties(customRoomProprties);
        }
        PhotonNetwork.IsMessageQueueRunning = false;
        UIWindow.transTo(EnumsData.WindowEnum.AnyFirstWindow, EnumsData.SceneEnum.Room);
    }


    public override void OnLeftLobby()
    {
        PhotonNetwork.IsMessageQueueRunning = false;
        UIWindow.transTo(EnumsData.WindowEnum.AnyFirstWindow, EnumsData.SceneEnum.MainMenu);
    }

 
}
