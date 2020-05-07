using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class Pickup : MonoBehaviour, IPunInstantiateMagicCallback
{
    public EnumsData.Team canBePickedBy;
    public BoxCollider2D _collider;
    public ParticleSystem destroyParticlePrefab;
  
    public string pickKey;

    public float spawnChance = 100f;
    Vector3 baseLocalScale;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        CPlayer player = collision.gameObject.GetComponent<CPlayer>();
        if (player != null)
        {
            _collider.enabled = false;
            
            if (player._thisPlayerTeam == canBePickedBy || canBePickedBy == EnumsData.Team.Both)
            {
                onPickedUp(player);
               
            }
        }
    }

    public virtual void onPickedUp(CPlayer palyerPickedIt)
    {

    }

    private void Awake()
    {
        baseLocalScale = transform.localScale;
        transform.localScale = Vector3.zero;
      
    }

    private void OnEnable()
    {
        PhotonNetwork.AddCallbackTarget(this);
    }

    private void OnDisable()
    {
        PhotonNetwork.RemoveCallbackTarget(this);

    }

    private void Start()
    {
        transform.DOScale(baseLocalScale, 1f).OnComplete(() => { onCustomInitComplete(); }).SetEase(Ease.InOutCubic).Play();
    }


    private void OnDestroy()
    {
        PickupSpawner._inst.removePickup(pickKey);
    }
    public void OnPhotonInstantiate(PhotonMessageInfo info)
    {
       
        object[] instantiationData = info.photonView.InstantiationData;
        pickKey = (string)instantiationData[0];
        Debug.Log("We got Key " + pickKey);
        if (PickupSpawner._inst.spawnedPickup.ContainsKey(pickKey))
        {
            Destroy(PickupSpawner._inst.spawnedPickup[pickKey].gameObject);
            PickupSpawner._inst.spawnedPickup.Remove(pickKey);
        }
        PickupSpawner._inst.spawnedPickup.Add(pickKey, this);
    }

    public virtual void onCustomInitComplete() { } 
}
