using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static EnumsData;

public class UIMenuCanvas : MonoBehaviour
{
    public void onCustomizePatientClicked()
    {
        UIWindow.transTo(WindowEnum.PatientCustomizition);
    }
    public void onCustomizeDoctorClicked()
    {
        UIWindow.transTo(WindowEnum.DoctorCustomizition);
    }

    public void onStartButtonClicked()
    {
        UIWindow.transTo(WindowEnum.ServerList, SceneEnum.Lobby);
        //CustomSceneLoader._inst.loadScene(SceneEnum.InGame);
    }

    public void showTestError()
    {
        PopupManager._inst.showError("Connection Error", "Please Make Sure That You Are Connected To The Internet");
    }
}
