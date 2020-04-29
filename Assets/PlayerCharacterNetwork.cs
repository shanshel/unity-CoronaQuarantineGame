using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static EnumsData;

public class PlayerCharacterNetwork : MonoBehaviour, IPunObservable
{

    public Player _player;
    public Team _team;
    public PhotonView _photonView;

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
   
        throw new System.NotImplementedException();
    }
}
