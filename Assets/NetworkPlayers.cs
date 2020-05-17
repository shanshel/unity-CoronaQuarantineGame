using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class NetworkPlayers : MonoBehaviour
{
    private static NetworkPlayers _instance;
    public static NetworkPlayers _inst { get { return _instance; } }

    public Dictionary<string, CPlayer> playerList = new Dictionary<string, CPlayer>() { };
    public CPlayer _localCPlayer;
    [SerializeField]
    CDoctor[] doctorCharactersPrefab;

    [SerializeField]
    Transform[] doctorSpawnPoints, patientSpawnPoints;


    [SerializeField]
    CPatient[] patientCharactersPrefab;

    bool isFirstSetToDoctors = false;

    int random100;

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

    public void setUpRoomInfo()
    {
        if (PhotonNetwork.CurrentRoom != null)
        {
            random100 = int.Parse((string)PhotonNetwork.CurrentRoom.CustomProperties["Random100"]);
        }
        else
        {
            //DevOnly
            random100 = Random.Range(0, 100);
        }
    }
    public void setUpRoomInfo_DevVersion()
    {
        random100 = Random.Range(0, 100);
    }

    public void initLocalPlayerInRightTeamBasedOnNetworkList()
    {
        if (random100 > 50f)
        {
            isFirstSetToDoctors = true;
        }
        int currentIndex = 0;
        foreach (var p in PhotonNetwork.PlayerList)
        {
          

            if (!p.IsLocal)
            {
                currentIndex++;
                continue;
            }
            //int doctorIndex = int.Parse((string)p.CustomProperties["DoctorsIndex"]);
            //int patientIndex = int.Parse((string)p.CustomProperties["PatientIndex"]);


            if (isFirstSetToDoctors)
            {
                if (currentIndex == 0 || currentIndex % 2 == 0)
                {
                    //doctor
                    var spawnPoint = doctorSpawnPoints[Random.Range(0, doctorSpawnPoints.Length)];
                    var charachter = PhotonNetwork.Instantiate(Path.Combine("Doctors", "DoctorCharacterPlayer_Champ2"), spawnPoint.position, Quaternion.identity);
                    _localCPlayer = charachter.GetComponent<CPlayer>();
                }
                else
                {
                    //not doctor
                    var spawnPoint = patientSpawnPoints[Random.Range(0, patientSpawnPoints.Length)];
                    var charachter = PhotonNetwork.Instantiate(Path.Combine("Patients", "PatientCharacterPlayer_Champ1"), spawnPoint.position, Quaternion.identity);
                    _localCPlayer = charachter.GetComponent<CPlayer>();

                }
            }
            else
            {
                if (currentIndex == 0 || currentIndex % 2 == 0)
                {
                    //not doctor
                    var spawnPoint = patientSpawnPoints[Random.Range(0, patientSpawnPoints.Length)];
                    var charachter = PhotonNetwork.Instantiate(Path.Combine("Patients", "PatientCharacterPlayer_Champ1"), spawnPoint.position, Quaternion.identity);
                    _localCPlayer = charachter.GetComponent<CPlayer>();
                }
                else
                {
                    //doctor
                    var spawnPoint = doctorSpawnPoints[Random.Range(0, doctorSpawnPoints.Length)];

                    var charachter = PhotonNetwork.Instantiate(Path.Combine("Doctors", "DoctorCharacterPlayer_Champ2"), spawnPoint.position, Quaternion.identity);
                    _localCPlayer = charachter.GetComponent<CPlayer>();

                }
            }
            //playerList
        }
    
    }

    public void changePlayerInfoBasedOnOtherPlayers()
    {
        foreach (var p in playerList)
        {
            p.Value.changeBasedOnOtherPlayersInfo();
            MatchStatistic.createPlayerStatistic(p.Value._photonView.Owner.NickName, p.Value._thisPlayerTeam, p.Value._photonView.Owner.ActorNumber);
        }
    }

    public Transform getSpawnPoint(EnumsData.Team team)
    {
        if (team == EnumsData.Team.Doctors)
        {
            return doctorSpawnPoints[Random.Range(0, doctorSpawnPoints.Length)];
        }
        else
        {
            return patientSpawnPoints[Random.Range(0, patientSpawnPoints.Length)];
        }
    }

    public CPlayer getCplayerByActorNumber(int actorNumber)
    {
        foreach (var p in playerList)
        {
            if (p.Value._photonView.CreatorActorNr == actorNumber)
            {
                return p.Value;
            }
        }
        return null;
    }

    public void markPlayersReady()
    {
        foreach (var p in playerList)
        {
            p.Value.markPlayerAsReady();
        }
    }

}
