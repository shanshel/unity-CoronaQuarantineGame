using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SpeedPickUp : Pickup
{
    public int speedAmount;
    public GameObject[] objectsToDisable;
    private CPlayer lastPlayer;

    public override void onPickedUp(CPlayer palyerPickedIt)
    {
        if (palyerPickedIt.baseSpeed != palyerPickedIt.speed)
        {
            _collider.enabled = true;
            return;
        }
        palyerPickedIt.speed += speedAmount;
        foreach (var sp in objectsToDisable)
        {
            sp.SetActive(false);
        }
        lastPlayer = palyerPickedIt;
        palyerPickedIt.speedTrailOn();
        Invoke("RemoveSpeedEffect", 5f);
        SoundManager._inst.playSoundOnce(EnumsData.SoundEnum.pickUpSound);
        Instantiate(destroyParticlePrefab, transform.position, Quaternion.identity);

    }
    private void Start()
    {
        gameObject.transform.DOScale(1.5f, 2f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InBounce).Play();
    }
    void RemoveSpeedEffect()
    {

        lastPlayer.speed = lastPlayer.baseSpeed;
        lastPlayer.speedTrailOff();

        Destroy(gameObject);
    }
}
