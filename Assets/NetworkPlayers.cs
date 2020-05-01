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
            random100 = int.Parse((string)PhotonNetwork.CurrentRoom.CustomProperties["Random100"]);
            setupPlayersInfo();
        }
    }

    void setupPlayersInfo()
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
            int doctorIndex = int.Parse((string)p.CustomProperties["DoctorsIndex"]);
            int patientIndex = int.Parse((string)p.CustomProperties["PatientIndex"]);


            if (isFirstSetToDoctors)
            {
                if (currentIndex == 0 || currentIndex % 2 == 0)
                {
                    //doctor
                    var spawnPoint = doctorSpawnPoints[Random.Range(0, doctorSpawnPoints.Length)];
                    var charachter = PhotonNetwork.Instantiate(Path.Combine("Doctors", "DoctorCharacterPlayer_Champ2"), spawnPoint.position, Quaternion.identity);
                    var charNetwork = charachter.GetComponent<CPlayer>();
                    charNetwork.SetPlayerNetworkInfo(p);
                    playerList.Add(p.NickName, charNetwork);
                }
                else
                {
                    //not doctor
                    var spawnPoint = patientSpawnPoints[Random.Range(0, patientSpawnPoints.Length)];
                    var charachter = PhotonNetwork.Instantiate(Path.Combine("Doctors", "DoctorCharacterPlayer_Champ2"), spawnPoint.position, Quaternion.identity);
                    var charNetwork = charachter.GetComponent<CPlayer>();
                    charNetwork.SetPlayerNetworkInfo(p);
                    playerList.Add(p.NickName, charNetwork);
                }
            }
            else
            {
                if (currentIndex == 0 || currentIndex % 2 == 0)
                {
                    //not doctor
                    var spawnPoint = patientSpawnPoints[Random.Range(0, patientSpawnPoints.Length)];

                    var charachter = PhotonNetwork.Instantiate(Path.Combine("Doctors", "DoctorCharacterPlayer_Champ2"), spawnPoint.position, Quaternion.identity);
                    var charNetwork = charachter.GetComponent<CPlayer>();
                    charNetwork.SetPlayerNetworkInfo(p);
                    playerList.Add(p.NickName, charNetwork);
                }
                else
                {
                    //doctor
                    var spawnPoint = doctorSpawnPoints[Random.Range(0, doctorSpawnPoints.Length)];

                    var charachter = PhotonNetwork.Instantiate(Path.Combine("Doctors", "DoctorCharacterPlayer_Champ2"), spawnPoint.position, Quaternion.identity);
                    var charNetwork = charachter.GetComponent<CPlayer>();
                    charNetwork.SetPlayerNetworkInfo(p);
                    playerList.Add(p.NickName, charNetwork);
                }
            }
            //playerList
        }
    
    }
    void Start()
    {
        foreach (var cp in playerList)
        {
            if (cp.Value._thisPlayer.IsLocal)
            {
                _localCPlayer = cp.Value;
                InGameManager._inst.setCameraFollow(cp.Value.transform);
            }
        }

    }


}
