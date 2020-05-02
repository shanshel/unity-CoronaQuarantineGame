using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPickup : Pickup
{

    public Weapon weaponPrefab;
    public override void onPickedUp(CPlayer palyerPickedIt)
    {
        palyerPickedIt.wearWeapon(weaponPrefab);
    }

}
