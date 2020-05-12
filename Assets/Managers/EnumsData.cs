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
        NeedleHit,
        Surprise,
        pickUpSound,
        takeHealth,
        takeDamage,
        Gassing,
        StartGassing,
        PatientKillDoctor,
        PlayerCollidWithWall,
        Hatshu,
        WhileWaitForRespawn,
        DoctorTakeDamage,
        PatientTakeDamage,
        DoctorDie, 
        PatientDie,
        Step1A,Step2A, Step1B, Step2B

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

    public enum UIOverlay
    {
        dead
    }
    public enum SceneEnum
    {
        MainMenu, Lobby, InGame, Room, None, PrevScene,
    }

    public enum popupStatus
    {
        hiding, idle
    }

    public enum Team
    {
        Doctors, Patients, Both
    }

    public enum VisbilityTeam
    {
        None, Doctors, Patients
    }
    public enum playerStatus
    {
        alive, dead, respawining
    }
}

