using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnumsData
{
    public enum SoundEnum
    {
        UISwipe, 
        UIClick, 
        UIBack, 
        MenuMusic, 
        SceneSwipe, 
        ErrorMessage, 
        WarningMessage,
        PlayerJoin, 
        PlayerLeave,
        DoctorSteps,
        DoctorSteps2,
        NeedleThrow,
        NeedlePop,
        Surprise,
    }

    public enum WindowEnum
    {
       
        Main, 
        CustomizeOption, 
        DoctorCustomizition, 
        PatientCustomizition, 
        ServerList, 
        CreateRoom,
        InRoom, 
        InGame, 
        AnyFirstWindow,
        None,

    }

    public enum PopupEnum
    {
        ServerCreate, ServerInfo, Info, Error
    }

    public enum SceneEnum
    {
        MainMenu, Lobby, InGame, Room, None, PrevScene,
    }

    public enum popupStatus
    {
        hiding, idle
    }

}

