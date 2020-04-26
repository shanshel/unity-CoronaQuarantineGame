using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class RoomController : MonoBehaviourPunCallbacks
{


    

    private void Start()
    {
        //Equal to On Join Room 
        PhotonNetwork.IsMessageQueueRunning = true;
        UIRoomCanvas._inst.updateRoomInfo(PhotonNetwork.CurrentRoom);
    }



    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        UIRoomCanvas._inst.updateRoomInfo(PhotonNetwork.CurrentRoom);
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        UIRoomCanvas._inst.updateRoomInfo(PhotonNetwork.CurrentRoom);
    }





}
