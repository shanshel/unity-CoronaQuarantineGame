using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{

    Rigidbody2D _rigid;
    public EnumsData.Team usedBy;
    public string weaponName;
    public float shotCooldown;
    public int damage;

    private int hitWallCounter = 0;

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Wall")
        {
            onHitWall();
        }
        else if (collision.gameObject.tag == "Player")
        {
            var cPlayer = collision.gameObject.GetComponent<CPlayer>();
       
        }
    }

    public virtual void onHitWall()
    {
        hitWallCounter++;

    }

    public virtual void onHitEnemy()
    {

    }
}
