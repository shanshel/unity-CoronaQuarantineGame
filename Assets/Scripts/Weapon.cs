using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Weapon : MonoBehaviour
{

    Rigidbody2D _rigid;
    public EnumsData.Team usedBy;
    public string weaponName;
    public float shotCooldown = 1f;
    public float lifeTime = -1;
    public float bulletCount = -1;
    public Projectile projectile;
    public Transform shotPoint;
    protected float lastShotTime;
    public GameObject VisiablePartContainer;
    private PhotonView _photonView;

    private void Awake()
    {
        _photonView = GetComponent<PhotonView>();
    }
    private void Start()
    {
        if (shotPoint == null)
            shotPoint = transform;

        //SetParent
        foreach (var pO in NetworkPlayers._inst.playerList)
        {
            if (pO.Value.isDevMe || pO.Value._photonView.Owner.NickName == _photonView.Owner.NickName)
            {
                transform.SetParent(pO.Value.aimContainerObject.transform);
                pO.Value._currentWeaponObject = this;
            }
        }
        
    }

    private void LateUpdate()
    {
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
    }
    public virtual void Setup() { }
    public virtual void Shot()
    {
        if (lastShotTime + shotCooldown > Time.time)
        {
            Debug.Log("Shot in Cooldown RightNow");
            return;
        }

        PhotonNetwork.Instantiate(Path.Combine("weapons", projectile.name), shotPoint.position, transform.rotation);
        lastShotTime = Time.time;
    }

}
