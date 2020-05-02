using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickup : Pickup
{
    public int healthAmount;
    public override void onPickedUp(CPlayer palyerPickedIt)
    {
        palyerPickedIt.takeHealth(healthAmount);
    }

}
