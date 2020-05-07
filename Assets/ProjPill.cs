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
        SoundManager._inst.playSoundOnce(EnumsData.SoundEnum.StartGassing);
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
        SoundManager._inst.playSoundOnce(EnumsData.SoundEnum.Gassing);
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
        PhotonNetwork.Destroy(gameObject);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
       if (collision.gameObject.layer == 12)
        {
            CPlayer hittedPlayer = collision.GetComponent<CPlayer>();
            if (hittedPlayer != null )
            {
                //&& hittedPlayer != NetworkPlayers._inst._localCPlayer
                //Here logic to damage the player 
                Debug.Log("damage player");
                hittedPlayer.takeDamage(damage);
            }
        }
    }

    private void Update()
    {
        if (isReachEnd)
        {
            gameObject.transform.Rotate(Vector3.forward, 360f * Time.deltaTime);
        }
    }
}
