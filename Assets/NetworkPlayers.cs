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
    CPatient[] patientCharactersPrefab;

    bool isFirstSetToDoctors = false;


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
    void Start()
    {
      
       
        float rand = Random.Range(0f, 101f);
        if (rand > 50f)
        {
            isFirstSetToDoctors = true;
        }
        int currentIndex = 0;
        foreach (var p in PhotonNetwork.PlayerList)
        {
       
            int doctorIndex = int.Parse ( (string)p.CustomProperties["DoctorsIndex"] );
            int patientIndex = int.Parse( (string)p.CustomProperties["PatientIndex"] );
           
       
            if (isFirstSetToDoctors)
            {
                if (currentIndex % 2 == 0)
                {
                    var charachter = PhotonNetwork.Instantiate(Path.Combine("Doctors", "DoctorCharacterPlayer_Champ2"), Vector2.zero, Quaternion.identity);
                    var charNetwork = charachter.GetComponent<CPlayer>();
                    charNetwork.SetPlayerNetworkInfo(p);
                    playerList.Add(p.NickName, charNetwork);
                } 
                else
                {
                    var charachter = PhotonNetwork.Instantiate(Path.Combine("Doctors", "DoctorCharacterPlayer_Champ2"), Vector2.zero, Quaternion.identity);
                    var charNetwork = charachter.GetComponent<CPlayer>();
                    charNetwork.SetPlayerNetworkInfo(p);
                    playerList.Add(p.NickName, charNetwork);
                }
            }
            else
            {
                if (currentIndex % 2 == 0)
                {
                    var charachter = PhotonNetwork.Instantiate(Path.Combine("Doctors", "DoctorCharacterPlayer_Champ2"), Vector2.zero, Quaternion.identity);
                    var charNetwork = charachter.GetComponent<CPlayer>();
                    charNetwork.SetPlayerNetworkInfo(p);
                    playerList.Add(p.NickName, charNetwork);
                }
                else
                {
                    var charachter = PhotonNetwork.Instantiate(Path.Combine("Doctors", "DoctorCharacterPlayer_Champ2"), Vector2.zero, Quaternion.identity);
                    var charNetwork = charachter.GetComponent<CPlayer>();
                    charNetwork.SetPlayerNetworkInfo(p);
                    playerList.Add(p.NickName, charNetwork);
                }
            }


     
            //playerList
        }

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
