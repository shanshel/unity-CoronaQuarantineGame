using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjNeedle : Projectile
{
    // Start is called before the first frame update
    bool isStopped = false;
    public override void whenHitWall(Collision2D collision)
    {
       
        if (isStopped) return;
        SoundManager._inst.playSoundOnce(EnumsData.SoundEnum.NeedlePop);

        _rigid.velocity = Vector3.zero;
        isStopped = true;
    }

    public override void whenHitPlayer(Collision2D collision)
    {
        base.whenHitPlayer(collision);
    }

    public override void whenHitBot(Collision2D collision)
    {
        base.whenHitBot(collision);
    }

    public override void Setup()
    {
        SoundManager._inst.playSoundOnce(EnumsData.SoundEnum.NeedleThrow);
    }
}
