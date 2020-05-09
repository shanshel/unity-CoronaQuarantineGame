using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static EnumsData;

public class CDoctor : CPlayer
{
    

    private void Awake()
    {
        _thisPlayerTeam = EnumsData.Team.Doctors;
    }


}
