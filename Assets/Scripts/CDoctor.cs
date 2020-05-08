using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CDoctor : CPlayer
{
    

    private void Awake()
    {
        _thisPlayerTeam = EnumsData.Team.Doctors;
    }


    protected override void Start()
    {
        base.Start();
    }

    public override void onNetworkPlayerDefine()
    {
        base.onNetworkPlayerDefine();
    }

}
