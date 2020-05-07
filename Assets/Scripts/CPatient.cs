using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CPatient : CPlayer
{
    public GameObject bigLightShadowCaster;
    public Transform baseMouthBone;
    private void Awake()
    {
        _thisPlayerTeam = EnumsData.Team.Patients;
    }


    protected override void Start()
    {
        base.Start();
    }

    public override void onNetworkPlayerDefine()
    {
        base.onNetworkPlayerDefine();
        if (base._thisPlayer.IsLocal)
        {
            bigLightShadowCaster.SetActive(true);
        }
    }


    public override void overableUpdate()
    {
        baseMouthBone.rotation = lookQuaternion;
    }
}
