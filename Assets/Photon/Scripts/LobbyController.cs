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



    public List<RoomInfo> roomList = new List<RoomInfo>();
    public void createRoom(string roomName, bool isPublic, int maxPlayerCount)
    {
        RoomOptions roomOption = new RoomOptions();
        roomOption.MaxPlayers = (byte)maxPlayerCount;
        roomOption.IsVisible = isPublic;
        roomOption.IsOpen = true;

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


    /* PUN Callbacks */
    public override void OnConnectedToMaster()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.NickName = "Player" + Random.Range(1, 10000).ToString();
        PhotonNetwork.JoinLobby();
    }
    public override void OnJoinedLobby()
    {
        canJoinRoom = true;
 
    }
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
 
        createRoom(lastRoomName, lastIsPublic, lastMaxPlayerCount);
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        this.roomList = roomList;
        UILobbyCanvas._inst.updateServerList(this.roomList);
    }

    public override void OnJoinedRoom()
    {
       

        if (PhotonNetwork.IsMasterClient)
        {
       
            PhotonNetwork.IsMessageQueueRunning = false;
            UIWindow.transTo(EnumsData.WindowEnum.AnyFirstWindow, EnumsData.SceneEnum.Room);
            //PhotonNetwork.LoadLevel(inRoomSceneIndex);
        }
    }

}
