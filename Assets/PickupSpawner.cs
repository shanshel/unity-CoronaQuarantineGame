using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;
public class PickupSpawner : MonoBehaviourPunCallbacks
{

    private static PickupSpawner _instance;
    public static PickupSpawner _inst { get { return _instance; } }
    //public PhotonView _photonView;
    public Pickup[] pickupPrefabs;
    public Transform[] pickupPoints;
    public int maxPickupSpawnedAtTime = 20;
    public float spawnRefreshTime = 20f;
    
    public Dictionary<string, Pickup> spawnedPickup = new Dictionary<string, Pickup>();


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
        InvokeRepeating("spawnPickups", 1f, spawnRefreshTime);
    }

    void spawnPickups()
    {

        if (!PhotonNetwork.IsMasterClient) return;
        foreach (var point in pickupPoints)
        {
            if (spawnedPickup.Count >= maxPickupSpawnedAtTime) return;
            var spawnChance = Random.Range(0, 10);
            //if (spawnChance < 5) continue;

            var randomPickupIndex = Random.Range(0, pickupPrefabs.Length);
            var pickupKey = point.name.Trim();
            if (spawnedPickup.ContainsKey(pickupKey))
            {
                continue;
            }
            object[] initData = new object[1] { pickupKey };

            var gameObj = PhotonNetwork.Instantiate(
                Path.Combine("Pickups", pickupPrefabs[randomPickupIndex].name),
                point.position,
                Quaternion.identity,
                0,
                initData
                );
        }
    }

    public void removePickup(string pickKey)
    {
        if (!spawnedPickup.ContainsKey(pickKey)) return;
        photonView.RPC("Nk_RemovePickUp", RpcTarget.AllViaServer, pickKey);
    }

    [PunRPC]
    public void Nk_RemovePickUp(string pickKey, PhotonMessageInfo info)
    {
        if (spawnedPickup.ContainsKey(pickKey))
        {
            if (spawnedPickup[pickKey] != null)
            {
                Destroy(spawnedPickup[pickKey].gameObject);
            }
            spawnedPickup.Remove(pickKey);
        }
    }

}
