using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Inventory : MonoBehaviour
{

    private static Inventory _instance;
    public static Inventory _inst { get { return _instance; } }

    public int maxSlot;
    public GameObject inventoryContainer, inventorySlotTemplate;
    public Dictionary<GameObject, bool> slotContainers = new Dictionary<GameObject, bool>() { };
    public Dictionary<int, InvItem> invItems = new Dictionary<int, InvItem>() { };
    public bool isReadyToUse;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    private void Start()
    {
        for (var i = 0; i < maxSlot; i++)
        {
            var newItem = Instantiate(inventorySlotTemplate, inventoryContainer.transform);
            var button = newItem.GetComponent<Button>();
            int slotIn = i;
            button.onClick.AddListener(delegate { onSlotClicked (slotIn);  });
            newItem.SetActive(true);
            slotContainers.Add(newItem, false);
        }
        
    }

    public bool addItem(InvItem invItemPrefab)
    {
        if (invItems.Count == maxSlot || !isReadyToUse) return false;
        int i = 0;
        foreach (var slotInfo in slotContainers)
        {
            if (slotInfo.Value == false)
            {
                slotContainers[slotInfo.Key] = true;
                var _item = Instantiate(invItemPrefab, slotInfo.Key.transform);
                invItems.Add(i, _item);
                return true;
            }
            i++;
        }
        return false;
    }

    public void removeItem(int index)
    {
        if (invItems.Count == 0 || invItems[index] == null || !isReadyToUse) return;
        invItems.Remove(index);
        int i = 0;
        foreach(var slot in slotContainers)
        {
            if (i == index)
            {
                slotContainers[slot.Key] = false;
                return;
            }
            i++;
        }
  
    }
    public void onSlotClicked(int index)
    {
        if (invItems.Count == 0 || invItems[index] == null || !isReadyToUse) return;

        Debug.Log("no Error Before Use");
        invItems[index].baseUseItem(index);
    }




}
