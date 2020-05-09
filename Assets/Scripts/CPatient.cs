using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CPatient : CPlayer
{
    public Transform baseMouthBone;
    private void Awake()
    {
        _thisPlayerTeam = EnumsData.Team.Patients;
    }


    public override void overableUpdate()
    {
        baseMouthBone.rotation = lookQuaternion;
    }
}
