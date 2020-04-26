using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class NetworkController : MonoBehaviourPunCallbacks
{
    // Start is called before the first frame update

    void Start()
    {
        OnConnectedToServer();
    }

    public void OnConnectedToServer()
    {
        PhotonNetwork.ConnectUsingSettings();
    }
    
 

}
