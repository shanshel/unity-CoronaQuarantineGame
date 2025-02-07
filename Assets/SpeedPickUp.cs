﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SpeedPickUp : Pickup
{
    public InvItem invItemSpeedPrefab;
    public override void onPickedUp(CPlayer palyerPickedIt)
    {

        if (palyerPickedIt.playerInventory.addItem(invItemSpeedPrefab))
        {
            SoundManager._inst.playSoundOnceAt(EnumsData.SoundEnum.pickUpSound, transform.position);
            Instantiate(destroyParticlePrefab, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
        else
        {
            _collider.enabled = true;
        }
      
    }


    public override void onCustomInitComplete()
    {
        gameObject.transform.DOScale(1.5f, 2f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InBounce).Play();
    }
}
