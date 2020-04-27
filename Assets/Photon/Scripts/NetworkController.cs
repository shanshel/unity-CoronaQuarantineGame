using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class NetworkController : MonoBehaviourPunCallbacks
{
    // Start is called before the first frame update

    void Start()
    {
        PhotonNetwork.IsMessageQueueRunning = true;
        ConnectedToServer();
    }

    public void ConnectedToServer()
    {
        PhotonNetwork.ConnectUsingSettings();
    }


    public override void OnDisconnected(DisconnectCause cause)
    {
        PhotonNetwork.IsMessageQueueRunning = false;
        UIWindow.transTo(EnumsData.WindowEnum.AnyFirstWindow, EnumsData.SceneEnum.MainMenu);
    }

}
