using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Photon.Pun;

public class ProjPill : Projectile
{
    public CircleCollider2D circleCollider;
    public SpriteRenderer zone;
    public GameObject partc1, partc2;
    bool isReachEnd;

    public override void Setup()
    {
        SoundManager._inst.playSoundOnceAt(EnumsData.SoundEnum.StartGassing, transform.position);
    }

    void hitSomething()
    {

    }
    public override void whenHitWall(Collision2D collision)
    {
        if (!_photonView.IsMine) return;
        var speed = lastVelocity.magnitude;
        var direction = Vector3.Reflect(lastVelocity.normalized, collision.contacts[0].normal);
        _rigid.velocity = direction * Mathf.Max(speed, 0f);
    }

    public override void disappear()
    {
        _polyCollider.enabled = false;
        _rigid.velocity = Vector2.zero;
        circleCollider.enabled = true;
        isReachEnd = true;
        partc1.SetActive(true);
        partc2.SetActive(true);
        zone.DOColor(new Color(0.9339623f, 0.3216002f, 0.3216002f, 1f), .3f).SetAutoKill(true).Play();
        Instantiate(disapearingEffectPrefab, transform.position, Quaternion.identity);
        SoundManager._inst.playSoundOnceAt(EnumsData.SoundEnum.Gassing, transform.position);
        Invoke("beforeDestroy", 2.5f);
    }

    void beforeDestroy()
    {
        circleCollider.enabled = false;
        transform.DOScale(0f, .3f).SetEase(Ease.OutBounce).SetAutoKill(true).Play();
        partc1.SetActive(false);
        partc2.SetActive(false);

        Invoke("destroyImiditly", 1f);
    }

    void destroyImiditly()
    {
        if (_photonView.IsMine)
            PhotonNetwork.Destroy(gameObject);
    }

    public override void whenTriggerWithPatient(Collider2D collision)
    {
        CPlayer hittedPlayer = collision.transform.parent.GetComponent<CPlayer>();
        if (hittedPlayer != null)
        {
            if (hittedPlayer.isWilldie())
            {
                bulletOwner.onKillSomeone();
            }
            hittedPlayer.takeDamage(damage);

        }
    }
 


    public override void overwriteableUpdate()
    {
        if (isReachEnd)
        {
            gameObject.transform.Rotate(Vector3.forward, 360f * Time.deltaTime);
        }
    }
}
