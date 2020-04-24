using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;
public class LobbyController : MonoBehaviourPunCallbacks
{
    // Start is called before the first frame update
    public bool canJoinRoom;


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





}
