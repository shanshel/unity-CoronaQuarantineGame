using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class HealthPickup : Pickup
{
    
    public int healthAmount;
    public override void onPickedUp(CPlayer palyerPickedIt)
    {
        if (palyerPickedIt.currentHealth == palyerPickedIt.maxHealth)
        {
            _collider.enabled = true;
            return;
        }
        palyerPickedIt.takeHealth(healthAmount);
        SoundManager._inst.playSoundOnce(EnumsData.SoundEnum.pickUpSound);
        Instantiate(destroyParticlePrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);

    }

    public override void onCustomInitComplete()
    {
        gameObject.transform.DOScale(1.5f, 2f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InBounce).Play();
    }

}
