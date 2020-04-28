using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

using static EnumsData;
using Photon.Pun;

public class UILobbyCanvas : MonoBehaviour
{
    private static UILobbyCanvas _instance;
    public static UILobbyCanvas _inst { get { return _instance; } }
   


    [SerializeField]
    GameObject serverListLoadingObject, serverListItemsContainerObject, actionBlockerObject;

    [SerializeField]
    UIServerItem uiServerItemPrefab;

    [SerializeField]
    TMP_Dropdown roomPrivicyInput, roomMaxPlayerCountInput;
    [SerializeField]
    TMP_InputField roomName, roomNameSearch;
    List<UIServerItem> currentUIServerItems = new List<UIServerItem>();



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


    public void onJoinRoomButtonClicked()
    {
        UIWindow.transTo(WindowEnum.InRoom);
    }

    public void onRoomCountCompleteTest()
    {
        UIWindow.transTo(WindowEnum.InGame, SceneEnum.InGame);
    }

    public void clearServerList()
    {
        foreach (var roomUIObj in currentUIServerItems)
        {
            Destroy(roomUIObj.gameObject);
        }
        currentUIServerItems.Clear();
    }


    public void onRoomSearchInputChange()
    {
        foreach (var UIRoom in currentUIServerItems)
        {
            if (UIRoom.roomName.text.Contains( roomNameSearch.text ) )
            {
                UIRoom.gameObject.SetActive(true);
            } else
            {
                UIRoom.gameObject.SetActive(false);
            }
        }
    }

    public void updateServerList(Dictionary<string, RoomInfo> roomList)
    {
       
        if (roomList.Count == 0)
        {
            serverListLoadingObject.SetActive(true);
        }
        else
        {
            serverListLoadingObject.SetActive(false);
        }
        foreach (var room in roomList)
        {
            var obj = Instantiate(uiServerItemPrefab, serverListItemsContainerObject.transform);
            obj.setRoomUIInfo(room.Value);
            currentUIServerItems.Add(obj);
        }
    }

    public void onCreateRoomButtonClicked()
    {
        if (!LobbyController._inst.canJoinRoom)
        {
            PopupManager._inst.showError("Not Connected", "If You Have Slow Internet Please Wait Few Seconds And Try Again");
            return;
        }
        UIWindow.transTo(WindowEnum.CreateRoom);
    }

    public void onCreateRoomConforim()
    {
        
        if (string.IsNullOrEmpty(roomName.text))
        {
            PopupManager._inst.showError("Room Name Required", "Please Fill The Room Name");
            return;
        }

        if (roomMaxPlayerCountInput.value != 0 && roomMaxPlayerCountInput.value != 1)
        {
            PopupManager._inst.showError("Max Player Count", "You Should Pick Max Player Count");
            return;
        }

        if (roomPrivicyInput.value != 0 && roomPrivicyInput.value != 1)
        {
            PopupManager._inst.showError("Wrong Privacy", "Please Select Prefered Privacy");
            return;
        }
        bool isPublic = false;
        if (roomPrivicyInput.value == 0)
        {
            isPublic = true;
        }

        int maxPlayer = 4;
        if (roomMaxPlayerCountInput.value == 0)
        {
            maxPlayer = 8;
        }

        LobbyController._inst.createRoom(roomName.text, isPublic, maxPlayer);
    }


    public void removeActionBlocker() 
    {
        actionBlockerObject.SetActive(false);
    }

    public void onBackButtonClicked()
    {
        PhotonNetwork.Disconnect();
    }


}
