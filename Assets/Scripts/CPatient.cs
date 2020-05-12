using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static EnumsData;

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

    protected override void overwriteableAttack()
    {
        SoundManager._inst.playSoundOnceAt(EnumsData.SoundEnum.Hatshu, transform.position);
        _animator.SetTrigger("Sneez");
    }
}
