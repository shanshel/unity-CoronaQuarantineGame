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

    // 0=preparing; 1=playing; 2=finishing; 3=finished
    public int GameStatus = 0;
    public float GameTimer = 300f;
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
        SoundManager._inst.stopAllMusic();

        StartCoroutine(gameLoop());
    }

    IEnumerator gameLoop()
    {

        //While Preparing 
        while(GameStatus == 0)
        {
            yield return new WaitForSeconds(1f);
        }

        //While InGame
        while(GameStatus == 1)
        {
            //Master
            if (PhotonNetwork.IsMasterClient && GameStatus == 1)
            {
                if (GameTimer > 0f)
                    GameTimer -= 1f;
            }

            //All Clients
            UIInGameCanvas._inst.setTime(GameTimer);
            yield return new WaitForSeconds(1f);
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


 
    

    public void whenReadyToPlay()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            GameStatus = 1;
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(this.GameStatus);
            stream.SendNext(this.GameTimer);
        }
        else
        {
            this.GameStatus = (int)stream.ReceiveNext();
            this.GameTimer = (float)stream.ReceiveNext();
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
        UIInGameCanvas._inst.setInflected(inflictedCount);
    }



}
