using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Photon.Pun;

public class ProjSneez : Projectile
{
    public CircleCollider2D circleCollider;
    public GameObject effectContainer, visiablePart;


    List<CPlayer> cPlayersHitted = new List<CPlayer>();

   public void whenHitSomething()
    {
        if (isLifetimeFinished) return;
        isLifetimeFinished = true;
        SoundManager._inst.playSoundOnceAt(EnumsData.SoundEnum.Gassing, transform.position);
        disappear();
        _rigid.velocity = Vector3.zero;
    }
    public override void whenHitWall(Collision2D collision)
    {
        whenHitSomething();
    }
    public override void whenHitBot(Collision2D collision)
    {
        whenHitSomething();
    }

    public override void whenHitPatient(Collision2D collision)
    {
        whenHitSomething();
    }


    public override void whenHitDoctor(Collision2D collision)
    {
        whenHitSomething();
    }

    public override void whenTriggerWithDoctor(Collider2D collision)
    {
        CPlayer player = collision.transform.parent.GetComponent<CPlayer>();
        if (player != null)
        {
            if (!cPlayersHitted.Contains(player))
            {
                cPlayersHitted.Add(player);
            }
            player.takeDamage(damage);
            if (player.cStatus == EnumsData.playerStatus.dead)
            {
                bulletOwner.onKillSomeone();
            }
            player.speed = player.speed / 2;
        }
    }

    public override void Setup()
    {
        
    }

    public override void disappear()
    {
        Debug.LogError("Disapearing now");
        _rigid.velocity = Vector2.zero;
        _polyCollider.enabled = false;
        circleCollider.enabled = true;
        effectContainer.SetActive(true);
        visiablePart.transform.DOScale(0, .3f).SetAutoKill(true).SetEase(Ease.InQuad).Play();
        Instantiate(disapearingEffectPrefab, transform.position, Quaternion.identity);
        SoundManager._inst.playSoundOnceAt(EnumsData.SoundEnum.Gassing, transform.position);
        Invoke("beforeDestroy", 5f);
    }

    void beforeDestroy()
    {
        Debug.LogError("destroy now");
        circleCollider.enabled = false;
        transform.DOScale(0f, .3f).SetEase(Ease.OutBounce).SetAutoKill(true).Play();
        effectContainer.SetActive(false);

        foreach (var p in cPlayersHitted)
        {
            p.speed = p.baseSpeed;
        }
        if (_photonView.IsMine)
            PhotonNetwork.Destroy(this.gameObject);
 
    }

   

    public override void whenTriggerWithBot(Collider2D collision)
    {
        AIBotController hittedBot = collision.transform.parent.GetComponent<AIBotController>();
        if (hittedBot != null)
        {
            hittedBot.getInflected();
        }
    }

}
