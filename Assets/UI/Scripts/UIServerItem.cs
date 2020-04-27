using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using TMPro;
public class UIServerItem : MonoBehaviour
{
    public RoomInfo _roomInfo;

    public TextMeshProUGUI roomName, playerCount;
    public GameObject lockIcon;
    public System.Action<string> onRoomPassowrdInputedCallBack;

    private void Start()
    {
        onRoomPassowrdInputedCallBack += onRoomPasswordInputed;
    }
    public void setRoomUIInfo(RoomInfo roomInfo)
    {
        _roomInfo = roomInfo;
        roomName.text = roomInfo.Name;
        playerCount.text = roomInfo.PlayerCount + "/" + roomInfo.MaxPlayers;
        if (roomInfo.CustomProperties.Count > 0)
        {
            lockIcon.SetActive(true);
        }
        else
        {
            lockIcon.SetActive(false);
        }
    }

    public void onJoinButtonClicked()
    {
       
        if (_roomInfo.CustomProperties.ContainsKey("Password"))
        {
            PopupManager._inst.showInputPopup("Room Password", "Please Enter The Room Password", onRoomPassowrdInputedCallBack);
        }
        else
        {
            LobbyController._inst.joinRoom(_roomInfo);
        }
       
    }

    public void onRoomPasswordInputed(string password)
    {
        bool isPasswordRight = false;
        if (_roomInfo.CustomProperties.ContainsKey("Password"))
        {
            isPasswordRight = _roomInfo.CustomProperties.ContainsValue(password);
        }

        if (isPasswordRight)
        {
            LobbyController._inst.joinRoom(_roomInfo);
        }
        else
        {
            PopupManager._inst.showError("Wrong Password", "Please Write The Correct Room Password");
        }
    }

}
