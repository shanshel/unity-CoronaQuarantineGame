using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvItem : MonoBehaviour
{
    [HideInInspector]
    public bool isUsed = false;
    public GameObject visiableObject;

    protected int slotIndex;
    public void baseUseItem(int itemIndex)
    {
        slotIndex = itemIndex;
        Debug.Log("baseUseItem");
        onUseItem();
    }

    public virtual void onUseItem()
    {

    }
}
