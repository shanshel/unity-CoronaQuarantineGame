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
    }


 


 
}
