using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class RoomController : MonoBehaviourPunCallbacks
{
    public int maxPlayerCount;
    public List<RoomInfo> roomList = new List<RoomInfo>();
    public string currentRoomName;
    string lastRoomName;
    bool lastIsPublic;


    public void createRoom(string roomName, bool isPublic)
    {
        RoomOptions roomOption = new RoomOptions();
        roomOption.MaxPlayers = (byte)maxPlayerCount;
        roomOption.IsVisible = isPublic;
        roomOption.IsOpen = true;

        lastRoomName = roomName;
        lastIsPublic = isPublic;
        currentRoomName = roomName + getRandomRoomNumber();
        PhotonNetwork.CreateRoom(currentRoomName, roomOption);
    }

    public string getRandomRoomNumber()
    {
        return Random.Range(0, 10000).ToString();
    }

    /* Callbacks */
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        this.roomList = roomList;
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log("RoomCreatedFailed");
        createRoom(lastRoomName, lastIsPublic);
    }

 

}
