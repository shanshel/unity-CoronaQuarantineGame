using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Realtime;

public class UIRoomCanvas : MonoBehaviour
{
    private static UIRoomCanvas _instance;
    public static UIRoomCanvas _inst { get { return _instance; } }


    [SerializeField]
    TextMeshProUGUI roomNameText, roomPasswordText, roomPlayerCountText, roomStatusText;

    [SerializeField]
    Transform playersContainer;

    [SerializeField]
    UIPlayer UIPlayerPrefab;

    List<UIPlayer> uiPlayerList = new List<UIPlayer>();
    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }



    public void updateRoomInfo(Room info)
    {
  
        roomNameText.text = info.Name;
        roomPlayerCountText.text = info.PlayerCount + "/" + info.MaxPlayers;
        roomStatusText.text = "Waiting For Other Players";
        roomPasswordText.text = "(NO Password)";

        foreach (var uiPlayer in uiPlayerList)
        {
            Destroy(uiPlayer.gameObject);
        }
        uiPlayerList.Clear();

        foreach (var player in info.Players)
        {
            var uiPlayer = Instantiate(UIPlayerPrefab, playersContainer);
            uiPlayer.playerName.text = player.Value.NickName;
            if (player.Value.IsLocal)
            {
                uiPlayer.myPlayerBackground.SetActive(true);
                uiPlayer.otherPlayerBackground.SetActive(false);

            }
            else
            {
                uiPlayer.myPlayerBackground.SetActive(false);
                uiPlayer.otherPlayerBackground.SetActive(true);
            }

            uiPlayerList.Add(uiPlayer);


        }
    }

 
    public void testInGame()
    {
        UIWindow.transTo(EnumsData.WindowEnum.AnyFirstWindow, EnumsData.SceneEnum.InGame);
    }
}
