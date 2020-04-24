using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnumsData
{
    public enum SFXEnum
    {
        dash, rotate, scaleUp, scaleDown, pass, hited, spawnObst,
        monsterLaugh, lose, gain, clap, firework, wallTouchedTop,
    }

    public enum WindowEnum
    {
       Main, CustomizeOption, DoctorCustomizition, PatientCustomizition, ServerList, InRoom

    }

    public enum PopupEnum
    {
        ServerCreate, ServerInfo, Info, Error
    }

}

