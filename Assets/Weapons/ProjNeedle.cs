using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjNeedle : Projectile
{
    // Start is called before the first frame update


    public override void Setup()
    {
        SoundManager._inst.playSoundOnceAt(EnumsData.SoundEnum.NeedleThrow, transform.position);
    }
    public override void whenHitWall(Collision2D collision)
    {
        hitSomething(collision);
    }
    void hitSomething(Collision2D collision, bool dealDamage = false)
    {
        if (isLifetimeFinished) return;
        isLifetimeFinished = true;
        _rigid.velocity = Vector3.zero;
        SoundManager._inst.playSoundOnceAt(EnumsData.SoundEnum.NeedleHit, transform.position);

        if (dealDamage)
        {
            CPlayer hittedPlayer = collision.transform.GetComponent<CPlayer>();
            if (hittedPlayer != null)
            {
                hittedPlayer.takeDamage(damage);
                if (hittedPlayer.cStatus == EnumsData.playerStatus.dead)
                {
                    bulletOwner.onKillSomeone();
                }
            }
        }
        disappear();
    }

    public override void whenHitDoctor(Collision2D collision)
    {
        hitSomething(collision);
    }


    public override void whenHitBot(Collision2D collision)
    {
        hitSomething(collision);
    }

    public override void whenHitPatient(Collision2D collision)
    {
        hitSomething(collision, true);
    }


}
