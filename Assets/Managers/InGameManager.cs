using Cinemachine;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class InGameManager : MonoBehaviourPunCallbacks, IPunObservable
{
    private static InGameManager _instance;
    public static InGameManager _inst { get { return _instance; } }

    public AIBotController aiBotPrefab;
    public List<AIBotController> aiBots = new List<AIBotController>();

    public bool isDev = false;
    // 0=preparing; 1=preparing; 2=playing; 3=finishing; 4=finished
    public int GameStatus = 0;
    public float GameTimer = 900f;
    public int InfectionCount;
    public int QuarantinedCount;
    int onPlayerReady = 0;
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
        if (!PhotonNetwork.IsConnected)
        {
            isDev = true;
        }

        if (!isDev)
        {
            SoundManager._inst.stopAllMusic();
            NetworkPlayers._inst.setUpRoomInfo();
        
        }
        else
        {
            PhotonNetwork.OfflineMode = true;
            NetworkPlayers._inst.setUpRoomInfo_DevVersion();
        }
        StartCoroutine(gameLoop());
        photonView.RPC("playerReady", RpcTarget.MasterClient);
    }

    [PunRPC]
    public void playerReady()
    {
        onPlayerReady++;
    }


    public void setInventoryReady()
    {
        Inventory._inst.isReadyToUse = true;
    }

    IEnumerator gameLoop()
    {

        while (PhotonNetwork.PlayerList.Length != onPlayerReady)
        {
            yield return null;
        }
        NetworkPlayers._inst.initLocalPlayerInRightTeamBasedOnNetworkList();

        while (NetworkPlayers._inst.playerList.Count != PhotonNetwork.PlayerList.Length)
        {
            yield return null;
        }
        if (PhotonNetwork.IsMasterClient)
        {
            GameStatus = 1;
        }
        NetworkPlayers._inst.changePlayerInfoBasedOnOtherPlayers();
        Tutorial._inst.showTutorial(NetworkPlayers._inst._localCPlayer._thisPlayerTeam);
        PickupSpawner._inst.startSpawning();
        yield return new WaitForSeconds(5f);
        Tutorial._inst.hideTutorial(NetworkPlayers._inst._localCPlayer._thisPlayerTeam);
        NetworkPlayers._inst.markPlayersReady();
        if (PhotonNetwork.IsMasterClient)
        {
            GameStatus = 2;
        }
       

        //While InGame
        while(GameStatus != 3)
        {
            //Master
            if (PhotonNetwork.IsMasterClient)
            {
                if (GameTimer > 0f)
                    GameTimer -= 1f;


                int doctorTeamPoints = 0;
                int patientTeamPoints = 0;
                foreach (var cP in NetworkPlayers._inst.playerList)
                {
                    if (cP.Value._thisPlayerTeam == EnumsData.Team.Doctors)
                    {
                        doctorTeamPoints = doctorTeamPoints + cP.Value.killsOnBotCount + cP.Value.killsOnPlayersCount;
                    }
                    else
                    {
                        patientTeamPoints = patientTeamPoints + cP.Value.killsOnBotCount + cP.Value.killsOnPlayersCount;
                    }
                }
 
                InfectionCount = patientTeamPoints;
                QuarantinedCount = doctorTeamPoints;
            }

            //All Clients
            UIInGameCanvas._inst.setTime(GameTimer);
            UIInGameCanvas._inst.updateInfectionSlider(InfectionCount, QuarantinedCount);

            if (GameTimer <= 0f)
            {
                GameStatus = 3;
            }
            yield return new WaitForSeconds(1f);
        }

        while(GameStatus == 3)
        {
            yield return new WaitForSeconds(2f);
            MatchStatistic.calcWinner();
            UIWindow.transTo(EnumsData.WindowEnum.AnyFirstWindow, EnumsData.SceneEnum.ResultScene);
            yield return new WaitForSeconds(5f);
        }
    }



    public void setCameraFollow(Transform follow)
    {

        ScreenManager._inst.setCameraFollow(follow);
    }


    public void setDeadFollowNexT()
    {
        ScreenManager._inst.setDeadFollowNexT();
    }


 
   

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(this.GameStatus);
            stream.SendNext(this.GameTimer);
            stream.SendNext(this.QuarantinedCount);
            stream.SendNext(this.InfectionCount);
            stream.SendNext(this.onPlayerReady);
            
        }
        else
        {
            this.GameStatus = (int)stream.ReceiveNext();
            this.GameTimer = (float)stream.ReceiveNext();
            this.QuarantinedCount = (int)stream.ReceiveNext();
            this.InfectionCount = (int)stream.ReceiveNext();
            this.onPlayerReady = (int)stream.ReceiveNext();
        }
    }

    /* RPC Bot Spawning */

    public void SpawnBots(List<Transform> points)
    {
        Vector3[] pointsArray = new Vector3[10];
        for (int i = 0; i < 10; i++)
        {
            pointsArray[i] = points[i].transform.position;
        }
   
        photonView.RPC("RPC_SpawnBots", RpcTarget.All, pointsArray);
    }

    [PunRPC]
    public void RPC_SpawnBots(Vector3[] points)
    {
        var container = new GameObject();
        container.name = "Bot Container";
        var i = 0;
        foreach (var p in points)
        {
            Vector3 pos = p;
            var bot = Instantiate(aiBotPrefab, pos, Quaternion.identity, container.transform);
            bot.isAlive = true;
            bot.botIndex = i;
            aiBots.Add(bot);
            i++;
        }
    }

    /* RPC Bot Status */
    public void UpdateBotStatus(int index, bool isAlive)
    {
        photonView.RPC("RPC_UpdateBotStatus", RpcTarget.All, index, isAlive);
        
    }
    [PunRPC]
    public void RPC_UpdateBotStatus(int index, bool isAlive)
    {
        if (isAlive == true)
        {
            aiBots[index].revive();
        }
        else
        {
            aiBots[index].die();
        }

        int inflictedCount = 0;
        foreach(var bot in aiBots)
        {
            if (!bot.isAlive)
            {
                inflictedCount++;
            }
        }
    }



}
