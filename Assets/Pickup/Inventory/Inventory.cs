using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public int maxSlot;
    //public List<InvItem> slots = new List<InvItem>();

    public Dictionary<int, InvItem> slots = new Dictionary<int, InvItem>() { };


    public void onSlotClicked(int slotIndex)
    {
        if (slots.Count >= slotIndex)
        {
            slots[slotIndex].whenUseItem();
        }
    }

    public void addItemToSlot(GameObject item)
    {
        if (slots.Count == maxSlot) return;

        for (var x = 0; x < slots.Count; x++) 
        {
            
        }


    }
}
