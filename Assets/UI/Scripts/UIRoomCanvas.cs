using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Realtime;
using Photon.Pun;
using UnityEngine.UI;
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

    public Button startGameButton;
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

    private void Start()
    {
        startGameButton.gameObject.SetActive(false);
        if (PhotonNetwork.IsMasterClient)
        {
            startGameButton.interactable = false;
            startGameButton.gameObject.SetActive(true);
        } 
    }



    public void updateRoomInfo(Room info)
    {
  
        roomNameText.text = info.Name;
        roomPlayerCountText.text = info.PlayerCount + "/" + info.MaxPlayers;

        if (!RoomController._inst.isReady)
        {
            if (info.PlayerCount < RoomController._inst.minPlayerRequiredToStart)
            {
                roomStatusText.text = "At Least " + RoomController._inst.minPlayerRequiredToStart + " Players Required To Start";
                startGameButton.interactable = false;
            }
            else
            {
                roomStatusText.text = "Ready To Play";
                startGameButton.interactable = true;
            }
        }


        if (info.CustomProperties.ContainsKey("Password"))
        {
            roomPasswordText.text = (string)info.CustomProperties["Password"];
        }
        else
        {
            roomPasswordText.text = "(NO Password)";
        }
       

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

 
    public void onLeaveRoomButtonClicked()
    {
        PhotonNetwork.LeaveRoom();
    }

    public void onStartGameButtonClicked()
    {
        RoomController._inst.onStartEvent();
    }
    public void testInGame()
    {
        UIWindow.transTo(EnumsData.WindowEnum.AnyFirstWindow, EnumsData.SceneEnum.InGame);
    }

    public void updateRoomTimer(int timer)
    {
        roomStatusText.text = "Game Start In " + timer.ToString() + " Seconds";
    }
}
