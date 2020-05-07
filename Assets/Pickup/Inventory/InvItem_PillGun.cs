using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvItem_PillGun : InvItem
{
    public Weapon weapon;
    // Start is called before the first frame update
    public override void onUseItem()
    {
        var localPlayer = NetworkPlayers._inst._localCPlayer;
        if (isUsed) return;
        //remove it from inventory
        isUsed = true;
        Inventory._inst.removeItem(slotIndex);

        localPlayer.wearWeapon(weapon);
        Destroy(gameObject);
    }
}
