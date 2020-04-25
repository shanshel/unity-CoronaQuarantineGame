using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static EnumsData;

public class UILobbyCanvas : MonoBehaviour
{



    public void onJoinRoomButtonClicked()
    {
        UIWindow.transTo(WindowEnum.InRoom);
    }

    public void onRoomCountCompleteTest()
    {
        UIWindow.transTo(WindowEnum.InGame, SceneEnum.InGame);
    }
}
