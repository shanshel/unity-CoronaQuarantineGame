using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvItem_SpeedPickup : InvItem
{
    public int speedAmount;

    public override void onUseItem()
    {
        Debug.Log("onUseItem Start");
        var localPlayer = NetworkPlayers._inst._localCPlayer;
        //don't use it if already boost speed
        if (localPlayer.speed != localPlayer.baseSpeed) return;

        //if it's used don't use
        if (isUsed) return;

        //remove it from inventory
        isUsed = true;
        Inventory._inst.removeItem(slotIndex);

     
        localPlayer.speed += speedAmount;
        visiableObject.SetActive(false);
        localPlayer.speedTrailOn();
        Invoke("RemoveSpeedEffect", 5f);
        Debug.Log("onUseItem End");
    }

    void RemoveSpeedEffect()
    {
        var localPlayer = NetworkPlayers._inst._localCPlayer;
        localPlayer.speed = localPlayer.baseSpeed;
        localPlayer.speedTrailOff();
        Destroy(gameObject);
    }
}
