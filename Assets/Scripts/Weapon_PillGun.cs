using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
public class Weapon_PillGun : Weapon
{

    Vector2 force;
    float forceTimer = 0;
    public TextMeshPro bulletCountText;
    public Transform bulletContainer;

    public override void Setup()
    {
        if (bulletCount != -1)
            bulletCountText.text = bulletCount.ToString();
    }
    public override void Shot()
    {
        if (lastShotTime + shotCooldown > Time.time)
        {
       
            return;
        }
        if (bulletCount != -1)
        {
            bulletCount--;
            bulletCountText.text = bulletCount.ToString();
        }
        if (bulletCount == 0)
        {
            NetworkPlayers._inst._localCPlayer.wearDefaultWeapon();
        }
        Instantiate(projectile, shotPoint.position, transform.rotation);
        forceBack();
        lastShotTime = Time.time;
    }

    private void Update()
    {
        forceTimer -= Time.deltaTime;
        if (forceTimer > 0f)
        {
            NetworkPlayers._inst._localCPlayer._rb.AddForce(force);
        }

        if (bulletCount != -1)
        {
            bulletContainer.transform.LookAt(Vector3.up, Vector3.up);
        }
        
    }
    void forceBack()
    {
        force = (NetworkPlayers._inst._localCPlayer.transform.position - shotPoint.position) * 500f;
        forceTimer = .1f;
        Invoke("forceBackReflect", .1f);
    }

    void forceBackReflect()
    {
        force = (-(NetworkPlayers._inst._localCPlayer.transform.position - shotPoint.position)) * 200f;
        forceTimer = .1f;
    }
}
