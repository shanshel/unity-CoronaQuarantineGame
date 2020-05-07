using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class ProjSneez : Projectile
{
    public CircleCollider2D circleCollider;
    bool isStopped = false;
    public GameObject effectContainer, visiablePart;
   
    public override void whenHitWall(Collision2D collision)
    {
        if (isStopped) return;
        SoundManager._inst.playSoundOnce(EnumsData.SoundEnum.NeedlePop);
        CancelInvoke("disappear");
        disappear();
        _rigid.velocity = Vector3.zero;
        isStopped = true;
    }

    public override void Setup()
    {
        SoundManager._inst.playSoundOnce(EnumsData.SoundEnum.NeedleThrow);
    }

    public override void disappear()
    {
        _rigid.velocity = Vector2.zero;
        _polyCollider.enabled = false;
        circleCollider.enabled = true;
        effectContainer.SetActive(true);
        visiablePart.transform.DOScale(0, .3f).SetAutoKill(true).SetEase(Ease.InQuad).Play();
        Instantiate(disapearingEffectPrefab, transform.position, Quaternion.identity);
        SoundManager._inst.playSoundOnce(EnumsData.SoundEnum.Gassing);
        Invoke("beforeDestroy", 5f);
    }

    void beforeDestroy()
    {
        circleCollider.enabled = false;
        transform.DOScale(0f, .3f).SetEase(Ease.OutBounce).SetAutoKill(true).Play();
        effectContainer.SetActive(false);
        Destroy(gameObject, 1f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("trigger start to work");
        if (collision.gameObject.layer == 12)
        {
            CPlayer hittedPlayer = collision.GetComponent<CPlayer>();
            if (hittedPlayer != null)
            {
                //&& hittedPlayer != NetworkPlayers._inst._localCPlayer
                //Here logic to damage the player 
                Debug.Log("damage player");
                hittedPlayer.takeDamage(damage);
            }
        }
    }

}
