using System.Collections;
using System.Collections.Generic;
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
    private int hitWallCounter = 0;
    private float lastShotTime;

    private void Start()
    {
        if (shotPoint == null)
            shotPoint = transform;


    }
    public void Shot()
    {
        if (lastShotTime + shotCooldown > Time.time)
        {
            Debug.Log("Shot in Cooldown RightNow");
            return;
        }

        Instantiate(projectile, shotPoint.position, transform.rotation);
        lastShotTime = Time.time;
    }

}
